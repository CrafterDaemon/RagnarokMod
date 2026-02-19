using Terraria;
using ThoriumMod.Projectiles.Bard;

namespace RagnarokMod.Projectiles.BardPro.Percussion
{
    public class EctambourinePro : TambourinePro
    {
        public override string Texture => "RagnarokMod/Items/BardItems/Percussion/Ectambourine";
        public override void SetBardDefaults()
        {
            base.SetBardDefaults();
            Projectile.width = 38;
            Projectile.height = 40;
        }
    }
    public class EctambourineProJingle : ShadeWoodTambourinePro2
    {
        public override string Texture => "RagnarokMod/Items/BardItems/Percussion/Ectambourine";
        public override void SetBardDefaults()
        {
            base.SetBardDefaults();
            Projectile.width = 38;
            Projectile.height = 40;
            fadeOutTime = 30;
        }
    }
}