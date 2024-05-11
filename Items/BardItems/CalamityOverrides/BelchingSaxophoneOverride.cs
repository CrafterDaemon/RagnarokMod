﻿using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Empowerments;
using ThoriumMod;
using ThoriumMod.Items;
using CalamityMod.Items;
using CalamityMod;

namespace Ragnarok.Items
{
    public class BelchingSaxophoneOverride : BardItem, ILocalizedModType
    {
        //belching sax as a bard instrument, had to be rewritten iniherriting from BardItem, same with the other overrides.
        public override string Texture => "CalamityMod/Items/Weapons/Magic/BelchingSaxophone";
        public override BardInstrumentType InstrumentType => BardInstrumentType.Wind;

        public override void SetStaticDefaults()
        {
            Empowerments.AddInfo<DamageReduction>(1);
            Empowerments.AddInfo<Defense>(1);
        }

        public const int BaseDamage = 32;
        private int counter = 0;

        public override void SetBardDefaults()
        {
            Item.damage = BaseDamage;
            InspirationCost = 1;
            Item.width = 46;
            Item.height = 22;
            Item.useTime = 12;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = 3500;
            Item.rare = ItemRarityID.Pink;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AcidicReed>();
            Item.shootSpeed = 20f;
        }

        public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            counter++;

            if (Main.rand.NextBool())
            {
                Vector2 speed = velocity.RotatedBy(MathHelper.ToRadians(Main.rand.Next(-15, 16)));
                speed.Normalize();
                speed *= 15f;
                speed.Y -= Math.Abs(speed.X) * 0.2f;
                Projectile.NewProjectile(source, position, speed, ModContent.ProjectileType<AcidicSaxBubble>(), damage, knockback, player.whoAmI);
            }

            velocity.X += Main.rand.Next(-40, 41) * 0.05f;
            velocity.Y += Main.rand.Next(-40, 41) * 0.05f;
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, counter % 2 == 0 ? 1f : 0f);

            if (Main.rand.NextBool())
            {
                int noteProj = Utils.SelectRandom(Main.rand, new int[]
                {
                    ProjectileID.QuarterNote,
                    ProjectileID.EighthNote,
                    ProjectileID.TiedEighthNote
                });
                int note = Projectile.NewProjectile(source, position.X, position.Y, velocity.X * 0.75f, velocity.Y * 0.75f, noteProj, (int)(damage * 0.75), knockback, player.whoAmI);
                if (note.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[note].DamageType = BardDamage.Instance;
                    Main.projectile[note].usesLocalNPCImmunity = true;
                    Main.projectile[note].localNPCHitCooldown = 10;
                }
            }
            return false;
        }

        public override Vector2? HoldoutOffset() => new Vector2(0, 18);
    }
}
