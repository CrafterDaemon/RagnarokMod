using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;

using ThoriumMod;
using ThoriumMod.Utilities;
using ThoriumMod.Projectiles.Bard;

using CalamityMod;
using CalamityMod.Dusts;

using RagnarokMod.Utils;
using RagnarokMod.Riffs;
using RagnarokMod.Riffs.RiffTypes;

namespace RagnarokMod.Projectiles.BardPro.String
{
    public class StellarRiffPro : BardProjectile, ILocalizedModType
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.String;
        public int spawnTime = 0;
        public int spawnFreq = 60;
        private Color astralRed = new Color(237, 93, 83);
        private Color astralCyan = new Color(66, 189, 181);
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
            ProjectileID.Sets.TrailCacheLength[Type] = 5;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetBardDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 255;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 180;
        }


        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<AstralOrange>(), Main.rand.Next(-3, 3), Main.rand.Next(-3, 3), 0, default, 1.25f).noGravity = true;
            }
            return true;
        }

        public override void AI()
        {
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 25;
                if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }
            }

            if (Projectile.ai[0] == 1f)
            {
                Projectile.extraUpdates = 2;
            }
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 3)
            {
                Projectile.frame = 0;
            }
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;
            if (spawnTime < spawnFreq)
                spawnTime++;
            else
            {
                spawnTime = 0;
                if (Projectile.ai[2] != 1f) SpawnChild();
            }
            Projectile.localAI[0]++;
            float homingDelay = 20f;
            if (Projectile.localAI[0] > homingDelay)
            {
                float inertia = 16f;
                float homeSpeed = 12f;
                float minDist = 40f;

                Player player = Main.player[Projectile.owner];
                if (player.GetRagnarokModPlayer().activeRiffType == RiffLoader.RiffType<AureusRiff>())
                {
                    homeSpeed = 11f;
                    inertia = 12f;
                }
                NPC target = Projectile.FindNearestNPCIgnoreTiles(800f);
                if (target != null && Projectile.Distance(target.Center) > minDist)
                {
                    Vector2 desiredVelocity = Projectile.DirectionTo(target.Center) * homeSpeed;
                    Projectile.velocity = (Projectile.velocity * (inertia - 1) + desiredVelocity) / inertia;
                }
            }
            if (Main.rand.NextBool())
            {
                Color color = Main.rand.NextBool() ? astralRed : astralCyan;
                Dust star = Dust.NewDustPerfect(Projectile.Center, DustID.TintableDustLighted,
                    -Projectile.velocity * 0.4f, 0, color, Main.rand.NextFloat(1.6f, 3f));
                star.noGravity = true;
            }
        }

        public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Prevent children from splitting again
            if (Projectile.ai[2] == 1f)
            {
                return;
            }
            SpawnChild();
        }

        private void SpawnChild()
        {
            float speed = 8f;
            // Use the raw velocity angle, NOT Projectile.rotation. rotation has +PiOver2 baked
            // in for sprite drawing and would offset every child direction by 90 degrees.
            float baseAngle = Projectile.velocity.ToRotation();
            for (int i = 0; i < RiffProjCount(); i++)
            {
                float angle;
                Player player = Main.player[Projectile.owner];
                if (player.GetRagnarokModPlayer().activeRiffType == RiffLoader.RiffType<AureusRiff>())
                    angle = MathHelper.ToRadians((90f * (i - 1)) - 90f) + baseAngle;
                else
                    angle = MathHelper.ToRadians((180f * (i - 1)) - 90f) + baseAngle;

                Vector2 velocity = angle.ToRotationVector2() * speed;

                Projectile newProj = Projectile.NewProjectileDirect(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    velocity,
                    Projectile.type,
                    Projectile.damage / 4,
                    Projectile.knockBack,
                    Projectile.owner
                );
                newProj.scale = 0.6f;
                newProj.ai[2] = 1f;
                newProj.timeLeft = 300;
                newProj.penetrate = 1;
            }
        }

        private int RiffProjCount()
        {
            Player player = Main.player[Projectile.owner];
            if (player.GetRagnarokModPlayer().activeRiffType == RiffLoader.RiffType<AureusRiff>())
            {
                return 4;
            }
            else return 2;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 4; i++)
            {
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<AstralOrange>(), Main.rand.Next(-2, 3), Main.rand.Next(-2, 3), 0, default, 1.5f).noGravity = true;
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<AstralBlue>(), Main.rand.Next(-2, 3), Main.rand.Next(-2, 3), 0, default, 1.5f).noGravity = true;
            }
        }


        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            // Use a round texture instead of pixel
            Texture2D glowTexture = TextureAssets.Extra[ExtrasID.ThePerfectGlow].Value; // Falling star sparkle

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            // Alternating color star
            Color starColor = (Main.GameUpdateCount % 20 < 10) ? astralRed : astralCyan;

            // alpha=255 means transparent, alpha=0 means opaque -- convert to a standard 0..1 opacity
            float opacity = (255 - Projectile.alpha) / 255f;

            // Draw star with bloom
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Vector2 origin = glowTexture.Size() / 2f;

            // Outer glow
            spriteBatch.Draw(glowTexture, drawPos, null, starColor * opacity * 0.5f, Projectile.rotation, origin, 1.5f, SpriteEffects.None, 0f);
            if (Projectile.ai[2] != 1f)
                spriteBatch.Draw(glowTexture, drawPos, null, starColor * opacity * 0.5f, Projectile.rotation + MathHelper.PiOver2, origin, 1.5f, SpriteEffects.None, 0f);

            // Mid glow
            spriteBatch.Draw(glowTexture, drawPos, null, starColor * opacity * 0.8f, Projectile.rotation, origin, 1f, SpriteEffects.None, 0f);
            if (Projectile.ai[2] != 1f)
                spriteBatch.Draw(glowTexture, drawPos, null, starColor * opacity * 0.8f, Projectile.rotation + MathHelper.PiOver2, origin, 1f, SpriteEffects.None, 0f);

            // Bright core
            spriteBatch.Draw(glowTexture, drawPos, null, Color.White * opacity, Projectile.rotation, origin, 0.5f, SpriteEffects.None, 0f);
            if (Projectile.ai[2] != 1f)
                spriteBatch.Draw(glowTexture, drawPos, null, Color.White * opacity, Projectile.rotation + MathHelper.PiOver2, origin, 0.5f, SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}