using CalamityMod;
using CalamityMod.Items;
using Microsoft.Xna.Framework;
using RagnarokMod.Projectiles.BardPro.Wind;
using RagnarokMod.Riffs;
using RagnarokMod.Riffs.RiffTypes;
using RagnarokMod.Sounds;
using RagnarokMod.Utils;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Empowerments;
using ThoriumMod.Items.BardItems;

namespace RagnarokMod.Items.BardItems.Wind
{
    public class InfectedOcarina : RiffInstrumentBase
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.Wind;
        public override SoundStyle RiffSound => RagnarokModSounds.HiveMindRiff;
        public override SoundStyle NormalSound => SoundID.Item42;
        public override byte RiffType => RiffLoader.RiffType<HiveMindRiff>();
        public override void SetStaticDefaults()
        {
            Empowerments.AddInfo<ResourceRegen>(2);
            Empowerments.AddInfo<ResourceMaximum>(1);
        }
        public override void SetBardDefaults()
        {
            Item.damage = 24;
            InspirationCost = 1;
            Item.width = 36;
            Item.height = 26;
            Item.scale = 0.9f;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.holdStyle = 3;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item42;
            Item.knockBack = 4f;
            Item.shoot = ModContent.ProjectileType<InfectedOcarinaPro>();
            Item.shootSpeed = 12f;
            Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
            Item.rare = ItemRarityID.Orange;
        }
        public override void SafeRiffBardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.rand.NextBool(6))
            {
                Vector2 velocityOffset = velocity.RotatedByRandom(MathHelper.ToRadians(5));
                Projectile.NewProjectile(source, position, velocityOffset, ModContent.ProjectileType<VileSpitFriendly>(), damage / 2, knockback, player.whoAmI, 1f);
            }
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 vector = Vector2.Normalize(velocity) * 25f;
            if (MiscHelper.CanHitLine(position, position + vector))
            {
                position += vector;
            }
            if (player.GetRagnarokModPlayer().activeRiffType == RiffLoader.RiffType<HiveMindRiff>())
            {
                Item.useTime = 12;
                Item.useAnimation = 12;
            }
            else
            {
                Item.useTime = 16;
                Item.useAnimation = 16;
            }
        }
    }
}