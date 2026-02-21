using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using RagnarokMod.Sounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RagnarokMod.Items
{
    public class PrimalTerror : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.MowTheLawn;
            Item.holdStyle = ItemHoldStyleID.HoldGuitar;
            Item.noMelee = true;
            Item.rare = ModContent.RarityType<CosmicPurple>();
        }

        public override bool? UseItem(Player player)
        {
            String text = "The Primordial Wyrm has awoken!";
            Color color = new Color(175, 75, 255);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                // Broadcast text
                if (Main.netMode == NetmodeID.SinglePlayer)
                    Main.NewText(text, color);
                else
                    ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), color);

                // Play sound globally
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    SoundEngine.PlaySound(new SoundStyle("CalamityMod/Sounds/Custom/Scare"));
                }
                else
                {
                    // Tell all clients to play it
                    ModPacket packet = ModContent.GetInstance<RagnarokMod>().GetPacket();
                    packet.Write((byte)2);
                    packet.Write((byte)player.whoAmI);
                    packet.Send();
                }
            }
            return true;
        }
    }
}
