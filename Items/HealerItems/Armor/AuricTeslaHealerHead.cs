using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Items.Armor.Auric;
using CalamityMod;
using CalamityMod.Items;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Utilities;
using ThoriumMod.Items.BossThePrimordials.Dream;
using Microsoft.Xna.Framework;
using Terraria.ID;
using RagnarokMod.Utils;
using RagnarokMod.Projectiles;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria.ModLoader.IO;

namespace RagnarokMod.Items.HealerItems.Armor
{
    //I love auric tesla armor
    [AutoloadEquip(EquipType.Head)]
    public class AuricTeslaHealerHead : ThoriumItem
    {
        private bool darkAura;
        public override LocalizedText DisplayName => ModLoader.HasMod("InfernalEclipseAPI") || ModLoader.HasMod("WHummusMultiModBalancing") ? this.GetLocalization("DisplayNameEclipse") : base.DisplayName;

        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.PreventBeardDraw[this.Item.headSlot] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.defense = 27; //132
            Item.rare = ModContent.RarityType<BurnishedAuric>();
            isHealer = true;
        }

        #region Toggleable Dark Aura
        bool toggleEnabled
        {
            get 
            {
                return darkAura && (!ModLoader.HasMod("ThoriumRework") || ModLoader.HasMod("InfernalEclipseAPI") || ModLoader.HasMod("WHummusMultiModBalancing")); 
            }
            set
            {
                darkAura = value;
            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (!toggleEnabled)
            {
                tooltips.RemoveAll(x => x.Name == "Tooltip0");
                isDarkHealer = false;
            }
            else
            {
                isDarkHealer = true;
            }
            tooltips.Add(new(Mod, "Toggle", this.GetLocalizedValue("ToggleTooltip")));
            base.ModifyTooltips(tooltips);
        }
        public override bool CanRightClick() => Main.keyState.PressingShift() && (!ModLoader.HasMod("ThoriumRework") || ModLoader.HasMod("InfernalEclipseAPI") || ModLoader.HasMod("WHummusMultiModBalancing"));
        public override void RightClick(Player player)
        {
            toggleEnabled = !toggleEnabled;
            Item.NetStateChanged();
        }
        public override bool ConsumeItem(Player player) => false;
        public override void SaveData(TagCompound tag)
        {
            tag.Add("toggleEffect", toggleEnabled);
        }
        public override void LoadData(TagCompound tag)
        {
            toggleEnabled = tag.GetBool("toggleEffect");
        }
        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(toggleEnabled);
        }
        public override void NetReceive(BinaryReader reader)
        {
            toggleEnabled = reader.ReadBoolean();
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (!ModLoader.HasMod("ThoriumRework") || ModLoader.HasMod("InfernalEclipseAPI") || ModLoader.HasMod("WHummusMultiModBalancing"))
            {
                CalamityUtils.DrawInventoryDot(spriteBatch, position, new Vector2(16, 16) * Main.inventoryScale, toggleEnabled);
            }
        }
        public override void UpdateInventory(Player player)
        {
        }
        #endregion

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<AuricTeslaBodyArmor>() && legs.type == ModContent.ItemType<AuricTeslaCuisses>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            if (!ModLoader.HasMod("ThoriumRework") || ModLoader.HasMod("InfernalEclipseAPI") || ModLoader.HasMod("WHummusMultiModBalancing"))
                player.setBonus = this.GetLocalizedValue("SetBonus");
            else
                player.setBonus = this.GetLocalizedValue("SetBonusHelheim");

            var modPlayer = player.Calamity();

            if (ModLoader.HasMod("ThoriumRework"))
                modPlayer.tarraSet = true;

            modPlayer.bloodflareSet = true;
            modPlayer.silvaSet = true;
            modPlayer.auricSet = true;
            player.thorns += 3f;
            player.ignoreWater = true;
            player.crimsonRegen = true;

            if (!ModLoader.HasMod("ThoriumRework") || ModLoader.HasMod("InfernalEclipseAPI") || ModLoader.HasMod("WHummusMultiModBalancing"))
            {
                player.GetRagnarokModPlayer().auricHealerSet = true;
                player.GetThoriumPlayer().darkAura = darkAura;
            }
            else
                player.GetRagnarokModPlayer().tarraHealer = true;

            player.GetRagnarokModPlayer().silvaHealer = true;
            player.GetRagnarokModPlayer().bloodflareHealer = true;
            //player.GetRagnarokModPlayer().nightfallen = true;

            player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 0.5f;

            if (Main.myPlayer == player.whoAmI && ModLoader.HasMod("ThoriumRework"))
            {
                int type = ModContent.ProjectileType<GuardianHealer>();
                if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[type] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromThis("tarraHealer"), player.Center, Vector2.Zero, type, 0, 0f, player.whoAmI, 0f, 0f, 0f);
                }
            }
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = player.GetRagnarokModPlayer();
            modPlayer.auricBoost = true;
            player.moveSpeed += 0.05f;
            player.manaCost *= 0.75f;
            ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
            player.GetDamage(DamageClass.Generic) -= 0.8f;
            player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 1.7f;
            thoriumPlayer.healBonus += 10;
            player.GetCritChance(ThoriumDamageBase<HealerDamage>.Instance) += 25f;
            player.statManaMax2 += 100;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            if (!ModLoader.HasMod("ThoriumRework") || ModLoader.HasMod("InfernalEclipseAPI") || ModLoader.HasMod("WHummusMultiModBalancing"))
            {
                recipe.AddIngredient<DreamWeaversHood>();
                recipe.AddIngredient<DreamWeaversHelmet>();
            }
            else
            {
                recipe.AddIngredient<TarragonCowl>();
            }

            recipe.AddIngredient<BloodflareHeadHealer>();
            recipe.AddIngredient<SilvaHeadHealer>();
            //recipe.AddIngredient<NightfallenHelmet>();

            if (ModLoader.HasMod("InfernalEclipseAPI") || ModLoader.HasMod("WHummusMultiModBalancing"))
                recipe.AddIngredient<ShadowspecBar>(12);
            else
                recipe.AddIngredient<AuricBar>(12);

            recipe.AddTile<CosmicAnvil>();
            recipe.Register();
        }
    }
}