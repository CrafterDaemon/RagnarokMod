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
using RagnarokMod.Projectiles.BardPro;
using CalamityMod.Items.Materials;
using RagnarokMod.Sounds;
using ThoriumMod.Items.BardItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace RagnarokMod.Items.BardItems
{
    public class ElysianSong : BardItem
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.Brass;

        public override void SetStaticDefaults()
        {
            this.Empowerments.AddInfo<Damage>(4, 0);
        }

        public override void SetBardDefaults()
        {
            Item.damage = 260;
            base.InspirationCost = 2;
            base.Item.width = 84;
            base.Item.height = 50;
            base.Item.scale = 0.6f;
            base.Item.useTime = 24;
            base.Item.useAnimation = 24;
            base.Item.useStyle = ItemUseStyleID.Shoot;
            base.Item.noMelee = true;
            base.Item.autoReuse = true;
            base.Item.knockBack = 20f;
            Item.value = 15000;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.UseSound = SoundID.DD2_BookStaffCast;
            base.Item.shoot = ModContent.ProjectileType<ElysianSongPro>();
            base.Item.shootSpeed = 15f;

        }
		
		public override void AddRecipes()
        {
			Recipe recipe = Recipe.Create(Item.type);
			recipe.AddIngredient(ModContent.ItemType<UelibloomBar>(), 12);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();

        }
    }
}