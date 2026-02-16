using CalamityMod.Items.Accessories;
using CalamityMod.Items.Accessories.Wings;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Systems;
using CalamityMod;
using Ragnarok.Items;
using RagnarokMod.Items;
using RagnarokMod.Items.BardItems;
using RagnarokMod.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using System.Reflection;
using RagnarokMod.Items.BardItems.Armor;
using RagnarokMod.Items.HealerItems.Armor;
using RagnarokMod.Items;
using RagnarokMod.Common.Configs;

namespace RagnarokMod.Common.ModSystems
{
    public class OtherModsCompat : ModSystem
    {
        // Thorium Bosses Reworked Config Settings
        public static bool tbr_loaded = false;
        public static bool tbr_defense_damage = false;
        public static bool tbr_configs_edited = false;
        private static Mod ThoriumRework;
        private int timer = 0;

        public override void PostAddRecipes()
        {
            if (ModLoader.TryGetMod("CalamityBardHealer", out Mod CalamityBardHealer))
            {
                if (ModContent.GetInstance<ModCompatConfig>().item_deduplication_mode == CalamityBardHealer_selection_mode.Off)
                {
                    ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<AerospecBard>(), "AerospecHeadphones");
                    ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<AerospecHealer>(), "AerospecBiretta");
                    ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<AuricTeslaFrilledHelmet>(), "AuricTeslaFeatheredHeadwear");
                    ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<AuricTeslaHealerHead>(), "AuricTeslaValkyrieVisage");
                    ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<BloodflareHeadHealer>(), "BloodflareRitualistMask");
                    ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<BloodflareHeadBard>(), "BloodflareSirenSkull");
                    ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<DaedalusHeadHealer>(), "DaedalusCowl");
                    ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<DaedalusHeadBard>(), "DaedalusHat");
                    ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<GodSlayerHeadBard>(), "GodSlayerDeathsingerCowl");
                    ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<SilvaHeadHealer>(), "SilvaGuardianHelmet");
                    ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<StatigelHeadBard>(), "StatigelEarrings");
                    ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<StatigelHeadHealer>(), "StatigelFoxMask");
                    ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<TarragonShroud>(), "TarragonChapeau");
                    ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<TarragonCowl>(), "TarragonParagonCrown");
                    ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<IntergelacticRobohelm>(), "IntergelacticCloche");
                    ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<IntergelacticRamhelm>(), "IntergelacticProtectorHelm");

                    //Weapons
                    ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<AnahitasArpeggioOverride>(), "AnahitasArpeggio");
                    ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<BelchingSaxophoneOverride>(), "BelchingSaxophone");
                    ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<FaceMelterOverride>(), "FaceMelter");
                }
                else if (ModContent.GetInstance<ModCompatConfig>().item_deduplication_mode == CalamityBardHealer_selection_mode.Ragnarok)
                {
                    RemoveOtherModRecipe(CalamityBardHealer, "AerospecHeadphones");
                    RemoveOtherModRecipe(CalamityBardHealer, "AerospecBiretta");
                    RemoveOtherModRecipe(CalamityBardHealer, "AuricTeslaFeatheredHeadwear");
                    RemoveOtherModRecipe(CalamityBardHealer, "AuricTeslaValkyrieVisage");
                    RemoveOtherModRecipe(CalamityBardHealer, "BloodflareRitualistMask");
                    RemoveOtherModRecipe(CalamityBardHealer, "BloodflareSirenSkull");
                    RemoveOtherModRecipe(CalamityBardHealer, "DaedalusCowl");
                    RemoveOtherModRecipe(CalamityBardHealer, "DaedalusHat");
                    RemoveOtherModRecipe(CalamityBardHealer, "GodSlayerDeathsingerCowl");
                    RemoveOtherModRecipe(CalamityBardHealer, "SilvaGuardianHelmet");
                    RemoveOtherModRecipe(CalamityBardHealer, "StatigelEarrings");
                    RemoveOtherModRecipe(CalamityBardHealer, "StatigelFoxMask");
                    RemoveOtherModRecipe(CalamityBardHealer, "TarragonChapeau");
                    RemoveOtherModRecipe(CalamityBardHealer, "TarragonParagonCrown");

                    if (ModLoader.HasMod("CatalystMod"))
                    { // Catalyst Content
                        RemoveOtherModRecipe(CalamityBardHealer, "AugmentedAuricTeslaFeatheredHeadwear");
                        RemoveOtherModRecipe(CalamityBardHealer, "AugmentedAuricTeslaValkyrieVisage");
                        RemoveOtherModRecipe(CalamityBardHealer, "IntergelacticCloche");
                        RemoveOtherModRecipe(CalamityBardHealer, "IntergelacticProtectorHelm");
                    }
                }
                else
                {
                    RemoveOwnRecipe(ModContent.ItemType<AerospecBard>());
                    RemoveOwnRecipe(ModContent.ItemType<AerospecHealer>());
                    RemoveOwnRecipe(ModContent.ItemType<AuricTeslaFrilledHelmet>());
                    RemoveOwnRecipe(ModContent.ItemType<AuricTeslaHealerHead>());
                    RemoveOwnRecipe(ModContent.ItemType<BloodflareHeadHealer>());
                    RemoveOwnRecipe(ModContent.ItemType<BloodflareHeadBard>());
                    RemoveOwnRecipe(ModContent.ItemType<DaedalusHeadHealer>());
                    RemoveOwnRecipe(ModContent.ItemType<DaedalusHeadBard>());
                    RemoveOwnRecipe(ModContent.ItemType<GodSlayerHeadBard>());
                    RemoveOwnRecipe(ModContent.ItemType<SilvaHeadHealer>());
                    RemoveOwnRecipe(ModContent.ItemType<StatigelHeadBard>());
                    RemoveOwnRecipe(ModContent.ItemType<StatigelHeadHealer>());
                    RemoveOwnRecipe(ModContent.ItemType<TarragonShroud>());
                    RemoveOwnRecipe(ModContent.ItemType<TarragonCowl>());
                    RemoveOwnRecipe(ModContent.ItemType<IntergelacticRobohelm>());
                    RemoveOwnRecipe(ModContent.ItemType<IntergelacticRamhelm>());
                }
            }
        }

        // Fixes the TBReworked bossrush when Ragnarok AI are enabled
        public override void PostUpdateEverything()
        {
            if (timer == 300)
            { // Timer to not apply changes every tick -> otherwise heavy performance impact
                timer = 0;
                if (tbr_loaded)
                {
                    if (CalamityGamemodeCheck.isBossrush)
                    {
                        if (tbr_configs_edited)
                        {
                            Main.NewText("Ragnarok Error: Please enable all Bosses in ThoriumBossRework config and set all BossAIs in RagnarokMod Config to TBR or Auto");
                        }
                    }
                }
            }
            timer++;
        }

        public static bool shouldRagnarokBossAILoad(ThoriumBossRework_selection_mode selected)
        {
            if (selected == ThoriumBossRework_selection_mode.Ragnarok)
            {
                return true;
            }
            else if (selected == ThoriumBossRework_selection_mode.Auto)
            {
                if (tbr_loaded)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else { return false; }
        }

        public override void PostSetupContent()
        {
            ModLoader.TryGetMod("ThoriumRework", out ThoriumRework);
            if (ThoriumRework != null)
            {
                tbr_loaded = true;
                ApplyRagnarokTBRBossChanges();
            }
        }

        public void ApplyRagnarokTBRBossChanges()
        {
            ModConfig tbr_compat = ThoriumRework.Find<ModConfig>("CompatConfig");
            ModConfig tbr_reworktoggles = ThoriumRework.Find<ModConfig>("ReworkTogglesConfig");
            if (tbr_compat != null && tbr_reworktoggles != null)
            {
                try
                {
                    tbr_defense_damage = (bool)(tbr_compat.GetType().GetField("defenseDamage", BindingFlags.Public | BindingFlags.Instance)).GetValue(tbr_compat);
                    if (OtherModsCompat.shouldRagnarokBossAILoad(ModContent.GetInstance<BossConfig>().bossrush))
                    {
                        (tbr_compat.GetType().GetField("thorlamityBR", BindingFlags.Public | BindingFlags.Instance)).SetValue(tbr_compat, false);
                    }
                    if (OtherModsCompat.shouldRagnarokBossAILoad(ModContent.GetInstance<BossConfig>().bird))
                    {
                        (tbr_reworktoggles.GetType().GetField("bird", BindingFlags.Public | BindingFlags.Instance)).SetValue(tbr_reworktoggles, false);
                        tbr_configs_edited = true;
                    }
                    if (OtherModsCompat.shouldRagnarokBossAILoad(ModContent.GetInstance<BossConfig>().jelly))
                    {
                        (tbr_reworktoggles.GetType().GetField("jelly", BindingFlags.Public | BindingFlags.Instance)).SetValue(tbr_reworktoggles, false);
                        tbr_configs_edited = true;
                    }
                    if (OtherModsCompat.shouldRagnarokBossAILoad(ModContent.GetInstance<BossConfig>().viscount))
                    {
                        (tbr_reworktoggles.GetType().GetField("bat", BindingFlags.Public | BindingFlags.Instance)).SetValue(tbr_reworktoggles, false);
                        tbr_configs_edited = true;
                    }
                    if (OtherModsCompat.shouldRagnarokBossAILoad(ModContent.GetInstance<BossConfig>().granite))
                    {
                        (tbr_reworktoggles.GetType().GetField("ges", BindingFlags.Public | BindingFlags.Instance)).SetValue(tbr_reworktoggles, false);
                        tbr_configs_edited = true;
                    }
                    if (OtherModsCompat.shouldRagnarokBossAILoad(ModContent.GetInstance<BossConfig>().champion))
                    {
                        (tbr_reworktoggles.GetType().GetField("champ", BindingFlags.Public | BindingFlags.Instance)).SetValue(tbr_reworktoggles, false);
                        tbr_configs_edited = true;
                    }
                    if (OtherModsCompat.shouldRagnarokBossAILoad(ModContent.GetInstance<BossConfig>().scouter))
                    {
                        (tbr_reworktoggles.GetType().GetField("scout", BindingFlags.Public | BindingFlags.Instance)).SetValue(tbr_reworktoggles, false);
                        tbr_configs_edited = true;
                    }
                    if (OtherModsCompat.shouldRagnarokBossAILoad(ModContent.GetInstance<BossConfig>().strider))
                    {
                        (tbr_reworktoggles.GetType().GetField("strider", BindingFlags.Public | BindingFlags.Instance)).SetValue(tbr_reworktoggles, false);
                        tbr_configs_edited = true;
                    }
                }
                catch
                {
                    Mod ragnarok = ModContent.GetInstance<RagnarokMod>();
                    ragnarok.Logger.Error("Ragnarok Error: Failed to read and modify TBReworked config. Fallback to Auto config");
                    ModContent.GetInstance<BossConfig>().bird = ThoriumBossRework_selection_mode.Auto;
                    ModContent.GetInstance<BossConfig>().jelly = ThoriumBossRework_selection_mode.Auto;
                    ModContent.GetInstance<BossConfig>().viscount = ThoriumBossRework_selection_mode.Auto;
                    ModContent.GetInstance<BossConfig>().granite = ThoriumBossRework_selection_mode.Auto;
                    ModContent.GetInstance<BossConfig>().champion = ThoriumBossRework_selection_mode.Auto;
                    ModContent.GetInstance<BossConfig>().scouter = ThoriumBossRework_selection_mode.Auto;
                    ModContent.GetInstance<BossConfig>().strider = ThoriumBossRework_selection_mode.Auto;
                }
            }
        }

        public void ExchangeRecipe(Mod othermod, int ritemtype, string oitemname)
        {
            try
            {
                Recipe recipe_ragnarok_to_othermod = Recipe.Create(othermod.Find<ModItem>(oitemname).Type, 1);
                recipe_ragnarok_to_othermod.AddIngredient(ritemtype, 1).AddTile(TileID.DemonAltar);
                recipe_ragnarok_to_othermod.Register();

                Recipe recipe_othermod_to_ragnarok = Recipe.Create(ritemtype, 1);
                recipe_othermod_to_ragnarok.AddIngredient(othermod.Find<ModItem>(oitemname).Type, 1).AddTile(TileID.DemonAltar);
                recipe_othermod_to_ragnarok.Register();
            }
            catch
            {
                Mod ragnarok = ModContent.GetInstance<RagnarokMod>();
                ragnarok.Logger.Error("Ragnarok Error: Failed to create exchange recipe for " + oitemname + " of another mod. Does this item even exist?");
            }
        }

        public void RemoveOwnRecipe(int ritemtype)
        {
            GetRecipe finder = new();
            try
            {
                finder.LookFor(ritemtype, 1);
                foreach (Recipe item in finder.Search())
                {
                    RecipeHelper helper = new(item);
                    helper.Disable();
                }
            }
            catch
            {
                Mod ragnarok = ModContent.GetInstance<RagnarokMod>();
                ragnarok.Logger.Error("Ragnarok Error: Failed to remove own recipe.");
            }
        }

        public void RemoveOtherModRecipe(Mod othermod, string oitemname)
        {
            GetRecipe finder = new();
            try
            {
                finder.LookFor(othermod.Find<ModItem>(oitemname).Type, 1);
                foreach (Recipe item in finder.Search())
                {
                    RecipeHelper helper = new(item);
                    helper.Disable();
                }
            }
            catch
            {
                Mod ragnarok = ModContent.GetInstance<RagnarokMod>();
                ragnarok.Logger.Error("Ragnarok Error: Failed to remove recipe for " + oitemname + " of another mod. Does this item even exist?");
            }

        }
    }
}
