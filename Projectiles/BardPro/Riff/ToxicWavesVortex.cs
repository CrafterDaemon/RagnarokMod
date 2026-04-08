using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Riffs;
using RagnarokMod.Riffs.RiffTypes;
using RagnarokMod.Utils;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Utilities;

namespace RagnarokMod.Projectiles.BardPro.Riffs
{
    public class ToxicWavesVortex : ModProjectile
    {
        Vector2 cen = Vector2.Zero;
        public override string Texture => "CalamityMod/Projectiles/Boss/OldDukeVortex";


        public byte RiffType => RiffLoader.RiffType<ToxicWavesRiff>();

        private int sharkronTimer = 0;
        private int SharkronInterval = 8;

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
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
            cen = Projectile.Center;
            Player owner = Main.player[Projectile.owner];

            if (owner.GetRagnarokModPlayer().activeRiffType == RiffType)
                Projectile.timeLeft++;
            else
            {
                Projectile.Kill();
                return;
            }

            // Follow behind and above the player
            Vector2 targetPos = owner.Center + new Vector2(-40f * owner.direction, -60f);
            Projectile.Center = Vector2.Lerp(Projectile.Center, targetPos, 0.15f);

            // Scale up
            if (Projectile.scale < 0.35f)
            {
                Projectile.scale += 0.006f;
                if (Projectile.scale > 0.35f)
                    Projectile.scale = 0.35f;
            }

            // Fade in
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 5;
                if (Projectile.alpha < 0)
                    Projectile.alpha = 0;
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
                    damage = (int)(owner.HeldItem.damage * owner.GetTotalDamage(ThoriumDamageBase<BardDamage>.Instance).Additive);

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
            Asset<Texture2D> Tex = ModContent.Request<Texture2D>(Texture);

            float sc = MathHelper.Lerp(1, 0, Projectile.localAI[2]);

            float alphaLerp = MathHelper.Lerp(1f, 0f, (float)Projectile.alpha / 255f);

            Main.EntitySpriteDraw(Tex.Value, cen - Main.screenPosition, Tex.Frame(), new Color(0f, 0f, 0f, 0.4f).MultiplyRGBA(new Color(alphaLerp, alphaLerp, alphaLerp, alphaLerp)), -Projectile.rotation / 2 * (4 + 1), Tex.Frame().Center(), 1.61f * Projectile.scale * sc, SpriteEffects.None);

            for (int i = 2; i >= 0; i--)
            {
                float lerp = (float)i / 3f;

                Main.EntitySpriteDraw(Tex.Value, cen - Main.screenPosition, Tex.Frame(), Color.Lerp(new Color(5, 155, 95, 100), new Color(255, 255, 255, 55), lerp).MultiplyRGBA(new Color(alphaLerp, alphaLerp, alphaLerp, alphaLerp)), -Projectile.rotation / 2 * (i + 1), Tex.Frame().Center(), MathHelper.Lerp(1f, 1.7f, lerp) * Projectile.scale * sc, SpriteEffects.None);
            }
            return false;
        }
    }
}
