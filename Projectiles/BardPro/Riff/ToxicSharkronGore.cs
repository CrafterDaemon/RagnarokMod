using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.NPCs.OldDuke;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Projectiles.Bard;

namespace RagnarokMod.Projectiles.BardPro.Riff
{
    // Friendly version of OldDukeGore, spawned by ToxicSharkron on death.
    public class ToxicSharkronGore : BardProjectile
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.String;

        public override string Texture => "CalamityMod/Projectiles/Boss/OldDukeGore";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 4;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }

        public override void SetBardDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 420;
            Projectile.alpha = 255;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
        }

        public override void AI()
        {
            Lighting.AddLight((int)((Projectile.position.X + (Projectile.width / 2)) / 16f), (int)((Projectile.position.Y + (Projectile.height / 2)) / 16f), 0.5f, 0.4f, 0f);

            Projectile.alpha -= 50;
            if (Projectile.alpha < 0)
                Projectile.alpha = 0;

            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= 15f)
                Projectile.velocity.Y += 0.1f;

            if (Projectile.velocity.Y > 12f)
                Projectile.velocity.Y = 12f;

            Projectile.tileCollide = Projectile.timeLeft < 300;

            Projectile.rotation += Projectile.velocity.X * 0.1f;

            MediumMistParticle mist2 = new MediumMistParticle(Projectile.Center, Projectile.velocity, OldDuke.GlowColor, Color.DarkSlateBlue, Main.rand.NextFloat(1f), 200f);
            mist2.AffectedByLight = true;

            GeneralParticleHandler.SpawnParticle(new HeavySmokeParticle(Projectile.Center, (Projectile.velocity / 2) + new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)), Color.DarkRed, 20, Main.rand.NextFloat(0.2f, 1f), 0.2f, MathHelper.ToRadians(Main.rand.NextFloat(-2f, 2f)), affectedByLight: true));
            GeneralParticleHandler.SpawnParticle(mist2);

            int blood = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 0f, 0f, 100, default, 1f);
            Main.dust[blood].noGravity = true;
            Main.dust[blood].velocity *= 0f;
        }

        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < 3; i++)
            {
                GeneralParticleHandler.SpawnParticle(new PointParticle(Projectile.Center + (Projectile.velocity * 2f), Projectile.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-20, 20))) * Main.rand.NextFloat(3f), true, 8, Main.rand.NextFloat(1f, 2f), Color.DarkRed.MultiplyRGBA(new Color(0.3f, 0.3f, 0.3f, 0.3f)), false, true));
            }
        }

        public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<SulphuricPoisoning>(), 300);
            target.AddBuff(ModContent.BuffType<Irradiated>(), 300);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath12, Projectile.Center);

            for (int i = 0; i < 15; i++)
            {
                GeneralParticleHandler.SpawnParticle(new PointParticle(Projectile.Center, new Vector2(Main.rand.NextFloat(10), 0).RotatedByRandom(MathHelper.TwoPi), true, 10, Main.rand.NextFloat(0.5f, 1.5f), Color.DarkRed, false, true));
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Type], lightColor, 1);
            return false;
        }
    }
}