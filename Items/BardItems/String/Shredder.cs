using CalamityMod.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Projectiles.BardPro.String;
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

namespace RagnarokMod.Items.BardItems.String
{
    public class Shredder : BigRiffInstrumentBase
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.String;
        public override SoundStyle RiffSound => RagnarokModSounds.shredderriff;
        public override SoundStyle NormalSound => RagnarokModSounds.shredder;
        public override byte RiffType => RiffLoader.RiffType<ShredderRiff>();

        public override void SafeSetStaticDefaults()
        {
            Empowerments.AddInfo<Defense>(2, 0);
            Empowerments.AddInfo<FlatDamage>(2, 0);
        }

        public override void SafeSetBardDefaults()
        {
            Item.damage = 40;
            InspirationCost = 1;
            Item.width = 88;
            Item.height = 88;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Guitar;
            Item.holdStyle = 5;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.knockBack = 3f;
            Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
            Item.rare = ItemRarityID.Green;
            Item.shoot = ModContent.ProjectileType<ShredderPro>();
            Item.shootSpeed = 11f;
        }
        public override void SafeRiffBardShoot(int success, int level, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var ragnarokPlayer = player.GetRagnarokModPlayer();

            Vector2 forwardOffset = Vector2.Normalize(velocity) * 1f;
            Vector2 downwardOffset = new Vector2(0f, 14f);

            Vector2 spawnPos = player.MountedCenter + forwardOffset + downwardOffset;

            if (ragnarokPlayer.fretPlaying)
            {
                float spreadAngle = MathHelper.ToRadians(8f);

                // Center
                Projectile.NewProjectile(source, spawnPos, velocity, type, damage, knockback, player.whoAmI);

                // Left
                Projectile.NewProjectile(
                    source,
                    spawnPos,
                    velocity.RotatedBy(-spreadAngle),
                    type,
                    damage,
                    knockback,
                    player.whoAmI);

                // Right
                Projectile.NewProjectile(
                    source,
                    spawnPos,
                    velocity.RotatedBy(spreadAngle),
                    type,
                    damage,
                    knockback,
                    player.whoAmI);
            }
            else
            {
                Projectile.NewProjectile(source, spawnPos, velocity, type, damage, knockback, player.whoAmI);
            }
        }

        public override Vector2? HoldoutOffset() => new Vector2(-18, 20);

        public override void HoldItemFrame(Player player)
        {
            player.itemLocation += new Vector2(-18, 20) * player.Directions;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation += new Vector2(-18, 20) * player.Directions;
        }
    }
}