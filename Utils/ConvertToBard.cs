//using System;
//using System.Collections.Generic;
//using System.IO;
//using Microsoft.Xna.Framework;
//using Terraria;
//using Terraria.Audio;
//using Terraria.DataStructures;
//using Terraria.ID;
//using Terraria.Localization;
//using Terraria.ModLoader;
//using Terraria.Utilities;
//using ThoriumMod;
//using ThoriumMod.Buffs;
//using ThoriumMod.Buffs.Healer;
//using ThoriumMod.Empowerments;
//using ThoriumMod.Items;
//using ThoriumMod.Items.BardItems;
//using ThoriumMod.Prefixes.BardPrefixes;
//using ThoriumMod.Projectiles.Bard;
//using ThoriumMod.Utilities;
//using static Terraria.NPC;

//namespace RagnarokMod.Utils;

//public class BardItem : ThoriumItem
//{
//    public BitsByte NetInfo;

//    public BitsByte HitInfo;

//    [CloneByReference]
//    public EmpowermentList Empowerments;

//    public virtual bool CanChangePitch => true;

//    public virtual bool PlayOnUse => true;

//    public virtual bool EmpowerOnUse => true;

//    public virtual bool ShowEmpowerments => true;

//    public virtual BardItemType ItemType => BardItemType.Instrument;

//    public virtual BardInstrumentType InstrumentType => BardInstrumentType.Other;

//    public int InspirationCost { get; protected set; }

//    public short DurationBonus { get; internal set; }

//    [CloneByReference]
//    public SoundStyle? PlayedSound { get; private set; }

//    public virtual void SetBardDefaults()
//    {
//    }

//    public virtual bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
//    {
//        return true;
//    }

//    public virtual float PlayingSpeedMultiplier(Player player)
//    {
//        return 1f;
//    }

//    public virtual bool CanPlayInstrument(Player player)
//    {
//        return true;
//    }

//    public virtual void BardUseAnimation(Player player)
//    {
//    }

//    public virtual void BardHoldItem(Player player)
//    {
//    }

//    public virtual void BardModifyHitNPC(ThoriumPlayer bard, NPC target, ref HitModifiers modifiers)
//    {
//    }

//    public virtual void BardOnHitNPC(ThoriumPlayer bard, NPC target, HitInfo hit, int damageDone)
//    {
//    }

//    public virtual void BardHitNPCEffect(Player player, NPC target, BitsByte netInfo)
//    {
//    }

//    public virtual void ModifyInspirationCost(Player player, ref int cost)
//    {
//    }

//    public virtual bool CanApplyEmpowerment(Player player, Player target, int group, int empowerment)
//    {
//        return true;
//    }

//    public virtual void ModifyEmpowermentPool(Player player, Player target, EmpowermentPool empPool)
//    {
//    }

//    public virtual void GetEmpowermentRange(Player player, Player target, ref float range)
//    {
//    }

//    public virtual void ModifyEmpowerment(ThoriumPlayer player, ThoriumPlayer target, byte type, ref byte level, ref short duration)
//    {
//    }

//    public virtual void OnPlayInstrument(Player player)
//    {
//    }

//    public virtual void ModifyInstrumentDamage(ThoriumPlayer bard, ref StatModifier damage)
//    {
//    }

//    public virtual void GetInstrumentCrit(ThoriumPlayer bard, ref float crit)
//    {
//    }

//    public virtual void BardTooltips(List<TooltipLine> tooltips)
//    {
//    }

//    public virtual void BardModifyTooltips(List<TooltipLine> tooltips)
//    {
//    }

//    public sealed override void AutoStaticDefaults()
//    {
//        ((ModItem)this).AutoStaticDefaults();
//        if (((ModItem)this).Item.useStyle != 0)
//        {
//            Empowerments = EmpowermentList.CreateList();
//        }
//    }

//    public sealed override void SetDefaults()
//    {
//        SetBardDefaults();
//        PlayedSound = ((ModItem)this).Item.UseSound;
//        ((ModItem)this).Item.UseSound = null;
//        ((ModItem)this).Item.DamageType = (DamageClass)(object)ThoriumDamageBase<BardDamage>.Instance;
//    }

//    public sealed override void ModifyWeaponDamage(Player player, ref StatModifier damage)
//    {
//        //IL_0019: Unknown result type (might be due to invalid IL or missing references)
//        //IL_0023: Unknown result type (might be due to invalid IL or missing references)
//        //IL_0028: Unknown result type (might be due to invalid IL or missing references)
//        base.ModifyWeaponDamage(player, ref damage);
//        ThoriumPlayer bard = player.GetThoriumPlayer();
//        if (bard.accPitchHelper)
//        {
//            damage *= 0f;
//        }
//        ModifyInstrumentDamage(bard, ref damage);
//    }

