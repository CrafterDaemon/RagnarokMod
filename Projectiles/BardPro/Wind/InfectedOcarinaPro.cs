using System;
using Microsoft.Xna.Framework;
using RagnarokMod.Utils;
using Terraria;
using Terraria.ID;
using ThoriumMod.Projectiles.Bard;

namespace RagnarokMod.Projectiles.BardPro.Wind
{
    public class InfectedOcarinaPro : ForestOcarinaPro2
    {
        public override void AI()
        {
            Projectile.SineWaveMovement(Projectile.ai[0], 6f, MathF.PI / 15f, Projectile.ai[0] == 0f, BardProjectile.HomeInHelper);
            Projectile.ai[0] += 1f;
            Vector2 value = Projectile.velocity.SafeNormalize(Vector2.Zero) * 2f;
            Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.CursedTorch, value);
            dust.noGravity = true;
            dust.fadeIn = 1.4f;
            dust.scale = 1.15f;
            Dust dust2 = Dust.NewDustPerfect(Projectile.Center - Projectile.velocity * 0.5f, DustID.CursedTorch, value);
            dust2.noGravity = true;
            dust2.fadeIn = 1.4f;
            dust2.scale = 1.15f;
            DelegateMethods.v3_1 = new Vector3(0.15f, 0.6f, 0.2f);
            Terraria.Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * 4f, 40f, DelegateMethods.CastLightOpen);
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.CursedTorch, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f)).noGravity = true;
            }
        }
    }
}