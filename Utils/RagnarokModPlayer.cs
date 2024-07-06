using Terraria.Audio;
using Terraria;
using Terraria.ID;
using Terraria.GameInput;
using Terraria.ModLoader;
using ThoriumMod.Buffs.Bard;
using ThoriumMod.Buffs.Healer;
using ThoriumMod.Buffs.Thrower;
using ThoriumMod.Items.BossThePrimordials.Rhapsodist;
using ThoriumMod.Items.BossThePrimordials.Dream;
using ThoriumMod.Items;
using ThoriumMod.Sounds;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using ThoriumMod;
using ThoriumMod.Utilities;
using ThoriumMod.Empowerments;
using RagnarokMod.Buffs;
using RagnarokMod.Items.HealerItems;
using CalamityMod.Cooldowns;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.CalPlayer;
using RagnarokMod.Items.BardItems.Armor;

namespace RagnarokMod.Utils
{
    public class RagnarokModPlayer : ModPlayer
    {
		
		private static Mod thorium = ModLoader.GetMod("ThoriumMod");
		private static Mod calamity = ModLoader.GetMod("CalamityMod");

		private static int debugcounter = 0;

		/// <summary>
		/// multiplies FinalDamage by (1 - this value) on the next damage taken.
		/// </summary>
		public float oneTimeDamageReduction = 0;

		/// <summary>
		/// if this is set to true, then all attacks will inflict brimstone flames for 5 seconds.
		/// </summary>
        public bool brimstoneFlamesOnHit = false;

        //this is most likely only gonna be for armor set abilities.
        public bool batpoop = false;
        public bool auricBardSet = false;
		public bool auricHealerSet = false;
		public bool tarraHealer = false;
		public bool tarraBard = false;
		public bool daedalusHealer = false;
		public bool daedalusBard = false;
		public bool godslayerBard = false;
		public bool silvaHealer = false;
		public bool bloodflareHealer = false;
		public bool bloodflareBard = false;
		public int godslayerBardcurrentemp = 0;
		public int godslayerBardcurrentemplevel = 0;
		private int bloodflarebloodlust = 0;
		private int bloodflarepointtimer = 0;
		private const int maxbloodlustpoints = 150;

