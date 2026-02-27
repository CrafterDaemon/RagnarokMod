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

using RagnarokMod.Projectiles.BardPro.Wind;
using RagnarokMod.Sounds;
using RagnarokMod.Utils;
using RagnarokMod.Common.Configs;

namespace RagnarokMod.Items.BardItems.Wind
{
    public class CrimslimeOboe : BardItem, ILocalizedModType
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.Wind;
        public override void SetStaticDefaults(){
            Empowerments.AddInfo<ResourceMaximum>(2, 0);
            Empowerments.AddInfo<ResourceConsumptionChance>(1, 0);
        }
		
		public override LocalizedText Tooltip{
			get{
				return base.Tooltip.WithFormatArgs(new object[]{
					CrimslimeOboePro.ExplodeDelay
				});
			}
		}
		
        public override void SetBardDefaults(){
            Item.damage = 50;
            InspirationCost = 1;
            Item.width = 72;
            Item.height = 16;
            Item.scale = 1f;
            Item.useTime = 23;
            Item.useAnimation = 23;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.knockBack = 4f;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;
            Item.rare = 4;
            Item.UseSound = RagnarokModSounds.crimslimeoboe;
            Item.shoot = ModContent.ProjectileType<CrimslimeOboePro>();
            Item.shootSpeed = 17;
            Item.holdStyle = 3;
        }
	
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback){
			Vector2 forward = Vector2.Normalize(velocity);
			Vector2 perpendicular = forward.RotatedBy(MathHelper.PiOver2);
			position += forward * 75f;
			position += perpendicular * ModContent.GetInstance<DebugConfig>().ShootOffsetPerpendicular;
		}
		
        public override Vector2? HoldoutOffset(){
            return new Vector2(7f, -4f);
        }
    }
}