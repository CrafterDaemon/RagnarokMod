using CalamityMod;
using Microsoft.Xna.Framework;
using RagnarokMod.Common.Configs;
using RagnarokMod.Riffs;
using RagnarokMod.Sounds;
using RagnarokMod.Utils;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;
using ThoriumMod.Items.BardItems;

namespace RagnarokMod.Items.BardItems
{
    public abstract class RiffInstrumentBase : BardItem
    {
        public abstract SoundStyle RiffSound { get; }
        public abstract SoundStyle NormalSound { get; }
        public abstract byte RiffType { get; }
        public virtual float RiffRange => 1000f;

        public sealed override void BardTooltips(List<TooltipLine> tooltips)
        {
            string keybind = PlayerInput.GenerateInputTag_ForCurrentGamemode(false, "SmartSelect");
            bool showFull = ThoriumConfigClient.Instance.ShowFullEmpowermentTooltip
                || keybind == string.Empty
                || PlayerInput.Triggers.Current.SmartSelect;

            Riff riff = RiffLoader.GetRiff(RiffType);
            if (riff == null || !showFull)
                return;

            int index = tooltips.FindLastIndex(tt => tt.Mod.Equals("Terraria") && tt.Name.StartsWith("Tooltip")) + 1;
            if (index == 0) index = tooltips.Count;

            tooltips.Insert(index++, new TooltipLine(Mod, "RiffHeader", "Riff Empowerments:")
            {
                OverrideColor = new Color(255, 40, 100)
            });

            int i = 0;
            foreach ((byte type, byte level) in riff.Empowerments)
            {
                string line = EmpowermentTagHandler.GenerateTag(type, level) + " " +
                    EmpowermentLoader.EmpowermentNameLevelText.Format(
                        EmpowermentLoader.GetDisplayName(type),
                        MiscHelper.TenToRoman(level));
                tooltips.Insert(index++, new TooltipLine(Mod, "RiffEmpowerment" + i++, line));
            }

            SafeBardTooltips(tooltips);
        }

        public virtual void SafeBardTooltips(List<TooltipLine> tooltips) { }
        public sealed override bool CanPlayInstrument(Player player)
        {
            SoundHandler(player);
            if (player.altFunctionUse == 2)
            {
                var ragnarokPlayer = player.GetRagnarokModPlayer();

                if (player.Calamity().cooldowns.ContainsKey(RiffLoader.Cooldown.ID))
                    return false;

                if (SoundEngine.TryGetActiveSound(ragnarokPlayer.fretSlot, out var sound) && sound.IsPlaying)
                    return false;

                foreach (Player other in MiscHelper.GetPlayersInRange(player, RiffRange))
                    if (other.GetRagnarokModPlayer().fretPlaying)
                        return false;
            }
            return SafeCanPlayInstrument(player) && base.CanPlayInstrument(player);
        }

        public sealed override bool AltFunctionUse(Player player) => true;
        public sealed override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var ragnarokPlayer = player.GetRagnarokModPlayer();
            if (player.altFunctionUse == 2 && !ragnarokPlayer.fretPlaying)
            {
                SyncRiffSound(player, true);
            }
            else if (player.altFunctionUse != 2)
            {
                SafeRiffBardShoot(player, source, position, velocity, type, damage, knockback);
            }
            return true;
        }

        public sealed override void BardHoldItem(Player player)
        {
            if (player.whoAmI != Main.myPlayer)
                return;

            player.Calamity().rightClickListener = true;

            var ragnarokPlayer = player.GetRagnarokModPlayer();

            if (ragnarokPlayer.fretPlaying)
            {
                if (!SoundEngine.TryGetActiveSound(ragnarokPlayer.fretSlot, out var sound) || !sound.IsPlaying)
                    SyncRiffSound(player, false);
            }

            if (player.HeldItem != Item)
                SyncRiffSound(player, false);

            SafeBardHoldItem(player);
        }

        // Safe overrides for subclasses
        public virtual bool SafeCanPlayInstrument(Player player) => true;
        public virtual void SafeRiffBardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) { }
        public virtual void SafeBardHoldItem(Player player) { }

        public virtual void ModifyRiffInstDamage(ThoriumPlayer bard, ref StatModifier damage) { }

        public override void ModifyInstrumentDamage(ThoriumPlayer bard, ref StatModifier damage)
        {
            base.ModifyInstrumentDamage(bard, ref damage);
            ModifyRiffInstDamage(bard, ref damage);
        }

        private void SoundHandler(Player player)
        {
            var ragnarokPlayer = player.GetRagnarokModPlayer();
            Item.UseSound = (player.altFunctionUse == 2 || ragnarokPlayer.fretPlaying)
                ? RagnarokModSounds.none
                : NormalSound;
        }

        protected void SyncRiffSound(Player player, bool start)
        {
            var ragnarokPlayer = player.GetModPlayer<RagnarokModPlayer>();
            if (start)
            {
                if (!SoundEngine.TryGetActiveSound(ragnarokPlayer.fretSlot, out var sound) || !sound.IsPlaying)
                {
                    ragnarokPlayer.fretSlot = SoundEngine.PlaySound(
                        RiffSound.WithVolumeScale(ModContent.GetInstance<ClientConfig>().RiffMusicVolume),
                        player.Center
                    );
                    ragnarokPlayer.fretPlaying = true;
                    ragnarokPlayer.activeRiffType = RiffType;
                }
            }
            else
            {
                if (SoundEngine.TryGetActiveSound(ragnarokPlayer.fretSlot, out var sound))
                    sound.Stop();
                ragnarokPlayer.fretPlaying = false;
                ragnarokPlayer.activeRiffType = 0;
            }
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = Mod.GetPacket();
                packet.Write((byte)1);
                packet.Write((byte)player.whoAmI);
                packet.Write(start);
                packet.Send();
            }
        }
    }
}