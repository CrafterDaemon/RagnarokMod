using Microsoft.Xna.Framework;
using ThoriumMod.Items.BossThePrimordials.Rhapsodist;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.CameraModifiers;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
namespace RagnarokMod.Projectiles.HealerPro.Other
{
    public class Splattershot : ThoriumProjectile, ILocalizedModType
    {
        public override string Texture => "RagnarokMod/Projectiles/NoProj";

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 4f)
            {
                for (int i = 0; i < 2; i++)
                {
                    int blood = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 5, 0f, 0f, 100, new Color(255, 0, 0), 1.5f);
                    Main.dust[blood].noGravity = true;
                    Main.dust[blood].velocity *= 0f;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int spawnChance;
            if (target.life <= 0)
            {
                spawnChance = Main.rand.Next(1, 4);
            }
            else
            {
                spawnChance = Main.rand.Next(1, 9);
            }
            Vector2 SpawnVel = new Vector2(Main.rand.NextFloat(-4.0f, 4.0f), Main.rand.NextFloat(-4.0f, 4.0f) * 2);
            if (spawnChance == 1 || spawnChance == 2) { Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, SpawnVel, ModContent.ProjectileType<BloodOrb>(), 0, 0f); }
        }
    }
}