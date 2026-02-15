using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items;
using ThoriumMod;
using ThoriumMod.Items.BasicAccessories;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace RagnarokMod.Common.GlobalItems
{
    public class ReworkClassRings : GlobalItem
    {
        //only ninja emblem
        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            return (item.type == ModContent.ItemType<OpalRing>() ||
            item.type == ModContent.ItemType<RubyRing>() ||
            item.type == ModContent.ItemType<AmberRing>() ||
            item.type == ModContent.ItemType<TopazRing>() ||
            item.type == ModContent.ItemType<EmeraldRing>() ||
            item.type == ModContent.ItemType<AquamarineRing>() ||
            item.type == ModContent.ItemType<SapphireRing>() ||
            item.type == ModContent.ItemType<AmethystRing>() ||
            item.type == ModContent.ItemType<TheRing>()
            );
        }
        //change to all class item
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ModContent.ItemType<OpalRing>())
            {
                //player.GetDamage(ThoriumDamageBase<BardDamage>.Instance).Flat -= 1f;
                player.GetDamage(ThoriumDamageBase<BardDamage>.Instance) += 0.03f;
            }
            else if (item.type == ModContent.ItemType<RubyRing>())
            {
                //player.GetDamage(DamageClass.Melee).Flat -= 1f;
                player.GetDamage(DamageClass.Melee) += 0.03f;
            }
            else if (item.type == ModContent.ItemType<AmberRing>())
            {
                player.GetDamage(DamageClass.Summon) += 0.03f;
            }
            else if (item.type == ModContent.ItemType<TopazRing>())
            {
                player.GetDamage(DamageClass.Throwing) += 0.03f;
            }
            else if (item.type == ModContent.ItemType<EmeraldRing>())
            {
                player.GetDamage(DamageClass.Ranged) += 0.03f;
            }
            else if (item.type == ModContent.ItemType<AquamarineRing>())
            {
                player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 0.03f;
            }
            else if (item.type == ModContent.ItemType<SapphireRing>())
            {
                player.GetDamage(DamageClass.Magic) += 0.03f;
            }
            else if (item.type == ModContent.ItemType<AmethystRing>())
            {
                player.GetArmorPenetration(DamageClass.Generic) += 1f;
            }
            else if (item.type == ModContent.ItemType<TheRing>())
            {
                player.GetArmorPenetration(DamageClass.Generic) += 1f;
                player.GetDamage(DamageClass.Generic) += 0.04f;

            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ModContent.ItemType<OpalRing>())
            {
                tooltips[3].Text = tooltips[3].Text + "\n3% increased symphonic damage";
            }
            else if (item.type == ModContent.ItemType<RubyRing>())
            {
                tooltips[3].Text = tooltips[3].Text + "\n3% increased melee damage";
            }
            else if (item.type == ModContent.ItemType<AmberRing>())
            {
                tooltips[3].Text = tooltips[3].Text + "\n3% increased summon damage";
            }
            else if (item.type == ModContent.ItemType<TopazRing>())
            {
                tooltips[3].Text = tooltips[3].Text + "\n3% increased rogue damage";
            }
            else if (item.type == ModContent.ItemType<EmeraldRing>())
            {
                tooltips[3].Text = tooltips[3].Text + "\n3% increased ranged damage";
            }
            else if (item.type == ModContent.ItemType<AquamarineRing>())
            {
                tooltips[3].Text = tooltips[3].Text + "\n3% increased radiant damage";
            }
            else if (item.type == ModContent.ItemType<SapphireRing>())
            {
                tooltips[3].Text = tooltips[3].Text + "\n3% increased magic damage";
            }
            else if (item.type == ModContent.ItemType<AmethystRing>())
            {
                tooltips[3].Text = "Increases armor penetration by 2";
            }
            else if (item.type == ModContent.ItemType<TheRing>())
            {
                tooltips[3].Text = "Increases armor penetration by 3\nIncreases damage by 4%";
            }

        }
    }
}
