using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;

namespace RagnarokMod.Utils
{
    public static class DrawHelper
    {
        public static bool DrawItemInWorldScaled(Item item, SpriteBatch spriteBatch,
            Color lightColor, ref float rotation, ref float scale, float targetScale)
        {
            scale = targetScale;
            var texture = TextureAssets.Item[item.type].Value;
            float scaledHeight = texture.Height * scale;
            var position = item.TopLeft - Main.screenPosition;
            position.Y += item.height - scaledHeight;
            spriteBatch.Draw(texture, position, null, lightColor, rotation, Vector2.Zero, scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
