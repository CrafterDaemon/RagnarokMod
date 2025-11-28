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
using ThoriumMod.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using RagnarokMod.Projectiles.BardPro.Brass;

namespace RagnarokMod.Items.BardItems.Brass
{
    public class ElysianSong : BardItem
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.Brass;

        public override void SetStaticDefaults()
        {
            Empowerments.AddInfo<Damage>(4, 0);
        }

        public override void SetBardDefaults()
        {
            Item.damage = 315;
            InspirationCost = 2;
            Item.width = 84;
            Item.height = 50;
            Item.scale = 0.6f;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.knockBack = 20f;
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.UseSound = new SoundStyle?(ThoriumSounds.Trumpet_Sound);
            Item.shoot = ModContent.ProjectileType<ElysianSongPro>();
            Item.shootSpeed = 15f;


            ((ModItem)this).Item.useStyle = 5;
            if (!ModLoader.HasMod("Look"))
            {
                ((ModItem)this).Item.holdStyle = 3;
            }
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