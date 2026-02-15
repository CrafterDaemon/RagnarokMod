using CalamityMod.Items;
using CalamityMod.NPCs.TownNPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RagnarokMod.Items.Materials
{
    public class VoidseerPearl : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;
            Item.rare = ItemRarityID.LightRed;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            scale = 0.5f;
            var texture = TextureAssets.Item[Item.type].Value;
            float scaledHeight = texture.Height * scale;
            var position = Item.TopLeft - Main.screenPosition;
            position.Y += Item.height - scaledHeight;
            spriteBatch.Draw(texture, position, null, lightColor, rotation, Vector2.Zero, scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
