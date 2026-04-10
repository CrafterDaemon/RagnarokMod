using CalamityMod.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
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
        public Player myPlayer = Main.LocalPlayer;
        public RagnarokModPlayer myRagnaPlayer => Main.LocalPlayer != null ? Main.LocalPlayer.GetRagnarokModPlayer() : null;
        public bool riffin => myRagnaPlayer != null ? (myRagnaPlayer.activeRiffType == RiffType ? true : false) : false;
        public override float DamageDecreaseOnFail => riffin ? 0f : 0.4f;
        public override int SuccessDecreaseOnFail => riffin ? 0 : 3;
        public override float DamageIncreasePerSuccess => 0.025f;

        public override void SafeSetStaticDefaults()
        {
            Empowerments.AddInfo<Defense>(2, 0);
            Empowerments.AddInfo<FlatDamage>(2, 0);
        }

        public override void SafeSetBardDefaults()
        {
            Item.damage = 58;
            InspirationCost = 1;
            Item.width = 88;
            Item.height = 88;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Guitar;
            Item.holdStyle = 5;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.knockBack = 3f;
            Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
            Item.rare = ItemRarityID.Green;
            Item.shoot = ModContent.ProjectileType<ShredderPro>();
            Item.shootSpeed = 16f;
        }
        public override void SafeRiffBardShoot(int success, int level, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile newProj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            newProj.ai[0] = Main.MouseWorld.X;
            newProj.ai[1] = Main.MouseWorld.Y;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {

            if (riffin)
            {
                Item.useTime = 11;
                Item.useAnimation = 11;
            }
            else
            {
                Item.useTime = 22;
                Item.useAnimation = 22;
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