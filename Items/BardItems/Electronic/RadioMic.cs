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
using ThoriumMod.Items.Donate;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using RagnarokMod.Projectiles.BardPro.Electronic;

namespace RagnarokMod.Items.BardItems.Electronic
{
    public class RadioMic : BardItem
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.Electronic;

        public override void SetStaticDefaults()
        {
            Empowerments.AddInfo<EmpowermentProlongation>(4, 0);
            Empowerments.AddInfo<Damage>(3, 0);
        }

        public override void SetBardDefaults()
        {
            Item.damage = 120;
            InspirationCost = 4;
            Item.width = 32;
            Item.height = 32;
            Item.scale = 0.6f;
            Item.useTime = 48;
            Item.useAnimation = 48;
            Item.useStyle = ItemUseStyleID.RaiseLamp;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.knockBack = 20f;
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.UseSound = RagnarokModSounds.RadioDemon;
            Item.shoot = ModContent.ProjectileType<TendrilStrike>();
            Item.shootSpeed = 10f;

        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(Item.type);
            recipe.AddIngredient(ModContent.ItemType<IdolsMicrophone>(), 1);
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 2);
            recipe.AddIngredient(ModContent.ItemType<DarkPlasma>(), 1);
            recipe.AddTile(412);
            recipe.Register();
        }
    }
}