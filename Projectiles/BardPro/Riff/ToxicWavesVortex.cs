using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Riffs;
using RagnarokMod.Riffs.RiffTypes;
using RagnarokMod.Utils;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Projectiles.Bard;
using ThoriumMod.Utilities;

namespace RagnarokMod.Projectiles.BardPro.Riff
{
    public class ToxicWavesVortex : BardProjectile
    {


        public byte RiffType => RiffLoader.RiffType<ToxicWavesRiff>();

        private int sharkronTimer = 0;
        private int SharkronInterval = 8;

        public override void SetBardDefaults()
        {
            Projectile.width = 416;
            Projectile.height = 416;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.timeLeft = 20;
            Projectile.scale = 0.004f;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            if (owner.GetRagnarokModPlayer().activeRiffType == RiffType)
                Projectile.timeLeft++;

            // Follow behind and above the player
            Vector2 targetPos = owner.Center + new Vector2(-40f * owner.direction, -60f);
            Projectile.Center = Vector2.Lerp(Projectile.Center, targetPos, 0.15f);
            float time = Main.GlobalTimeWrappedHourly;
            float bob = MathF.Sin(time * 2.2f);          // 5px up/down

            Projectile.Center += new Vector2(0f, bob);
            // Scale up
            if (Projectile.scale < 0.66f && Projectile.timeLeft > 19)
            {
                Projectile.scale += 0.006f;
                if (Projectile.scale > 0.66f)
                    Projectile.scale = 0.66f;
            }

            // Fade in
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 5;
                if (Projectile.alpha < 0)
                    Projectile.alpha = 0;
            }

            if (Projectile.timeLeft < 19)
            {
                Projectile.scale -= 0.5f/18f;
            }

            // Rotation
            Projectile.rotation -= 0.08f;

            // Swirling dust particles
            if (Main.rand.NextBool(3))
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                float dist = Main.rand.NextFloat(40f, 90f) * Projectile.scale;
                Vector2 dustPos = Projectile.Center + new Vector2(dist, 0).RotatedBy(angle);
                Vector2 dustVel = (Projectile.Center - dustPos).SafeNormalize(Vector2.Zero) * 1.5f;
                Dust d = Dust.NewDustPerfect(dustPos, DustID.TintableDustLighted, dustVel, 0, new Color(55, 195, 0), Main.rand.NextFloat(0.8f, 1.5f));
                d.noGravity = true;
            }

            // Green light
            float lightAmt = 1.2f * Projectile.scale;
            Lighting.AddLight(Projectile.Center, lightAmt * 0.3f, lightAmt, lightAmt * 0.3f);

            // Spawn sharkrons
            sharkronTimer++;
            if (sharkronTimer >= SharkronInterval && Projectile.scale >= 0.3f)
            {
                sharkronTimer = 0;
                SpawnSharkron();
            }
        }

        private void SpawnSharkron()
        {
            List<NPC> nearbyEnemies = new List<NPC>();
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.lifeMax > 5 && !npc.dontTakeDamage)
                {
                    if (Vector2.Distance(Projectile.Center, npc.Center) < 1000f)
                        nearbyEnemies.Add(npc);
                }
            }

            if (nearbyEnemies.Count > 0)
            {
                NPC target = nearbyEnemies[Main.rand.Next(nearbyEnemies.Count)];
                Vector2 spawnPos = target.Center + new Vector2(Main.rand.NextFloat(-600f, 600f), -Main.screenHeight);
                Vector2 toTarget = Vector2.Normalize((target.Center - new Vector2(0f, 420f)) - spawnPos) * 10f;

                Player owner = Main.player[Projectile.owner];
                int damage = 1;
                if (owner.GetThoriumPlayer() != null)
                    damage = (int)((owner.HeldItem.damage * owner.GetTotalDamage(ThoriumDamageBase<BardDamage>.Instance).Additive)/2);

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPos, toTarget,
                    ModContent.ProjectileType<ToxicSharkron>(), damage, 3f, Projectile.owner, target.whoAmI);
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(6f, 6f);
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.TintableDustLighted, velocity, 0, new Color(55, 195, 0), Main.rand.NextFloat(1f, 2f));
                d.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            float alphaLerp = 1f - Projectile.alpha / 255f;
            float sc = MathHelper.Lerp(1f, 0f, Projectile.localAI[2]);
            Player owner = Main.player[Projectile.owner];

            CalamityUtils.CalculatePerspectiveMatricies(out Matrix view, out Matrix projection);

            void ApplyVortexShader(Color primary, Color secondary, float opacity, float circularRot, BlendState blend)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, blend, Main.DefaultSamplerState,
                    DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

                var shader = GameShaders.Misc["CalamityMod:RancorMagicCircle"];
                shader.UseColor(primary);
                shader.UseSecondaryColor(secondary);
                shader.UseSaturation(MathHelper.ToRadians(22.5f * owner.direction));               
                shader.UseOpacity(opacity * alphaLerp);
                shader.Shader.Parameters["uDirection"].SetValue((float)owner.direction);
                shader.Shader.Parameters["uCircularRotation"].SetValue(circularRot);
                shader.Shader.Parameters["uImageSize0"].SetValue(tex.Size());
                shader.Shader.Parameters["overallImageSize"].SetValue(tex.Size());
                shader.Shader.Parameters["uWorldViewProjection"].SetValue(view * projection);
                shader.Apply();
            }

            float rot = Projectile.rotation;
            float scale = Projectile.scale * sc;

            // Three layered additive passes
            for (int i = 2; i >= 0; i--)
            {
                float lerp = i / 3f;
                Color layerColor = Color.Lerp(Color.White, Color.DarkSeaGreen, lerp);
                float layerScale = MathHelper.Lerp(1f, 1.7f, lerp) * scale;

                ApplyVortexShader(
                    i == 0 ? Color.White : layerColor,
                    i == 0 ? Color.White * 0.75f : layerColor * 0.5f,
                    alphaLerp * MathHelper.Lerp(0.9f, 0.7f, lerp),
                    MathHelper.WrapAngle(-rot / 2f * (i + 1)),
                    BlendState.AlphaBlend);

                Main.EntitySpriteDraw(tex, drawPos, tex.Bounds, layerColor,
                    0f, tex.Size() / 2f, layerScale, SpriteEffects.None, 0);
            }

            Main.spriteBatch.ExitShaderRegion();
            return false;
        }
    }
}
