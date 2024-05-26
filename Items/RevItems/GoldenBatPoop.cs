using System;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using RagnarokMod.Utils;
using ThoriumMod;
using ThoriumMod.Utilities;
using System.Collections.Generic;
using CalamityMod.Rarities;
using CalamityMod.Items;

namespace RagnarokMod.Items.RevItems
{
	public class GoldenBatPoop : ModItem
	{

		public override void SetDefaults()
		{
			base.Item.width = 26;
			base.Item.height = 26;
			base.Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
			base.Item.rare = 3;
			base.Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
			thoriumPlayer.statCoinDrops += 0.3f;
			player.GetRagnarokModPlayer().batpoop = true;
		}
	}
}
