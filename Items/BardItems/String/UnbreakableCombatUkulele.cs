﻿using ThoriumMod;
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
using CalamityMod.Tiles.Furniture.CraftingStations;
using static Mono.CompilerServices.SymbolWriter.CodeBlockEntry;
using RagnarokMod.Projectiles.BardPro.Electronic;
using CalamityMod.Items.Placeables.Ores;
using RagnarokMod.Projectiles.BardPro.Wind;
using RagnarokMod.Projectiles.BardPro.String;

namespace RagnarokMod.Items.BardItems.String
{
    public class UnbreakableCombatUkulele : BardItem
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.String;
        private int counter = 1;
        public override void SetStaticDefaults()
        {
            Empowerments.AddInfo<LifeRegeneration>(3, 0);
            Empowerments.AddInfo<Defense>(5, 0);
        }

        public override void SetBardDefaults()
        {
            Item.damage = 385;
            InspirationCost = 1;
            Item.width = 60;
            Item.height = 60;
            Item.scale = 1f;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.knockBack = 20f;
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.UseSound = new SoundStyle?(ThoriumSounds.SuperGuitarNoise);
            Item.shoot = ModContent.ProjectileType<UnbreakableCombatUkulelePro1>();
            Item.shootSpeed = 20;


        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool CanShoot(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<UnbreakableCombatUkulelePro2>()] <= 0;
        }
        public override bool BardShoot(
            Player player,
            EntitySource_ItemUse_WithAmmo source,
            Vector2 position,
            Vector2 velocity,
            int type,
            int damage,
            float knockback)
        {
            if (player.altFunctionUse != 2)
                {            
                return true;
                }
            if (player.altFunctionUse == 2)
                {
                    Projectile.NewProjectile(source, position.X, position.Y, velocity.X * 1.25f, velocity.Y * 1.25f, ModContent.ProjectileType<UnbreakableCombatUkulelePro2>(), (int) ((double) damage * 1.25), 3f, ((Entity) player).whoAmI, 0.0f, 0.0f, 0.0f); 
                }
            return false;
        }
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(Item.type);
            recipe.AddIngredient(ModContent.ItemType<ArmoredShell>(), 12);
            recipe.AddIngredient(ModContent.ItemType<Ukulele>(), 1);
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 8);
            recipe.AddIngredient(ModContent.ItemType<ExodiumCluster>(), 30);
            recipe.AddTile<CosmicAnvil>();
            recipe.Register();

        }
    }
}