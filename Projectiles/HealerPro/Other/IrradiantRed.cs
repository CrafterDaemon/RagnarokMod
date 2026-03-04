using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Sounds;
using RagnarokMod.Utils;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;

namespace RagnarokMod.Projectiles.HealerPro.Other
{
    public class IrradiantRed : ModProjectile
    {
        private int trail = 8;
        private const float CollisionRadius = 60f;
        private bool hasHit = false;

        private static readonly Rectangle PixelRect = new Rectangle(0, 0, 1, 1);
        private static readonly Vector2 PixelOriginLeft = new Vector2(0, 0.5f);
        private static readonly Vector2 PixelOriginCenter = new Vector2(0.5f, 0.5f);

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = trail;
            ProjectileID.Sets.TrailingMode[base.Projectile.type] = 2;
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.oldPos = new Vector2[trail];
            Projectile.oldRot = new float[trail];
            Projectile.width = 96;
            Projectile.height = 96;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.knockBack = 20f;
        }

        public override void AI()
        {
            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Type];
            }

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile other = Main.projectile[i];
                if (!other.active || other.whoAmI == Projectile.whoAmI) continue;
                if (other.type != ModContent.ProjectileType<IrradiantBlue>()) continue;
                if (other.owner != Projectile.owner) continue;
                if (other.ai[0] != 1) continue;
                float dist = Vector2.Distance(Projectile.Center, other.Center);
                if (dist <= CollisionRadius + other.scale * 20f)
                {
                    Vector2 spawnPos = (Projectile.Center + other.Center) / 2f;
                    Vector2 cursorDir = (Main.MouseWorld - spawnPos).SafeNormalize(Vector2.UnitX);
                    if (Main.myPlayer == Projectile.owner)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPos, cursorDir,
                            ModContent.ProjectileType<IrradiantPurple>(),
                            Projectile.damage + other.damage, Projectile.knockBack, Projectile.owner);
                        SoundEngine.PlaySound(RagnarokModSounds.PurpleCharge, spawnPos);
                    }
                    other.Kill();
                    Projectile.Kill();
                    return;
                }
            }

            if (Main.rand.NextBool(2))
            {
                Vector2 offset = Main.rand.NextVector2Circular(Projectile.width * 0.2f, Projectile.height * 0.2f);
                Dust fire = Dust.NewDustPerfect(Projectile.Center + offset, DustID.RedTorch,
                    -Projectile.velocity * 0.15f + Main.rand.NextVector2Circular(1f, 1f),
                    0, default, Main.rand.NextFloat(1.2f, 2f));
                fire.noGravity = true;
                fire.fadeIn = 0.5f;
            }

            if (Main.rand.NextBool(4))
            {
                Dust spark = Dust.NewDustPerfect(Projectile.Center, DustID.RedTorch,
                    -Projectile.velocity * 0.05f + new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(0.5f, 2f)),
                    0, default, Main.rand.NextFloat(0.8f, 1.4f));
                spark.noGravity = false;
            }

            Lighting.AddLight(Projectile.Center, 0.7f, 0.15f, 0.1f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!hasHit)
            {
                Projectile.ThoriumHeal(5, radius: 1000, specificPlayer: Projectile.owner);
                hasHit = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D texture = TextureAssets.Projectile[ModContent.ProjectileType<NoProj>()].Value;
            Texture2D pixel = TextureAssets.MagicPixel.Value;
            int frameHeight = texture.Height / Main.projFrames[Type];
            Rectangle sourceRect = new Rectangle(0, Projectile.frame * frameHeight, texture.Width, frameHeight);
            Vector2 origin = new Vector2(texture.Width / 2f, frameHeight / 2f);
            float drawScale = (float)Projectile.width / texture.Width * Projectile.scale;
            float time = Main.GlobalTimeWrappedHourly;

            // STATE: BEGUN (vanilla)
            sb.End();
            // STATE: ENDED
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            // STATE: BEGUN (additive)

            // Pulsing glow
            float pulse = 1f + MathF.Sin(time * 7f) * 0.15f;
            sb.Draw(pixel, Projectile.Center - Main.screenPosition, PixelRect,
                new Color(200, 30, 20) * 0.18f,
                0f, PixelOriginCenter, new Vector2(Projectile.width * 1.4f * pulse), SpriteEffects.None, 0f);

            // Sprite afterimages with glow halo at each position
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                if (Projectile.oldPos[k] == Vector2.Zero) continue;
                float t = 1f - (float)k / Projectile.oldPos.Length;
                Vector2 trailPos = Projectile.oldPos[k] + new Vector2(Projectile.width, Projectile.height) / 2f - Main.screenPosition;

                sb.Draw(pixel, trailPos, PixelRect,
                    new Color(200, 20, 10) * 0.3f * t,
                    0f, PixelOriginCenter, new Vector2(Projectile.width * t), SpriteEffects.None, 0f);
                sb.Draw(pixel, trailPos, PixelRect,
                    new Color(255, 80, 40) * 0.5f * t,
                    0f, PixelOriginCenter, new Vector2(Projectile.width * 0.4f * t), SpriteEffects.None, 0f);
            }


            // STATE: BEGUN (additive)
            sb.End();
            // STATE: ENDED
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            // STATE: BEGUN (normal)

            sb.EnterShaderRegion();
            // STATE: BEGUN (shader)

            Texture2D vortexNoise = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/Cracks").Value;
            GameShaders.Misc["CalamityMod:DoGPortal"].UseOpacity(0.7f);
            GameShaders.Misc["CalamityMod:DoGPortal"].UseColor(new Color(200, 30, 10));
            GameShaders.Misc["CalamityMod:DoGPortal"].UseSecondaryColor(new Color(255, 100, 50));
            GameShaders.Misc["CalamityMod:DoGPortal"].Apply();

            for (int i = 0; i < 4; i++)
            {
                float angle = MathHelper.TwoPi * i / 4f + time * MathHelper.TwoPi * 1.2f;
                Color dc = Color.White * 0.6f;
                dc.A = 0;
                Main.EntitySpriteDraw(vortexNoise, Projectile.Center - Main.screenPosition + angle.ToRotationVector2() * 2f,
                    null, dc, angle + MathHelper.PiOver2, vortexNoise.Size() * 0.5f,
                    Projectile.scale * 0.35f, SpriteEffects.None, 0);
            }

            sb.ExitShaderRegion();
            // STATE: BEGUN (normal) — correct for vanilla

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                float angle = MathHelper.TwoPi * i / 20f;
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.RedTorch,
                    angle.ToRotationVector2() * Main.rand.NextFloat(3f, 8f), 0, default, 1.8f);
                d.noGravity = true;
            }
            for (int i = 0; i < 8; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.RedTorch);
                d.velocity *= 2.5f;
                d.noGravity = false;
                d.scale = 1.5f;
            }
        }
    }
}