		public override void OnHurt(Player.HurtInfo info)
		{
			if (bloodflareHealer || bloodflareBard)
			{
				RemoveBloodFlareBloodlustPoints(25);
			}
		}

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (oneTimeDamageReduction != 0)
            {
                modifiers.FinalDamage *= (1 - oneTimeDamageReduction);
                oneTimeDamageReduction = 0;
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (ThoriumHotkeySystem.ArmorKey.JustPressed)
            {
                if (auricBardSet && !((ModPlayer)this).Player.HasBuff(ModContent.BuffType<CreativeBlock>()) && BardItem.ConsumeInspiration(((ModPlayer)this).Player, 20))
                {
                    SoundEngine.PlaySound(ThoriumSounds.PassbySurge, (Vector2?)null, (SoundUpdateCallback)null);
                    ((ModPlayer)this).Player.AddBuff(ModContent.BuffType<SoloistsHatSetBuff>(), 600, true, false);
                    ModContent.GetInstance<InspiratorsHelmet>().NetApplyEmpowerments(((ModPlayer)this).Player, 0);
                    ((ModPlayer)this).Player.AddBuff(ModContent.BuffType<CreativeBlock>(), 2400, true, false);
                    for (int l = 0; l < 5; l++)
                    {
                        Vector2 offset3 = new Vector2(Main.rand.Next(-50, 51), Main.rand.Next(-50, 51));
                        Dust obj4 = Dust.NewDustDirect(((Entity)((ModPlayer)this).Player).position + offset3, ((Entity)((ModPlayer)this).Player).width, ((Entity)((ModPlayer)this).Player).height, 87, 0f, 0f, 0, default(Color), 2f);
                        obj4.noGravity = true;
                        obj4.velocity = -offset3 * 0.075f;
                    }
                }
				
				if(auricHealerSet && !base.Player.HasBuff(ModContent.BuffType<DreamWeaversHoodDebuff>()) && base.Player.CheckMana(200, true, false)) 
				{
					base.Player.AddBuff(ModContent.BuffType<DreamWeaversHoodDreamBuff>(), 600, true, false);
					base.Player.AddBuff(ModContent.BuffType<DreamWeaversMaskBuff>(), 900, true, false);
					base.Player.AddBuff(ModContent.BuffType<DreamWeaversHoodDebuff>(), 3660, true, false);
					SoundEngine.PlaySound(ThoriumSounds.PassbySurge, (Vector2?)null, (SoundUpdateCallback)null);
					for (int n = 0; n < 20; n++)
					{
						Dust.NewDustDirect(base.Player.position, base.Player.width, base.Player.height, 173, (float)Main.rand.Next(-8, 9), (float)Main.rand.Next(-8, 9), 0, default(Color), 2f).noGravity = true;
					}
					for (int k2 = 0; k2 < 10; k2++)
					{
						Dust.NewDustDirect(base.Player.position, base.Player.width, base.Player.height, 65, (float)Main.rand.Next(-8, 9), (float)Main.rand.Next(-8, 9), 0, default(Color), 1.4f).noGravity = true;
					}
				}
				
				if (daedalusHealer && !base.Player.HasBuff(ModContent.BuffType<AntarcticExhaustionDebuff>()) && base.Player.CheckMana(100, true, false)) 
				{
					SoundEngine.PlaySound(ThoriumSounds.PassbySurge, (Vector2?)null, (SoundUpdateCallback)null);
					ThoriumPlayer thoriumPlayer = ThoriumMod.Utilities.PlayerHelper.GetThoriumPlayer(base.Player);
					this.Player.AddBuff(ModContent.BuffType<AntarcticExhaustionDebuff>(), 3600, true, false);
					this.Player.AddBuff(ModContent.BuffType<AntarcticReinforcementBuff>(), 900, true, false);
					thoriumPlayer.shieldHealth += 50;
					this.Player.statLife += 50;
				}
				
				if(daedalusBard && !base.Player.HasBuff(ModContent.BuffType<AntarcticExhaustionDebuff>()) && BardItem.ConsumeInspiration(((ModPlayer)this).Player, 12)) 
				{
					SoundEngine.PlaySound(ThoriumSounds.PassbySurge, (Vector2?)null, (SoundUpdateCallback)null);
					ModContent.GetInstance<DaedalusHeadBard>().NetApplyEmpowerments(base.Player, 0);	
					this.Player.AddBuff(ModContent.BuffType<AntarcticExhaustionDebuff>(), 3600, true, false);
					this.Player.AddBuff(ModContent.BuffType<AntarcticCreativityBuff>(), 900, true, false);
				}
            }
        }
		
		
		public override void PreUpdateBuffs()
		{
			
			if (base.Player.HasBuff(ModContent.BuffType<LastStandBuff>()))
			{
				int bufftypeindex = -100;
				for( int i = 0; i < base.Player.buffType.Length; i++) 
				{
					if(base.Player.buffType[i] == ModContent.BuffType<LastStandBuff>()) 
					{
						bufftypeindex = i;
					}
				}
				if(base.Player.buffTime[bufftypeindex] > 480 && bufftypeindex != -100) 
				{
					base.Player.buffTime[bufftypeindex] = 480;
				}
			}
			
		}
		
		
		public override void PostUpdateMiscEffects() 
		{
			if (tarraBard) 
			{
				ThoriumPlayer thoriumPlayer = ThoriumMod.Utilities.PlayerHelper.GetThoriumPlayer(base.Player);
				thoriumPlayer.setOrnate = true;
				thoriumPlayer.bardBuffDurationX *= 1.5f;
				thoriumPlayer.bardBuffDurationXDisplay *= 1.5f;
			}
			if (godslayerBard &&  base.Player.HeldItem.DamageType == ThoriumDamageBase<BardDamage>.Instance) 
			{
				Random rnd = new Random();
				int num = rnd.Next(180);
				if(num == 0) 
				{
					godslayerBardcurrentemp = rnd.Next(1,18);
					godslayerBardcurrentemplevel = rnd.Next(1,4);
					ModContent.GetInstance<GodSlayerHeadBard>().NetApplyEmpowerments(((ModPlayer)this).Player, 0);
					godslayerBardcurrentemp = 0;
					godslayerBardcurrentemplevel = 0;
				}	
			} 
			if (silvaHealer) 
			{
				ThoriumPlayer thoriumPlayer = ThoriumMod.Utilities.PlayerHelper.GetThoriumPlayer(base.Player);
				thoriumPlayer.setBlooming = true;
				thoriumPlayer.setLifeBinder = true;
				Random rnd = new Random();
				int triggerchance = 400;
				if (base.Player.statLife >= 0.8 * base.Player.statLifeMax2 && base.Player.statLife < 0.9 * base.Player.statLifeMax2) 
				{
					triggerchance = 360;
				}
				else if (base.Player.statLife >= 0.5* base.Player.statLifeMax2 && base.Player.statLife < 0.8* base.Player.statLifeMax2 )
				{
					triggerchance = 240;
				}
				else if (base.Player.statLife >= 0.2 * base.Player.statLifeMax2 && base.Player.statLife < 0.5* base.Player.statLifeMax2 ) 
				{
					triggerchance = 160;
				}
				else if (base.Player.statLife < 0.2 * base.Player.statLifeMax2) 
				{
					triggerchance = 80;
				}
				int num = rnd.Next(triggerchance);
				if(num == 1) 
				{
					if(thoriumPlayer.shieldHealth + 7 <= 50) 
					{
						thoriumPlayer.shieldHealth += 7;
						this.Player.statLife += 7;
					}
				} 
				else if (num == 2) 
				{
					this.Player.Heal(12);
				}
			}
			
			
			if(bloodflareHealer) 
			{
					if(bloodflarepointtimer == 0) 
					{
						bloodflarepointtimer = 30;
						AddBloodFlareBloodlustPoints(1);
					}
					else 
					{
						bloodflarepointtimer--;
					}
					if(bloodflarebloodlust >= 100) 
					{
						base.Player.moveSpeed += 0.2f;
						ThoriumPlayer thoriumPlayer = ThoriumMod.Utilities.PlayerHelper.GetThoriumPlayer(base.Player);
						thoriumPlayer.healBonus += 3;
						base.Player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 0.20f;
						base.Player.GetCritChance(ThoriumDamageBase<HealerDamage>.Instance) += 20f;
					}	
			}
			if(bloodflareBard) 
			{
				if(bloodflarepointtimer == 0) 
				{
					bloodflarepointtimer = 30;
					AddBloodFlareBloodlustPoints(1);
				}
				else 
				{
					bloodflarepointtimer--;
				}
				
				if(bloodflarebloodlust >= 100) 
					{
						base.Player.moveSpeed += 0.25f;
						ThoriumPlayer thoriumPlayer = ThoriumMod.Utilities.PlayerHelper.GetThoriumPlayer(base.Player);
						thoriumPlayer.inspirationRegenBonus += 0.1f;
						base.Player.GetDamage(ThoriumDamageBase<BardDamage>.Instance) += 0.20f;
						base.Player.GetCritChance(ThoriumDamageBase<BardDamage>.Instance) += 20f;
						thoriumPlayer.bardBuffDuration += 180;
					}
			}
			
			if(batpoop) 
			{
				long effectivecoinvalue = 0;
				// for every possible coin the 4 coin slots
				for(int i = 50; i < 54; i++) 
				{
					// is it a coin?
					if(base.Player.inventory[i].type >= 71 && base.Player.inventory[i].type <= 74) 
					{
						effectivecoinvalue += (long)(Math.Pow( 100, base.Player.inventory[i].type - 71) * base.Player.inventory[i].stack);
					}
				}
				float damagemodifier = (float)Math.Pow(((float)effectivecoinvalue / 10000000), 0.2) / 10;
				base.Player.GetDamage(DamageClass.Generic) += damagemodifier;	
			}
			
			this.ApplyRogueUseTimeFix();
			
			// The debugcounter
			if(debugcounter >= 59) 
			{
				debugcounter = 0;
			} 
			else
			{
				debugcounter++;
			}
			
			
			
				
		}
		