//    public sealed override void ModifyWeaponCrit(Player player, ref float crit)
//    {
//        ((ModItem)this).ModifyWeaponCrit(player, ref crit);
//        ThoriumPlayer bard = player.GetThoriumPlayer();
//        GetInstrumentCrit(bard, ref crit);
//    }

//    public sealed override float UseSpeedMultiplier(Player player)
//    {
//        return PlayingSpeedMultiplier(player);
//    }

//    public sealed override bool CanUseItem(Player player)
//    {
//        if (!CanPlayInstrument(player))
//        {
//            return false;
//        }
//        ThoriumPlayer bard = player.GetThoriumPlayer();
//        if (!bard.accPitchHelper)
//        {
//            return bard.bardResource >= GetInspirationCost(player);
//        }
//        return true;
//    }

//    public sealed override void UseAnimation(Player player)
//    {
//        BardUseAnimation(player);
//        if (((ModItem)this).Item.UseSound.HasValue)
//        {
//            PlayedSound = ((ModItem)this).Item.UseSound;
//            ((ModItem)this).Item.UseSound = null;
//        }
//        ConsumeInspiration(player, GetInspirationCost(player));
//        if (Main.myPlayer == ((Entity)player).whoAmI && ItemType == BardItemType.Instrument && !PlayOnUse && ((ModItem)this).Item.useStyle != 0)
//        {
//            PlayInstrument(player);
//        }
//    }

//    private static ModPacket GetPacket(int capacity = 256)
//    {
//        return ModLoader.GetMod("ThoriumMod").GetPacket(capacity);
//    }
//    public static void PlayInstrumentEmpowermentSend(Player player, int itemType, int toWho = -1, int fromWho = -1)
//    {
//        ModPacket packet = GetPacket();
//        packet.Write((byte)54);
//        packet.Write((byte)player.whoAmI);
//        packet.Write7BitEncodedInt(itemType);
//        packet.Send(toWho, fromWho);
//    }
//    private void UseItem_Inner(Player player)
//    {
//        if (ItemType != BardItemType.Instrument)
//        {
//            return;
//        }
//        if (EmpowerOnUse)
//        {
//            if (((Entity)player).whoAmI == Main.myPlayer)
//            {
//                ApplyEmpowerments(player);
//            }
//            if (((Entity)player).whoAmI == Main.myPlayer && Main.netMode != 0)
//            {
//                PlayInstrumentEmpowermentSend(player, ((ModItem)this).Item.type);
//            }
//        }
//        if (Main.netMode != 2)
//        {
//            OnPlayInstrument(player);
//        }
//        if (Main.myPlayer == ((Entity)player).whoAmI && PlayOnUse && ((ModItem)this).Item.useStyle != 0])
//        {
//            PlayInstrument(player);
//        }
//    }

//    public sealed override bool? UseItem(Player player)
//    {
//        bool? flag = BardUseItem(player);
//        if (flag != false)
//        {
//            UseItem_Inner(player);
//        }
//        return flag ?? base.UseItem(player);
//    }

//    public virtual bool? BardUseItem(Player player)
//    {
//        return null;
//    }

//    public sealed override void HoldItem(Player player)
//    {
//        BardHoldItem(player);
//    }

//    public sealed override void ModifyHitNPC(Player player, NPC target, ref HitModifiers modifiers)
//    {
//        //IL_002a: Unknown result type (might be due to invalid IL or missing references)
//        //IL_0034: Unknown result type (might be due to invalid IL or missing references)
//        //IL_0039: Unknown result type (might be due to invalid IL or missing references)
//        //IL_0066: Unknown result type (might be due to invalid IL or missing references)
//        //IL_006b: Unknown result type (might be due to invalid IL or missing references)
//        //IL_0070: Unknown result type (might be due to invalid IL or missing references)
//        //IL_004a: Unknown result type (might be due to invalid IL or missing references)
//        //IL_0054: Unknown result type (might be due to invalid IL or missing references)
//        //IL_0059: Unknown result type (might be due to invalid IL or missing references)
//        ThoriumPlayer bard = player.GetThoriumPlayer();
//        BardModifyHitNPC(bard, target, ref modifiers);
//        if (InstrumentType == BardInstrumentType.Percussion)
//        {
//            ref StatModifier critDamageBonus = ref bard.bardPercussionCritDamage;
//            if (bard.accPercussionTuner2)
//            {
//                critDamageBonus += 0.1f;
//            }
//            else if (bard.accPercussionTuner1)
//            {
//                critDamageBonus += 0.05f;
//            }
//            modifiers.CritDamage = ((StatModifier)(ref modifiers.CritDamage)).CombineWith(critDamageBonus);
//        }
//    }

