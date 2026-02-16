using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using ThoriumMod.Items.Misc;
using ThoriumMod.Rarities;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Items.BossMini;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Projectiles.Healer;
using ThoriumMod.Utilities;
using CalamityMod.Items.Materials;
using RagnarokMod.Projectiles.HealerPro.Other;
using CalamityMod.Items.Placeables;
using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Items;
using CalamityMod.Items.Placeables.SunkenSea;

namespace RagnarokMod.Items.HealerItems.Other
{
    public class Prisma : ThoriumItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Type] = true;
        }


        public override void SetDefaults()
        {
            base.Item.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            base.Item.damage = 8;
            this.isHealer = true;
            this.healDisplay = true;
            base.Item.width = 48;
            base.Item.mana = 6;
            base.Item.height = 50;
            base.Item.useTime = 24;
            base.Item.useAnimation = 24;
            base.Item.useStyle = ItemUseStyleID.Shoot;
            base.Item.noMelee = true;
            base.Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
            base.Item.rare = ItemRarityID.Green;
            base.Item.UseSound = new SoundStyle?(SoundID.Item8);
            base.Item.shoot = ModContent.ProjectileType<PrismaPro1>();
            base.Item.shootSpeed = 12f;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<PearlShard>(), 8);
            recipe.AddIngredient(ModContent.ItemType<SeaPrism>(), 10);
            recipe.AddIngredient(ModContent.ItemType<Navystone>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
