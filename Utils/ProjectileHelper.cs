using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Utilities;
using ThoriumMod.Buffs.Healer;
using ThoriumMod.NPCs;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Healer;
using RagnarokMod.Buffs;

namespace RagnarokMod.Utils
{
	public static class ProjectileHelper
	{
		private static bool ThoriumHealTarget(this Projectile projectile, Player target, int healAmount, bool onHealEffects = true, bool bonusHealing = true, bool ignoreSetTarget = false, bool statistics = true, ProjectileHelper.CustomHealing customHealing = null)
		{
			if (projectile.owner != Main.myPlayer)
			{
				return false;
			}
			Player player = Main.player[projectile.owner];
			ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
			ThoriumPlayer thoriumTarget = target.GetThoriumPlayer();
			RagnarokModPlayer ragnarokModPlayer = player.GetRagnarokModPlayer();
			RagnarokModPlayer ragnarokModTarget = target.GetRagnarokModPlayer();
			int heals = healAmount;
			int selfHeals = 0;
			bool self = player == target;
			if (customHealing != null)
			{
				customHealing(player, target, ref heals, ref selfHeals);
			}
			if (bonusHealing)
			{
				heals += thoriumPlayer.healBonus;
			}
			if (onHealEffects)
			{
                foreach (var effect in RagnarokModPlayer.OnHealEffects)
                    effect(player, target);
                if (thoriumPlayer.accForgottenCrossNecklace)
				{
					target.AddBuff(ModContent.BuffType<ForgottenCrossNecklaceBuff>(), 900, false, false);
				}
				if (thoriumPlayer.setBlooming)
				{
					target.AddBuff(ModContent.BuffType<BloomingSetBuff>(), 600, false, false);
				}
				if (thoriumPlayer.setLifeBinder)
				{
					target.AddBuff(ModContent.BuffType<LifeBinderSetBuff>(), 600, false, false);
				}
				if (thoriumPlayer.accVerdantOrnament)
				{
					target.AddBuff(ModContent.BuffType<VerdantOrnamentBuff>(), 300, false, false);
				}
				if (thoriumPlayer.buffDreamWeaversHoodDream)
				{
					target.AddBuff(ModContent.BuffType<DreamWeaversHoodDreamAllyBuff>(), 60, false, false);
				}
				if (ragnarokModPlayer.leviathanHeart && !thoriumPlayer.darkAura)
				{
					target.AddBuff(ModContent.BuffType<LeviathanHeartBubble>(), 5 * 60);
				}
				if (!self && thoriumPlayer.honeyHeart && target.statLife <= player.statLife)
				{
					target.AddPVPBuff(48, 300);
				}
				ref int coralShieldCounter = ref thoriumPlayer.setCoralShieldCounter;
				if (!self && target != player && thoriumPlayer.setCoral && coralShieldCounter > 0)
				{
					thoriumPlayer.HandleCoralSetTransfer(thoriumTarget, coralShieldCounter, true);
					coralShieldCounter = 0;
				}
				if (!self && thoriumPlayer.accBeltoftheQuickResponse)
				{
					player.AddBuff(ModContent.BuffType<BeltoftheQuickResponseBuff>(), 180, false, false);
				}
				if (!self && thoriumPlayer.prydwen)
				{
					selfHeals += 4;
				}
				if (!self && thoriumPlayer.innerFlame.Active && thoriumPlayer.LowestPlayer != player.whoAmI)
				{
					Projectile.NewProjectile(player.GetSource_Accessory(thoriumPlayer.innerFlame.Item, null), player.Center.X, player.Center.Y - 50f, 0f, 0f, ModContent.ProjectileType<InnerFlamePro>(), 0, 0f, player.whoAmI, 0f, 0f, 0f);
				}
				if (!self && thoriumPlayer.accDewCollector.Active)
				{
					Projectile.NewProjectile(player.GetSource_Accessory(thoriumPlayer.accDewCollector.Item, null), target.Center.X, target.Center.Y, Terraria.Utils.NextFloat(Main.rand, -1f, 1f), Terraria.Utils.NextFloat(Main.rand, -3f, -1f), ModContent.ProjectileType<DewCollectorPro>(), 0, 0f, player.whoAmI, 0f, 0f, 0f);
				}
				if (thoriumPlayer.aloePlant)
				{
					thoriumTarget.SetLifeRecoveryEffect(LifeRecoveryEffectType.AloeLeaf, 600, true);
				}
				if (thoriumPlayer.medicalAcc && !thoriumTarget.OutOfCombat)
				{
					thoriumTarget.SetLifeRecoveryEffect(LifeRecoveryEffectType.MedicalBag, 300, true);
				}
				if (!self && thoriumPlayer.equilibrium)
				{
					((player.statLife > target.statLife) ? thoriumTarget : thoriumPlayer).SetLifeRecoveryEffect(LifeRecoveryEffectType.Equalizer, 300, true);
				}
			}
			if (heals > 0)
			{
				target.HealLife(heals, player, true, statistics);
				thoriumTarget.mostRecentHeal = heals;
				thoriumTarget.mostRecentHealer = player.whoAmI;
				if (!ignoreSetTarget)
				{
					thoriumPlayer.healedTarget = target.whoAmI;
				}
				player.ApplyInteractionNearbyNPCs();
			}
			if (selfHeals > 0)
			{
				player.HealLife(selfHeals, null, true, true);
			}
			if (projectile.penetrate > 0)
			{
				projectile.penetrate--;
			}
			if (projectile.penetrate == 0)
			{
				projectile.Kill();
				return true;
			}
			return false;
		}

