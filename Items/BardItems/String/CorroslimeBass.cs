using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Empowerments;
using ThoriumMod.Items.BardItems;

using CalamityMod.Items;

using RagnarokMod.Projectiles.BardPro.String;
using RagnarokMod.Sounds;
using RagnarokMod.Utils;

namespace RagnarokMod.Items.BardItems.String
{
    public class CorroslimeBass : BardItem, ILocalizedModType
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.String;
        public override void SetStaticDefaults(){
            Empowerments.AddInfo<LifeRegeneration>(2, 0);
            Empowerments.AddInfo<InvincibilityFrames>(1, 0);
        }
		
		public override LocalizedText Tooltip{
			get{
				return base.Tooltip.WithFormatArgs(new object[]{
					CorroslimeBassPro.ProjperSplit,
					CorroslimeBassPro.MaxSplits
				});
			}
		}

        public override void SetBardDefaults(){
            Item.damage = 50;
            InspirationCost = 1;
            Item.width = 60;
            Item.height = 60;
            Item.scale = 1f;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Guitar;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.knockBack = 4f;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;
            Item.rare = 4;
            Item.UseSound = RagnarokModSounds.corroslimebass;
            Item.shoot = ModContent.ProjectileType<CorroslimeBassPro>();
            Item.shootSpeed = 20;
            ((ModItem)this).Item.holdStyle = 5;
        }
		public override bool AltFunctionUse(Player player) => true;
		public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback){
            if (player.altFunctionUse == 2){
                List<int> toSplit = new List<int>();
				for (int i = 0; i < Main.maxProjectiles; i++){
					Projectile proj = Main.projectile[i];
					if (proj.active && proj.owner == player.whoAmI && proj.type == type && proj.ModProjectile is CorroslimeBassPro modProj && modProj.SplitCount < CorroslimeBassPro.MaxSplits){
						toSplit.Add(i);
					}
				}
				foreach (int index in toSplit){
					Projectile proj = Main.projectile[index];
					if (proj.active && proj.ModProjectile is CorroslimeBassPro modProj){
						modProj.SplitProjectile();
					}
				}
                return false;
            }
            else{
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<CorroslimeBassPro>(), damage, knockback, player.whoAmI);
                return false;
            }
        }
        public override Vector2? HoldoutOffset(){
            return new Vector2(-15, 16);
        }

        public override void HoldItemFrame(Player player){
            player.itemLocation += new Vector2(-15, 16f) * player.Directions;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame){
            Vector2 offset = new Vector2(-15, 16f) * player.Directions;
            player.itemLocation += offset;
        }
    }
}