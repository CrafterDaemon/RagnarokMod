using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Utils;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Projectiles.Bard;
using ThoriumMod.Utilities;

namespace RagnarokMod.Projectiles.BardPro.String
{
    public class ToxicWavesPro : BardProjectile, ILocalizedModType
    {
        public override string Texture => "CalamityMod/Projectiles/Boss/SandPoisonCloudOldDuke";
        public override BardInstrumentType InstrumentType => BardInstrumentType.String;
        public int collide;
        public int collideMax = 1;
        public int duration = 300;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 10;
            ProjectileID.Sets.TrailCacheLength[Type] = 2;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }

        public override void SetBardDefaults()
        {
            Projectile.width = 45;
            Projectile.height = 45;
            Projectile.friendly = true;
            Projectile.hostile = false;
            base.Projectile.aiStyle = ProjAIStyleID.Bubble;
            base.AIType = ProjectileID.Bullet;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = duration;
            Projectile.alpha = 0;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.penetrate += Main.player[Projectile.owner].GetThoriumPlayer().bardBounceBonus;
        }
        public override void AI()
        {
            Projectile.spriteDirection = 1;
            if (Projectile.Center.X < Main.LocalPlayer.Center.X) Projectile.spriteDirection = -1;

            Lighting.AddLight(Projectile.Center, 0.1f, 0.7f, 0f);

            Projectile.ai[0] += 1f;
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.ai[0] < duration - 180)
            {
                GeneralParticleHandler.SpawnParticle(new GlowOrbParticle(Projectile.Top + new Vector2(Main.rand.NextFloat(-12, 12), 6f), new Vector2(0, -Main.rand.NextFloat(2)), false, 20, Main.rand.NextFloat(0.5f, 1.2f), new Color(100, 255, 0)));

                if (Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }
            if (Projectile.ai[0] > duration - 180)
            {
                Projectile.damage = 0;
            }
            else if (Projectile.frame >= Main.projFrames[Type])
            {
                Projectile.Kill();
            }

            Projectile.velocity *= 0.995f;

            if (Math.Abs(Projectile.velocity.X) > 0f)
            {
                Projectile.spriteDirection = -Projectile.direction;
            }
        }

        public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<SulphuricPoisoning>(), 300);
            target.AddBuff(ModContent.BuffType<Irradiated>(), 300);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDustDirect(base.Projectile.position, base.Projectile.width, base.Projectile.height, DustID.Poisoned, (float)Main.rand.Next(-3, 3), (float)Main.rand.Next(-3, 3), 0, default(Color), 1.25f).noGravity = true;
            }
            return base.Projectile.Bounce(oldVelocity, this.collideMax + Main.player[base.Projectile.owner].GetThoriumPlayer().bardBounceBonus, ref this.collide, 1f);

        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor.R = (byte)(100 * Projectile.Opacity);
            lightColor.G = (byte)(155 * Projectile.Opacity);
            lightColor.B = (byte)(55 * Projectile.Opacity);
            lightColor.A = 0;
            CalamityUtils.DrawProjectileWithBackglow(Projectile, new Color(20, 60, 26, 0), Color.White, 2f);
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}