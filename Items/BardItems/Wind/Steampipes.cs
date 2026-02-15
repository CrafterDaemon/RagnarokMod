using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Empowerments;
using ThoriumMod.Projectiles.Bard;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using Terraria.DataStructures;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Rarities;
using RagnarokMod.Sounds;
using ThoriumMod.Items.BardItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using RagnarokMod.Projectiles.HealerPro;
using CalamityMod.Items.Materials;
using RagnarokMod.Projectiles.BardPro.Wind;
using System.Collections.Generic;
using ThoriumMod.Items;
using ThoriumMod.Empowerments;
using ThoriumMod.Sounds;
using CalamityMod.Items;
using CalamityMod.Rarities;


namespace RagnarokMod.Items.BardItems.Wind
{
    public class Steampipes : BardItem
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.Wind;

        public override void SetStaticDefaults()
        {
            Empowerments.AddInfo<ResourceRegen>(3, 0);
            Empowerments.AddInfo<ResourceMaximum>(2, 0);
        }

        public override void SetBardDefaults()
        {
            Item.damage = 165;
            InspirationCost = 1;
            Item.width = 56;
            Item.height = 50;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.knockBack = 2f;
            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<SteampipesPro>();
            Item.shootSpeed = 13f;
            Item.UseSound = RagnarokModSounds.Steampipes;

            ((ModItem)this).Item.useStyle = ItemUseStyleID.Shoot;
            if (!ModLoader.HasMod("Look"))
            {
                ((ModItem)this).Item.holdStyle = 3;
            }
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(4, 6f);
        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(Item.type);
            recipe.AddIngredient(ModContent.ItemType<ScoriaBar>(), 12);
            recipe.AddIngredient(ModContent.ItemType<Bagpipe>(), 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();

        }
    }
}