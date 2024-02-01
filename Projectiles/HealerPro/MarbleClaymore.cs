using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using static Humanizer.In;

namespace RagnarokMod.Projectiles.HealerPro
{
    public class MarbleClaymore : ModItem
    {
        public override void SetDefaults()
        {
            base.Item.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            base.Item.autoReuse = true;
            base.Item.useTime = 44;
            base.Item.useAnimation = 22;
            base.Item.maxStack = 1;
            base.Item.knockBack = 6.5f;
            base.Item.useStyle = ItemUseStyleID.Swing;
            base.Item.UseSound = SoundID.Item1;
            base.Item.shootSpeed = 0.1f;
            base.Item.damage = 18;
            base.Item.width = 54;
            base.Item.height = 42;
            base.Item.value = Item.sellPrice(0, 0, 27);
            base.Item.rare = ItemRarityID.Orange;
            base.Item.shoot = ModContent.ProjectileType<MarbleStar>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(in SoundID.Item8, player.Center);
            float num = velocity.Length();
            for (int i = 0; i < 2; i++)
            {
                float fallspeedmult = 50;
                float projSpeed = num * Main.rand.NextFloat(0.7f, 1.4f);
                float x = Main.MouseWorld.X + Main.rand.NextFloat(0f - 50f, 50f);
                float y = Main.MouseWorld.Y - Main.rand.NextFloat(650f, 850f);
                Vector2 vector = new Vector2(x, y);
                Vector2 vel = Main.MouseWorld - vector;
                vel.X += Main.rand.NextFloat(0f - 130f, 130f);
                vel.Y += Main.rand.NextFloat(0f - 260f, 260f);
                float n = vel.Length();
                n = projSpeed/n;
                vel.X *= n;
                vel.Y *= n*fallspeedmult;
                Projectile.NewProjectileDirect(source, vector, vel, ModContent.ProjectileType<MarbleStar>(), damage, knockback, player.whoAmI);
            }

            return false;
        }
    }
}