//    public sealed override void OnHitNPC(Player player, NPC target, HitInfo hit, int damageDone)
//    {
//        //IL_0052: Unknown result type (might be due to invalid IL or missing references)
//        //IL_0141: Unknown result type (might be due to invalid IL or missing references)
//        //IL_0439: Unknown result type (might be due to invalid IL or missing references)
//        //IL_053e: Unknown result type (might be due to invalid IL or missing references)
//        //IL_0687: Unknown result type (might be due to invalid IL or missing references)
//        //IL_068c: Unknown result type (might be due to invalid IL or missing references)
//        //IL_0694: Unknown result type (might be due to invalid IL or missing references)
//        //IL_069c: Unknown result type (might be due to invalid IL or missing references)
//        //IL_06b7: Unknown result type (might be due to invalid IL or missing references)
//        //IL_06ba: Unknown result type (might be due to invalid IL or missing references)
//        //IL_06a7: Unknown result type (might be due to invalid IL or missing references)
//        //IL_06c6: Unknown result type (might be due to invalid IL or missing references)
//        //IL_06f0: Unknown result type (might be due to invalid IL or missing references)
//        ThoriumPlayer bard = player.GetThoriumPlayer();
//        IEntitySource source = ((Entity)player).GetSource_OnHit((Entity)(object)target, (string)null);
//        Item heldItem = player.HeldItem;
//        int projSpawnDamage = ((heldItem != null && !heldItem.IsAir) ? player.GetWeaponDamage(player.HeldItem, false) : ((HitInfo)(ref hit)).SourceDamage);
//        bool tuneEffect = false;
//        bool muteEffect = false;
//        bool noteEffect = false;
//        if (InstrumentType == BardInstrumentType.Percussion)
//        {
//            if (hit.Crit && (bard.accPercussionTuner1 || bard.accPercussionTuner2))
//            {
//                tuneEffect = true;
//                if (bard.accPercussionTuner2)
//                {
//                    target.AddBuff(ModContent.BuffType<Stunned>(), 90, false);
//                }
//                else if (bard.accPercussionTuner1)
//                {
//                    target.AddBuff(ModContent.BuffType<Stunned>(), 60, false);
//                }
//            }
//            if (bard.accKickPedal.Active && Utils.NextFloat(Main.rand) <= 0.25f)
//            {
//                int petalType = ModContent.ProjectileType<KickPetalPro>();
//                for (int k = 0; k < Main.maxProjectiles; k++)
//                {
//                    Projectile petal = Main.projectile[k];
//                    if (((Entity)petal).active && petal.owner == ((Entity)player).whoAmI && petal.ai[1] != 1f && petal.type == petalType)
//                    {
//                        petal.ai[1] = 1f;
//                        petal.NetSync();
//                        break;
//                    }
//                }
//            }
//        }
//        else if (InstrumentType == BardInstrumentType.Brass)
//        {
//            if (hit.Crit && (bard.accBrassMute1 || bard.accBrassMute2))
//            {
//                muteEffect = true;
//                float angleDiff = 0.0875f;
//                float speedMult = Utils.NextFloat(Main.rand) * 0.2f + 0.95f;
//                Vector2 velocity = Utils.RotatedBy(((Entity)player).velocity, (double)(0f - angleDiff), default(Vector2)) * speedMult * new Vector2(1f, 1.45f);
//                if (bard.accBrassMute2)
//                {
//                    for (int l = -2; l <= 2; l++)
//                    {
//                        bool isEven = l % 2 == 0;
//                        Projectile.NewProjectile(source, ((Entity)target).Center, velocity, isEven ? ModContent.ProjectileType<MuteBurst1>() : ModContent.ProjectileType<MuteBurst2>(), (int)((float)projSpawnDamage * (isEven ? 1.5f : 1.75f)), isEven ? 2f : 4f, ((Entity)player).whoAmI, 0f, 0f, 0f);
//                        velocity = Utils.RotatedBy(velocity, (double)(angleDiff / 2f), default(Vector2));
//                    }
//                }
//                else if (bard.accBrassMute1)
//                {
//                    for (int m = -1; m <= 1; m++)
//                    {
//                        Projectile.NewProjectile(source, ((Entity)target).Center, velocity, ModContent.ProjectileType<MuteBurst1>(), (int)((float)projSpawnDamage * 1.5f), 2f, ((Entity)player).whoAmI, 0f, 0f, 0f);
//                        velocity = Utils.RotatedBy(velocity, (double)angleDiff, default(Vector2));
//                    }
//                }
//                if (bard.accBrassCap && Utils.NextFloat(Main.rand) <= 0.15f)
//                {
//                    target.AddBuff(ModContent.BuffType<Tuned>(), 300, false);
//                    Projectile.NewProjectile(source, ((Entity)target).Center, Vector2.Zero, ModContent.ProjectileType<BongoEffect>(), 0, 0f, ((Entity)player).whoAmI, 0f, 0f, 0f);
//                    for (int k4 = 0; k4 < 10; k4++)
//                    {
//                        Dust.NewDustDirect(((Entity)target).Center, 10, 10, 87, Utils.NextFloat(Main.rand, -3f, 3f), Utils.NextFloat(Main.rand, -3f, 3f), 75, default(Color), 1f).noGravity = true;
//                    }
//                }
//            }
//        }
//        else if (InstrumentType == BardInstrumentType.Wind && bard.accFabergeEgg && target.IsBossOrRelated())
//        {
//            bard.accFabergeEggHit++;
//            if (bard.accFabergeEggHit >= 25)
//            {
//                Projectile.NewProjectile(source, ((Entity)target).Center, Vector2.Zero, ModContent.ProjectileType<FabergeEggPro>(), 0, 0f, ((Entity)player).whoAmI, 0f, 0f, 0f);
//                bard.accFabergeEggHit = 0;
//            }
//        }
//        int note2 = ModContent.ProjectileType<MixtapeNote>();
//        int note3 = ModContent.ProjectileType<MixtapeNote2>();
//        if (bard.accMixtape || bard.accDissTrack)
//        {
//            if (bard.accMixtape)
//            {
//                target.AddBuff(ModContent.BuffType<Singed>(), 120, false);
//            }
//            if (bard.accDissTrack)
//            {
//                target.AddBuff(153, 120, false);
//            }
//            if (hit.Crit)
//            {
//                noteEffect = true;
//                int shot = Main.rand.Next(3);
//                if (bard.accMixtape)
//                {
//                    for (int k3 = 0; k3 < 6; k3++)
//                    {
//                        Projectile.NewProjectile(source, ((Entity)target).Center, new Vector2(Utils.NextFloat(Main.rand, -6f, 6f), Utils.NextFloat(Main.rand, -6f, 6f)), note2, projSpawnDamage / 4, 2f, ((Entity)player).whoAmI, (float)shot, 0f, 0f);
//                    }
//                }
//                if (bard.accDissTrack)
//                {
//                    for (int k2 = 0; k2 < 10; k2++)
//                    {
//                        Projectile.NewProjectile(source, ((Entity)target).Center, new Vector2(Utils.NextFloat(Main.rand, -10f, 10f), Utils.NextFloat(Main.rand, -10f, 10f)), note3, projSpawnDamage / 2, 3f, ((Entity)player).whoAmI, (float)shot, 0f, 0f);
//                    }
//                }
//            }
//        }
//        if (bard.setJester && hit.Crit)
//        {
//            for (int j = 0; j < Main.maxProjectiles; j++)
//            {
//                Projectile bell = Main.projectile[j];
//                if (((Entity)bell).active && bell.owner == ((Entity)player).whoAmI && bell.type == ModContent.ProjectileType<JestersBell>())
//                {
//                    bell.Kill();
//                }
//            }
//            float maxRange = 500 + bard.bardRangeBoost;
//            maxRange *= maxRange;
//            for (int i = 0; i < Main.maxNPCs; i++)
//            {
//                NPC victim = Main.npc[i];
//                if (victim.CanBeChasedBy((object)null, false) && ((Entity)victim).DistanceSQ(((Entity)player).Center) < maxRange)
//                {
//                    victim.AddBuff(ModContent.BuffType<DistortedTimeEnemy>(), 90, false);
//                }
//            }
//            tuneEffect = true;
//            Projectile.NewProjectile(source, ((Entity)player).Center.X, ((Entity)player).Center.Y - 50f, 0f, 0f, ModContent.ProjectileType<JestersBell>(), 0, 0f, ((Entity)player).whoAmI, 0f, 0f, 0f);
//        }
//        if (bard.accShockAbsorber.Active && target.IsHostile())
//        {
//            bard.accShockAbsorberStorage += (ushort)Math.Min(damageDone, 65535);
//        }
//        BitsByte effects = default(BitsByte);
//        ((BitsByte)(ref effects))._002Ector(tuneEffect, muteEffect, noteEffect, false, false, false, false, false);
//        HitInfo = BitsByte.op_Implicit((byte)0);
//        BardOnHitNPC(bard, target, hit, damageDone);
//        if (BitsByte.op_Implicit(effects) > 0 || BitsByte.op_Implicit(NetInfo) > 0)
//        {
//            NetHitNPCEffect(player, target, effects, HitInfo);
//        }
//        BitsByte empowerBools = default(BitsByte);
//        ((BitsByte)(ref empowerBools))._002Ector(hit.Crit, bard.accAutoTuner && Utils.NextBool(Main.rand, 10), false, false, false, false, false, false);
//        EmpowermentLoader.OnHitEmpowerments(player, target, empowerBools);
//        if (((Entity)target).active)
//        {
//            return;
//        }
//        if (InstrumentType == BardInstrumentType.Wind && bard.accFabergeEgg)
//        {
//            Projectile.NewProjectile(source, ((Entity)target).Center, Vector2.Zero, ModContent.ProjectileType<FabergeEggPro>(), 0, 0f, ((Entity)player).whoAmI, 0f, 0f, 0f);
//        }
//        if (bard.accFullScore)
//        {
//            int clapDamage = (int)((float)projSpawnDamage * 0.25f);
//            int clapType = ModContent.ProjectileType<ClapPro>();
//            for (int n = 0; n < 4; n++)
//            {
//                int clap = Projectile.NewProjectile(source, ((Entity)target).Center.X + (float)Main.rand.Next(-20, 21), ((Entity)target).Center.Y + (float)Main.rand.Next(-20, 21), 0f, 0f, clapType, clapDamage, 1f, ((Entity)player).whoAmI, 0f, 0f, 0f);
//                Main.projectile[clap].timeLeft = 20 + 15 * n;
//            }
//        }
//        if (target.damage <= 0 || target.GetThoriumGlobalNPC().netSpawnedFromStatue)
//        {
//            return;
//        }
//        float noteChance = 0.1f + bard.bardResourceDrop;
//        if (!(Utils.NextFloat(Main.rand) <= noteChance))
//        {
//            return;
//        }
//        int item;
//        if (!bard.armInspirator)
//        {
//            item = ((!bard.setNoble) ? Item.NewItem(source, ((Entity)target).Hitbox, ModContent.ItemType<InspirationNote>(), 1, false, 0, false, false) : Item.NewItem(source, ((Entity)target).Hitbox, ModContent.ItemType<InspirationNoteNoble>(), 1, false, 0, false, false));
//        }
//        else
//        {
//            item = Item.NewItem(source, ((Entity)target).Hitbox, ModContent.ItemType<InspirationNoteRhapsodist>(), 1, false, 0, false, false);
//            Item obj = Main.item[item];
//            if (((obj != null) ? obj.ModItem : null) is InspirationNoteRhapsodist note)
//            {
//                note.OwnerRangeBonus = bard.bardRangeBoost;
//                note.OwnerDurationBonus = bard.bardBuffDuration;
//            }
//        }
//        if (Main.netMode == 1)
//        {
//            NetMessage.SendData(21, -1, -1, (NetworkText)null, item, 1f, 0f, 0f, 0, 0, 0);
//        }
//    }

