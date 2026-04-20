using CalamityMod.Buffs.DamageOverTime;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Scythe;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class GodSlayersReapPro1 : ScythePro2
    {
        private bool spawned = false;
        private int miniDoGIndex = -1;
        private int timeSpinning = 0;
        public Player player => Main.player[Projectile.owner];
        public override void SafeSetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 1;
            ProjectileID.Sets.TrailingMode[Projectile.type] = -1;
        }

        public override void SafeSetDefaults()
        {
            Projectile.Size = new Vector2(304f, 304f);
            Projectile.timeLeft = 20;
            Projectile.idStaticNPCHitCooldown = 21;
            rotationSpeed = 0.4f;
        }

        public override void AI()
        {
            Projectile.timeLeft++;
            timeSpinning++;

            rotationSpeed = Math.Max(rotationSpeed - 0.001f, 0.15f);

            Projectile.spriteDirection = Projectile.direction = player.direction;
            Lighting.AddLight(Projectile.Center, 0.9f, 0.2f, 0.9f);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            base.Projectile.rotation += (float)player.direction * rotationSpeed;
            base.Projectile.spriteDirection = player.direction;
            player.heldProj = base.Projectile.whoAmI;
            base.Projectile.Center = player.Center;
            base.Projectile.gfxOffY = player.gfxOffY;
            if (player.dead || !player.channel)
            {
                Projectile.Kill();
            }
            if (!spawned && Projectile.owner == Main.myPlayer && timeSpinning == 180)
            {
                spawned = true;
                miniDoGIndex = Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.position,
                    Vector2.Zero,
                    ModContent.ProjectileType<MiniDoGHead>(),
                    Projectile.damage,
                    Projectile.knockBack,
                    Projectile.owner
                );
            }
        }

        public override void OnKill(int timeLeft)
        {
            // When scythe dies, release DoG to home
            if (miniDoGIndex >= 0 && miniDoGIndex < Main.maxProjectiles)
            {
                Projectile miniDoG = Main.projectile[miniDoGIndex];
                if (miniDoG.active && miniDoG.ModProjectile is MiniDoGHead head)
                {
                    SoundEngine.PlaySound(new SoundStyle("CalamityMod/Sounds/Custom/DevourerAttack"), Projectile.Center);
                    head.ReleaseToHoming();
                }
            }
        }

        public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 180);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // TRUE texture center
            Vector2 origin = texture.Size() / 2f;

            SpriteEffects effects =
                Projectile.spriteDirection == 1
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            // Draw Trail
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos =
                    Projectile.oldPos[k]
                    + Projectile.Size / 2f
                    - Main.screenPosition
                    + new Vector2(0f, Projectile.gfxOffY);

                Color drawColor =
                    Projectile.GetAlpha(lightColor) *
                    (1f - k / (float)Projectile.oldPos.Length);

                Main.EntitySpriteDraw(
                    texture,
                    drawPos,
                    null,
                    drawColor,
                    Projectile.oldRot[k],
                    origin,
                    Projectile.scale,
                    effects,
                    0
                );
            }

            // Draw Current Projectile
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                Projectile.GetAlpha(lightColor),
                Projectile.rotation,
                origin,
                Projectile.scale,
                effects,
                0
            );

            // afterimage
            float fadeDuration = 300f;
            float fade = MathHelper.Clamp(1f - timeSpinning / fadeDuration, 0f, 1f);

            Color scytheColor = Color.Purple * fade * 0.15f;
            scytheColor.A = 0;

            Texture2D slashTexture = (Texture2D)ModContent.Request<Texture2D>("RagnarokMod/Effects/Assets/Slash_3");
            float slashScale = 3.9f;
            float rotationOffset = 40;

            if (player.direction != 1)
            {
                rotationOffset *= -1;
            }


            // Two forward-facing afterimages
            Main.EntitySpriteDraw(slashTexture, Projectile.Center - Main.screenPosition, null, scytheColor,
                Projectile.rotation + rotationOffset, slashTexture.Size() / 2f, Projectile.scale * slashScale,
                Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            Main.EntitySpriteDraw(slashTexture, Projectile.Center - Main.screenPosition, null, scytheColor,
                Projectile.rotation + rotationOffset, slashTexture.Size() / 2f, Projectile.scale * slashScale,
                Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            // Two mirrored afterimages (rotated by 180)
            Main.EntitySpriteDraw(slashTexture, Projectile.Center - Main.screenPosition, null, scytheColor,
                Projectile.rotation + MathHelper.Pi + rotationOffset, slashTexture.Size() / 2f, Projectile.scale * slashScale,
                Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            Main.EntitySpriteDraw(slashTexture, Projectile.Center - Main.screenPosition, null, scytheColor,
                Projectile.rotation + MathHelper.Pi + rotationOffset, slashTexture.Size() / 2f, Projectile.scale * slashScale,
                Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            return false; // prevent vanilla draw
        }
    }
}
