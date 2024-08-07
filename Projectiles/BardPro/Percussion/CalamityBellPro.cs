using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Projectiles.Bard;
using ThoriumMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.NPCs.CalClone;

namespace RagnarokMod.Projectiles.BardPro.Percussion
{
    public class CalamityBellPro : BardProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override BardInstrumentType InstrumentType
        {
            get
            {
                return BardInstrumentType.Percussion;
            }
        }

        public override void SetBardDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.alpha = 255;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 240;
            forwardRotation = true;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            if (Projectile.ai[1] > 0f)
            {
                hitbox.Inflate(8, 8);
            }
        }

        public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120, false);
            if (target.type == ModContent.NPCType<CalamitasClone>() || target.type == ModContent.NPCType<Cataclysm>() || target.type == ModContent.NPCType<Catastrophe>())
            {
                damageDone = (int)(damageDone * 1.5f);
            }
        }


        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(Color.White * (0.25f + 0.01f * Projectile.timeLeft));
        }


        public override void AI()
        {
            int dustType = 90;
            int numDusts = 3;
            for (int k = 0; k < numDusts; k++)
            {
                Vector2 offset = Projectile.velocity / numDusts * k;
                Dust.NewDustPerfect(Projectile.Center - offset, dustType, new Vector2?(Vector2.Zero), 0, default, 1f).noGravity = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int j = 0; j < 10; j++)
            {
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GemRuby, Main.rand.Next(-4, 5), Main.rand.Next(-4, 5), 0, default, 1.25f).noGravity = true;
            }
        }
    }
}
