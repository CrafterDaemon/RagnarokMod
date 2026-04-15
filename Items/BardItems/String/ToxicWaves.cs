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
    public class ToxicWaves : BigRiffInstrumentBase
    {
        public string TextureBase => "RagnarokMod/Items/BardItems/String/ToxicWaves";
        public string TextureAlt => "RagnarokMod/Items/BardItems/String/ToxicWavesAlt";
        public override BardInstrumentType InstrumentType => BardInstrumentType.String;
        public override SoundStyle RiffSound => RagnarokModSounds.ToxicWisdomRiff;
        public override SoundStyle NormalSound => RagnarokModSounds.toxicwaves;
        public override byte RiffType => RiffLoader.RiffType<ToxicWavesRiff>();
        public Player myPlayer = Main.LocalPlayer;
        public RagnarokModPlayer myRagnaPlayer => Main.LocalPlayer != null ? Main.LocalPlayer.GetRagnarokModPlayer() : null;
        public bool riffin => myRagnaPlayer != null ? (myRagnaPlayer.activeRiffType == RiffType ? true : false) : false;
        public override int SuccessDecreaseOnFail => riffin ? 0 : default;
        public override float DamageIncreasePerSuccess => riffin ? 0.025f : default;

        public override void SafeSetStaticDefaults()
        {
            Empowerments.AddInfo<Defense>(2, 0);
            Empowerments.AddInfo<LifeRegeneration>(2, 0);
            Empowerments.AddInfo<AquaticAbility>(2, 0);
        }

        public override void SafeSetBardDefaults()
        {
            Item.damage = 625;
            InspirationCost = 1;
            Item.width = 92;
            Item.height = 90;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.Guitar;
            Item.holdStyle = 5;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.knockBack = 4f;
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.rare = ItemRarityID.Cyan;
            Item.shoot = ModContent.ProjectileType<ToxicWavesPro>();
            Item.shootSpeed = 32f;
        }

        public override void ModifyRiffInstDamage(ThoriumPlayer bard, ref StatModifier damage)
        {
            Player player = bard.Player;
            if (player.GetRagnarokModPlayer().activeRiffType == RiffType)
                damage -= 0.1f;
        }

        public override void SafeRiffBardShoot(int success, int level, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.GetRagnarokModPlayer().activeRiffType == RiffType)
            {
                float spread = MathHelper.ToRadians(8f);
                for (int i = -2; i <= 2; i++)
                {
                    Vector2 dir = velocity.RotatedBy(spread * i);
                    Projectile.NewProjectile(source, position, dir, type, damage, knockback, player.whoAmI);
                }
            }
            else
            {
                float spread = MathHelper.ToRadians(15f);
                for (int i = -1; i <= 1; i++)
                {
                    Vector2 dir = velocity.RotatedBy(spread * i);
                    Projectile.NewProjectile(source, position, dir, type, damage, knockback, player.whoAmI);
                }
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
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame,
            Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D tex = ModContent.Request<Texture2D>(riffin ? TextureAlt : TextureBase).Value;
            Rectangle texFrame = tex.Bounds;
            Vector2 texOrigin = texFrame.Size() / 2f;
            spriteBatch.Draw(tex, position, texFrame, drawColor, 0f, texOrigin, scale, SpriteEffects.None, 0f);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor,
            ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D tex = ModContent.Request<Texture2D>(riffin ? TextureAlt : TextureBase).Value;
            spriteBatch.Draw(tex, Item.Center - Main.screenPosition, null, lightColor,
                rotation, tex.Size() / 2f, scale, SpriteEffects.None, 0f);
            return false;
        }
    }
    public class ToxicWavesDrawLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() =>
            new AfterParent(PlayerDrawLayers.HeldItem);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            return player.HeldItem?.type == ModContent.ItemType<ToxicWaves>()
                && player.GetRagnarokModPlayer().activeRiffType == RiffLoader.RiffType<ToxicWavesRiff>();
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;
            Texture2D tex = ModContent.Request<Texture2D>("RagnarokMod/Items/BardItems/String/ToxicWavesAlt").Value;

            // Match how Terraria positions held items
            Vector2 drawPos = drawInfo.ItemLocation - Main.screenPosition;
            Vector2 origin = player.direction == -1
                ? new Vector2(tex.Width, tex.Height)
                : new Vector2(0, tex.Height);
            DrawData dd = new DrawData(
                tex,
                drawPos,
                tex.Bounds,
                drawInfo.colorArmorBody,
                player.itemRotation,
                origin,
                player.HeldItem.scale,
                player.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0
            );
            drawInfo.DrawDataCache.Add(dd);
        }
    }
}
