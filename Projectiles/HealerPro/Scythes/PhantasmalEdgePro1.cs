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
    public class PhantasmalEdgePro1 : ScythePro2
    {
        private int trail = 6;
        private int fireBallCounter = 0;
        public Player player => Main.player[Projectile.owner];
        public override void SafeSetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = trail;
            ProjectileID.Sets.TrailingMode[base.Projectile.type] = 2;
        }

        public override void SafeSetDefaults()
        {
            Projectile.oldPos = new Vector2[trail];
            Projectile.oldRot = new float[trail];
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 6;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            scytheCount = 2;
            Projectile.Size = new Vector2(278f, 274f);
            Projectile.timeLeft = 20;
            rotationSpeed = 0.2f;
            fadeOutSpeed = 30;
            fadeOutTime = 10;
        }

        public override void AI()
        {
            Projectile.timeLeft++;
            Projectile.spriteDirection = Projectile.direction = player.direction;
            Lighting.AddLight(Projectile.Center, 1f, 0.686f, 0.686f);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            base.Projectile.rotation += (float)player.direction * rotationSpeed;
            base.Projectile.spriteDirection = player.direction;
            player.heldProj = base.Projectile.whoAmI;
            base.Projectile.Center = player.Center;
            base.Projectile.gfxOffY = player.gfxOffY;
            Vector2 spawnLoc = Vector2.Zero;
            spawnLoc.X = Main.rand.Next(-Projectile.width / 2, Projectile.width / 2);
            spawnLoc.Y = Main.rand.Next(-Projectile.height / 2, Projectile.height / 2);
            if (Main.rand.Next(1, 34) == 33 && fireBallCounter != 15)
            {
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center + spawnLoc, Vector2.Zero, ModContent.ProjectileType<PhantasmalEdgeBombs>(), Projectile.damage, Projectile.knockBack);
                fireBallCounter++;
            }
            if (player.dead || !player.channel)
            {
                Projectile.Kill();
            }
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