		public static bool CanBeHealed(this Projectile projectile, Player healer, Player target, float radius = 0f, Func<Player, bool> canHealPlayer = null, int specificPlayer = -1, bool ignoreHealer = true)
		{
			bool isSpecificPlayer = specificPlayer > -1 && specificPlayer < 255;
			return target.active && !target.dead && target.statLife < target.statLifeMax2 && (isSpecificPlayer || !ignoreHealer || healer.whoAmI != target.whoAmI) && (!isSpecificPlayer || specificPlayer == target.whoAmI) && (canHealPlayer == null || canHealPlayer(target)) && (healer.team == target.team || healer.team == 0) && (radius == 0f || target.DistanceSQ(projectile.Center) <= radius * radius);
		}

		internal static void ThoriumHeal(this Projectile projectile, int healAmount, float radius = 30f, bool onHealEffects = true, bool bonusHealing = true, ProjectileHelper.CustomHealing customHealing = null, Func<Player, bool> canHealPlayer = null, int specificPlayer = -1, bool ignoreHealer = true, bool ignoreSetTarget = false, bool statistics = true)
		{
			if (projectile.owner != Main.myPlayer)
			{
				return;
			}
			Player healer = Main.player[projectile.owner];
			if (specificPlayer > -1 && specificPlayer < 255)
			{
				Player target = Main.player[specificPlayer];
				if (projectile.CanBeHealed(healer, target, radius, canHealPlayer, specificPlayer, ignoreHealer))
				{
					projectile.ThoriumHealTarget(target, healAmount, onHealEffects, bonusHealing, ignoreSetTarget, statistics, customHealing);
				}
			}
			else
			{
				for (int i = 0; i < 255; i++)
				{
					Player target = Main.player[i];
					if (projectile.CanBeHealed(healer, target, radius, canHealPlayer, specificPlayer, ignoreHealer) && projectile.ThoriumHealTarget(target, healAmount, onHealEffects, bonusHealing, ignoreSetTarget, statistics, customHealing))
					{
						break;
					}
				}
			}
			if (projectile.penetrate != 0)
			{
				ThoriumPlayer thoriumPlayer = healer.GetThoriumPlayer();
				int dummyType = ModContent.NPCType<HealingDummy>();
				for (int u = 0; u < Main.maxNPCs; u++)
				{
					NPC dummy = Main.npc[u];
					if (dummy.active && dummy.type == dummyType && dummy.DistanceSQ(projectile.Center) <= radius * radius)
					{
						int heals = healAmount;
						if (bonusHealing)
						{
							heals += thoriumPlayer.healBonus;
						}
						dummy.life += heals;
						dummy.HealEffect(heals, true);
						if (dummy.localAI[0] <= 0f)
						{
							dummy.localAI[0] = 300f;
						}
						if (projectile.penetrate > 0)
						{
							projectile.penetrate--;
						}
						if (projectile.penetrate == 0)
						{
							projectile.Kill();
							return;
						}
					}
				}
			}
		}

		public static bool IsRanged(this Projectile projectile)
		{
			return projectile.CountsAsClass(DamageClass.Ranged);
		}
		public static bool IsThrown(this Projectile projectile)
		{
			return projectile.CountsAsClass(DamageClass.Throwing);
		}