//    public sealed override void HitNPCEffect(Player player, NPC target, BitsByte netInfo, BitsByte netInfo2)
//    {
//        //IL_0066: Unknown result type (might be due to invalid IL or missing references)
//        if (((BitsByte)(ref netInfo))[0])
//        {
//            SoundEngine.PlaySound(ref SoundID.Item35, (Vector2?)((Entity)target).Center, (SoundUpdateCallback)null);
//        }
//        if (((BitsByte)(ref netInfo))[1])
//        {
//            SoundEngine.PlaySound(ref SoundID.Item42, (Vector2?)((Entity)player).Center, (SoundUpdateCallback)null);
//        }
//        if (((BitsByte)(ref netInfo))[2])
//        {
//            SoundEngine.PlaySound(ref SoundID.Item73, (Vector2?)((Entity)target).Center, (SoundUpdateCallback)null);
//        }
//        BardHitNPCEffect(player, target, netInfo2);
//    }

//    public sealed override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
//    {
//        return BardShoot(player, source, position, velocity, type, damage, knockback);
//    }

//    public sealed override void ModifyTooltips(List<TooltipLine> tooltips)
//    {
//        //IL_0114: Unknown result type (might be due to invalid IL or missing references)
//        //IL_0119: Unknown result type (might be due to invalid IL or missing references)
//        //IL_0139: Expected O, but got Unknown
//        //IL_00a2: Unknown result type (might be due to invalid IL or missing references)
//        //IL_00a7: Unknown result type (might be due to invalid IL or missing references)
//        //IL_00c7: Expected O, but got Unknown
//        //IL_01b6: Unknown result type (might be due to invalid IL or missing references)
//        //IL_01c0: Expected O, but got Unknown
//        //IL_0284: Unknown result type (might be due to invalid IL or missing references)
//        //IL_0289: Unknown result type (might be due to invalid IL or missing references)
//        //IL_0290: Unknown result type (might be due to invalid IL or missing references)
//        //IL_02a1: Expected O, but got Unknown
//        base.ModifyTooltips(tooltips);
//        Player player = Main.LocalPlayer;
//        ThoriumPlayer bard = player.GetThoriumPlayer();
//        if (ThoriumConfigClient.Instance.ShowInstrumentTypeTags && InstrumentType != 0)
//        {
//            int index3 = tooltips.FindIndex((TooltipLine tt) => tt.Mod.Equals("Terraria") && tt.Name.Equals("ItemName"));
//            if (index3 != -1)
//            {
//                tooltips.Insert(index3 + 1, new TooltipLine(((ModType)this).Mod, "InstrumentTag", $"-{ThoriumLocalization.GetEnumText(InstrumentType)} Instrument-")
//                {
//                    OverrideColor = new Color(0, 255, 128)
//                });
//            }
//        }
//        if (ThoriumConfigClient.Instance.ShowClassTags)
//        {
//            int index2 = tooltips.FindIndex((TooltipLine tt) => tt.Mod.Equals("Terraria") && tt.Name.Equals("ItemName"));
//            if (index2 != -1)
//            {
//                tooltips.Insert(index2 + 1, new TooltipLine(((ModType)this).Mod, "BardTag", "-Bard Class-")
//                {
//                    OverrideColor = new Color(0, 255, 128)
//                });
//            }
//        }
//        int cost = GetInspirationCost(player);
//        if (cost > 0 && !BigInstrumentItemBase.BigInstruments.ContainsKey(((ModItem)this).Item.type))
//        {
//            if (bard.accPitchHelper)
//            {
//                cost = 0;
//            }
//            int index = tooltips.FindIndex((TooltipLine tt) => (tt.Mod.Equals("Terraria") || tt.Mod.Equals("ThoriumMod")) && ((((ModItem)this).Item.damage > 0 && tt.Name.Equals("Knockback")) || tt.Name.Equals("Tooltip0")));
//            if (index != -1)
//            {
//                tooltips.Insert(index + ((((ModItem)this).Item.damage > 0) ? 1 : 0), new TooltipLine(((ModType)this).Mod, "InspirationCost", "Uses " + cost + " inspiration"));
//            }
//        }
//        if (((ModItem)this).Item.useStyle != 0 && ShowEmpowerments)
//        {
//            Empowerments.ModifyTooltips(tooltips, ItemType, Utils.PressingShift(Main.keyState));
//            BardTooltips(tooltips);
//        }
//        if (((ModItem)this).Item.prefix >= PrefixID.Count && BardPrefix.BardPrefixes.Contains(((ModItem)this).Item.prefix) && DurationBonus != 0)
//        {
//            int ttindex = tooltips.FindLastIndex((TooltipLine t) => (t.Mod == "Terraria" || t.Mod == ((ModType)this).Mod.Name) && (t.IsModifier || t.Name.StartsWith("Tooltip") || t.Name.Equals("Material") || t.Name.StartsWith("Empowerment")));
//            if (ttindex != -1)
//            {
//                TooltipLine tt2 = new TooltipLine(((ModType)this).Mod, "PrefixEmpowermentDuration", ((DurationBonus > 0) ? "+" : "") + DurationBonus / 60 + "s empowerment duration")
//                {
//                    IsModifier = true,
//                    IsModifierBad = (DurationBonus < 0)
//                };
//                tooltips.Insert(ttindex + 1, tt2);
//            }
//        }
//        BardModifyTooltips(tooltips);
//    }

