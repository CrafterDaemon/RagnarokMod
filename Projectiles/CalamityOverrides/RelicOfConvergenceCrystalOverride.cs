using System;
using System.Collections.Generic;
using CalamityMod;
using CalamityMod.Dusts;
using CalamityMod.Enums;
using CalamityMod.Items.Tools;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Utilities;
using RagnarokMod.Items.HealerItems.CalamityOverrides;
using RagnarokMod.Utils;

namespace RagnarokMod.Projectiles.CalamityOverrides{
    public class RelicOfConvergenceCrystalOverride : ModProjectile{
        public override string Texture => "CalamityMod/Projectiles/Typeless/RelicOfConvergenceCrystal";
		
		public ref float time{
			get{
				return ref base.Projectile.ai[0];
			}
		}
		private Player Owner{
			get{
				return Main.player[base.Projectile.owner];
			}
		}
		public override void SetDefaults(){
			base.Projectile.width = 32;
			base.Projectile.height = 46;
			base.Projectile.penetrate = -1;
			base.Projectile.tileCollide = false;
			base.Projectile.ignoreWater = true;
			base.Projectile.timeLeft = 125;
		}

		public override void AI(){
			this.completion = Terraria.Utils.GetLerpValue(120f, 0f, (float)base.Projectile.timeLeft, true);
			this.fade = MathHelper.Lerp(this.fade, 0f, 0.04f);
			if (base.Projectile.timeLeft >= 5){
				this.mousePos = this.Owner.ClampedMouseWorld();
			}
			if (this.Owner.channel){
				this.killTimer = 18;
			}
			if (this.killTimer <= 0){
				base.Projectile.Kill();
				return;
			}
			if (this.Owner.Calamity().profanedSoulRelicBuff){
				base.Projectile.extraUpdates = 1;
			}
			this.killTimer--;
			this.UpdatePlayerVisuals(this.Owner);
			if (base.Projectile.soundDelay <= 0){
				SoundStyle soundStyle = SoundID.DD2_WitherBeastCrystalImpact;
				soundStyle.Volume = 0.5f * (float)((this.time >= (float)this.CrystalsDrawTime) ? 1 : 2);
				soundStyle.Pitch = 0.5f * this.completion;
				SoundEngine.PlaySound(ref soundStyle, new Vector2?(base.Projectile.Center), null);
				if (this.time >= (float)this.CrystalsDrawTime){
					soundStyle = new SoundStyle("CalamityMod/Sounds/Item/NullHit", 0);
					soundStyle.Volume = 0.4f;
					soundStyle.Pitch = -0.3f + 0.7f * this.completion;
					SoundEngine.PlaySound(ref soundStyle, new Vector2?(base.Projectile.Center), null);
					float numberOfDusts = 10f;
					int i = 0;
					while ((float)i < numberOfDusts){
						GeneralParticleHandler.SpawnParticle(new VelChangingSpark(base.Projectile.Center, Terraria.Utils.RotatedByRandom(Vector2.One, 100.0) * Terraria.Utils.NextFloat(Main.rand, 9f, 18f), Terraria.Utils.DirectionFrom(this.mousePos, base.Projectile.Center) * 35f, "CalamityMod/Particles/BloomCircle", 25, Terraria.Utils.NextFloat(Main.rand, 0.1f, 0.35f) * this.completion, Color.Lerp(Color.Orange, Color.Orchid, this.completion), new Vector2(1f, 1f), true, false, 0f, false, 0.15f, 0.04f), false, null);
						i++;
					}
				}
				base.Projectile.soundDelay = (int)((float)this.SoundInterval * ((this.time >= (float)this.CrystalsDrawTime) ? (1f - 0.9f * this.completion) : 0.5f));
				this.fade = 1f;
			}
			if (base.Projectile.timeLeft == 5){
				for (int playerIndex = 0; playerIndex < 255; playerIndex++){
					Player player = Main.player[playerIndex];
					if (Terraria.Utils.DistanceSQ(player.Center, this.mousePos) < 19044f && player.team == this.Owner.team && !this.healList[playerIndex]){
						this.healList[playerIndex] = true;
						int totalHealValue = RelicOfConvergenceOverride.HealValue + (Owner.GetThoriumPlayer().healBonus * RelicOfConvergenceOverride.BonusHealMultiplier);
						int trueHealValue = (int)((float)totalHealValue * ((player.whoAmI == this.Owner.whoAmI) ? 1f : 1.5f) * (this.Owner.Calamity().profanedSoulRelicBuff ? 1.25f : 1f));
						base.Projectile.ThoriumHeal(trueHealValue, 30f, true, false, delegate(Player player, Player target, ref int healAmount, ref int selfHealAmount){}, null, -1, false, false, true);
						if (this.playSound){
							SoundStyle soundStyle = new SoundStyle("CalamityMod/Sounds/Custom/ProfanedGuardians/GuardianHeal", 0);
							soundStyle.Volume = 1f;
							soundStyle.MaxInstances = -1;
							SoundEngine.PlaySound(ref soundStyle, new Vector2?(base.Projectile.Center), null);
							this.playSound = false;
						}
						for (int j = 0; j < 5; j++){
							GeneralParticleHandler.SpawnParticle(new CustomSpark(player.Center + Terraria.Utils.NextVector2Circular(Main.rand, 15f, 15f), -Vector2.UnitY * Terraria.Utils.NextFloat(Main.rand, 0.2f, 3f), "CalamityMod/Particles/HealingPlus", false, Main.rand.Next(35, 51), Terraria.Utils.NextFloat(Main.rand, 1.1f, 1.9f), Color.Lerp(Color.Orchid, Color.White, (float)j * 0.1f), Vector2.One, true, true, 0f, false, false, 0.1f, 1f, 1f, false, false, 0f), false, null);
						}
					}
				}
			}
			if (this.time >= (float)this.CrystalsDrawTime){
				this.GeneratePassiveDust(this.Owner);
				Lighting.AddLight(base.Projectile.Center, Color.Lerp(Color.Orange, Color.Orchid, this.completion).ToVector3() * (2.5f * (this.completion - 0.375f) + this.fade));
			}
			this.time += 1f;
		}

