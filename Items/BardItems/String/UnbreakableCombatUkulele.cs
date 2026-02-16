using ThoriumMod;
using ThoriumMod.Empowerments;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using Terraria.DataStructures;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod.Items.Materials;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Sounds;
using Microsoft.Xna.Framework;
using CalamityMod.Tiles.Furniture.CraftingStations;
using RagnarokMod.Projectiles.BardPro.String;
using ThoriumMod.Items;
using CalamityMod.Items.Placeables.Ores;

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
            Item.useStyle = ItemUseStyleID.Guitar;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.knockBack = 20f;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = ModContent.RarityType<CosmicPurple>();
            Item.UseSound = new SoundStyle?(ThoriumSounds.SuperGuitarNoise);
            Item.shoot = ModContent.ProjectileType<UnbreakableCombatUkulelePro1>();
            Item.shootSpeed = 20;

            ((ModItem)this).Item.holdStyle = 5;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 11);
        }

        public override void HoldItemFrame(Player player)
        {
            player.itemLocation += new Vector2(-10, 11f) * player.Directions;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            Vector2 offset = new Vector2(-10, 11f) * player.Directions;

            player.itemLocation += offset;
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
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X * 1.25f, velocity.Y * 1.25f, ModContent.ProjectileType<UnbreakableCombatUkulelePro2>(), (int)((double)damage * 1.25), 3f, ((Entity)player).whoAmI, 0.0f, 0.0f, 0.0f);
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