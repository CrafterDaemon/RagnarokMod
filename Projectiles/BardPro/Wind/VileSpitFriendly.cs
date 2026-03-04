using Terraria;
using Terraria.ID;
using ThoriumMod.Projectiles.Enemy;

namespace RagnarokMod.Projectiles.BardPro.Wind
{
    public class VileSpitFriendly : VileSpit
    {
        public override string Texture => "ThoriumMod/Projectiles/Enemy/VileSpit";
        public override void SetDefaults()
        {
            //    Projectile.width = 12;
            //    Projectile.height = 12;
            //    Projectile.aiStyle = -1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            //    Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            target.AddBuff(BuffID.CursedInferno, 120);
        }
    }
}