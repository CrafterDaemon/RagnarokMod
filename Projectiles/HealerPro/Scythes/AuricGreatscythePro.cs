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
    public class AuricGreatscythePro : ScythePro2
    {
        private int trail = 8;
        public Player player => Main.player[Projectile.owner];
        public override void SafeSetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = trail;
            ProjectileID.Sets.TrailingMode[base.Projectile.type] = 4;
        }

        public override void SafeSetDefaults()
        {
            Projectile.oldPos = new Vector2[trail]; // However many trail segments you want
            Projectile.oldRot = new float[trail];
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 32;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            scytheCount = 2;
            Projectile.Size = new Vector2(206f, 190f);
            Projectile.timeLeft = 20;
            rotationSpeed = 0.1f;
            fadeOutSpeed = 30;
            fadeOutTime = 10;
            dustCount = 64;
            dustType = DustID.Electric;
            Projectile.light = 1f;
            fadeOutTime = 10;
            fadeOutSpeed = 30;
            dustOffset = new Vector2(-100f, 220f);
            dustNoGravity = false;
        }

        public override void AI()
        {
            Projectile.timeLeft++;
            rotationSpeed = Math.Min(rotationSpeed + 0.001f, 0.6f);
            float t = (rotationSpeed) / (0.4f);
            Projectile.idStaticNPCHitCooldown = Math.Max((int)MathHelper.Lerp(30, 3, t), 3);
            dustVel = new Vector2(rotationSpeed * 4, -rotationSpeed * 4);
            Projectile.spriteDirection = Projectile.direction = player.direction;
            dustScale = Math.Min(rotationSpeed * 5, 2f);
            Lighting.AddLight(Projectile.Center, 1f, 1f, 1f);
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
            SpawnDust();
        }



        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 drawOrigin = Projectile.Size / 2;
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 pos = (k == 0 ? Projectile.Center : Projectile.oldPos[k] + drawOrigin);
                float trailProgress = 1f - (float)k / Projectile.oldPos.Length;
                Color drawColor = lightColor * MathF.Pow(trailProgress, 1.1f);
                SpriteEffects flipped = Projectile.oldDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, null, drawColor, Projectile.oldRot[k], drawOrigin, Projectile.scale, flipped);
            }
            lightColor = Color.White;
            return true;
        }
    }
}
