using CalamityMod;
using CalamityMod.ChatTags;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Buffs;
using ReLogic.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace RagnarokMod.ChatTags
{
    public sealed class RagnarokBuffTagHandler : AbstractTagHandler<RagnarokBuffTagHandler>
    {
        public sealed class Snippet(int buffId) : TextSnippet
        {
            private const float IconSize = 26f;

            public int BuffId => buffId;

            public bool DrawIcon => true;


            private static Dictionary<int, Color> _buffColorOverrides;

            private static Dictionary<int, Color> BuffColorOverrides => _buffColorOverrides ??= new()
            {
                [ModContent.BuffType<Buffs.AuricSurge>()] = new Color(159, 230, 252),
                [ModContent.BuffType<Buffs.LeviathanHeartBubble>()] = new Color(46, 139, 87),
                [ModContent.BuffType<Buffs.LeviathanHeartBubbleCorrupted>()] = new Color(255, 105, 237),
                [ModContent.BuffType<Buffs.ScytheAirBuff>()] = new Color(180, 230, 255),
                [ModContent.BuffType<Buffs.ScytheBrimstoneBuff>()] = new Color(255, 80, 30), 
                [ModContent.BuffType<Buffs.ScytheEarthBuff>()] = new Color(79, 55, 39),
                [ModContent.BuffType<Buffs.ScytheOasisBuff>()] = new Color(80, 210, 150),
                [ModContent.BuffType<Buffs.ScytheSandBuff>()] = new Color(230, 190, 90), 
                [ModContent.BuffType<Buffs.ScytheWaterBuff>()] = new Color(90, 160, 255),
                [ModContent.BuffType<Buffs.AntarcticCreativityBuff>()] = new Color(151, 193, 230),
                [ModContent.BuffType<Buffs.AntarcticReinforcementBuff>()] = new Color(151, 193, 230),
                [ModContent.BuffType<Buffs.GuardianHealerBeamBuff>()] = new Color(46, 139, 87)
            };

            public override bool UniqueDraw(bool justCheckingString, out Vector2 size, SpriteBatch spriteBatch, Vector2 position = new Vector2(), Color color = new Color(), float scale = 1)
            {
                size = new Vector2(GetStringLength(FontAssets.MouseText.Value), IconSize);

                if (!justCheckingString && (color.R != 0 || color.G != 0 || color.B != 0))
                {
                    if (DrawIcon)
                    {
                        if (Main.netMode != NetmodeID.Server && !Main.dedServ)
                        {
                            var texture = TextureAssets.Buff[BuffId];
                            spriteBatch.Draw(texture.Value, new Rectangle((int)position.X, (int)position.Y - 2, (int)IconSize, (int)IconSize), null, Color.White);
                        }

                        position.X += IconSize;
                    }
                    Color buffColor;
                    if (BuffColorOverrides.TryGetValue(buffId, out Color overrideColor))
                        buffColor = overrideColor;
                    else
                        buffColor = CalamityUtils.GetDebuffTooltipNameColor(buffId);

                    var name = $"{(DrawIcon ? " " : "")}{Lang.GetBuffName(buffId)}";
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, name, position, buffColor, 0f, Vector2.Zero, new Vector2(scale));
                }
                return true;
            }

            public override float GetStringLength(DynamicSpriteFont font)
            {
                float iconSize = !DrawIcon ? 0f : IconSize + font.MeasureString(" ").X;
                float size = iconSize + font.MeasureString(Lang.GetBuffName(buffId)).X;
                return size * Scale;
            }
        }

        protected override string[] TagNames { get; } = ["rbuff"];

        public override TextSnippet Parse(string text, Color baseColor = new(), string options = null)
        {
            if (int.TryParse(text, out int buffId) && buffId >= 0 && buffId < BuffLoader.BuffCount)
                return new Snippet(buffId);

            if (BuffID.Search.TryGetId(text, out buffId))
                return new Snippet(buffId);

            return new TextSnippet(text);
        }
    }
}