		public void UpdatePlayerVisuals(Player player){
			Vector2 vel = Terraria.Utils.DirectionTo(player.Center, this.mousePos);
			float rot = Terraria.Utils.ToRotation(vel) + ((player.direction == -1) ? MathHelper.ToRadians(270f) : MathHelper.ToRadians(-90f));
			player.ChangeDir(MathF.Sign(vel.X));
			base.Projectile.Center = player.Center + vel * 15f;
			player.heldProj = base.Projectile.whoAmI;
			player.itemTime = 2;
			player.itemAnimation = 2;
			player.SetCompositeArmFront(true, 0, rot);
			player.SetCompositeArmBack(true, 0, rot);
		}

		public void GeneratePassiveDust(Player player){
			float radius = MathHelper.Lerp(0f, 200f, this.completion - 0.375f);
			for (float angle = 0f; angle <= 6.2831855f; angle += MathHelper.ToRadians(Terraria.Utils.NextFloat(Main.rand, 6f, 8f))){
				Vector2 drawPos = this.mousePos + Terraria.Utils.ToRotationVector2(angle) * radius;
				Color useColor = Color.Lerp(Color.Orange, Color.Orchid, this.completion) * (this.completion - 0.25f);
				float particleScale = 0.01f + this.fade * 0.08f + this.completion * 0.08f;
				GeneralParticleHandler.SpawnParticle(new CustomSpark(drawPos, Terraria.Utils.DirectionTo(this.mousePos, drawPos), "CalamityMod/Particles/SmallBloom", false, 4, particleScale, useColor, new Vector2(0.5f + this.completion, (2f - this.completion) * 7f - this.completion * 7f), true, false, 0f, false, false, 0f, 1f, 1f, false, false, 0f), false, null);
				if (Terraria.Utils.NextBool(Main.rand, 70)){
					Dust dust = Dust.NewDustPerfect(base.Projectile.Center + Terraria.Utils.ToRotationVector2(angle) * radius, ModContent.DustType<LightDust>(), null, 0, default(Color), 1f);
					dust.position = this.mousePos + Terraria.Utils.ToRotationVector2(angle) * radius;
					dust.scale = Terraria.Utils.NextFloat(Main.rand, 1.4f, 1.9f) * this.completion;
					dust.noGravity = false;
					dust.velocity = new Vector2(0f, Terraria.Utils.NextFloat(Main.rand, 1f, 5f));
					dust.color = useColor;
				}
				if (base.Projectile.timeLeft == 5){
					Dust dust2 = Dust.NewDustPerfect(base.Projectile.Center + Terraria.Utils.ToRotationVector2(angle) * radius, ModContent.DustType<LightDust>(), null, 0, default(Color), 1f);
					dust2.position = drawPos;
					dust2.scale = Terraria.Utils.NextFloat(Main.rand, 1.6f, 1.9f);
					dust2.noGravity = !Terraria.Utils.NextBool(Main.rand, 5);
					dust2.velocity = Terraria.Utils.DirectionTo(this.mousePos, drawPos) * Terraria.Utils.NextFloat(Main.rand, 2f, 4f);
					dust2.color = Color.Orchid;
					dust2.noLightEmittence = true;
				}
			}
		}
		
