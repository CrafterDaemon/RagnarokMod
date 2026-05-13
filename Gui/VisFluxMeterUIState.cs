using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Common.Configs;
using RagnarokMod.Utils;
using ReLogic.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace RagnarokMod.Common.UI
{
    /// <summary>
    /// The Vis/Flux meter HUD element.
    ///
    /// Sprite layout (14x64px, renders ABOVE the anchor point):
    ///   y  0– 3  -> top endcap    (4px, orange trim)
    ///   y  4–59  -> interior      (56px, fillable)
    ///   y 60–63  -> bottom endcap (4px, orange trim)
    ///
    /// Fill order (bottom-up inside the 56px interior):
    ///   Vis  (white)  fills from the bottom of the interior upward
    ///   Flux (purple) floats on top of Vis, filling upward from the Vis surface
    ///
    /// Position is driven by ClientConfig (client-side config).
    /// Hovering shows exact Vis / Flux values near the cursor.
    /// </summary>
    public class VisFluxMeterUIState : UIState
    {
        // Sprite constants (match VisFluxMeter.png exactly)
        private const int SpriteW = 14;
        private const int SpriteH = 64;
        private const int EndcapTop = 4;  // rows 0-3
        private const int EndcapBottom = 4;  // rows 60-63
        private const int InteriorH = SpriteH - EndcapTop - EndcapBottom; // 56

        // Colours
        private static readonly Color VisColor = new Color(230, 230, 255);
        private static readonly Color FluxColor = new Color(140, 0, 200);

        private UIImage _background;

        public override void OnInitialize()
        {
            Width = new StyleDimension(SpriteW, 0f);
            Height = new StyleDimension(SpriteH, 0f);
            HAlign = 1f; // anchor right
            VAlign = 1f; // anchor bottom

            _background = new UIImage(
                ModContent.Request<Texture2D>("RagnarokMod/Gui/VisFluxMeter"));
            _background.Width = new StyleDimension(SpriteW, 0f);
            _background.Height = new StyleDimension(SpriteH, 0f);
            _background.Left = StyleDimension.Empty;
            _background.Top = StyleDimension.Empty;
            _background.IgnoresMouseInteraction = true;

            Append(_background);
        }

        /// <summary>Pulls offset from config and recalculates layout.</summary>
        private void RefreshPosition()
        {
            var cfg = ClientConfig.Instance;
            //Left = new StyleDimension(-(cfg.MeterOffsetX + SpriteW), 0f);
            //Top = new StyleDimension(-(cfg.MeterOffsetY + SpriteH), 0f);
            Recalculate();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var vfp = Main.LocalPlayer.GetRagnarokModPlayer();
            if (!vfp.HasThaumonomicon) return;

            RefreshPosition();
            CalculatedStyle dims = GetDimensions();
            int intX = (int)dims.X;
            int intY = (int)dims.Y + EndcapTop; // skip top endcap

            float visFraction = MathHelper.Clamp(vfp.Vis / Math.Max(RConsts.MaxVis, 100f), 0f, 1f);
            float fluxFraction = MathHelper.Clamp(vfp.Flux / RConsts.MaxFlux, 0f, 1f);

            int visRows = (int)(InteriorH * visFraction);
            int fluxRows = (int)(InteriorH * fluxFraction);

            // Vis: bottom of interior upward
            int visStartRow = InteriorH - visRows;
            // Flux: directly above Vis
            int fluxStartRow = System.Math.Max(visStartRow - fluxRows, 0);

            var pixel = Terraria.GameContent.TextureAssets.MagicPixel.Value;

            // Flux bar
            if (fluxRows > 0)
            {
                int clampedFluxRows = visStartRow - fluxStartRow; // already clamped above
                spriteBatch.Draw(pixel,
                    new Rectangle(intX, intY + fluxStartRow, SpriteW, clampedFluxRows),
                    FluxColor * 0.85f);
            }

            // Vis bar
            if (visRows > 0)
            {
                spriteBatch.Draw(pixel,
                    new Rectangle(intX, intY + visStartRow, SpriteW, visRows),
                    VisColor * 0.85f);
            }

            base.Draw(spriteBatch); // draws the background sprite
            // Hover tooltip
            if (dims.ToRectangle().Contains(Main.mouseX, Main.mouseY))
            {
                string tooltip = $"Vis:  {vfp.Vis:F1} / {RConsts.MaxVis}\n" +
                                 $"Flux: {vfp.Flux:F1} / {RConsts.MaxFlux}";
                DrawTooltip(spriteBatch, tooltip, Main.mouseX + 12, Main.mouseY + 12);
            }
        }

        private static void DrawTooltip(SpriteBatch sb, string text, int x, int y)
        {
            var font = Terraria.GameContent.FontAssets.MouseText.Value;
            var lines = text.Split('\n');
            int padding = 6;
            int lineH = (int)font.MeasureString("W").Y;
            int boxW = 0;

            foreach (var line in lines)
            {
                int w = (int)font.MeasureString(line).X;
                if (w > boxW) boxW = w;
            }

            int boxH = lineH * lines.Length;

            if (x + boxW + padding * 2 > Main.screenWidth)
                x = Main.screenWidth - boxW - padding * 2 - 2;
            if (y + boxH + padding * 2 > Main.screenHeight)
                y = Main.screenHeight - boxH - padding * 2 - 2;

            var pixel = Terraria.GameContent.TextureAssets.MagicPixel.Value;
            sb.Draw(pixel,
                new Rectangle(x - padding, y - padding, boxW + padding * 2, boxH + padding * 2),
                Color.Black * 0.75f);

            for (int i = 0; i < lines.Length; i++)
            {
                Color lineColor = lines[i].StartsWith("Vis") ? VisColor :
                                  lines[i].StartsWith("Flux") ? FluxColor : Color.White;
                sb.DrawString(font, lines[i], new Vector2(x, y + i * lineH), lineColor);
            }
        }
    }
}