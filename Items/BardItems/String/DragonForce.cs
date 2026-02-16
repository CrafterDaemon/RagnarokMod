using ThoriumMod;
using ThoriumMod.Empowerments;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using CalamityMod.Items;
using CalamityMod.Rarities;
using ThoriumMod.Items.BardItems;
using Microsoft.Xna.Framework;
using RagnarokMod.Projectiles.BardPro.String;
using ThoriumMod.Items;
using Terraria.DataStructures;


namespace RagnarokMod.Items.BardItems.String
{
    public class DragonForce : BardItem
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.String;
        public override void SetStaticDefaults()
        {
            Empowerments.AddInfo<Damage>(6);
            Empowerments.AddInfo<AttackSpeed>(6);
        }

        public override void SetBardDefaults()
        {
            Item.damage = 275;
            InspirationCost = 1;
            Item.width = 86;
            Item.height = 92;
            Item.scale = 1f;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Guitar;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.knockBack = 20f;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = ModContent.RarityType<BurnishedAuric>();
            Item.UseSound = new SoundStyle?(SoundID.Item47);
            Item.shoot = ModContent.ProjectileType<DragonForcePro1>();
            Item.shootSpeed = 20;

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

        public override bool BardShoot(
            Player player,
            EntitySource_ItemUse_WithAmmo source,
            Vector2 position,
            Vector2 velocity,
            int type,
            int damage,
            float knockback)
        {
            return true;
        }
    }
}