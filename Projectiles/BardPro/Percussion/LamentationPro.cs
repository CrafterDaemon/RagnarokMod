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
using CalamityMod.Projectiles.Boss;
using RagnarokMod.Utils;

namespace RagnarokMod.Projectiles.BardPro.Percussion
{
    public class LamentationPro : BardProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/Boss/SCalBrimstoneFireblast";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            Main.projFrames[Projectile.type] = 5;
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
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.alpha = 255;
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
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300, false);
        }


        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(Color.White * (0.25f + 0.01f * Projectile.timeLeft));
        }


        public override void AI()
        {
            base.Projectile.frameCounter++;
            if (base.Projectile.frameCounter > 4)
            {
                base.Projectile.frame++;
                base.Projectile.frameCounter = 0;
            }
            if (base.Projectile.frame >= 5)
            {
                base.Projectile.frame = 0;
            }

            NPC target = base.Projectile.FindNearestNPC(700f, true, false, null);
            if (target != null)
            {
                base.Projectile.HomeInOnTarget(target, 25f, 0.04761905f);
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 20;
            height = 20;
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SCalBrimstoneFireblast.ImpactSound, new Vector2?(base.Projectile.Center), null);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(null), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<LamentationBoom>(), (int)(Projectile.damage * 0.75f), 1f, Projectile.owner, 0f, 0f, 0f);
            for (int j = 0; j < 10; j++)
            {
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GemRuby, Main.rand.Next(-4, 5), Main.rand.Next(-4, 5), 0, default, 1.25f).noGravity = true;
            }
        }
    }
}
