using Microsoft.Xna.Framework;
using RagnarokMod.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace RagnarokMod.Common.UI
{
    public class VisFluxMeterUISystem : ModSystem
    {
        private UserInterface _meterInterface;
        internal VisFluxMeterUIState MeterState;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                MeterState = new VisFluxMeterUIState();
                _meterInterface = new UserInterface();

                MeterState.Activate();
                _meterInterface.SetState(MeterState);
            }
        }

        public override void Unload()
        {
            MeterState = null;
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (_meterInterface == null) return;

            // Only update when player has the accessory
            if (Main.LocalPlayer.GetRagnarokModPlayer().HasThaumonomicon)
                _meterInterface.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            // Draw just after the inventory (so it appears on the HUD)
            int inventoryIndex = layers.FindIndex(l => l.Name == "Vanilla: Inventory");
            if (inventoryIndex >= 0)
            {
                layers.Insert(inventoryIndex + 1, new LegacyGameInterfaceLayer(
                    "RagnarokMod: Vis/Flux Meter",
                    () =>
                    {
                        if (Main.LocalPlayer.GetRagnarokModPlayer().HasThaumonomicon)
                            _meterInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI
                ));
            }
        }
    }
}