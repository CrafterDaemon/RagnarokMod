using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod.Items.Materials;

using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Empowerments;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Sounds;

using CalamityMod.Tiles.Furniture.CraftingStations;
using RagnarokMod.Projectiles.BardPro.String;

namespace RagnarokMod.Items.BardItems.String
{
    public class WulfrumGuitar : BardItem{
        public override BardInstrumentType InstrumentType => BardInstrumentType.String;
        public override void SetStaticDefaults(){
            Empowerments.AddInfo<DamageReduction>(1, 0);
        }

        public override void SetBardDefaults(){
            base.Item.damage = 15;
			base.InspirationCost = 2;
			base.Item.width = 30;
			base.Item.height = 30;
			base.Item.useTime = 22;
			base.Item.useAnimation = 22;
			base.Item.autoReuse = true;
			base.Item.useStyle = 5;
			base.Item.noMelee = true;
			base.Item.knockBack = 4f;
			Item.useStyle = ItemUseStyleID.Guitar;
			base.Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
			base.Item.rare = 1;
			base.Item.UseSound = new SoundStyle?(ThoriumSounds.String_Sound);
			base.Item.shoot = ModContent.ProjectileType<WulfrumGuitarPro>();
			base.Item.shootSpeed = 6f;
			((ModItem)this).Item.holdStyle = 5;
        }

        public override bool AltFunctionUse(Player player) => true;
        public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback){
            if (player.altFunctionUse == 2){
                List<int> toReturn = new List<int>();
				for (int i = 0; i < Main.maxProjectiles; i++){
					Projectile proj = Main.projectile[i];
					if (proj.active && proj.owner == player.whoAmI && proj.type == type && proj.ModProjectile is WulfrumGuitarPro modProj && modProj.returning == false){
						toReturn.Add(i);
					}
				}
				foreach (int index in toReturn){
					Projectile proj = Main.projectile[index];
					if (proj.active && proj.ModProjectile is WulfrumGuitarPro modProj){
						modProj.ReturnProjectile();
					}
				}
                return false;
            }
            else{
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<WulfrumGuitarPro>(), damage, knockback, player.whoAmI);
                return false;
            }
        }
		
		public override float UseTimeMultiplier(Player player){
            return player.altFunctionUse == 2 ? 0.5f : 1f;
        }

        public override float UseAnimationMultiplier(Player player){
            return player.altFunctionUse == 2 ? 0.5f : 1f;
        }
		
        public override void AddRecipes(){
           base.CreateRecipe(1).AddIngredient<WulfrumMetalScrap>(10).AddTile(16).Register();
        }
    }
}