//    public override int ChoosePrefix(UnifiedRandom rand)
//    {
//        if (BardPrefix.DoConditionsApply(((ModItem)this).Item))
//        {
//            return Utils.Next<int>(rand, (IList<int>)BardPrefix.BardPrefixes);
//        }
//        return ((ModItem)this).ChoosePrefix(rand);
//    }

//    public override void GrabRange(Player player, ref int grabRange)
//    {
//        if (ItemType == BardItemType.InspirationNote)
//        {
//            ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
//            if (thoriumPlayer.buffInspirationReachPotion)
//            {
//                grabRange += 500 - Player.defaultItemGrabRange;
//            }
//            if (thoriumPlayer.empowerGrabRange > 1f && thoriumPlayer.bardResource < thoriumPlayer.bardResourceMax2)
//            {
//                grabRange += (int)((float)Player.defaultItemGrabRange * (thoriumPlayer.empowerGrabRange - 1f));
//            }
//        }
//    }

//    public override bool GrabStyle(Player player)
//    {
//        if (ItemType == BardItemType.InspirationNote && player.GetThoriumPlayer().buffInspirationReachPotion)
//        {
//            float speed = 10f;
//            Vector2 direction = Utils.SafeNormalize(((Entity)player).Center - ((Entity)((ModItem)this).Item).Center, -Vector2.UnitY);
//            int accelMult = 5;
//            ((Entity)((ModItem)this).Item).velocity = (((Entity)((ModItem)this).Item).velocity * (accelMult - 1) + direction * speed) / accelMult;
//            return false;
//        }
//        return ((ModItem)this).GrabStyle(player);
//    }