		private void ApplyRogueUseTimeFix() 
		{
			var calamityPlayer = base.Player.Calamity();
			Item it = base.Player.ActiveItem();
            if (!calamityPlayer.wearingRogueArmor || it.useAnimation == it.useTime)
			{
				return;
			}
			
			bool flag = it.damage > 0;
			bool hasHitboxes = !it.noMelee || it.shoot > 0;
			bool flag2 = it.pick > 0;
			bool isAxe = it.axe > 0;
			bool isHammer = it.hammer > 0;
			bool isPlaced = it.createTile != -1;
			bool isChannelable = it.channel;
			bool hasNonWeaponFunction = flag2 || isAxe || isHammer || isPlaced || isChannelable;
			bool playerUsingWeapon = flag && hasHitboxes && !hasNonWeaponFunction;
			if ((it.IsAir || !it.CountsAsClass<RogueDamageClass>()) && calamityPlayer.GemTechSet && calamityPlayer.GemTechState.IsRedGemActive)
			{
				return;
			}
			
			float attackSpeed = base.Player.GetAttackSpeed<RogueDamageClass>() * base.Player.GetAttackSpeed(DamageClass.Throwing) * base.Player.GetAttackSpeed(DamageClass.Generic);
			int adjustedUseTime = (int)(it.useTime / attackSpeed);
			
			bool animationCheck = (base.Player.itemTime == adjustedUseTime );
			if (!calamityPlayer.stealthStrikeThisFrame && animationCheck)
			{
				if (calamityPlayer.StealthStrikeAvailable())
				{
					calamityPlayer.ConsumeStealthByAttacking();
					return;
				}
				calamityPlayer.rogueStealth = 0f;
			}
		}
		
		
		public override void PostUpdateEquips() 
		{
			if(base.Player.armor[0].type == thorium.Find<ModItem>("SandStoneHelmet").Type 
		   && base.Player.armor[1].type == thorium.Find<ModItem>("SandStoneMail").Type 
		   && base.Player.armor[2].type == thorium.Find<ModItem>("SandStoneGreaves").Type) 
		   {
			   AddStealth(60);
		   }
		   else if(base.Player.armor[0].type == thorium.Find<ModItem>("FlightMask").Type 
		   && base.Player.armor[1].type == thorium.Find<ModItem>("FlightMail").Type 
		   && base.Player.armor[2].type == thorium.Find<ModItem>("FlightBoots").Type)
		   {
			   AddStealth(70);
		   }
		   else if(base.Player.armor[0].type == thorium.Find<ModItem>("BronzeHelmet").Type 
		   && base.Player.armor[1].type == thorium.Find<ModItem>("BronzeBreastplate").Type 
		   && base.Player.armor[2].type == thorium.Find<ModItem>("BronzeGreaves").Type)
		   {
			   AddStealth(85);
		   }
		   else if(base.Player.armor[0].type == thorium.Find<ModItem>("PlagueDoctorsMask").Type 
		   && base.Player.armor[1].type == thorium.Find<ModItem>("PlagueDoctorsGarb").Type 
		   && base.Player.armor[2].type == thorium.Find<ModItem>("PlagueDoctorsLeggings").Type)
		   {
			   AddStealth(100);
		   }
		   else if(base.Player.armor[0].type == thorium.Find<ModItem>("FungusHat").Type 
		   && base.Player.armor[1].type == thorium.Find<ModItem>("FungusGuard").Type 
		   && base.Player.armor[2].type == thorium.Find<ModItem>("FungusLeggings").Type)
		   {
			   AddStealth(105);
		   }
		   else if( (base.Player.armor[0].type == thorium.Find<ModItem>("HallowedGuise").Type 
		   || base.Player.armor[0].type == thorium.Find<ModItem>("AncientHallowedGuise").Type)
		   && (base.Player.armor[1].type == 551 || base.Player.armor[1].type == 4900)
		   && (base.Player.armor[2].type == 552 || base.Player.armor[2].type == 4901) )
		   {
			   AddStealth(110);
		   }
		   else if(base.Player.armor[0].type == thorium.Find<ModItem>("LichCowl").Type 
		   && base.Player.armor[1].type == thorium.Find<ModItem>("LichCarapace").Type 
		   && base.Player.armor[2].type == thorium.Find<ModItem>("LichTalon").Type)
		   {
			   AddStealth(110);
		   }
		   else if(base.Player.armor[0].type == thorium.Find<ModItem>("ShadeMasterMask").Type 
		   && base.Player.armor[1].type == thorium.Find<ModItem>("ShadeMasterGarb").Type 
		   && base.Player.armor[2].type == thorium.Find<ModItem>("ShadeMasterTreads").Type)
		   {
			   AddStealth(110);
		   }
		    else if(base.Player.armor[0].type == thorium.Find<ModItem>("WhiteDwarfMask").Type 
		   && base.Player.armor[1].type == thorium.Find<ModItem>("WhiteDwarfGuard").Type 
		   && base.Player.armor[2].type == thorium.Find<ModItem>("WhiteDwarfGreaves").Type)
		   {
			   AddStealth(115);
		   }
		     else if(base.Player.armor[0].type == thorium.Find<ModItem>("TideTurnersGaze").Type 
		   && base.Player.armor[1].type == thorium.Find<ModItem>("TideTurnerBreastplate").Type 
		   && base.Player.armor[2].type == thorium.Find<ModItem>("TideTurnerGreaves").Type)
		   {
			   AddStealth(120);
		   }
		}
		
		
		
