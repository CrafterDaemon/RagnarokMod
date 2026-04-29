using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items.BasicAccessories;

namespace RagnarokMod.Common.GlobalItems
{
    public class ReworkClassRings : GlobalItem
    {
        public override bool AppliesToEntity(Item item, bool lateInstantiation) =>
            item.type == ModContent.ItemType<OpalRing>() ||
            item.type == ModContent.ItemType<RubyRing>() ||
            item.type == ModContent.ItemType<AmberRing>() ||
            item.type == ModContent.ItemType<TopazRing>() ||
            item.type == ModContent.ItemType<EmeraldRing>() ||
            item.type == ModContent.ItemType<AquamarineRing>() ||
            item.type == ModContent.ItemType<SapphireRing>() ||
            item.type == ModContent.ItemType<AmethystRing>() ||
            item.type == ModContent.ItemType<TheRing>();

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ModContent.ItemType<OpalRing>())
            {
                player.GetDamage(ThoriumDamageBase<BardDamage>.Instance).Flat -= 1;

                player.GetDamage(ThoriumDamageBase<BardDamage>.Instance) += 0.03f;
            }
            else if (item.type == ModContent.ItemType<RubyRing>())
            {
                player.GetDamage(DamageClass.Melee).Flat -= 1f;

                player.GetDamage(DamageClass.Melee) += 0.03f;
            }
            else if (item.type == ModContent.ItemType<AmberRing>())
            {
                player.GetDamage(DamageClass.Summon).Flat -= 1f;

                player.GetDamage(DamageClass.Summon) += 0.03f;
            }
            else if (item.type == ModContent.ItemType<TopazRing>())
            {
                player.GetDamage(DamageClass.Throwing).Flat -= 1f;

                player.GetDamage(DamageClass.Throwing) += 0.03f;
            }
            else if (item.type == ModContent.ItemType<EmeraldRing>())
            {
                player.GetDamage(DamageClass.Ranged).Flat -= 1f;

                player.GetDamage(DamageClass.Ranged) += 0.03f;
            }
            else if (item.type == ModContent.ItemType<AquamarineRing>())
            {
                player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance).Flat -= 1f;

                player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 0.03f;
            }
            else if (item.type == ModContent.ItemType<SapphireRing>())
            {
                player.GetDamage(DamageClass.Magic).Flat -= 1f;

                player.GetDamage(DamageClass.Magic) += 0.03f;
            }
            else if (item.type == ModContent.ItemType<AmethystRing>())
            {
                player.GetArmorPenetration(DamageClass.Generic) += 1f;
            }
            else if (item.type == ModContent.ItemType<TheRing>())
            {
                player.GetDamage(DamageClass.Generic).Flat -= TheRing.FlatDamage;

                player.GetArmorPenetration(DamageClass.Generic) += 1f;
                player.GetDamage(DamageClass.Generic) += 0.04f;
            }
        }
    }
}
