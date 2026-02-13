using CalamityMod.Items;
using Microsoft.Xna.Framework;
using RagnarokMod.Buffs;
using RagnarokMod.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Utilities;

namespace RagnarokMod.Items.HealerItems.Accessories
{
    public class LeviathanHeart : ModItem
    {
        ThoriumPlayer rag;
        public bool appliedChange = false;
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.buyPrice(gold: 15);
        }

        public override void SetStaticDefaults()
        {
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (rag != null)
            {
                if (rag.darkAura)
                {
                    tooltips.Add(new TooltipLine(Mod, "tooltip", Language.GetTextValue("Mods.RagnarokMod.Tooltips.LeviathanHeart.CorruptTooltip")));
                }
                else
                {
                    tooltips.Add(new TooltipLine(Mod, "tooltip", Language.GetTextValue("Mods.RagnarokMod.Tooltips.LeviathanHeart.NormalTooltip")));
                }
            }
            else
            {
                tooltips.Add(new TooltipLine(Mod, "tooltip", Language.GetTextValue("Mods.RagnarokMod.Tooltips.LeviathanHeart.NormalTooltip")));
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
            RagnarokModPlayer ragPlayer = player.GetRagnarokModPlayer();
            rag = thoriumPlayer;
            ragPlayer.leviathanHeart = true;
            if (thoriumPlayer.darkAura)
            {
                UpdateCorrupted(player, thoriumPlayer);
            }
            else
            {
                UpdateNormal(player, thoriumPlayer);
            }
        }

        private void UpdateNormal(Player player, ThoriumPlayer thoriumPlayer)
        {
            thoriumPlayer.healBonus = Math.Max((int)Math.Ceiling(thoriumPlayer.healBonus * 1.33), 2);
        }

        private void UpdateCorrupted(Player player, ThoriumPlayer thoriumPlayer)
        {
            RagnarokModPlayer ragPlayer = player.GetRagnarokModPlayer();

            if (player.HeldItem.ModItem is ThoriumItem thoriumItem
                && thoriumItem != ragPlayer.lastHeldItem)
            {
                // Restore previous item's cost before modifying the new one
                if (ragPlayer.lastHeldItem != null)
                    ragPlayer.lastHeldItem.radiantLifeCost = ragPlayer.origLifeCost;

                if (thoriumItem.radiantLifeCost > 1)
                {
                    ragPlayer.origLifeCost = thoriumItem.radiantLifeCost;
                    ragPlayer.lastHeldItem = thoriumItem;
                    thoriumItem.radiantLifeCost -= 1;
                }
                else
                {
                    ragPlayer.lastHeldItem = null;
                    ragPlayer.origLifeCost = 0;
                }
            }

            player.GetAttackSpeed(ThoriumDamageBase<HealerDamage>.Instance) += 0.25f;
        }

        public static void OnHealAlly(Player healer, Player target)
        {
            if (!healer.HasItem(ModContent.ItemType<LeviathanHeart>()))
                return;

            var thoriumPlayer = healer.GetModPlayer<ThoriumPlayer>();
            if (thoriumPlayer.darkAura)
                return;

            target.AddBuff(ModContent.BuffType<LeviathanHeartBubble>(), 5 * 60);
        }

        public static void OnSpendHealth(Player player)
        {
            if (!player.HasItem(ModContent.ItemType<LeviathanHeart>()))
                return;

            var thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            if (!thoriumPlayer.darkAura)
                return;

            player.AddBuff(ModContent.BuffType<LeviathanHeartBubbleCorrupted>(), 5 * 60);
        }
    }
}
