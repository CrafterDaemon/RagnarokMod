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
    public class ScourgesFrets : BigRiffInstrumentBase
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.String;
        public override SoundStyle RiffSound => RagnarokModSounds.fretsriff;
        public override SoundStyle NormalSound => RagnarokModSounds.scourgesfrets;
        public override byte RiffType => RiffLoader.RiffType<ScourgeRiff>();

        public override void SafeSetStaticDefaults()
        {
            Empowerments.AddInfo<LifeRegeneration>(1, 0);
        }

        public override void SafeSetBardDefaults()
        {
            Item.damage = 21;
            InspirationCost = 1;
            Item.width = 88;
            Item.height = 88;
            Item.useTime = 23;
            Item.useAnimation = 23;
            Item.useStyle = ItemUseStyleID.Guitar;
            Item.holdStyle = 5;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.knockBack = 3f;
            Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
            Item.rare = ItemRarityID.Green;
            Item.shoot = ModContent.ProjectileType<ScourgesFretsPro>();
            Item.shootSpeed = 11f;
        }
        public override void ModifyInstrumentDamage(ThoriumPlayer bard, ref StatModifier damage)
        {
            Player player = bard.Player;
            if (player.GetRagnarokModPlayer().fretPlaying)
                damage += 0.25f;
        }
        public override void SafeRiffBardShoot(int success, int level, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
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