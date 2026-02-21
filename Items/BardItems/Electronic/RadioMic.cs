using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Projectiles.BardPro.Electronic;
using RagnarokMod.Sounds;
using RagnarokMod.Utils;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Items.Donate;
using ThoriumMod.Projectiles.Bard;
using ThoriumMod.Utilities;

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
            Item.damage = 485;
            InspirationCost = 2;
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
        public override void ModifyInspirationCost(Player player, ref int cost)
        {
            if (player.GetRagnarokModPlayer().redglassMonocle)
                cost = 2;
            else
                cost = 4;
        }

        public override void ModifyEmpowerment(
            ThoriumPlayer player,
            ThoriumPlayer target,
            byte type,
            ref byte level,
            ref short duration)
        {
            if (player.Player.GetRagnarokModPlayer().redglassMonocle)
            {
                if (type == EmpowermentLoader.EmpowermentType<Damage>())
                    level += 1;
            }
        }
        public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numberProjectiles = 5;
            float rotation = MathHelper.ToRadians(12);
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed =
                    velocity.RotatedBy(
                        MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1f))
                    );

                int proj = Projectile.NewProjectile(
                    source,
                    position,
                    perturbedSpeed,
                    ModContent.ProjectileType<RadioMicPro>(),
                    damage,
                    knockback,
                    player.whoAmI
                );

                if (i == 2)
                {
                    Main.projectile[proj].ai[0] = 1f;
                }
            }

            return false;
        }


        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(Item.type);
            recipe.AddIngredient(ModContent.ItemType<IdolsMicrophone>(), 1);
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 2);
            recipe.AddIngredient(ModContent.ItemType<DarkPlasma>(), 1);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}