//    public int GetInspirationCost(Player player)
//    {
//        int cost = InspirationCost;
//        ModifyInspirationCost(player, ref cost);
//        int animationTime = CombinedHooks.TotalAnimationTime((float)((ModItem)this).Item.useAnimation, player, ((ModItem)this).Item);
//        int useTime = CombinedHooks.TotalUseTime((float)((ModItem)this).Item.useTime, player, ((ModItem)this).Item);
//        if (cost > 0)
//        {
//            return cost * (int)Math.Ceiling((double)animationTime / (double)useTime);
//        }
//        return 0;
//    }

//    public void PlayInstrument(Player player, float pitch = 0f)
//    {
//        //IL_0065: Unknown result type (might be due to invalid IL or missing references)
//        if (Sets.SkipsInitialUseSound[((ModItem)this).Item.type])
//        {
//            if (CanChangePitch && ((Entity)player).whoAmI == Main.myPlayer && PlayedSound.HasValue)
//            {
//                ModifyPitch(((Entity)player).Center, Main.MouseWorld, ref pitch);
//            }
//            if (PlayedSound.HasValue)
//            {
//                PlaySound(player, PlayedSound.Value, pitch);
//            }
//            if (Main.netMode == 2 || (Main.netMode == 1 && ((Entity)player).whoAmI == Main.myPlayer))
//            {
//                ModPacket packet = ((ModType)this).Mod.GetPacket(256);
//                ((BinaryWriter)(object)packet).Write((byte)53);
//                ((BinaryWriter)(object)packet).Write((byte)((Entity)player).whoAmI);
//                ((BinaryWriter)(object)packet).Write(pitch);
//                packet.Send(-1, ((Entity)player).whoAmI);
//            }
//        }
//    }

