using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class AstralRipperStarfall : ModProjectile, ILocalizedModType
    {
        public override string Texture => "RagnarokMod/Projectiles/HealerPro/Scythes/AstralRipperStarPro";
        private Color astralRed = new Color(237, 93, 83);
        private Color astralCyan = new Color(66, 189, 181);

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 6;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 44;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            // Play falling sound once on spawn
            if (Projectile.localAI[1] == 0f)
            {
                Projectile.localAI[1] = 1f;
                SoundEngine.PlaySound(SoundID.Item9 with { Pitch = -0.5f, Volume = 0.6f }, Projectile.Center);
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Projectile.velocity.Y = System.Math.Min(Projectile.velocity.Y + 0.3f, 26f);

            if (Main.rand.NextBool(3))
            {
                int dType = Main.rand.NextBool()
                    ? ModContent.DustType<AstralBlue>()
                    : ModContent.DustType<AstralOrange>();
                Dust d = Dust.NewDustPerfect(Projectile.Center, dType,
                    -Projectile.velocity * 0.15f + Main.rand.NextVector2Circular(1f, 1f),
                    0, default, 0.9f);
                d.noGravity = true;
            }

            Lighting.AddLight(Projectile.Center, new Vector3(0.4f, 0.2f, 0.6f));
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 120);
            Explode();
        }

        public override void OnKill(int timeLeft)
        {
            Explode();
        }

        private void Explode()
        {
            for (int i = 0; i < 14; i++)
            {
                float angle = MathHelper.TwoPi * i / 14f;
                int dType = i % 2 == 0
                    ? ModContent.DustType<AstralBlue>()
                    : ModContent.DustType<AstralOrange>();
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center, dType,
                    angle.ToRotationVector2() * Main.rand.NextFloat(3f, 7f),
                    0, default, 1.1f);
                d.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D tex = TextureAssets.Extra[ExtrasID.ThePerfectGlow].Value;

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            Color starColor = (Main.GameUpdateCount % 20 < 10) ? astralRed : astralCyan;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Vector2 origin = tex.Size() / 2f;

            // Draw trail
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float alpha = (1f - i / (float)Projectile.oldPos.Length) * 0.5f;
                Vector2 trailPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                sb.Draw(tex, trailPos, null, starColor * alpha, Projectile.oldRot[i],
                    origin, 1.0f * alpha, SpriteEffects.None, 0f);
            }

            // Core
            sb.Draw(tex, drawPos, null, starColor * 0.5f, 0f, origin, 1.5f, SpriteEffects.None, 0f);
            sb.Draw(tex, drawPos, null, starColor * 0.5f, MathHelper.PiOver2, origin, 1.5f, SpriteEffects.None, 0f);
            sb.Draw(tex, drawPos, null, starColor * 0.8f, 0f, origin, 1.0f, SpriteEffects.None, 0f);
            sb.Draw(tex, drawPos, null, starColor * 0.8f, MathHelper.PiOver2, origin, 1.0f, SpriteEffects.None, 0f);
            sb.Draw(tex, drawPos, null, Color.White, 0f, origin, 0.45f, SpriteEffects.None, 0f);

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}