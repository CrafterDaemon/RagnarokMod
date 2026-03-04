using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Graphics.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Items.BardItems.String;
using RagnarokMod.Projectiles.BardPro.String;
using RagnarokMod.Riffs;
using RagnarokMod.Riffs.RiffTypes;
using RagnarokMod.Utils;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Projectiles.Bard;

namespace RagnarokMod.Projectiles.BardPro.Riff
{
    public class DragonForceRiffBeam : BardProjectile, ILocalizedModType
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        private const float MaxBeamLength = 1200f;
        private const int DrawPointCount = 12;
        private const int RingCooldownMax = 20;
        private int ringCooldown = 0;
        private int inspirationDrainTimer = 0;
        private const int InspirationDrainRate = 16;
        private float beamWidth = 0f;
        private const float MaxBeamWidth = 1.5f;
        private const int ChargeUpFrames = 20;
        private const int FadeOutFrames = 12;
        private bool fadingOut = false;
        private float fadeProgress = 1f;
        private int savedDamage = -1;
        private Vector2 beamStart;
        private Vector2 beamEnd;
        private float beamLength;
        private Vector2 beamDir;

        public override BardInstrumentType InstrumentType => BardInstrumentType.String;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
        }

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
            Projectile.localNPCHitCooldown = 5;
        }

        private bool OwnerIsChanneling()
        {
            Player owner = Main.player[Projectile.owner];
            return owner.active
                && !owner.dead
                && !owner.noItems
                && !owner.CCed
                && owner.controlUseItem
                && owner.HeldItem.type == ModContent.ItemType<DragonForce>()
                && owner.GetRagnarokModPlayer().activeRiffType == RiffLoader.RiffType<DragonRiff>();
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (savedDamage == -1)
                savedDamage = Projectile.damage;
            Projectile.damage = savedDamage;
            Projectile.penetrate = -1;


            if (!fadingOut)
            {
                if (OwnerIsChanneling())
                {
                    inspirationDrainTimer++;
                    if (inspirationDrainTimer >= InspirationDrainRate)
                    {
                        inspirationDrainTimer = 0;
                        var thoriumPlayer = owner.GetModPlayer<ThoriumMod.ThoriumPlayer>();
                        if (!BardItem.ConsumeInspiration(owner, 2))
                            fadingOut = true;
                    }
                    Projectile.timeLeft = 600;
                    owner.itemTime = owner.HeldItem.useTime;
                    owner.itemAnimation = owner.HeldItem.useAnimation;
                    owner.ChangeDir((Main.MouseWorld.X > owner.Center.X) ? 1 : -1);
                }
                else
                {
                    fadingOut = true;
                }
            }

            if (!fadingOut && beamWidth < MaxBeamWidth)
            {
                beamWidth += MaxBeamWidth / ChargeUpFrames;
                if (beamWidth > MaxBeamWidth)
                    beamWidth = MaxBeamWidth;
            }

            if (fadingOut)
            {
                fadeProgress -= 1f / FadeOutFrames;
                beamWidth = MaxBeamWidth * Math.Max(fadeProgress, 0f);
                if (fadeProgress <= 0f)
                {
                    Projectile.Kill();
                    return;
                }
            }

            Projectile.Center = owner.Center;
            beamStart = owner.Center;

            Vector2 toMouse = Main.MouseWorld - beamStart;
            if (toMouse.LengthSquared() < 1f)
                toMouse = Vector2.UnitX * owner.direction;

            beamDir = Vector2.Normalize(toMouse);
            beamLength = Math.Min(toMouse.Length(), MaxBeamLength);
            beamEnd = beamStart + beamDir * beamLength;

            if (ringCooldown > 0)
                ringCooldown--;

            SpawnBeamParticles();

            float intensity = beamWidth / MaxBeamWidth;
            for (float d = 0; d < beamLength; d += 50f)
            {
                Vector2 lightPos = beamStart + beamDir * d;
                Lighting.AddLight(lightPos, 1.2f * intensity, 0.5f * intensity, 0.08f * intensity);
            }
        }

        private void SpawnBeamParticles()
        {
            float intensity = beamWidth / MaxBeamWidth;
            if (intensity < 0.15f)
                return;

            // Heavy ember stream along the beam
            int particleCount = (int)(beamLength / 25f * intensity);
            for (int i = 0; i < particleCount; i++)
            {
                if (!Main.rand.NextBool(3))
                    continue;

                float t = Main.rand.NextFloat();
                Vector2 pos = Vector2.Lerp(beamStart, beamEnd, t);

                Vector2 perp = new Vector2(-beamDir.Y, beamDir.X);
                pos += perp * Main.rand.NextFloat(-30f, 30f) * intensity;

                Vector2 vel = perp * Main.rand.NextFloat(-2f, 2f) + beamDir * Main.rand.NextFloat(-1f, 3f);

                Dust fire = Dust.NewDustPerfect(pos, DustID.Torch, vel, 80, default, Main.rand.NextFloat(1.2f, 2.8f) * intensity);
                fire.noGravity = true;
                fire.fadeIn = 0.4f;
            }

            // Thick smoke plumes
            if (Main.rand.NextBool(2))
            {
                float t = Main.rand.NextFloat(0.05f, 0.95f);
                Vector2 pos = Vector2.Lerp(beamStart, beamEnd, t);
                Vector2 perp = new Vector2(-beamDir.Y, beamDir.X);
                pos += perp * Main.rand.NextFloat(-20f, 20f);

                Dust smoke = Dust.NewDustPerfect(pos, DustID.Smoke,
                    new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-3f, -1f)),
                    180, new Color(50, 25, 8), Main.rand.NextFloat(2f, 3.5f));
                smoke.noGravity = true;
            }

            // Heavy falling embers
            if (Main.rand.NextBool(2))
            {
                float t = Main.rand.NextFloat(0.1f, 1f);
                Vector2 pos = Vector2.Lerp(beamStart, beamEnd, t);
                Dust ember = Dust.NewDustPerfect(pos, DustID.Torch,
                    new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(2f, 6f)),
                    0, default, Main.rand.NextFloat(1f, 2f));
                ember.noGravity = false;
            }

            // Violent sparks at the tip
            for (int i = 0; i < 2; i++)
            {
                if (!Main.rand.NextBool(2))
                    continue;

                Vector2 tipPos = beamEnd + Main.rand.NextVector2Circular(20f, 20f);
                Dust spark = Dust.NewDustPerfect(tipPos, DustID.Torch,
                    Main.rand.NextVector2Unit() * Main.rand.NextFloat(4f, 10f),
                    0, default, Main.rand.NextFloat(1.5f, 2.5f));
                spark.noGravity = true;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (beamWidth < 0.1f)
                return false;

            // Wider collision than the normal beam
            float collisionRadius = 36f * (beamWidth / MaxBeamWidth);
            float _ = 0f;

            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), beamStart, beamEnd, collisionRadius, ref _))
                return true;

            return false;
        }

        public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Dragonfire>(), 420, false);

            if (ringCooldown <= 0)
            {
                ringCooldown = RingCooldownMax;

                Player owner = Main.player[Projectile.owner];
                int ringCount = 6; // more fireballs than normal beam
                float ringRadius = 90f;

                SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot, owner.Center);

                for (int i = 0; i < ringCount; i++)
                {
                    float angle = MathHelper.TwoPi * i / ringCount;
                    Vector2 spawnPos = owner.Center + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * ringRadius;
                    Vector2 velocity = Vector2.Normalize(spawnPos - owner.Center) * 10f;

                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        spawnPos,
                        velocity,
                        ModContent.ProjectileType<DragonForcePro2>(),
                        Projectile.damage,
                        Projectile.knockBack,
                        Projectile.owner
                    );
                }

                for (int i = 0; i < 30; i++)
                {
                    Vector2 vel = Main.rand.NextVector2Unit() * Main.rand.NextFloat(5f, 14f);
                    Dust ring = Dust.NewDustPerfect(owner.Center, DustID.Torch, vel, 0, default, Main.rand.NextFloat(1.8f, 3f));
                    ring.noGravity = true;
                }
            }
        }


        internal float WidthFunction(float completionRatio, Vector2 vertexPos)
        {
            completionRatio = 1f - completionRatio;
            float intensity = beamWidth / MaxBeamWidth;
            float taper = MathHelper.Lerp(1f, 0.5f, completionRatio);
            float width = 80f * taper * intensity;

            // Organic pulsing
            float time = Main.GlobalTimeWrappedHourly;
            width += (float)Math.Sin(time * 11f + completionRatio * 12f) * 6f * intensity;
            width += (float)Math.Sin(time * 11f - completionRatio * 12f) * 6f * intensity;

            return Math.Max(width, 2f);
        }

        internal Color ColorFunction(float completionRatio, Vector2 vertexPos)
        {
            completionRatio = 1f - completionRatio;
            float intensity = beamWidth / MaxBeamWidth;

            // Gradient along the beam, darker at tip
            Color rootColor = new Color(180, 50, 5);
            Color midColor = new Color(255, 130, 15);
            Color tipColor = new Color(200, 60, 5);

            Color result;
            if (completionRatio < 0.5f)
                result = Color.Lerp(rootColor, midColor, completionRatio * 2f);
            else
                result = Color.Lerp(midColor, tipColor, (completionRatio - 0.5f) * 2f);

            return result * intensity;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (beamWidth < 0.01f || beamLength < 1f)
                return false;

            float intensity = beamWidth / MaxBeamWidth;

            // Bordernado shader with Perlin noise
            Main.spriteBatch.EnterShaderRegion();

            GameShaders.Misc["CalamityMod:Bordernado"].UseSaturation(-0.3f);
            GameShaders.Misc["CalamityMod:Bordernado"].UseOpacity(intensity);
            GameShaders.Misc["CalamityMod:Bordernado"].SetShaderTexture(ModContent.Request<Texture2D>("Terraria/Images/Misc/Perlin"));

            // Build draw points along the beam direction
            Vector2[] drawPoints = new Vector2[DrawPointCount];
            Vector2 perp = new Vector2(-beamDir.Y, beamDir.X);
            float time = Main.GlobalTimeWrappedHourly;

            for (int i = 0; i < DrawPointCount; i++)
            {
                float t = i / (float)(DrawPointCount - 1);
                Vector2 basePos = Vector2.Lerp(beamStart, beamEnd, t);

                // Add organic lateral sway that increases toward the tip
                float sway = (float)Math.Sin(time * 4f + t * 10f) * 8f * t * intensity;
                sway += (float)Math.Sin(time * 7f - t * 6f) * 5f * t * t * intensity;

                drawPoints[i] = basePos + perp * sway;
            }
            Array.Reverse(drawPoints);

            PrimitiveRenderer.RenderTrail(drawPoints, new(WidthFunction, ColorFunction,
                shader: GameShaders.Misc["CalamityMod:Bordernado"]), 60);

            Main.spriteBatch.ExitShaderRegion();

            // beam origin DoGPortal shader
            Main.spriteBatch.EnterShaderRegion();

            Texture2D vortexNoise = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/Cracks").Value;
            GameShaders.Misc["CalamityMod:DoGPortal"].UseOpacity(intensity);
            GameShaders.Misc["CalamityMod:DoGPortal"].UseColor(new Color(220, 10, 5));
            GameShaders.Misc["CalamityMod:DoGPortal"].UseSecondaryColor(new Color(255, 60, 15));

            GameShaders.Misc["CalamityMod:DoGPortal"].Apply();

            // Spinning vortex at the beam origin
            for (int i = 0; i < 6; i++)
            {
                float angle = MathHelper.TwoPi * i / 6f + time * MathHelper.TwoPi * 0.8f;
                Color drawColor = Color.White * intensity;
                drawColor.A = 0;
                Vector2 drawPosition = beamStart - Main.screenPosition + angle.ToRotationVector2() * 4f;
                float vortexScale = 1.2f * intensity;

                Main.EntitySpriteDraw(vortexNoise, drawPosition, null, drawColor,
                    angle + MathHelper.PiOver2, vortexNoise.Size() * 0.5f,
                    vortexScale, SpriteEffects.None, 0);
            }

            Main.spriteBatch.ExitShaderRegion();

            // beam tip same shader, smaller
            Main.spriteBatch.EnterShaderRegion();

            GameShaders.Misc["CalamityMod:DoGPortal"].UseOpacity(intensity * 0.7f);
            GameShaders.Misc["CalamityMod:DoGPortal"].UseColor(new Color(220, 10, 5));
            GameShaders.Misc["CalamityMod:DoGPortal"].UseSecondaryColor(new Color(255, 60, 15));
            GameShaders.Misc["CalamityMod:DoGPortal"].Apply();

            for (int i = 0; i < 4; i++)
            {
                float angle = MathHelper.TwoPi * i / 4f - time * MathHelper.TwoPi * 1.2f; // spin opposite direction
                Color drawColor = Color.White * intensity * 0.8f;
                drawColor.A = 0;
                Vector2 drawPosition = beamEnd - Main.screenPosition + angle.ToRotationVector2() * 3f;
                float vortexScale = 0.8f * intensity;

                Main.EntitySpriteDraw(vortexNoise, drawPosition, null, drawColor,
                    angle + MathHelper.PiOver2, vortexNoise.Size() * 0.5f,
                    vortexScale, SpriteEffects.None, 0);
            }

            Main.spriteBatch.ExitShaderRegion();

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            // Massive death burst along the beam path
            for (int i = 0; i < 40; i++)
            {
                Vector2 pos = Vector2.Lerp(beamStart, beamEnd, Main.rand.NextFloat());
                Vector2 vel = Main.rand.NextVector2Unit() * Main.rand.NextFloat(3f, 10f);
                Dust d = Dust.NewDustPerfect(pos, DustID.Torch, vel, 60, default, Main.rand.NextFloat(1.5f, 3f));
                d.noGravity = true;
            }

            for (int i = 0; i < 15; i++)
            {
                Vector2 pos = Vector2.Lerp(beamStart, beamEnd, Main.rand.NextFloat());
                Dust smoke = Dust.NewDustPerfect(pos, DustID.Smoke,
                    Main.rand.NextVector2Unit() * Main.rand.NextFloat(2f, 5f),
                    150, new Color(50, 25, 10), Main.rand.NextFloat(2f, 4f));
                smoke.noGravity = true;
            }
        }
    }
}