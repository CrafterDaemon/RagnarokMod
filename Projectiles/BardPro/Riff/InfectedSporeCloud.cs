using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Projectiles.BardPro.Riffs;
using RagnarokMod.Riffs;
using RagnarokMod.Riffs.RiffTypes;
using RagnarokMod.Utils;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Bard;

namespace RagnarokMod.Projectiles.BardPro.Riffs
{
    public class InfectedSporeCloud : BardProjectile
    {
        public override string Texture => ModContent.GetInstance<NoProj>().Texture;
        private const float BaseRadius = 200f;
        private const float MaxRadius = 300f;
        private const float GrowthRate = 0.3f;
        private const int PulseInterval = 50;
        private const int PustuleInterval = 150;
        private const int MaxPustules = 3;

        private float currentRadius;
        private int pulseTimer;
        private int pustuleTimer;

        private static readonly Rectangle PixelRect = new Rectangle(0, 0, 1, 1);
        private static readonly Vector2 PixelOriginCenter = new Vector2(0.5f, 0.5f);

        public override BardInstrumentType InstrumentType => BardInstrumentType.Wind;

        public override void SetBardDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;

            currentRadius = BaseRadius;
        }

        private bool RiffIsActive()
        {
            Player owner = Main.player[Projectile.owner];
            return owner.active
                && !owner.dead
                && owner.GetRagnarokModPlayer().activeRiffType == RiffLoader.RiffType<HiveMindRiff>();
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            if (!RiffIsActive())
            {
                Projectile.Kill();
                return;
            }

            Projectile.timeLeft = 600;
            Projectile.Center = owner.Center;

            if (currentRadius < MaxRadius)
            {
                currentRadius += GrowthRate;
                if (currentRadius > MaxRadius)
                    currentRadius = MaxRadius;
            }

            // Slow enemies inside the cloud
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (!npc.active || npc.friendly || npc.dontTakeDamage)
                        continue;

                    float dist = Vector2.Distance(npc.Center, Projectile.Center);
                    if (dist < currentRadius && !npc.boss)
                        npc.velocity *= 0.97f;
                }
            }

            // ring puls
            pulseTimer++;
            if (pulseTimer >= PulseInterval)
            {
                pulseTimer = 0;
                SpawnSporePulse();
            }

            // spawn pustules
            pustuleTimer++;

            int activePustules = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].active
                    && Main.projectile[i].type == ModContent.ProjectileType<InfectedPustule>()
                    && Main.projectile[i].owner == Projectile.owner)
                    activePustules++;
            }

            if (pustuleTimer >= PustuleInterval && activePustules < MaxPustules)
            {
                pustuleTimer = 0;
                if (Main.myPlayer == Projectile.owner)
                    SpawnPustule();
            }

            // particles
            SpawnAmbientSpores();

            float lightScale = currentRadius / MaxRadius;
            Lighting.AddLight(Projectile.Center, 0.1f * lightScale, 0.3f * lightScale, 0.05f * lightScale);
        }

        private void SpawnSporePulse()
        {
            int count = 24;
            for (int i = 0; i < count; i++)
            {
                float angle = MathHelper.TwoPi * i / count + Main.rand.NextFloat(-0.1f, 0.1f);
                Vector2 dir = angle.ToRotationVector2();
                Vector2 pos = Projectile.Center + dir * currentRadius * 0.3f;
                Vector2 vel = dir * Main.rand.NextFloat(3f, 6f);

                int dustType = Main.rand.NextBool(3) ? DustID.CorruptionThorns : DustID.CursedTorch;
                Dust pulse = Dust.NewDustPerfect(pos, dustType, vel, 80, default, Main.rand.NextFloat(1.2f, 2f));
                pulse.noGravity = true;
                pulse.fadeIn = 1f;
            }
        }

        private void SpawnPustule()
        {
            Player owner = Main.player[Projectile.owner];
            float offsetX = Main.rand.NextFloat(-currentRadius * 0.7f, currentRadius * 0.7f);
            float offsetY = Main.rand.NextFloat(-currentRadius * 0.4f, currentRadius * 0.4f);
            Vector2 spawnPos = owner.Center + new Vector2(offsetX, offsetY);

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                spawnPos,
                Vector2.Zero,
                ModContent.ProjectileType<InfectedPustule>(),
                Projectile.damage,
                Projectile.knockBack,
                Projectile.owner
            );
        }

        private void SpawnAmbientSpores()
        {
            // spore particles throughout the cloud
            for (int i = 0; i < 2; i++)
            {
                if (!Main.rand.NextBool(2))
                    continue;

                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                float dist = Main.rand.NextFloat(20f, currentRadius * 0.9f);
                Vector2 pos = Projectile.Center + angle.ToRotationVector2() * dist;

                Vector2 tangent = new Vector2(-MathF.Sin(angle), MathF.Cos(angle));
                Vector2 vel = tangent * Main.rand.NextFloat(0.5f, 2f) + new Vector2(0, Main.rand.NextFloat(-0.5f, 0.3f));

                int dustType = Main.rand.NextBool(3) ? DustID.CorruptionThorns : DustID.CursedTorch;
                Dust spore = Dust.NewDustPerfect(pos, dustType, vel, 120, default, Main.rand.NextFloat(0.6f, 1.2f));
                spore.noGravity = true;
            }

            // wisps at the cloud edge
            if (Main.rand.NextBool(4))
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                Vector2 pos = Projectile.Center + angle.ToRotationVector2() * currentRadius * Main.rand.NextFloat(0.7f, 1f);
                Dust wisp = Dust.NewDustPerfect(pos, DustID.CursedTorch,
                    new Vector2(Main.rand.NextFloat(-0.3f, 0.3f), Main.rand.NextFloat(-1.5f, -0.5f)),
                    150, default, Main.rand.NextFloat(0.8f, 1.4f));
                wisp.noGravity = true;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float dist = Vector2.Distance(Projectile.Center, targetHitbox.Center.ToVector2());
            return dist < currentRadius;
        }

        public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.CursedInferno, 180);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D pixel = TextureAssets.MagicPixel.Value;
            float time = Main.GlobalTimeWrappedHourly;
            float radiusRatio = currentRadius / MaxRadius;

            // STATE: BEGUN (vanilla)
            sb.End();
            // STATE: ENDED
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            // STATE: BEGUN (additive)

            float breathe = 1f + MathF.Sin(time * 2.5f) * 0.15f;

            // Outer corruption haze
            float outerSize = currentRadius * 2.2f * breathe;
            sb.Draw(pixel, Projectile.Center - Main.screenPosition, PixelRect,
                new Color(30, 8, 40) * 0.06f * radiusRatio,
                0f, PixelOriginCenter, new Vector2(outerSize), SpriteEffects.None, 0f);

            // Mid green layer
            float midSize = currentRadius * 1.6f * breathe;
            sb.Draw(pixel, Projectile.Center - Main.screenPosition, PixelRect,
                new Color(20, 50, 10) * 0.05f * radiusRatio,
                0f, PixelOriginCenter, new Vector2(midSize), SpriteEffects.None, 0f);

            // Inner glow
            float innerSize = currentRadius * 0.6f;
            float innerPulse = 1f + MathF.Sin(time * 5f) * 0.1f;
            sb.Draw(pixel, Projectile.Center - Main.screenPosition, PixelRect,
                new Color(40, 80, 15) * 0.08f * radiusRatio * innerPulse,
                0f, PixelOriginCenter, new Vector2(innerSize), SpriteEffects.None, 0f);

            // STATE: BEGUN (additive)
            sb.End();
            // STATE: ENDED
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            // STATE: BEGUN (normal)

            sb.EnterShaderRegion();
            // STATE: BEGUN (shader)

            Texture2D vortexNoise = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/Cracks").Value;
            GameShaders.Misc["CalamityMod:DoGPortal"].UseOpacity(0.4f * radiusRatio);
            GameShaders.Misc["CalamityMod:DoGPortal"].UseColor(new Color(30, 10, 50));
            GameShaders.Misc["CalamityMod:DoGPortal"].UseSecondaryColor(new Color(60, 140, 20));
            GameShaders.Misc["CalamityMod:DoGPortal"].Apply();

            for (int i = 0; i < 4; i++)
            {
                float angle = MathHelper.TwoPi * i / 4f + time * MathHelper.TwoPi * 0.3f;
                Color dc = Color.White * 0.5f * radiusRatio;
                dc.A = 0;
                Main.EntitySpriteDraw(vortexNoise,
                    Projectile.Center - Main.screenPosition + angle.ToRotationVector2() * 3f,
                    null, dc, angle + MathHelper.PiOver2, vortexNoise.Size() * 0.5f,
                    0.6f * radiusRatio, SpriteEffects.None, 0);
            }

            sb.ExitShaderRegion();
            // STATE: BEGUN (normal), correct for vanilla

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                float dist = Main.rand.NextFloat(20f, currentRadius);
                Vector2 pos = Projectile.Center + angle.ToRotationVector2() * dist;
                Vector2 vel = (pos - Projectile.Center).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(2f, 6f);

                int dustType = Main.rand.NextBool() ? DustID.CursedTorch : DustID.CorruptionThorns;
                Dust d = Dust.NewDustPerfect(pos, dustType, vel, 100, default, Main.rand.NextFloat(1f, 2f));
                d.noGravity = true;
            }
        }
    }
}