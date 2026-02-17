using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Utils;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RagnarokMod.Projectiles.Riffs
{
    public class ScourgesFretsWormFrontLayer : ModSystem
    {
        public override void Load()
        {
            On_Main.DrawPlayers_AfterProjectiles += DrawWormFrontLayer;
        }

        public override void Unload()
        {
            On_Main.DrawPlayers_AfterProjectiles -= DrawWormFrontLayer;
        }

        private void DrawWormFrontLayer(Terraria.On_Main.orig_DrawPlayers_AfterProjectiles orig, Main self)
        {
            orig(self);
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            foreach (Projectile p in Main.projectile)
            {
                if (!p.active) continue;
                if (p.type != ModContent.ProjectileType<ScourgesFretsWorm>()) continue;
                if (p.ModProjectile is not ScourgesFretsWorm worm) continue;

                worm.DrawBelowPlayer(Main.spriteBatch);
            }

            Main.spriteBatch.End();
        }
    }
    public class ScourgesFretsWorm : ModProjectile
    {
        public override string Texture => "CalamityMod/NPCs/DesertScourge/DesertNuisanceHead";

        private static Asset<Texture2D> HeadTexture;
        private static Asset<Texture2D> BodyTexture;
        private static Asset<Texture2D> BodyTexture2;
        private static Asset<Texture2D> BodyTexture3;
        private static Asset<Texture2D> BodyTexture4;
        private static Asset<Texture2D> TailTexture;

        private int SegmentCount = 14;
        private float OrbitRadius = 90f;
        private float OrbitSpeed = 0.02f;
        private int SegmentOffset = 31;
        private int FrameCount = 7;
        private int FrameSpeed = 10;
        private float alpha = 0f;
        private int fadetimer = 60;
        private bool dying = false;

        // ai[0] = target player index
        // ai[1] = orbit angle offset

        public override void SetStaticDefaults()
        {
            if (!Main.dedServ)
            {
                HeadTexture = ModContent.Request<Texture2D>("CalamityMod/NPCs/DesertScourge/DesertNuisanceHead", AssetRequestMode.AsyncLoad);
                BodyTexture = ModContent.Request<Texture2D>("CalamityMod/NPCs/DesertScourge/DesertNuisanceBody", AssetRequestMode.AsyncLoad);
                BodyTexture2 = ModContent.Request<Texture2D>("CalamityMod/NPCs/DesertScourge/DesertNuisanceBody2", AssetRequestMode.AsyncLoad);
                BodyTexture3 = ModContent.Request<Texture2D>("CalamityMod/NPCs/DesertScourge/DesertNuisanceBody3", AssetRequestMode.AsyncLoad);
                BodyTexture4 = ModContent.Request<Texture2D>("CalamityMod/NPCs/DesertScourge/DesertNuisanceBody4", AssetRequestMode.AsyncLoad);
                TailTexture = ModContent.Request<Texture2D>("CalamityMod/NPCs/DesertScourge/DesertNuisanceTail", AssetRequestMode.AsyncLoad);
            }
        }

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
            Projectile.timeLeft = int.MaxValue;
        }

        public override void AI()
        {
            if (!dying)
                Fade(true);
            int targetIndex = (int)Projectile.ai[0];
            if (targetIndex < 0 || targetIndex >= Main.maxPlayers || !Main.player[targetIndex].active || Main.player[targetIndex].dead || !Main.player[targetIndex].GetRagnarokModPlayer().fretPlaying)
            {
                Fade(false);
            }

            Projectile.Center = Main.player[targetIndex].Center;
        }

        private void Fade(bool fadeIn)
        {
            if (fadetimer > 0 && !fadeIn)
            {
                alpha -= 1f / 60f;
                fadetimer--;
            }
            else if (fadetimer <= 0 && !fadeIn)
                Projectile.Kill();
            if (fadetimer > 0 && fadeIn)
            {
                alpha += 1f / 60f;
                fadetimer--;
            }
            else if (fadetimer <= 0 && fadeIn)
            {
                fadetimer = 60;
                dying = true;
            }
        }

        private Vector2 GetSegmentPosition(int segmentIndex, float currentAngle)
        {
            float angle = currentAngle - segmentIndex * (SegmentOffset / OrbitRadius);
            return Projectile.Center + new Vector2(
                -(float)Math.Cos(angle) * OrbitRadius,
                (float)Math.Sin(angle) * OrbitRadius * 0.35f
            );
        }

        private void DrawSegments(SpriteBatch spriteBatch, bool drawBelow)
        {
            if (HeadTexture == null || !HeadTexture.IsLoaded) return;
            if (BodyTexture == null || !BodyTexture.IsLoaded) return;
            if (TailTexture == null || !TailTexture.IsLoaded) return;

            Player player = Main.player[Projectile.owner];
            float currentAngle = Main.GameUpdateCount * OrbitSpeed + Projectile.ai[1];
            Color drawColor = Color.White * alpha;
            int currentFrame = (int)(Main.GameUpdateCount / FrameSpeed) % FrameCount;
            Asset<Texture2D>[] bodyTextures = { BodyTexture, BodyTexture2, BodyTexture3, BodyTexture4 };

            for (int i = SegmentCount - 1; i >= 0; i--)
            {
                Vector2 segPos = GetSegmentPosition(i, currentAngle);
                Vector2 nextSegPos = GetSegmentPosition(i + 1, currentAngle);

                // drawBelow=true  -> PostDraw  -> segments below player center → render in front of player
                // drawBelow=false -> PreDraw   -> segments above player center → render behind player
                bool isBelow = segPos.Y >= player.Center.Y + player.gfxOffY;
                if (isBelow != drawBelow) continue;

                Vector2 direction = nextSegPos - segPos;
                float rotation = direction.ToRotation() - MathHelper.PiOver2;
                SpriteEffects effects = direction.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

                bool isTail = i == SegmentCount - 1;
                bool isPreTail = i == SegmentCount - 2;
                bool isHead = i == 0;

                Texture2D texture;
                if (isHead)
                    texture = HeadTexture.Value;
                else if (isTail)
                    texture = TailTexture.Value;
                else if (isPreTail)
                    texture = bodyTextures[3].Value;
                else
                    texture = bodyTextures[(i - 1) % 3].Value;

                bool isAnimatedBody = !isHead && !isTail && (i - 1) % 3 == 0;
                int frameHeight = (isAnimatedBody || isHead) ? texture.Height / FrameCount : texture.Height;
                int frame = (isAnimatedBody || isHead) ? currentFrame : 0;
                Rectangle sourceRect = new Rectangle(0, frame * frameHeight, texture.Width, frameHeight);
                Vector2 origin = new Vector2(texture.Width / 2f, frameHeight / 2f);

                spriteBatch.Draw(texture, segPos - Main.screenPosition, sourceRect, drawColor, rotation, origin, 0.6f, effects, 0f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Segments above player center, drawn behind player
            DrawSegments(Main.spriteBatch, drawBelow: false);
            return false;
        }

        public void DrawBelowPlayer(SpriteBatch spriteBatch)
        {
            // Temporarily draw ALL segments to confirm layer is working
            DrawSegments(spriteBatch, drawBelow: true);
        }
    }

}