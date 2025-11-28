using CalamityMod.Items;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Rarities;
using CalamityMod;
using CalamityMod.Items.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using ThoriumMod.Items;
using Microsoft.Xna.Framework;
using ThoriumMod;
using Terraria.Graphics.CameraModifiers;
using ThoriumMod.Utilities;
using RagnarokMod.Projectiles.HealerPro.Other;
using RagnarokMod.Projectiles;

namespace RagnarokMod.Items.HealerItems.Other
{
    public class Splattercannon : ThoriumItem
    {
        public static readonly SoundStyle clickclick = SoundID.Item149;
        public static readonly SoundStyle pow = SoundID.Item38;

        public int NumProjectiles = 0;
        public override void SetDefaults()
        {
            Item.damage = 85;
            Item.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Item.width = 60;
            Item.height = 40;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.autoReuse = true;
            Item.reuseDelay = 10;
            isHealer = true;
            healDisplay = true;
            Item.holdStyle = ItemHoldStyleID.HoldFront;
            Item.UseSound = pow;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.shootSpeed = 5f;
            Item.shoot = ModContent.ProjectileType<Splattershot>();
            radiantLifeCost = 8;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-5, 0);

        public override bool AltFunctionUse(Player player) => true;
        public override void HoldItem(Player player)
        {
            player.Calamity().mouseWorldListener = true;
            player.GetThoriumPlayer().healBonus = (int)(player.GetThoriumPlayer().healBonus * 0.5);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                if (NumProjectiles < 24)
                {
                    if (player.statLife <= 0)
                    {
                        PlayerDeathReason pdr = PlayerDeathReason.ByCustomReason(player.name.ToString() + " was converted into ammunition.");
                        player.KillMe(pdr, 1000.0, 0, false);
                        return false;
                    }
                    else
                    {
                        NumProjectiles += 4;
                        return false;
                    }
                }
                return false;
            }
            else
            {
                PunchCameraModifier modifier = new PunchCameraModifier(player.Center, new Vector2(-player.direction), 15f, 12f, 20);
                Main.instance.CameraModifiers.Add(modifier);

                for (int i = 0; i < NumProjectiles; i++)
                {
                    Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(5));
                    newVelocity *= 4f - Main.rand.NextFloat(0.3f);
                    Projectile.NewProjectileDirect(source, position, newVelocity, type, damage, knockback, player.whoAmI);
                }
                if (player.statLife <= 0)
                {
                    PlayerDeathReason pdr = PlayerDeathReason.ByCustomReason(player.name.ToString() + " could not handle the recoil.");
                    player.KillMe(pdr, 1000.0, 0, false);
                    return false;
                }
                NumProjectiles = 0;
                return false;
            }
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position -= Vector2.UnitY * 12f;
            type = ModContent.ProjectileType<Splattershot>();

            if (player.altFunctionUse == 2)
            {
                damage = 0;
                type = ModContent.ProjectileType<NoProj>();
                velocity = Vector2.Zero;
            }
        }
        public override float UseSpeedMultiplier(Player player)
        {
            if (player.altFunctionUse == 2)
                return 2f;
            return 1f;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                if (NumProjectiles >= 24) { return false; } else { return true; }
            }
            else
            {
                if (NumProjectiles == 0) { return false; } else { return true; }
            }
        }

        public override void UseAnimation(Player player)
        {
            Item.UseSound = pow;
            if (player.altFunctionUse == 2)
                Item.UseSound = clickclick;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            player.ChangeDir(Math.Sign((player.Calamity().mouseWorld - player.Center).X));
            float itemRotation = player.compositeFrontArm.rotation + MathHelper.PiOver2 * player.gravDir;
            Vector2 itemSize = new Vector2(60, 40);
            Vector2 itemOrigin = new Vector2(0, 5);
            Vector2 itemPosition = player.MountedCenter + (itemRotation.ToRotationVector2() * 40);

            CalamityUtils.CleanHoldStyle(player, itemRotation, itemPosition, itemSize, itemOrigin);
        }

        public override void HoldStyle(Player player, Rectangle heldItemFrame)
        {
            float itemRotation = player.compositeFrontArm.rotation + MathHelper.PiOver2 * player.gravDir;
            Vector2 itemSize = new Vector2(60, 40);
            Vector2 itemOrigin = new Vector2(0, 5);
            Vector2 itemPosition = player.MountedCenter + (itemRotation.ToRotationVector2() * 40);

            CalamityUtils.CleanHoldStyle(player, itemRotation, itemPosition, itemSize, itemOrigin);
        }

        public override void UseItemFrame(Player player)
        {
            player.ChangeDir(Math.Sign((player.Calamity().mouseWorld - player.Center).X));

            float animProgress = 1 - player.itemTime / (float)player.itemTimeMax;
            float rotation = (player.Center - player.Calamity().mouseWorld).ToRotation() * player.gravDir + MathHelper.PiOver2;
            if (animProgress < 0.4f)
                rotation += -0.45f * (float)Math.Pow((0.4f - animProgress) / 0.4f, 2) * player.direction;

            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rotation);
        }

        public override void HoldItemFrame(Player player)
        {

            float animProgress = 1 - player.itemTime / (float)player.itemTimeMax;
            float rotation = (player.Center - player.Calamity().mouseWorld).ToRotation() * player.gravDir + MathHelper.PiOver2;
            if (animProgress < 0.4f)
                rotation += -0.45f * (float)Math.Pow((0.4f - animProgress) / 0.4f, 2) * player.direction;

            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rotation);
        }
		
		public override void AddRecipes()
		{
			base.CreateRecipe(1)
				.AddIngredient<BloodstoneCore>(10)
				.AddIngredient<RuinousSoul>(8)
				.AddIngredient<CalamityMod.Items.Materials.BloodOrb>(100)
				.AddTile(412)
				.Register();
		}
    }
}
