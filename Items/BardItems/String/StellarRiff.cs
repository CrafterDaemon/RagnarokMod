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
    public class StellarRiff : RiffInstrumentBase
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.String;
        public override SoundStyle RiffSound => RagnarokModSounds.aureusriff;
        public override SoundStyle NormalSound => RagnarokModSounds.stellarriff;
        public override byte RiffType => RiffLoader.RiffType<AureusRiff>();

        public override void SetStaticDefaults(){
            Empowerments.AddInfo<Defense>(1, 0);
			Empowerments.AddInfo<JumpHeight>(3, 0);
        }

        public override void SetBardDefaults()
        {
            Item.damage = 105;
            InspirationCost = 1;
            Item.width = 58;
            Item.height = 66;
            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.Guitar;
            Item.holdStyle = 5;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.knockBack = 3f;
			Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Purple;
            Item.shoot = ModContent.ProjectileType<StellarRiffPro>();
            Item.shootSpeed = 14f;
        }
		
        public override Vector2? HoldoutOffset() => new Vector2(-10, 12);

        public override void HoldItemFrame(Player player)
        {
            player.itemLocation += new Vector2(-10, 12) * player.Directions;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation += new Vector2(-10, 12) * player.Directions;
        }
    }
}