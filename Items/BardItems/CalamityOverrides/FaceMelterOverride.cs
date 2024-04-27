using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Empowerments;
using ThoriumMod;
using ThoriumMod.Items;
using CalamityMod.Items;
using CalamityMod;
using Ragnarok.Projectiles;
using ThoriumMod.Items.BardItems;

namespace RagnarokMod.Items
{
    public class FaceMelterOverride : BardItem, ILocalizedModType
    {
        public override string Texture => "CalamityMod/Items/Weapons/Magic/FaceMelter";
        public override BardInstrumentType InstrumentType => BardInstrumentType.String;

        public override void SetStaticDefaults()
        {
            Empowerments.AddInfo<Damage>(4);
            Empowerments.AddInfo<AttackSpeed>(4);
        }
        public override void SetBardDefaults()
        {
            InspirationCost = 1;
            Item.damage = 180;
            Item.width = 56;
            Item.height = 50;
            Item.useTime = 6;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            

            Item.value = CalamityGlobalItem.Rarity14BuyPrice;
            Item.rare = ModContent.RarityType<DarkBlue>();
            Item.Calamity().donorItem = true;

            Item.shoot = ModContent.ProjectileType<MelterNote1>();
            Item.UseSound = SoundID.Item47;
            Item.autoReuse = true;
            Item.shootSpeed = 20f;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-15, 0);

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanPlayInstrument(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useTime = 20;
                Item.useAnimation = 20;
            }
            else
            {
                Item.useTime = 5;
                Item.useAnimation = 10;
            }
            return base.CanPlayInstrument(player);
        }

        public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<MelterAmpOverride>(), damage, knockback, player.whoAmI);
                return false;
            }
            else
            {
                int note = Main.rand.Next(2);
                if (note == 0)
                {
                    damage = (int)(damage * 1.5f);
                    type = ModContent.ProjectileType<MelterNote1>();
                }
                else
                {
                    velocity.X *= 1.5f;
                    velocity.Y *= 1.5f;
                    type = ModContent.ProjectileType<MelterNote2>();
                }
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                return false;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(1305).AddIngredient(4715).AddIngredient<CosmiliteBar>(8)
                .AddIngredient<NightmareFuel>(20)
                .AddTile<CosmicAnvil>()
                .Register();
        }
    }
}
