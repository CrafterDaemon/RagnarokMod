using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using ThoriumMod.Projectiles.Bard;
using ThoriumMod;

namespace RagnarokMod.Projectiles.BardPro.Percussion
{
    public class AuricDamruShock : BardProjectile
    {
        public override BardInstrumentType InstrumentType
        {
            get
            {
                return BardInstrumentType.Percussion;
            }
        }

        public override void SetBardDefaults()
        {
            Projectile.ownerHitCheck = true;
            Projectile.width = 700;
            Projectile.height = 700;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 vector = player.Center - Main.screenPosition;
            vector.Y += player.gfxOffY;
            Texture2D value = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 vector2 = value.Size() / 2f;
            Main.EntitySpriteDraw(value, vector, null, new Color(255, 255, 255, 0) * (0.02f * Projectile.timeLeft), Projectile.rotation, vector2, 1.7f, 0, 0f);
            return false;
        }

        public override void BardModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.HitDirectionOverride = new int?(target.Center.X < Main.player[Projectile.owner].Center.X ? -1 : 1);
        }

        public override bool? CanCutTiles()
        {
            return new bool?(false);
        }
    }
}
