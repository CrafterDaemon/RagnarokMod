using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Scythe;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework.Graphics;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class ElementalReaperPro : ScythePro2
    {
        public override void SafeSetStaticDefaults()
        {
        }

        public override void SafeSetDefaults()
        {
            Projectile.aiStyle = 0;
            Projectile.light = 0.5f;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 26;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            fadeOutTime = 10;
            fadeOutSpeed = 30;
            rotationSpeed = 0.25f;
            scytheCount = 2;
            Projectile.Size = new Vector2(300, 300);
            dustCount = 4;
            dustType = DustID.RainbowTorch;
            dustOffset = new Vector2(-80f, 0f);

        }
        public override void SafeModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            target.AddBuff(ModContent.BuffType<ElementalMix>(), 360);
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
            Color scytheColor = Color.OrangeRed *
                                MathHelper.Lerp(0.15f, 0f, Projectile.alpha / 255f);
            scytheColor.A = 0;

            Texture2D slashTexture = (Texture2D)ModContent.Request<Texture2D>("RagnarokMod/Effects/Assets/Slash_3");
            float slashScale = 4f;

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
