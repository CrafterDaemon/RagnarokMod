using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Scythe;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class LucentGeminiPro : ScythePro2
    {
        private const int TrailLen = 8;
        private const int PulseInterval = 45;

        private int pulseTimer = 0;
        private float pulseFade = 0f;

        private Player Owner => Main.player[Projectile.owner];

        private static Asset<Texture2D> GlowTex = null;
        private static Texture2D GetGlowTex()
        {
            GlowTex ??= ModContent.Request<Texture2D>("CalamityMod/Particles/BloomCircle");
            return GlowTex.Value;
        }

        public override void SafeSetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = TrailLen;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SafeSetDefaults()
        {
            Projectile.oldPos = new Vector2[TrailLen];
            Projectile.oldRot = new float[TrailLen];
            Projectile.Size = new Vector2(220f, 220f);
            Projectile.timeLeft = 20;
            Projectile.idStaticNPCHitCooldown = 8;
            Projectile.width = 256;
            Projectile.height = 288;
            rotationSpeed = 0.24f;
        }

        public override void SafeAI()
        {
            Player player = Owner;

            Projectile.timeLeft++;
            Projectile.spriteDirection = Projectile.direction = player.direction;
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            Projectile.Center = player.Center;
            Projectile.gfxOffY = player.gfxOffY;

            // Cyan glow lighting
            Lighting.AddLight(Projectile.Center, 0.15f, 0.35f, 0.6f);

            // Pulse timer
            pulseFade = MathHelper.Max(pulseFade - 1f / PulseInterval, 0f);
            pulseTimer++;
            if (pulseTimer >= PulseInterval)
            {
                pulseTimer = 0;
                pulseFade = 1f;
            }

            if (player.dead || !player.channel)
                Projectile.Kill();
        }

        public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player owner = Main.player[Projectile.owner];

            // 33% chance to generate Stratus Starburst (same as FractalOrb)
            if (Main.rand.NextBool(3))
            {
                owner.Calamity().StratusStarburst++;
                owner.Calamity().StratusStarburstResetTimer = (int)MathHelper.Max(
                    owner.Calamity().StratusStarburstResetTimer, 600);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Texture2D glowTex = GetGlowTex();
            Vector2 drawOrigin = Projectile.Size / 2;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            // Draw trail afterimages
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                if (Projectile.oldPos[k] == Vector2.Zero && k > 0) continue;
                Vector2 pos = Projectile.oldPos[k] + drawOrigin;
                Color drawColor = lightColor * (1f / ((float)k + 1));
                SpriteEffects flip = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, null, drawColor, Projectile.oldRot[k], drawOrigin, Projectile.scale, flip);
            }

            // Draw main scythe
            lightColor = Color.White;

            // Draw pulse ring effect
            // Fire pulse effect
            if (pulseFade > 0f)
            {
                float ringExpand = 1f - pulseFade;

                Main.spriteBatch.EnterShaderRegion();
                GameShaders.Misc["CalamityMod:OtherworldBarrierDistortion"].UseOpacity(0.1f * pulseFade);
                GameShaders.Misc["CalamityMod:OtherworldBarrierDistortion"].UseSaturation(0.1f);
                GameShaders.Misc["CalamityMod:OtherworldBarrierDistortion"].SetShaderTexture(
                    ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/Neurons"), 1);
                GameShaders.Misc["CalamityMod:OtherworldBarrierDistortion"].Apply();
                Texture2D ringTex = ModContent.Request<Texture2D>("CalamityMod/Particles/HigResThinCircle").Value;
                Main.EntitySpriteDraw(ringTex, drawPos, null,
                    new Color(120, 200, 255) * pulseFade * 0.75f,
                    0f, ringTex.Size() * 0.5f,
                    Projectile.width * 2f * ringExpand / ringTex.Width,
                    SpriteEffects.None, 0);
                Main.spriteBatch.ExitShaderRegion();

                Main.spriteBatch.EnterShaderRegion();
                GameShaders.Misc["CalamityMod:OtherworldBarrierDistortion"].UseOpacity(0.5f * pulseFade);
                GameShaders.Misc["CalamityMod:OtherworldBarrierDistortion"].UseSaturation(0.2f);
                GameShaders.Misc["CalamityMod:OtherworldBarrierDistortion"].SetShaderTexture(
                    ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/MeltyNoiseHighContrast"), 1);
                GameShaders.Misc["CalamityMod:OtherworldBarrierDistortion"].Apply();
                Texture2D bloomTex = ModContent.Request<Texture2D>("CalamityMod/Particles/BloomCircle").Value;
                Main.EntitySpriteDraw(bloomTex, drawPos, null,
                    new Color(100, 180, 255) * pulseFade * 0.35f,
                    0f, bloomTex.Size() * 0.5f,
                    Projectile.width * 2f * ringExpand / bloomTex.Width,
                    SpriteEffects.None, 0);
                Main.spriteBatch.ExitShaderRegion();
            }

            return true;
        }
    }
}