//    public static bool ConsumeInspiration(Player player, int cost, bool pay = true)
//    {
//        ThoriumPlayer bard = player.GetThoriumPlayer();
//        if (cost <= 0 || bard.accPitchHelper)
//        {
//            return true;
//        }
//        if (bard.bardResource < cost)
//        {
//            return false;
//        }
//        if (pay)
//        {
//            bard.inspirationRegenTimer = -60f;
//            bard.inspirationRegenBase = 0f;
//            if (((Entity)player).whoAmI == Main.myPlayer && bard.inspirationConsume > 0f && (bard.inspirationConsume >= 1f || Utils.NextFloat(Main.rand) < bard.inspirationConsume))
//            {
//                CombatText.NewText(((Entity)player).Hitbox, new Color(225, 200, 255), "Freebie!", false, true);
//                bard.netBardResource = true;
//            }
//            else
//            {
//                bard.bardResource -= cost;
//            }
//        }
//        return true;
//    }

//    public void ApplyEmpowerments(Player player)
//    {
//        if (!player.GetThoriumPlayer().accPitchHelper)
//        {
//            ApplyEmpowerments(player, Main.LocalPlayer);
//        }
//    }

//    public void NetApplyEmpowerments(Player player, BitsByte info)
//    {
//        //IL_0085: Unknown result type (might be due to invalid IL or missing references)
//        //IL_0086: Unknown result type (might be due to invalid IL or missing references)
//        //IL_0064: Unknown result type (might be due to invalid IL or missing references)
//        if (player.GetThoriumPlayer().accPitchHelper)
//        {
//            return;
//        }
//        if ((Main.netMode == 1 && ((Entity)player).whoAmI == Main.myPlayer) || Main.netMode == 2)
//        {
//            ModPacket packet = ((ModType)this).Mod.GetPacket(256);
//            ((BinaryWriter)(object)packet).Write((byte)57);
//            ((BinaryWriter)(object)packet).Write((byte)((Entity)player).whoAmI);
//            ((BinaryWriter)(object)packet).Write7BitEncodedInt(((ModItem)this).Item.type);
//            ((BinaryWriter)(object)packet).Write(BitsByte.op_Implicit(info));
//            packet.Send(-1, ((Entity)player).whoAmI);
//            if (Main.netMode == 2)
//            {
//                return;
//            }
//        }
//        NetInfo = info;
//        ApplyEmpowerments(player);
//    }

