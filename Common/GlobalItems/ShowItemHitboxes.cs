using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Buffs;
using RagnarokMod.Common.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using ThoriumMod.Items;

namespace RagnarokMod.Common.GlobalItems
{
    public class ShowItemHitboxes : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return true;
        }

        public override void PostDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            if (ModContent.GetInstance<UIConfig>().ShowItemHitboxes)
            {
                var rect = new Rectangle((int)(item.position.X - Main.screenPosition.X), (int)(item.position.Y - Main.screenPosition.Y), item.width, item.height);
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, rect, Color.Red * 0.5f);
            }
        }
    }
}
