using Terraria.Audio;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using ThoriumMod.Buffs.Bard;
using ThoriumMod.Items.BossThePrimordials.Rhapsodist;
using ThoriumMod.Items;
using ThoriumMod.Sounds;
using Microsoft.Xna.Framework;
using System;
using ThoriumMod;
using ThoriumMod.Utilities;

namespace RagnarokMod.Utils
{
    public class RagnarokModPlayer : ModPlayer
    {
        //this is most likely only gonna be for armor set abilities.
        public bool auricBardSet = false;
		public bool tarraHealer = false;
		public bool tarraBard = false;
		
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (ThoriumHotkeySystem.ArmorKey.JustPressed)
            {
                if (auricBardSet && !((ModPlayer)this).Player.HasBuff(ModContent.BuffType<CreativeBlock>()) && BardItem.ConsumeInspiration(((ModPlayer)this).Player, 20))
                {
                    SoundEngine.PlaySound(ThoriumSounds.PassbySurge, (Vector2?)null, (SoundUpdateCallback)null);
                    ((ModPlayer)this).Player.AddBuff(ModContent.BuffType<SoloistsHatSetBuff>(), 600, true, false);
                    ModContent.GetInstance<InspiratorsHelmet>().NetApplyEmpowerments(((ModPlayer)this).Player, 0);
                    ((ModPlayer)this).Player.AddBuff(ModContent.BuffType<CreativeBlock>(), 1800, true, false);
                    for (int l = 0; l < 5; l++)
                    {
                        Vector2 offset3 = new Vector2(Main.rand.Next(-50, 51), Main.rand.Next(-50, 51));
                        Dust obj4 = Dust.NewDustDirect(((Entity)((ModPlayer)this).Player).position + offset3, ((Entity)((ModPlayer)this).Player).width, ((Entity)((ModPlayer)this).Player).height, 87, 0f, 0f, 0, default(Color), 2f);
                        obj4.noGravity = true;
                        obj4.velocity = -offset3 * 0.075f;
                    }
                }

            }
        }
		
		public override void PostUpdateMiscEffects() 
		{
			if (tarraBard) 
			{
				ThoriumPlayer thoriumPlayer = ThoriumMod.Utilities.PlayerHelper.GetThoriumPlayer(base.Player);
				thoriumPlayer.setOrnate = true;
				thoriumPlayer.bardBuffDuration = (short)(thoriumPlayer.bardBuffDuration * 1.5);
			}
		}
		
		
		public override void ResetEffects()
		{
				this.tarraHealer = false;
				this.tarraBard = false;
		}
    }
}
