using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    public class GlacialHarvesterPro : ScythePro2
    {
        public Player player => Main.player[Projectile.owner];
        public override void SafeSetDefaults()
        {
            Projectile.aiStyle = 0;
            Projectile.light = 0.5f;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 20;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 16;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.width = 100;
            Projectile.height = 100;
            fadeOutTime = 10;
            fadeOutSpeed = 30;
            rotationSpeed = 0.24f;
            scytheCount = 2;
            Projectile.Size = new Vector2(100f, 100f);
            Projectile.scale = 2f;
            dustOffset = new Vector2(0f, 10f);
            dustCount = 4;
            dustType = DustID.SnowBlock;
        }
        public override void AI()
        {
            base.AI();
            Projectile.timeLeft++;
            if (player.dead || !player.channel)
            {
                Projectile.Kill();
            }
        }
        public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(3))
                target.AddBuff(BuffID.Frostburn, 300);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Vector2 origin = tex.Size() / 2f;
            float myScale = 2f;

            Main.EntitySpriteDraw(
                tex,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor,
                Projectile.rotation,
                origin,
                myScale,
                player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0
            );

            return false; // skip vanilla drawing
        }
    }
}