//    private void ApplyEmpowerments(Player player, Player target)
//    {
//        ThoriumPlayer bard = player.GetThoriumPlayer();
//        if (Main.netMode == 2 || !((Entity)target).active || target.dead || bard.accPitchHelper || (((Entity)target).whoAmI != ((Entity)player).whoAmI && target.hostile && (target.team != player.team || target.team == 0)))
//        {
//            return;
//        }
//        float maxRange = 500 + bard.bardRangeBoost;
//        GetEmpowermentRange(player, target, ref maxRange);
//        if (maxRange <= 0f)
//        {
//            return;
//        }
//        maxRange *= maxRange;
//        if (((Entity)target).DistanceSQ(((Entity)player).Center) > maxRange)
//        {
//            return;
//        }
//        EmpowermentPool empPool = new EmpowermentPool();
//        if (Empowerments != null)
//        {
//            for (int i = 0; i < Empowerments.Count; i++)
//            {
//                for (int j = 0; j < Empowerments[i].Count; j++)
//                {
//                    if (CanApplyEmpowerment(player, target, i, j))
//                    {
//                        empPool.Add(Empowerments[i][j]);
//                    }
//                }
//            }
//        }
//        ModifyEmpowermentPool(player, target, empPool);
//        if (empPool.Count == 0)
//        {
//            return;
//        }
//        ThoriumPlayer bardTarget = target.GetThoriumPlayer();
//        short duration = (short)((float)(300 + bard.bardBuffDuration + DurationBonus) * bard.bardBuffDurationX);
//        foreach (var item in (IEnumerable<(byte, byte)>)empPool)
//        {
//            byte type = item.Item1;
//            byte tempLevel = item.Item2;
//            short tempDuration = duration;
//            ModifyEmpowerment(bard, bardTarget, type, ref tempLevel, ref tempDuration);
//            EmpowermentLoader.ApplyEmpowerment(bard, bardTarget, type, tempLevel, tempDuration);
//        }
//    }

//    internal static void ModifyPitch(Vector2 from, Vector2 to, ref float pitch)
//    {
//        Vector2 toMouse = to - from;
//        pitch = (toMouse / new Vector2((float)Main.screenWidth * 0.4f, (float)Main.screenHeight * 0.4f)).Length();
//        if (pitch > 1f)
//        {
//            pitch = 1f;
//        }
//        pitch = pitch * 2f - 1f;
//    }

//    internal static void PlaySound(Player player, SoundStyle soundStyle, float pitch)
//    {
//        //IL_0025: Unknown result type (might be due to invalid IL or missing references)
//        //IL_002a: Unknown result type (might be due to invalid IL or missing references)
//        //IL_002e: Unknown result type (might be due to invalid IL or missing references)
//        //IL_0033: Unknown result type (might be due to invalid IL or missing references)
//        SoundStyle val = ((SoundStyle)(ref soundStyle)).WithVolumeScale((((Entity)player).whoAmI == Main.myPlayer) ? ThoriumConfigClient.Instance.InstrumentSoundVolume : ThoriumConfigClient.Instance.OthersInstrumentSoundVolume);
//        val = ((SoundStyle)(ref val)).WithPitchOffset(pitch);
//        SoundEngine.PlaySound(ref val, (Vector2?)((Entity)player).Center, (SoundUpdateCallback)null);
//    }
//}
