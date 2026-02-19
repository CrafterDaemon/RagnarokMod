using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RagnarokMod.Items.Pets
{
    public class KellyPetProjectile : ModProjectile
    {
        private int _frame;
        private float _frameTimer;
        private bool _isIdling;
        private bool _isWalking;
        private bool _isFlying;
        private Vector2 _breathingScale;

        private Asset<Texture2D> _outlineTextureAsset;
        private float _landingTimer;
        private Vector2 _landingScale;
        public override void Unload()
        {
            base.Unload();
            _outlineTextureAsset = null;
        }
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 17;
            Main.projPet[Type] = true;
            ProjectileID.Sets.CharacterPreviewAnimations[Type] =
                ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Type], 6)
                .WithOffset(-25, -20f)
                .WithSpriteDirection(1)
                .WithCode(DelegateMethods.CharacterPreview.SlimePet);
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BabyDino);
            AIType = ProjectileID.BabyDino;
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            player.zephyrfish = false;
            return true;
        }
        public static float Osc(float from, float to, float speed = 1f, float offset = 0f)
        {
            float dif = (to - from) / 2f;
            return from + dif + dif * (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * speed + offset);
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<KellyPetBuff>()))
            {
                Projectile.timeLeft = 2;
            }
            bool oldIsFlying = _isFlying;
            _isIdling = Projectile.frame == 0;
            _isWalking = Projectile.frame >= 1 && Projectile.frame <= 8;
            _isFlying = Projectile.frame >= 9;

            if (oldIsFlying && !_isFlying)
            {
                _landingTimer = 0;
            }
            _landingTimer++;
            float time = 15;
            float ratio = _landingTimer / time;
            float ease = EasingFunction.QuadraticBump(ratio);
            _landingScale = Vector2.Lerp(Vector2.One, new Vector2(1.2f, 0.9f), ease);

            _frameTimer++;
            float frameSpeed = 4;
            if (_isFlying)
                frameSpeed = 2;
            if (_frameTimer >= frameSpeed)
            {
                _frame++;

                _frameTimer = 0;
            }
            if (_isIdling)
            {
                float breathingOsc = Osc(0f, 1f);
                Vector2 targetScale = Vector2.Lerp(Vector2.One, new Vector2(1.05f, 0.92f), breathingOsc);
                _breathingScale = Vector2.Lerp(_breathingScale, targetScale, 0.1f);
                _frame = 0;
            }
            else if (_isWalking)
            {
                _breathingScale = Vector2.Lerp(_breathingScale, Vector2.One, 0.1f);
                if (_frame < 9 || _frame > 16)
                    _frame = 9;

            }
            else if (_isFlying)
            {
                _breathingScale = Vector2.Lerp(_breathingScale, Vector2.One, 0.1f);
                if (_frame < 1 || _frame > 8)
                    _frame = 1;
            }
            else
            {
                _frame = 0;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            _outlineTextureAsset ??= ModContent.Request<Texture2D>(Texture + "_Outline");
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            int frameWidth = texture.Width;
            int frameHeight = texture.Height / Main.projFrames[Type];
            Rectangle srcRect = new Rectangle(0, frameHeight * _frame, frameWidth, frameHeight);
            SpriteEffects spriteEffects = SpriteEffects.None;
            Vector2 origin = new Vector2(34, 68);
            if (Projectile.spriteDirection == 1)
            {
                origin.X = srcRect.Width - origin.X;
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Texture2D outlineTexture = _outlineTextureAsset.Value;

            float rotation = 0;
            Color drawColor = lightColor;
            drawColor *= 0.33f;
            drawColor.A = 0;

            spriteBatch.Draw(outlineTexture, Projectile.Center - Main.screenPosition + new Vector2(0, 16), srcRect, drawColor, rotation, origin,
                Projectile.scale * _breathingScale * _landingScale, spriteEffects, 0);
            spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0, 16), srcRect, lightColor, rotation, origin,
                Projectile.scale * _breathingScale * _landingScale, spriteEffects, 0);
            if (Main.gameMenu)
            {
                return true;
            }
            return false;
        }
    }

    internal delegate float Easer(float t);
    internal static class EasingFunction
    {
        internal static float Clamp(float t)
        {
            return Math.Clamp(t, 0, 1);
        }
        internal static readonly Easer QuadraticBump = delegate (float t)
        {
            t = Clamp(t);
            const float factor = 4;
            return t * (factor - t * factor);
        };
        internal static readonly Easer QuadraticBumpP05 = delegate (float t)
        {
            t = Clamp(t);
            const float factor = 4;
            return MathF.Pow(t * (factor - t * factor), 0.5f);
        };
    }
}