		public void AddStealth(int stealth) 
		{
		   CalamityPlayer calamityPlayer = base.Player.Calamity();
		   calamityPlayer.rogueStealthMax += ((float)stealth / 100);
		   base.Player.Calamity().wearingRogueArmor = true;
		   base.Player.setBonus = base.Player.setBonus + "\n+" + stealth + " maximum stealth";
		}
			
		
		public void InstrumentRotationModifier(float modifier) 
		{
			ThoriumPlayer thoriumPlayer = ThoriumMod.Utilities.PlayerHelper.GetThoriumPlayer(base.Player);
			if (!thoriumPlayer.bardBigInstrumentRotationShift) 
				{
					thoriumPlayer.bardBigInstrumentRotation -= (1f - modifier) * thoriumPlayer.bardBigInstrumentRotationSpeed;
				}
				else
				{
					thoriumPlayer.bardBigInstrumentRotation += (1f - modifier) * thoriumPlayer.bardBigInstrumentRotationSpeed;
				}
		}
		
		public override void OnHitNPCWithProj(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (brimstoneFlamesOnHit)
            {
                target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
            }
            if (this.bloodflareHealer && projectile.DamageType == ThoriumDamageBase<HealerDamage>.Instance)
			{
				ApplyBloodFlareOnHit(target, damageDone);	
			}
			if (this.bloodflareBard && projectile.DamageType == ThoriumDamageBase<BardDamage>.Instance)
			{
				ApplyBloodFlareOnHit(target, damageDone);
			}
		}
		