		public static void SineWaveMovement(this Projectile projectile, float timer, float amplitude, float waveStep, bool firstTick, Action<Projectile> changeDirection = null, bool reverseWave = false)
		{
			float time = timer * waveStep;
			float curHeight = (float)Math.Sin((double)time) * amplitude;
			float realSpeed;
			float realRot;
			if (firstTick)
			{
				realSpeed = projectile.velocity.Length();
				realRot = Terraria.Utils.ToRotation(projectile.velocity);
			}
			else
			{
				float heightDiff = curHeight - (float)Math.Sin((double)(time - waveStep)) * amplitude;
				realSpeed = (float)Math.Sqrt((double)(projectile.velocity.LengthSquared() - heightDiff * heightDiff));
				realRot = Terraria.Utils.ToRotation(Terraria.Utils.RotatedBy(projectile.velocity, (double)(-(double)Terraria.Utils.ToRotation(new Vector2(realSpeed, heightDiff))), default(Vector2)));
			}
			if (changeDirection != null)
			{
				projectile.velocity = Terraria.Utils.ToRotationVector2(realRot) * realSpeed;
				changeDirection(projectile);
				realRot = Terraria.Utils.ToRotation(projectile.velocity);
				realSpeed = projectile.velocity.Length();
			}
			if (reverseWave)
			{
				amplitude *= -1f;
				curHeight *= -1f;
			}
			projectile.velocity = Terraria.Utils.RotatedBy(new Vector2(realSpeed, (float)Math.Sin((double)(time + waveStep)) * amplitude - curHeight), (double)realRot, default(Vector2));
			projectile.rotation = Terraria.Utils.ToRotation(projectile.velocity) + 1.5707964f;
		}
		public static NPC FindNearestNPC(this Projectile projectile, float maxRange, bool absoluteDistance = true, bool ignoreDontTakeDamage = false, Func<NPC, bool> isValidTarget = null)
		{
			NPC nearest = null;
			if (!absoluteDistance)
			{
				maxRange *= maxRange;
			}
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];
				if (npc.CanBeChasedBy(projectile, ignoreDontTakeDamage) && (isValidTarget == null || isValidTarget(npc)))
				{
					float currentDistance = ((!absoluteDistance) ? projectile.DistanceSQ(npc.Center) : (Math.Abs(projectile.Center.X - npc.Center.X) + Math.Abs(projectile.Center.Y - npc.Center.Y)));
					if (currentDistance < maxRange && projectile.CanHitLine(npc))
					{
						maxRange = currentDistance;
						nearest = npc;
					}
				}
			}
			return nearest;
        }
        public static NPC FindNearestNPCIgnoreTiles(this Projectile projectile, float maxRange, bool absoluteDistance = true, bool ignoreDontTakeDamage = false, Func<NPC, bool> isValidTarget = null)
        {
            NPC nearest = null;
            if (!absoluteDistance)
            {
                maxRange *= maxRange;
            }
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.CanBeChasedBy(projectile, ignoreDontTakeDamage) && (isValidTarget == null || isValidTarget(npc)))
                {
                    float currentDistance = ((!absoluteDistance) ? projectile.DistanceSQ(npc.Center) : (Math.Abs(projectile.Center.X - npc.Center.X) + Math.Abs(projectile.Center.Y - npc.Center.Y)));
                    if (currentDistance < maxRange)
                    {
                        maxRange = currentDistance;
                        nearest = npc;
                    }
                }
            }
            return nearest;
        }

        public static void HomeInOnTarget(this Projectile projectile, Entity target, float maxVelocity, float velocityWeight = 0.04761905f)
		{
			Vector2 direction = target.Center - projectile.Center;
			direction *= maxVelocity / direction.Length();
			projectile.velocity *= 1f - velocityWeight;
			projectile.velocity += direction * velocityWeight;
		}

		public static bool HandleStuck(this Projectile projectile, float restingX, ref int stuckTimer, int stuckTimerMax)
		{
			if (projectile.position.X == projectile.oldPosition.X && Math.Abs(projectile.Center.X - restingX) >= 50f)
			{
				stuckTimer++;
				if (stuckTimer >= stuckTimerMax)
				{
					stuckTimer = 0;
					return true;
				}
			}
			else if (stuckTimer >= 2)
			{
				stuckTimer -= 2;
			}
			return false;
		}

		public static bool HandleChaining(this Projectile projectile, ICollection<int> hitTargets, ICollection<int> foundTargets, int max, Func<NPC, bool> condition = null)
		{
			foreach (int f in foundTargets)
			{
				if (!hitTargets.Contains(f))
				{
					hitTargets.Add(f);
				}
			}
			foundTargets.Clear();
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];
				if (npc.active && !npc.dontTakeDamage && (!projectile.friendly || !npc.townNPC))
				{
					Rectangle hitbox = new Rectangle(
					Convert.ToInt32(projectile.position.X + projectile.velocity.X),
					Convert.ToInt32(projectile.position.Y + projectile.velocity.Y),
					projectile.width,
					projectile.height
					);
					ProjectileLoader.ModifyDamageHitbox(projectile, ref hitbox);
					if (projectile.Colliding(hitbox, npc.Hitbox) && (condition == null || condition(npc)))
					{
						foundTargets.Add(i);
					}
				}
			}
			if (hitTargets.Count >= max)
			{
				projectile.Kill();
				return true;
			}
			return false;
		}

		public static bool MagicMissileAI(this Projectile projectile, ref bool homing, float channelSpeed, float releaseSpeed)
		{
			Player player = Main.player[projectile.owner];
			Vector2 mousePosition;
			if (homing && player.TryGetMousePosition(out mousePosition))
			{
				if (player.channel)
				{
					Vector2 toMouse = mousePosition - projectile.Center;
					if (toMouse.Length() > channelSpeed)
					{
						toMouse.Normalize();
						toMouse *= channelSpeed;
					}
					projectile.velocity = toMouse;
				}
				else if (Main.myPlayer == projectile.owner)
				{
					homing = false;
					projectile.netUpdate = true;
					Vector2 direction = mousePosition - projectile.Center;
					float length = direction.Length();
					if (length == 0f)
					{
						direction = projectile.Center - player.Center;
						length = direction.Length();
					}
					if (length == 0f)
					{
						projectile.Kill();
						return true;
					}
					direction.Normalize();
					direction *= releaseSpeed;
					projectile.velocity = direction;
				}
			}
			return false;
		}

		public static void FixProjectileOverlap(this Projectile projectile, float spacingMult, float idleAccel, params int[] types)
		{
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile otherProj = Main.projectile[i];
				if (i != projectile.whoAmI && otherProj.active && otherProj.owner == projectile.owner && ((types.Length == 0) ? (otherProj.type == projectile.type) : (Array.IndexOf<int>(types, otherProj.type) > -1)) && Math.Abs(projectile.position.X - otherProj.position.X) + Math.Abs(projectile.position.Y - otherProj.position.Y) < (float)projectile.width * spacingMult)
				{
					if (projectile.position.X < otherProj.position.X)
					{
						projectile.velocity.X = projectile.velocity.X - idleAccel;
					}
					else
					{
						projectile.velocity.X = projectile.velocity.X + idleAccel;
					}
					if (projectile.position.Y < otherProj.position.Y)
					{
						projectile.velocity.Y = projectile.velocity.Y - idleAccel;
					}
					else
					{
						projectile.velocity.Y = projectile.velocity.Y + idleAccel;
					}
				}
			}
		}

		public static bool Bounce(this Projectile projectile, Vector2 oldVelocity, int maxBounces, ref int currentBounces, float bouncyness = 1f)
		{
			if (currentBounces < maxBounces)
			{
				currentBounces++;
				projectile.Bounce(oldVelocity, 1f);
				return false;
			}
			return true;
		}


		public static void Bounce(this Projectile projectile, Vector2 oldVelocity, float bouncyness = 1f)
		{
			if (projectile.velocity.X != oldVelocity.X)
			{
				projectile.velocity.X = -oldVelocity.X * bouncyness;
			}
			if (projectile.velocity.Y != oldVelocity.Y)
			{
				projectile.velocity.Y = -oldVelocity.Y * bouncyness;
			}
		}

		public static void NetSync(this Projectile projectile)
		{
			projectile.netUpdate = (projectile.netUpdate2 = false);
			if (projectile.netSpam < 60)
			{
				projectile.netSpam += 5;
			}
			if (Main.netMode != 0 && projectile.owner == Main.myPlayer)
			{
				NetMessage.SendData(27, -1, -1, null, projectile.identity, 0f, 0f, 0f, 0, 0, 0);
			}
		}

		public static Projectile NetGetProjectile(int owner, int identity, int type, out int index)
		{
			short i = 0;
			while ((int)i < Main.maxProjectiles)
			{
				Projectile proj = Main.projectile[(int)i];
				if (proj.active && proj.owner == owner && proj.identity == identity && proj.type == type)
				{
					index = (int)i;
					return proj;
				}
				i += 1;
			}
			index = Main.maxProjectiles;
			return null;
		}
		internal delegate void CustomHealing(Player player, Player target, ref int heals, ref int selfHeals);
	}
}
