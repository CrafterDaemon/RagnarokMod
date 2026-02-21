using CalamityMod.Items;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using RagnarokMod.Projectiles.BardPro.Riff;
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
using ThoriumMod.Items;
using ThoriumMod.Items.BardItems;

namespace RagnarokMod.Items.BardItems.String
{
    public class DragonForce : RiffInstrumentBase
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.String;
        public override SoundStyle NormalSound => RagnarokModSounds.dragonforce;
        public override SoundStyle RiffSound => RagnarokModSounds.rotjdriff;
        public override byte RiffType => RiffLoader.RiffType<DragonRiff>();

        public override void SetStaticDefaults()
        {
            Empowerments.AddInfo<Damage>(6);
            Empowerments.AddInfo<AttackSpeed>(6);
        }

        public override void SetBardDefaults()
        {
            Item.damage = 333;
            InspirationCost = 0;
            Item.width = 86;
            Item.height = 92;
            Item.scale = 1f;
            Item.useTime = 6;
            Item.useAnimation = 6;
            Item.useStyle = ItemUseStyleID.Guitar;
            Item.noMelee = true;
            Item.autoReuse = false;
            Item.reuseDelay = 40;
            Item.knockBack = 20f;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = ModContent.RarityType<BurnishedAuric>();
            Item.shoot = ModContent.ProjectileType<DragonForcePro1>();
            Item.shootSpeed = 1f;

            ((ModItem)this).Item.holdStyle = 5;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-18, 20);
        }

        public override void HoldItemFrame(Player player)
        {
            player.itemLocation += new Vector2(-18, 20) * player.Directions;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            Vector2 offset = new Vector2(-18, 20) * player.Directions;
            player.itemLocation += offset;
        }

        public override bool SafeCanPlayInstrument(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<DragonForcePro1>()] <= 0;
        }

        public override void SafeRiffBardShoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.GetRagnarokModPlayer().activeRiffType == RiffLoader.RiffType<DragonRiff>())
                type = ModContent.ProjectileType<DragonForceRiffBeam>();

            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
        }
    }
}