using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ReLogic.Utilities;

using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Empowerments;
using ThoriumMod.Projectiles.Bard;
using ThoriumMod.Items.BardItems;

using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Rarities;

using RagnarokMod.Utils;
using RagnarokMod.Sounds;
using RagnarokMod.Projectiles.BardPro.String;

namespace RagnarokMod.Items.BardItems.String
{
    public class ScourgesFrets : BigInstrumentItemBase{
        public override BardInstrumentType InstrumentType => BardInstrumentType.String;
        private ActiveSound activeSound;
		private SlotId soundSlot;
        private bool soundPlaying = false;
		private int stopTimer = 0;
		
        public override void SafeSetStaticDefaults(){
            Empowerments.AddInfo<LifeRegeneration>(1, 0);
        }

        public override void SafeSetBardDefaults(){
            Item.damage = 21;
            InspirationCost = 1;
            Item.width = 88;
            Item.height = 88;
            Item.useTime = 23;
			Item.useAnimation = 23;
            Item.useStyle = ItemUseStyleID.Guitar;
			Item.holdStyle = 5;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.knockBack = 3f;
			Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
			Item.rare = 2;
			Item.UseSound = null;
            Item.shoot = ModContent.ProjectileType<ScourgesFretsPro>();
            Item.shootSpeed = 11f;
        }
		
		 public override Vector2? HoldoutOffset(){
            return new Vector2(-18, 20);
        }

        public override void HoldItemFrame(Player player){
            player.itemLocation += new Vector2(-18, 20) * player.Directions;
        }
		
		 public override void UseStyle(Player player, Rectangle heldItemFrame){
            Vector2 offset = new Vector2(-18, 20) * player.Directions;

            player.itemLocation += offset;
        }
		
		public override void SafeBardShoot(int success, int level, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (success != 0){
				if (!SoundEngine.TryGetActiveSound(soundSlot, out activeSound) || !activeSound.IsPlaying){
					SyncFretSound(player, true);
				}
			}
			else {
					SyncFretSound(player, false);
			}
			
			stopTimer = 0;
			Projectile.NewProjectile(source, position, velocity , type, damage, knockback, player.whoAmI, 0f, 0f, 0f);
		}
		
		private void SyncFretSound(Player player, bool start){
			if (Main.netMode == NetmodeID.SinglePlayer){
				var ragnarokplayer = player.GetModPlayer<RagnarokModPlayer>();
				if (start){
					if (!SoundEngine.TryGetActiveSound(ragnarokplayer.fretSlot, out var sound) || !sound.IsPlaying){
						ragnarokplayer.fretSlot = SoundEngine.PlaySound(
							RagnarokModSounds.fretsriff,
							player.Center
						);
						ragnarokplayer.fretPlaying = true;
					}
				}
				else{
					if (SoundEngine.TryGetActiveSound(ragnarokplayer.fretSlot, out var sound)) {
						sound.Stop();
					}
					ragnarokplayer.fretPlaying = false;
				}
				return;
			}
		
			if (Main.netMode != NetmodeID.MultiplayerClient){
				ModPacket packet = Mod.GetPacket();
				packet.Write((byte)1);
				packet.Write((byte)player.whoAmI);
				packet.Write(true);
				packet.Send();
			}
		}
		
        public override void BardHoldItem(Player player){
			if (player.whoAmI != Main.myPlayer) {
				return;
			}
			
			 if (player.HeldItem != Item){
				SyncFretSound(player, false);
			}
			
            if (!player.controlUseItem){
				stopTimer++;
				if (stopTimer >= 60){
						SyncFretSound(player, false);
						stopTimer = 0;
				}
			}
			else{
				stopTimer = 0;
			}
		}
		public override void UpdateInventory(Player player){
			if (player.whoAmI != Main.myPlayer){
				return;
			}
			if (player.HeldItem != Item){
				SyncFretSound(player, false);
			}
		}
    }
}