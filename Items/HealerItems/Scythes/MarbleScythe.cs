using CalamityMod.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Items.Materials;
using RagnarokMod.Projectiles.HealerPro.Scythes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items.HealerItems;

namespace RagnarokMod.Items.HealerItems.Scythes
{
    public class MarbleScythe : ScytheItem
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
            base.Item.useStyle = ItemUseStyleID.Swing;
            base.Item.UseSound = SoundID.Item1;
            base.Item.shootSpeed = 0.1f;
            //end
            base.Item.damage = 21;
            scytheSoulCharge = 2;
            base.Item.width = 54;
            base.Item.height = 42;
            base.Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
            base.Item.rare = ItemRarityID.Orange;
            base.Item.shoot = ModContent.ProjectileType<MarbleScythePro>();
        }


        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            scale = 1f;
            return true;
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<EnchantedMarble>(8);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
