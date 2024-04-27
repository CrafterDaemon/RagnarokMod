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
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Armor;
using CalamityMod.Items.Materials;
using CalamityMod.CalPlayer;
using CalamityMod.Rarities;
using RagnarokMod.Projectiles.HealerPro;

namespace RagnarokMod.Items.HealerItems
{
    public class ScoriaDualscythe : ScytheItem
    {
        public override void SetStaticDefaults()
        {
            SetStaticDefaultsToScythe();
        }

        public override void SetDefaults()
        {
            SetDefaultsToScythe();
            base.Item.damage = 90;
            scytheSoulCharge = 3;
            base.Item.width = 114;
            base.Item.height = 116;
            base.Item.value = Item.sellPrice(0, 28, 0);
            base.Item.rare = ModContent.RarityType<Turquoise>();
            base.Item.shoot = ModContent.ProjectileType<ScoriaDualscythePro>();
        }
    }
}
