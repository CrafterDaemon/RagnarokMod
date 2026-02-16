using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Empowerments;
using ThoriumMod.Projectiles.Bard;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using Terraria.DataStructures;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Rarities;
using RagnarokMod.Sounds;
using ThoriumMod.Items.BardItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using RagnarokMod.Projectiles.BardPro.Percussion;

namespace RagnarokMod.Items.BardItems.Percussion
{
    public class CalamityBell : BardItem
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;

        public override void SetStaticDefaults()
        {
            Empowerments.AddInfo<MovementSpeed>(2, 0);
            Empowerments.AddInfo<FlightTime>(2, 0);
            Empowerments.AddInfo<JumpHeight>(2, 0);
        }

        public override void SetBardDefaults()
        {
            Item.damage = 165;
            InspirationCost = 2;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.knockBack = 2f;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = RagnarokModSounds.calamitybell;
            Item.shoot = ModContent.ProjectileType<CalamityBellPro>();
            Item.shootSpeed = 20f;

        }
        /*
            public override void Shoot_OnSuccess(Player player)
            {
                int numDusts = 40;
                for (int i = 0; i < numDusts; i++)
                {
                    Vector2 offset = Terraria.Utils.RotatedBy(-Terraria.Utils.RotatedBy(Vector2.UnitY, (double)((float)i * 6.2831855f / (float)numDusts), default(Vector2)), (double)Terraria.Utils.ToRotation(player.velocity), default(Vector2));
                    Dust dust = Dust.NewDustPerfect(player.Center + offset * 15f, 205, new Vector2?(offset * 6f), 0, default(Color), 1f);
                    dust.scale = 1.65f;
                    dust.noGravity = true;
                }
            }hi

            public override void SafeBardShoot(int success, int level, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
            {
                float bardHit = 1f + (float)success * 0.02f;
                if(success >= 15) 
                {
                    Projectile.NewProjectile(source, position, velocity * bardHit, ModContent.ProjectileType<CalamityBellPro2>(), (int)(damage * 1.3f) , knockback, player.whoAmI, (float)(level - 1), 0f, 0f);
                }
                else 
                {
                    Projectile.NewProjectile(source, position, velocity * bardHit, type, damage, knockback, player.whoAmI, (float)(level - 1), 0f, 0f);
                }

            }
            */
    }
}