
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Dusts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Scythe;


namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class NightmareFreezerPro : ScythePro2
    {
        public override void SafeSetStaticDefaults()
        {
        }

        public override void SafeSetDefaults()
        {
            base.Projectile.Size = new Vector2(204, 182);
            dustOffset = new Vector2(-35f, 7f);
            dustCount = 4;
            dustType = DustID.BorealWood;
            base.rotationSpeed = 0.25f;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 6;
            Projectile.light = 1f;
            fadeOutSpeed = 10;
            rotationSpeed = 0.25f;
            scytheCount = 2;
        }

        public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Voidfrost>(), 180);
            target.AddBuff(ModContent.BuffType<GlacialState>(), 60);
            target.AddBuff(BuffID.Frostburn2, 300);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Fade with projectile alpha
            lightColor *= MathHelper.Lerp(1f, 0f, Projectile.alpha / 255f);

            // Main scythe texture
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor,
                Projectile.rotation + MathHelper.PiOver4 * Projectile.spriteDirection,
                texture.Size() / 2f,
                Projectile.scale,
                Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0f
            );

            // afterimage
            Color scytheColor = Color.DarkCyan *
                                MathHelper.Lerp(0.15f, 0f, Projectile.alpha / 255f);
            scytheColor.A = 0;

            Texture2D slashTexture = (Texture2D)ModContent.Request<Texture2D>("RagnarokMod/Effects/Assets/Slash_3");
            float slashScale = 2.65f;

            // Two forward-facing afterimages
            Main.EntitySpriteDraw(slashTexture, Projectile.Center - Main.screenPosition, null, scytheColor,
                Projectile.rotation, slashTexture.Size() / 2f, Projectile.scale * slashScale,
                Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            Main.EntitySpriteDraw(slashTexture, Projectile.Center - Main.screenPosition, null, scytheColor,
                Projectile.rotation, slashTexture.Size() / 2f, Projectile.scale * slashScale,
                Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            // Two mirrored afterimages (rotated by 180)
            Main.EntitySpriteDraw(slashTexture, Projectile.Center - Main.screenPosition, null, scytheColor,
                Projectile.rotation + MathHelper.Pi, slashTexture.Size() / 2f, Projectile.scale * slashScale,
                Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            Main.EntitySpriteDraw(slashTexture, Projectile.Center - Main.screenPosition, null, scytheColor,
                Projectile.rotation + MathHelper.Pi, slashTexture.Size() / 2f, Projectile.scale * slashScale,
                Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            return false;
        }
    }
}
