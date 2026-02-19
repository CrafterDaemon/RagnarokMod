using CalamityMod.Items;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using RagnarokMod.Projectiles.BardPro.Percussion;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod.Empowerments;
using ThoriumMod.Items.BardItems;

namespace RagnarokMod.Items.BardItems.Percussion
{
    public class Ectambourine : ShadeWoodTambourine
    {
        public override void SetStaticDefaults()
        {
            Empowerments.AddInfo<MovementSpeed>(4);
            Empowerments.AddInfo<JumpHeight>(3);
            Empowerments.AddInfo<FlightTime>(4);
        }
        public override void SetBardDefaults()
        {
            base.SetBardDefaults();
            Item.damage = 320;
            InspirationCost = 2;
            Item.width = 38;
            Item.height = 40;
            Item.knockBack = 5f;
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.shoot = ModContent.ProjectileType<EctambourinePro>();
            Item.shootSpeed = 15f;
        }
        public override bool AltFunctionUse(Player player) => player.ownedProjectileCounts[Item.shoot] > 0;
        public override void ModifyInspirationCost(Player player, ref int cost) => cost = player.altFunctionUse == 2 ? 0 : 1;
        public override bool CanPlayInstrument(Player player) => true;
        public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Code sampled from Thorium's Shadewood Tambourine, as there is not much of a reason to remake code that does the same thing
            if (player.altFunctionUse == 2)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile projectile = Main.projectile[i];
                    
                    if (projectile.active && projectile.owner == player.whoAmI && projectile.type == Item.shoot)
                    {
                        Projectile.NewProjectile(source, projectile.Center, Vector2.Zero, ModContent.ProjectileType<EctambourineProJingle>(), damage, 0f, player.whoAmI);
                    }
                }
                return false;
            }
            return true;
        }
        public override void AddRecipes() { }
    }
}