		public override bool PreDraw(ref Color lightColor){
			float num = this.time / (float)this.CrystalsDrawTime;
			Texture2D crystalTexture = TextureAssets.Projectile[base.Type].Value;
			for (int i = 0; i < this.TotalCrystalsToDraw; i++){
				float num2 = 6.2831855f / (float)this.TotalCrystalsToDraw * (float)i + this.time / 10f;
				float radius = MathHelper.Lerp(this.MaxCrystalOffsetRadius, 0f, this.time / (float)this.CrystalsDrawTime);
				Vector2 drawPositionOffset = Terraria.Utils.ToRotationVector2(num2) * radius;
				Vector2 drawPosition = (this.time >= (float)this.CrystalsDrawTime) ? base.Projectile.Center : (base.Projectile.Center + drawPositionOffset + Terraria.Utils.NextVector2Circular(Main.rand, 12f, 12f));
				Projectile projectile = base.Projectile;
				Color color = Color.Lerp(Color.Orchid, Color.Goldenrod, this.fade);
				color.A = 0;
				Color backglowColor = color * this.completion * 0.5f;
				Color white = Color.White;
				color = Color.White;
				color.A = 0;
				Color lightColor2 = Color.Lerp(white, color, this.fade * 0.5f) * MathHelper.Clamp(this.completion * 1.5f, (this.time >= (float)this.CrystalsDrawTime) ? 0.8f : 0f, 1f);
				float backglowArea = 4f * this.completion + this.fade * 3f;
				Texture2D texture = crystalTexture;
				float x = drawPosition.X;
				float y = drawPosition.Y;
				projectile.DrawProjectileWithBackglow(backglowColor, lightColor2, backglowArea, texture, null, 0, x, y);
			}
			return false;
		}

		public int SoundInterval = 25;
		public int TotalCrystalsToDraw = 3;
		public int CrystalsDrawTime = 40;
		public float MaxCrystalOffsetRadius = 80f;
		public float MaxDustOffsetRadius = 70f;
		public List<bool> healList = new List<bool>(new bool[255]);
		public float completion;
		public float fade;
		public int killTimer;
		public Vector2 mousePos;
		public bool playSound = true;
    }
}
