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
            Projectile.Size = new Vector2(222f, 304f);
            Projectile.timeLeft = 20;
            Projectile.idStaticNPCHitCooldown = 21;
            rotationSpeed = 0.15f;
        }

        public override void AI()
        {
            Projectile.timeLeft++;
            timeSpinning++;
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
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 drawOrigin = Projectile.Size / 2;
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 pos = Projectile.oldPos[k] + drawOrigin;
                Color drawColor = lightColor * (1 / ((float)k + 1));
                SpriteEffects flipped;
                if (Projectile.oldDirection == 1) { flipped = SpriteEffects.None; }
                else { flipped = SpriteEffects.FlipHorizontally; }
                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, null, drawColor, Projectile.oldRot[k], drawOrigin, Projectile.scale, flipped);
            }
            lightColor = Color.White;
            return true;
        }
    }
}
