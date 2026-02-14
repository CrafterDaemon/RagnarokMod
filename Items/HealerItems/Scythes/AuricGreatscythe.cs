using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Projectiles;
using RagnarokMod.Projectiles.HealerPro.Other;
using RagnarokMod.Projectiles.HealerPro.Scythes;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items.HealerItems;

namespace RagnarokMod.Items.HealerItems.Scythes
{
    public class AuricGreatscythe : ScytheItem, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            SetStaticDefaultsToScythe();
        }

        public override void SetDefaults()
        {
            SetDefaultsToScythe();
            base.Item.damage = 450;
            scytheSoulCharge = 5;
            base.Item.width = 52;
            base.Item.height = 48;
            base.Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            base.Item.rare = ModContent.RarityType<PureGreen>();
            base.Item.shoot = ModContent.ProjectileType<AuricGreatscythePro>();
            Item.reuseDelay = 8;
            Item.channel = true;
            Item.autoReuse = false;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override void HoldItem(Player player)
        {
            player.Calamity().rightClickListener = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<AuricGreatscythePro>();

            if (player.altFunctionUse == 2)
            {
            }
        }


        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            scale = 0.75f;
            var texture = TextureAssets.Item[Item.type].Value;
            float scaledHeight = texture.Height * scale;
            var position = Item.TopLeft - Main.screenPosition;
            position.Y += Item.height - scaledHeight;
            spriteBatch.Draw(texture, position, null, lightColor, rotation, Vector2.Zero, scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void AddRecipes()
        {
        }
    }
}
