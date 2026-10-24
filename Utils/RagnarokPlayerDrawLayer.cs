using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace RagnarokMod.Utils{
	public class RagnarokPlayerDrawLayer : PlayerDrawLayer{
		public override Position GetDefaultPosition() =>
			new AfterParent(PlayerDrawLayers.Head);
	
		protected override void Draw(ref PlayerDrawSet drawInfo){
			Player player = drawInfo.drawPlayer;
			 var modPlayer = player.GetModPlayer<RagnarokModPlayer>();
			if ((!modPlayer.bloodflareHealer && !modPlayer.bloodflareBard ))
				return;
			Vector2 pos = player.Top - Main.screenPosition + new Vector2(0, -20);
			string text = modPlayer.bloodflarebloodlust.ToString();
			Terraria.Utils.DrawBorderString(Main.spriteBatch, text, pos, Color.Red, 1f, 0.5f, 0.5f);
		}
	}
}

