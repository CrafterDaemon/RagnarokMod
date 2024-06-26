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
            Item.value = 241115;
            Item.rare = ItemRarityID.Lime;
            Item.shoot = ModContent.ProjectileType<SteampipesPro>();
            Item.shootSpeed = 13f;
            Item.UseSound = RagnarokModSounds.Steampipes;

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