		public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (brimstoneFlamesOnHit)
            {
                target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
            }
            if (this.bloodflareHealer && item.DamageType == ThoriumDamageBase<HealerDamage>.Instance)
			{
				ApplyBloodFlareOnHit(target, damageDone);	
			}
			if (this.bloodflareBard && item.DamageType == ThoriumDamageBase<BardDamage>.Instance)
			{
				ApplyBloodFlareOnHit(target, damageDone);
			}
		}
		
		public void ApplyBloodFlareOnHit(NPC target, int damageDone) 
		{
				target.AddBuff(ModContent.BuffType<BurningBlood>(), 240, false);
				if (bloodflarebloodlust > 100 && this.bloodflareHealer) 
				{
					this.Player.Heal((int)Math.Sqrt(damageDone / 100));
				}	
		}
		
		public void RemoveBloodFlareBloodlustPoints(int points) 
		{
			if(bloodflarebloodlust - points >= 0) 
			{
				bloodflarebloodlust-= points;
			} 
			else
			{
				bloodflarebloodlust = 0;
			}
		}
		
		public void AddBloodFlareBloodlustPoints(int points) 
		{
				if (bloodflarebloodlust + points <= maxbloodlustpoints) 
				{
					bloodflarebloodlust += points;
				}
				else 
				{
					bloodflarebloodlust = maxbloodlustpoints;
				}	
				
				if(bloodflarebloodlust == 100) 
				{
					 SoundEngine.PlaySound(SoundID.NPCHit13, (Vector2?)null, (SoundUpdateCallback)null);
				}
		}
		
		public override void ResetEffects()
		{
				if(this.tarraBard == false  && this.tarraHealer == false) 
				{
					bloodflarebloodlust = 0;
				}
				this.brimstoneFlamesOnHit = false;
				this.tarraHealer = false;
				this.tarraBard = false;
				this.daedalusHealer = false;
				this.daedalusBard = false;
				this.godslayerBard = false;
				this.silvaHealer = false;
				this.bloodflareHealer = false;
				this.bloodflareBard = false;
				this.auricBardSet = false;
		        this.auricHealerSet = false;
				this.batpoop = false;
		}
    }
}
