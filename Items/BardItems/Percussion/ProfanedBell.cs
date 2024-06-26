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
using CalamityMod.Items.Materials;
using RagnarokMod.Sounds;
using ThoriumMod.Items.BardItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using RagnarokMod.Projectiles.BardPro.Percussion;

namespace RagnarokMod.Items.BardItems.Percussion
{
    public class ProfanedBell : BardItem
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;

        public override void SetStaticDefaults()
        {
            Empowerments.AddInfo<MovementSpeed>(2, 0);
            Empowerments.AddInfo<FlightTime>(3, 0);
            Empowerments.AddInfo<JumpHeight>(2, 0);
        }

        public override void SetBardDefaults()
        {
            Item.damage = 370;
            InspirationCost = 3;
            Item.width = 65;
            Item.height = 65;
            Item.useTime = 27;
            Item.useAnimation = 27;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.knockBack = 2f;
            Item.value = Item.sellPrice(0, 25, 0);
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.UseSound = RagnarokModSounds.calamitybell;
            Item.shoot = ModContent.ProjectileType<ProfanedBellBlast>();
            Item.shootSpeed = 10f;

        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(Item.type);
            recipe.AddIngredient(ModContent.ItemType<UelibloomBar>(), 10);
            recipe.AddIngredient(ModContent.ItemType<DivineGeode>(), 4);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}