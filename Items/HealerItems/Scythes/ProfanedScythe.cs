using Terraria.ModLoader;
using Terraria;
using ThoriumMod.Items.HealerItems;
using CalamityMod.Rarities;
using RagnarokMod.Projectiles.HealerPro.Scythes;

namespace RagnarokMod.Items.HealerItems.Scythes
{
    public class ProfanedScythe : ScytheItem
    {
        public override void SetStaticDefaults()
        {
            SetStaticDefaultsToScythe();
        }

        public override void SetDefaults()
        {
            SetDefaultsToScythe();
            base.Item.damage = 464;
            scytheSoulCharge = 3;
            base.Item.width = 86;
            base.Item.height = 90;
            base.Item.value = Item.sellPrice(0, 28, 0);
            base.Item.rare = ModContent.RarityType<Turquoise>();
            base.Item.shoot = ModContent.ProjectileType<ProfanedScythePro>();
        }
    }
}
