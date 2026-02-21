using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Items.BardItems.String;
using RagnarokMod.Riffs;
using RagnarokMod.Riffs.RiffTypes;
using RagnarokMod.Sounds;
using RagnarokMod.Utils;
using ReLogic.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Projectiles.Bard;

namespace RagnarokMod.Projectiles.BardPro.String
{
    public class DragonForcePro1 : BardProjectile, ILocalizedModType
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.ShadowBeamFriendly}";

        private const float MaxBeamLength = 900f;
        private const int RingCooldownMax = 30;
        private int ringCooldown = 0;

        private float beamWidth = 0f;
        private const float MaxBeamWidth = 1f;
        private const int ChargeUpFrames = 15;
        private const int FadeOutFrames = 10;
        private bool fadingOut = false;
        private float fadeProgress = 1f;

        private Vector2 beamStart;
        private Vector2 beamEnd;
        private float beamLength;
        private Vector2 beamDir;
        private int inspirationDrainTimer = 0;
        private const int InspirationDrainRate = 12; // drain 1 inspiration every 12 frames
        private static readonly Rectangle PixelRect = new Rectangle(0, 0, 1, 1);
        private static readonly Vector2 PixelOriginLeft = new Vector2(0, 0.5f);
        private static readonly Vector2 PixelOriginCenter = new Vector2(0.5f, 0.5f);

        private int savedDamage = -1;
        private SlotId soundSlot;
        public override BardInstrumentType InstrumentType => BardInstrumentType.String;

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
            Projectile.localNPCHitCooldown = 6;
        }

        /// <summary>
        /// Returns true if the owner is actively holding use on this weapon.
        /// Bypasses Terraria's channel system entirely.
        /// </summary>
        private bool OwnerIsChanneling()
        {
            Player owner = Main.player[Projectile.owner];
            return owner.active
                && !owner.dead
                && !owner.noItems
                && !owner.CCed
                && owner.controlUseItem
                && owner.HeldItem.type == ModContent.ItemType<DragonForce>()
                && owner.GetRagnarokModPlayer().activeRiffType != RiffLoader.RiffType<DragonRiff>();
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            Vector2 vector = owner.Center - Main.MouseWorld;
            float pitcha = (vector / new Vector2((float)Main.screenWidth * 0.4f, (float)Main.screenHeight * 0.4f)).Length();
            if (pitcha > 1f)
            {
                pitcha = 1f;
            }

            pitcha = pitcha * 2f - 1f;
            SoundStyle riffin = (owner.GetRagnarokModPlayer().riffPlaying || owner.altFunctionUse == 2)
                ? RagnarokModSounds.none
                : RagnarokModSounds.dragonforce;
            if (!SoundEngine.TryGetActiveSound(soundSlot, out var active) || !active.IsPlaying)
                soundSlot = SoundEngine.PlaySound(riffin.WithVolumeScale((owner.whoAmI == Main.myPlayer) ? ThoriumConfigClient.Instance.InstrumentSoundVolume : ThoriumConfigClient.Instance.OthersInstrumentSoundVolume).WithPitchOffset(pitcha), owner.Center);
            else
                active.Pitch = pitcha;
            if (savedDamage == -1)
                savedDamage = Projectile.damage;
            Projectile.damage = savedDamage;
            Projectile.penetrate = -1;
            if (!fadingOut)
            {
                if (OwnerIsChanneling())
                {
                    Projectile.timeLeft = 600;
                    inspirationDrainTimer++;
                    if (inspirationDrainTimer >= InspirationDrainRate)
                    {
                        inspirationDrainTimer = 0;
                        var thoriumPlayer = owner.GetModPlayer<ThoriumMod.ThoriumPlayer>();
                        if (!BardItem.ConsumeInspiration(owner, 2))
                            fadingOut = true;
                        
                    }
                    // Lock the player into use animation so the item doesn't re-fire
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
                    if (active != null)
                        active.Stop();
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
            for (float d = 0; d < beamLength; d += 60f)
            {
                Vector2 lightPos = beamStart + beamDir * d;
                Lighting.AddLight(lightPos, 0.9f * intensity, 0.35f * intensity, 0.05f * intensity);
            }
        }

        private void SpawnBeamParticles()
        {
            float intensity = beamWidth / MaxBeamWidth;
            if (intensity < 0.2f)
                return;

            int particleCount = (int)(beamLength / 35f * intensity);
            for (int i = 0; i < particleCount; i++)
            {
                if (!Main.rand.NextBool(3))
                    continue;

                float t = Main.rand.NextFloat();
                Vector2 pos = Vector2.Lerp(beamStart, beamEnd, t);

                Vector2 perp = new Vector2(-beamDir.Y, beamDir.X);
                pos += perp * Main.rand.NextFloat(-18f, 18f) * intensity;

                Vector2 vel = perp * Main.rand.NextFloat(-1.5f, 1.5f) + beamDir * Main.rand.NextFloat(-0.5f, 2f);

                Dust fire = Dust.NewDustPerfect(pos, DustID.Torch, vel, 100, default, Main.rand.NextFloat(1.0f, 2.2f) * intensity);
                fire.noGravity = true;
                fire.fadeIn = 0.3f;
            }

            if (Main.rand.NextBool(2))
            {
                float t = Main.rand.NextFloat(0.1f, 0.9f);
                Vector2 pos = Vector2.Lerp(beamStart, beamEnd, t);
                Vector2 perp = new Vector2(-beamDir.Y, beamDir.X);
                pos += perp * Main.rand.NextFloat(-15f, 15f);

                Dust smoke = Dust.NewDustPerfect(pos, DustID.Smoke, new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-2f, -0.5f)), 150, new Color(40, 20, 10), Main.rand.NextFloat(1.5f, 2.5f));
                smoke.noGravity = true;
            }

            if (Main.rand.NextBool(3))
            {
                float t = Main.rand.NextFloat(0.2f, 1f);
                Vector2 pos = Vector2.Lerp(beamStart, beamEnd, t);
                Dust ember = Dust.NewDustPerfect(pos, DustID.Torch, new Vector2(Main.rand.NextFloat(-1.5f, 1.5f), Main.rand.NextFloat(1f, 4f)), 0, default, Main.rand.NextFloat(0.8f, 1.5f));
                ember.noGravity = false;
            }

            if (Main.rand.NextBool(2))
            {
                Vector2 tipPos = beamEnd + Main.rand.NextVector2Circular(12f, 12f);
                Dust spark = Dust.NewDustPerfect(tipPos, DustID.Torch, Main.rand.NextVector2Unit() * Main.rand.NextFloat(3f, 7f), 0, default, Main.rand.NextFloat(1.2f, 2f));
                spark.noGravity = true;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (beamWidth < 0.1f)
                return false;

            float collisionRadius = 20f * (beamWidth / MaxBeamWidth);
            float _ = 0f;

            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), beamStart, beamEnd, collisionRadius, ref _))
                return true;

            return false;
        }

        public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Dragonfire>(), 300, false);

            if (ringCooldown <= 0)
            {
                ringCooldown = RingCooldownMax;

                Player owner = Main.player[Projectile.owner];
                int ringCount = 4;
                float ringRadius = 80f;

                SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot, owner.Center);

                for (int i = 0; i < ringCount; i++)
                {
                    float angle = MathHelper.TwoPi * i / ringCount;
                    Vector2 spawnPos = owner.Center + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * ringRadius;
                    Vector2 velocity = Vector2.Normalize(spawnPos - owner.Center) * 8f;

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

                for (int i = 0; i < 20; i++)
                {
                    Vector2 vel = Main.rand.NextVector2Unit() * Main.rand.NextFloat(4f, 10f);
                    Dust ring = Dust.NewDustPerfect(owner.Center, DustID.Torch, vel, 0, default, Main.rand.NextFloat(1.5f, 2.5f));
                    ring.noGravity = true;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (beamWidth < 0.01f || beamLength < 1f)
                return false;

            SpriteBatch sb = Main.spriteBatch;
            Texture2D pixel = TextureAssets.MagicPixel.Value;

            float intensity = beamWidth / MaxBeamWidth;
            float time = Main.GameUpdateCount * 0.04f;

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            float rotation = beamDir.ToRotation();

            // Layer 1: Dark heat distortion
            DrawBeamLayer(sb, pixel, rotation, time,
                outerWidth: 64f * intensity,
                color: new Color(80, 20, 5) * 0.12f * intensity,
                waveAmplitude: 10f,
                waveFrequency: 2.5f,
                segmentStep: 8f);

            // Layer 2: Deep crimson outer fire
            DrawBeamLayer(sb, pixel, rotation, time,
                outerWidth: 40f * intensity,
                color: new Color(140, 30, 5) * 0.3f * intensity,
                waveAmplitude: 6f,
                waveFrequency: 4f,
                segmentStep: 6f);

            // Layer 3: Fiery orange core
            DrawBeamLayer(sb, pixel, rotation, time,
                outerWidth: 22f * intensity,
                color: new Color(230, 90, 10) * 0.5f * intensity,
                waveAmplitude: 4f,
                waveFrequency: 6f,
                segmentStep: 5f);

            // Layer 4: Hot inner core
            DrawBeamLayer(sb, pixel, rotation, time,
                outerWidth: 10f * intensity,
                color: new Color(255, 140, 30) * 0.65f * intensity,
                waveAmplitude: 2f,
                waveFrequency: 8f,
                segmentStep: 4f);


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
                float vortexScale = 0.6f * intensity;

                Main.EntitySpriteDraw(vortexNoise, drawPosition, null, drawColor,
                    angle + MathHelper.PiOver2, vortexNoise.Size() * 0.5f,
                    vortexScale, SpriteEffects.None, 0);
            }

            Main.spriteBatch.ExitShaderRegion();

            // === IMPACT VORTEX at beam tip - same shader, smaller ===
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
                float vortexScale = 0.3f * intensity;

                Main.EntitySpriteDraw(vortexNoise, drawPosition, null, drawColor,
                    angle + MathHelper.PiOver2, vortexNoise.Size() * 0.5f,
                    vortexScale, SpriteEffects.None, 0);
            }

            Main.spriteBatch.ExitShaderRegion();

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }

        private void DrawBeamLayer(SpriteBatch sb, Texture2D pixel, float rotation,
            float time, float outerWidth, Color color, float waveAmplitude, float waveFrequency, float segmentStep)
        {
            Vector2 perp = new Vector2(-beamDir.Y, beamDir.X);

            for (float d = 0; d < beamLength; d += segmentStep)
            {
                float t = d / beamLength;

                float envelope = (float)Math.Sin(t * MathHelper.Pi);
                envelope = Math.Max(envelope, (1f - t) * 0.4f);

                float wobble = (float)Math.Sin(time * waveFrequency + d * 0.05f) * waveAmplitude;
                float wobble2 = (float)Math.Sin(time * waveFrequency * 1.7f - d * 0.08f) * waveAmplitude * 0.5f;

                float width = outerWidth * envelope + wobble + wobble2;
                if (width < 1f) width = 1f;

                Vector2 pos = beamStart + beamDir * d + perp * (wobble * 0.3f);

                sb.Draw(pixel, pos - Main.screenPosition, PixelRect,
                    color, rotation, PixelOriginLeft,
                    new Vector2(segmentStep, width),
                    SpriteEffects.None, 0f);
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 25; i++)
            {
                Vector2 pos = Vector2.Lerp(beamStart, beamEnd, Main.rand.NextFloat());
                Vector2 vel = Main.rand.NextVector2Unit() * Main.rand.NextFloat(2f, 8f);
                Dust d = Dust.NewDustPerfect(pos, DustID.Torch, vel, 0, default, Main.rand.NextFloat(1f, 2f));
                d.noGravity = true;
            }
        }
    }
}