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
using ThoriumMod.Items.ArcaneArmor;
using CalamityMod.Rarities;
using ThoriumMod.Items.Donate;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items;

namespace RagnarokMod.Items.HealerItems.Other
{
    public class CosmicInjector : ThoriumItem
    {
        private int counter = 0;
        public override void SetDefaults()
        {
            this.Item.DamageType = (DamageClass)ThoriumDamageBase<HealerDamage>.Instance;
            this.Item.damage = 180;
            this.healType = HealType.Ally;
            this.healAmount = 3;
            this.healDisplay = true;
            this.isHealer = true;
            this.Item.width = 82;
            this.Item.height = 34;
            this.Item.useTime = 7;
            this.Item.useAnimation = 7;
            this.Item.useStyle = ItemUseStyleID.Shoot;
            this.Item.noMelee = true;
            this.Item.knockBack = 4f;
            this.Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            this.Item.rare = ModContent.RarityType<CosmicPurple>();
            this.Item.UseSound = new SoundStyle?(SoundID.Item17);
            this.Item.autoReuse = true;
            this.Item.shoot = ModContent.ProjectileType<CosmicInjectorPro1>();
            this.Item.shootSpeed = 20f;
            this.Item.useAmmo = ModContent.ItemType<Syringe>();
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.Next(0, 100) < 50)
                return false;
            return true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 8);
            recipe.AddIngredient(ModContent.ItemType<LethalInjection>(), 1);
            recipe.AddTile<CosmicAnvil>();
            recipe.Register();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (counter == 2)
            {
                Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<CosmicInjectorPro2>(), damage, knockback, player.whoAmI);
                counter = 0;
            }
            else
            {
                Projectile.NewProjectileDirect(source, position, velocity, this.Item.shoot, damage, knockback, player.whoAmI);
            }
            counter += 1;

            float randAngle = Main.rand.NextFloat(0.06f);
            Vector2 ccwVelocity = velocity.RotatedBy(-randAngle);
            Vector2 cwVelocity = velocity.RotatedBy(randAngle);
            Projectile.NewProjectile(source, position, ccwVelocity, this.Item.shoot, damage, knockback, player.whoAmI, 0f, 0f);
            Projectile.NewProjectile(source, position, cwVelocity, this.Item.shoot, damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
