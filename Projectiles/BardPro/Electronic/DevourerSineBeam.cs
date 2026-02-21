using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Items.BardItems;
using RagnarokMod.Sounds;
using RagnarokMod.Utils;
using ReLogic.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;

namespace RagnarokMod.Projectiles.BardPro.Electronic
{
    public class DevourerSineBeam : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_0";
        private float BeamHalfLength = (Main.screenWidth - 50) / 2;
        private float BeamThickness = 12f;
        private float MaxAmplitude = (float)Main.screenHeight / 4f;
        private float MinAmplitude = 60f;
        private float MinFrequency = 0.4f;
        private float MaxFrequency = 16f;
        private float smoothFrequency = 1f;
        private float smoothAmplitude = 120f;
        private float phaseOffset = 0f;
        private float prevFrequency = 1f;
        private float WaveSpeed = 0.12f;
        private int inspirationTimer;
        private SlotId soundSlot;
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.timeLeft = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
        }


        public override void AI()
        {
            float targetFrequency = MathHelper.Lerp(MinFrequency, MaxFrequency, Projectile.ai[0]);
            float targetAmplitude = MathHelper.Lerp(MaxAmplitude, MinAmplitude, Projectile.ai[0]);

            float newFrequency = MathHelper.Lerp(smoothFrequency, targetFrequency, 0.05f);

            // Adjust phase offset to keep wave continuous at player center (x=0)
            phaseOffset += (smoothFrequency - newFrequency) * MathHelper.TwoPi;

            smoothFrequency = newFrequency;
            smoothAmplitude = MathHelper.Lerp(smoothAmplitude, targetAmplitude, 0.05f);
            prevFrequency = smoothFrequency;

            smoothFrequency = MathHelper.Lerp(smoothFrequency, targetFrequency, 0.05f);
            smoothAmplitude = MathHelper.Lerp(smoothAmplitude, targetAmplitude, 0.05f);
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
                : RagnarokModSounds.devourersine;
            if (!SoundEngine.TryGetActiveSound(soundSlot, out var active) || !active.IsPlaying)
                soundSlot = SoundEngine.PlaySound(riffin.WithVolumeScale((owner.whoAmI == Main.myPlayer) ? ThoriumConfigClient.Instance.InstrumentSoundVolume : ThoriumConfigClient.Instance.OthersInstrumentSoundVolume).WithPitchOffset(pitcha), owner.Center);
            else
                active.Pitch = pitcha;

            if (!owner.channel || owner.dead || !owner.active)
            {
                if (active != null)
                    active.Stop();
                Projectile.Kill();
                return;
            }

            // Drain inspiration every 30 ticks
            if (Projectile.owner == Main.myPlayer)
            {
                if (++inspirationTimer >= 30)
                {
                    inspirationTimer = 0;
                    if (!BardItem.ConsumeInspiration(owner, 2))
                    {
                        active.Stop();
                        Projectile.Kill();
                        return;
                    }
                }
            }

            Projectile.timeLeft = 2;
            Projectile.Center = owner.Center;
            Projectile.velocity = Vector2.Zero;

            if (Projectile.owner == Main.myPlayer)
            {
                Vector2 screenCenter = new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.5f);
                float pitch = (Main.MouseScreen - screenCenter).Length() / screenCenter.Length();
                Projectile.ai[0] = MathHelper.Clamp(pitch, 0f, 1f);
                Projectile.netUpdate = true;
            }

            owner.heldProj = Projectile.whoAmI;
            owner.SetDummyItemTime(2);
        }


        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player owner = Main.player[Projectile.owner];
            float pitch = Projectile.ai[0];
            float amplitude = MathHelper.Lerp(MaxAmplitude, MinAmplitude, pitch);
            float frequency = MathHelper.Lerp(MinFrequency, MaxFrequency, pitch);
            float waveOffset = Main.GameUpdateCount * WaveSpeed;

            // Check if NPC center falls within the wave's swept area
            Vector2 npcRelative = targetHitbox.Center() - owner.Center;
            float xProgress = (npcRelative.X + BeamHalfLength) / (BeamHalfLength * 2f);

            // NPC is outside the beam's horizontal range
            if (xProgress < 0f || xProgress > 1f)
                return null;
            float waveY = (float)Math.Sin(xProgress * MathHelper.TwoPi * frequency + waveOffset) * amplitude;

            float distFromWave = Math.Abs(npcRelative.Y - waveY);
            float hitThickness = amplitude * 0.4f + 20f; // generous hit area for late game feel

            return distFromWave <= hitThickness ? true : null;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player owner = Main.player[Projectile.owner];
            float pitch = Projectile.ai[0];

            float frequency = smoothFrequency;
            float amplitude = smoothAmplitude;
            float waveOffset = Main.GameUpdateCount * WaveSpeed + phaseOffset;

            Texture2D pixel = TextureAssets.MagicPixel.Value;
            Rectangle pixelRect = new Rectangle(0, 0, 1, 1);

            // Use screen width to determine visible segment range
            float visibleStart = -BeamHalfLength;
            float visibleEnd = BeamHalfLength;
            int segments = (int)((visibleEnd - visibleStart) / 2f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < segments; i++)
            {
                float xOffset = MathHelper.Lerp(visibleStart, visibleEnd, i / (float)segments);

                // Use world-space X for wave calculation so it doesn't shimmer when moving
                float xProgress = xOffset / (BeamHalfLength * 2f);
                float wave = (float)Math.Sin(xProgress * MathHelper.TwoPi * smoothFrequency + waveOffset) * smoothAmplitude;

                Vector2 pos = owner.Center + new Vector2(xOffset, wave);

                // Intensity pulses slightly with wave height for dramatic effect
                float intensity = 0.7f + 0.3f * ((wave / amplitude + 1f) * 0.5f);
                float edgeFade = MathHelper.Clamp(1f - Math.Abs(xOffset) / BeamHalfLength, 0f, 1f);
                edgeFade = (float)Math.Pow(edgeFade, 0.3f); // soften the falloff curve

                // Outer glow
                Color glowColor = new Color(100, 40, 200) * intensity * 0.4f * edgeFade;
                Main.spriteBatch.Draw(pixel, pos - Main.screenPosition, pixelRect,
                    glowColor, 0f, Vector2.Zero, BeamThickness * 4f, SpriteEffects.None, 0f);

                // Mid glow
                Color midColor = new Color(160, 70, 255) * intensity * 0.65f * edgeFade;
                Main.spriteBatch.Draw(pixel, pos - Main.screenPosition, pixelRect,
                    midColor, 0f, Vector2.Zero, BeamThickness * 2f, SpriteEffects.None, 0f);

                // Core
                Color coreColor = new Color(230, 140, 255) * intensity *edgeFade;
                Main.spriteBatch.Draw(pixel, pos - Main.screenPosition, pixelRect,
                    coreColor, 0f, Vector2.Zero, BeamThickness, SpriteEffects.None, 0f);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}