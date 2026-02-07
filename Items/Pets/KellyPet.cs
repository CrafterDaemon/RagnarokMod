using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RagnarokMod.Items.Pets{

	public class KellyPet : ModItem{
		public override void SetDefaults(){
				Item.CloneDefaults(ItemID.AmberMosquito);
				Item.shoot = ModContent.ProjectileType<KellyPetProjectile>();
				Item.buffType = ModContent.BuffType<KellyPetBuff>();
		}
	
		public override bool? UseItem(Player player){
			if (player.whoAmI == Main.myPlayer){
				player.AddBuff(Item.buffType, 3600);
			}
			return true;
		}
	
		public override void AddRecipes(){
				base.AddRecipes();
				CreateRecipe()
					.AddIngredient(ItemID.FallenStar)
					.AddIngredient(ItemID.SilverDye)
					.AddIngredient(ItemID.BlackDye).Register();
		}
	}
}