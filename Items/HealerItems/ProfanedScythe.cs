using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Projectiles.Scythe;
using Terraria.ID;
using RagnarokMod.Items.Materials;
using RagnarokMod.Projectiles.HealerPro;

namespace RagnarokMod.Items.HealerItems
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
            base.Item.damage = 524;
            scytheSoulCharge = 3;
            base.Item.width = 86;
            base.Item.height = 90;
            base.Item.value = Item.sellPrice(0, 28, 0);
            base.Item.rare = ItemRarityID.12;
            base.Item.shoot = ModContent.ProjectileType<ProfanedScythePro>();
        }
    }
}
