using Terraria.ModLoader;
using Terraria;
using ThoriumMod.Items.HealerItems;
using Terraria.ID;
using RagnarokMod.Items.Materials;
using RagnarokMod.Projectiles.HealerPro.Scythes;
using ThoriumMod;
using CalamityMod.Items;
using CalamityMod.Rarities;

namespace RagnarokMod.Items.HealerItems.Scythes
{
    public class GodSlayersReap : ScytheItem
    {
        public override void SetStaticDefaults()
        {
            SetStaticDefaultsToScythe();
        }

        public override void SetDefaults()
        {
            SetDefaultsToScythe();
            //this is what SetDefaultsToScythe does
            scytheSoulCharge = 0;
            isHealer = true;
            base.Item.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            base.Item.noMelee = true;
            base.Item.noUseGraphic = true;
            base.Item.autoReuse = true;
            base.Item.useTime = 22;
            base.Item.useAnimation = 22;
            base.Item.maxStack = 1;
            base.Item.knockBack = 6.5f;
            base.Item.useStyle = 1;
            base.Item.UseSound = SoundID.Item1;
            base.Item.shootSpeed = 0.1f;
            //end
            base.Item.damage = 18;
            scytheSoulCharge = 2;
            base.Item.width = 54;
            base.Item.height = 42;
            base.Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            base.Item.rare = ModContent.RarityType<DarkBlue>();
            base.Item.shoot = ModContent.ProjectileType<MarbleScythePro>();
        }
        public override void AddRecipes()
        {
        }
    }
}
