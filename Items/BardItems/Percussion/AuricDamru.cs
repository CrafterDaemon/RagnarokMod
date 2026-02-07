using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using ThoriumMod.Empowerments;
using ThoriumMod.Projectiles.Bard;
using ThoriumMod.Sounds;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Items;
using ThoriumMod;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items;
using CalamityMod;
using RagnarokMod.Projectiles.BardPro.Percussion;

namespace RagnarokMod.Items.BardItems.Percussion
{
    public class AuricDamru : BardItem
    {
        public override BardInstrumentType InstrumentType
        {
            get
            {
                return BardInstrumentType.Percussion;
            }
        }

        public override void SetStaticDefaults()
        {
            Empowerments.AddInfo<AttackSpeed>(3, 0);
            Empowerments.AddInfo<LifeRegeneration>(3, 0);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2?(new Vector2(0f, 10f));
        }

        public override void SetBardDefaults()
        {
            Item.damage = 1950;
            InspirationCost = 2;
            Item.width = 60;
            Item.height = 60;
            Item.scale = 0.7f;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = 5;
            Item.holdStyle = 3;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.knockBack = 0.5f;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = ModContent.RarityType<BurnishedAuric>();
            Item.UseSound = new SoundStyle?(SoundID.Item1);
            Item.UseSound = new SoundStyle?(ThoriumSounds.Bongo);
            Item.shoot = ModContent.ProjectileType<AuricDamruShock>();
            Item.shootSpeed = 0f;
            Item.autoReuse = true;
            Item.Calamity().donorItem = true;
        }

        public override bool CanPlayInstrument(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.shootSpeed = 13f;
            }
            else
            {
                Item.shootSpeed = 0f;
            }
            return base.CanPlayInstrument(player);
        }


        public override bool AltFunctionUse(Player player) => true;

        public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<AuricDamruFlareBomb>(), (int)(damage * 0.85f), knockback, player.whoAmI);
                return false;
            }
            else
            {
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<AuricDamruShock>(), damage, knockback, player.whoAmI);
                return false;
            }
        }

        public override void OnPlayInstrument(Player player)
        {
            int num = 50;
            for (int i = 0; i < num; i++)
            {
                Vector2 vector = (-Vector2.UnitY.RotatedBy((double)(i * 6.2831855f / num), default)).RotatedBy((double)player.velocity.ToRotation(), default);
                Dust dust = Dust.NewDustPerfect(player.Center + vector * 15f, 127, new Vector2?(vector * 4f), 0, default, 1f);
                dust.scale = 1.35f;
                dust.noGravity = true;
            }
        }

        // Token: 0x060068D0 RID: 26832 RVA: 0x002CFE2D File Offset: 0x002CE02D
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<AuricBar>(), 5);
            recipe.AddIngredient(ModContent.ItemType<Bongos>(), 1);
            recipe.AddTile<CosmicAnvil>();
            recipe.Register();
        }
    }
}
