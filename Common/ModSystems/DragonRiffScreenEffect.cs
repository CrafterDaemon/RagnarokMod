using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Riffs;
using RagnarokMod.Riffs.RiffTypes;
using RagnarokMod.Utils;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace RagnarokMod.Common.ModSystems
{
    public class DragonRiffScreenEffect : ModSystem
    {
        private float tintIntensity = 0f;
        private const float MaxIntensity = 0.12f;
        private const float FadeSpeed = 0.008f;

        public override void PostUpdateEverything()
        {
            Player player = Main.LocalPlayer;
            bool riffActive = player.active
                && !player.dead
                && player.GetRagnarokModPlayer().activeRiffType == RiffLoader.RiffType<DragonRiff>();

            if (riffActive)
                tintIntensity = MathHelper.Min(tintIntensity + FadeSpeed, MaxIntensity);
            else
                tintIntensity = MathHelper.Max(tintIntensity - FadeSpeed, 0f);
        }

        public override void PostDrawTiles()
        {
            if (tintIntensity <= 0f)
                return;

            SpriteBatch sb = Main.spriteBatch;
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            Texture2D pixel = TextureAssets.MagicPixel.Value;
            Color tint = new Color(255, 100, 20) * tintIntensity;

            sb.Draw(pixel, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight),
                new Rectangle(0, 0, 1, 1), tint);

            sb.End();
        }
    }
}