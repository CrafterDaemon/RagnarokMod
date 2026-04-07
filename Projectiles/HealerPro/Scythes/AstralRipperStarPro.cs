using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class AstralRipperStarPro : ModProjectile, ILocalizedModType
    {
        private Color astralRed = new Color(237, 93, 83);
        private Color astralCyan = new Color(66, 189, 181);
        public const float MaxRadius = 150f;
        public const float GrowthRate = 2.5f;  // pixels per frame toward max radius
        public const float OrbitSpeed = 0.06f; // radians per frame while growing
        public const float StandbyOrbitSpeed = 0.06f;
        private const float ReleaseSpeed = 14f;

        // ai[0] = current orbit angle
        // ai[1] = scythe whoAmI, -2 = ready (at max radius), -1 = released
        // ai[2] = current radius

        private bool Ready => Projectile.localAI[0] == 1f;
        private bool Released => Projectile.localAI[0] == 2f;

        private bool _exploded = false;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 4;
            ProjectileID.Sets.TrailingMode[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 44;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.localNPCHitCooldown = -1; // explodes on first hit
        }

        public override void AI()
        {
            if (Released)
            {
                Projectile.ai[0] += StandbyOrbitSpeed;
                Vector2 tangent = new Vector2(
                    -System.MathF.Sin(Projectile.ai[0]),
                     System.MathF.Cos(Projectile.ai[0])
                );
                Projectile.velocity = tangent * ReleaseSpeed * 1.01f;
                Projectile.rotation += 0.3f;
                SpawnDust(fast: true);
                Lighting.AddLight(Projectile.Center, new Vector3(0.9f, 0.5f, 0.1f));
                return;
            }

            int scytheIndex = (int)Projectile.ai[1];
            Projectile scythe = scytheIndex >= 0 ? Main.projectile[scytheIndex] : null;
            bool scytheGone = scythe == null || !scythe.active
                || scythe.type != ModContent.ProjectileType<AstralRipperPro>();

            bool shouldRelease = scytheGone || (scythe != null && scythe.ai[2] == 1f);

            if (shouldRelease)
            {
                // Fire tangentially from current position
                Vector2 tangent = new Vector2(
                    -System.MathF.Sin(Projectile.ai[0]),
                     System.MathF.Cos(Projectile.ai[0])
                );
                Projectile.velocity = tangent * ReleaseSpeed;
                Projectile.localAI[0] = 2f;
                Projectile.timeLeft = 120;
                Projectile.netUpdate = true;
                return;
            }

            // Growing outward toward max radius
            if (!Ready)
            {
                Projectile.ai[2] = System.Math.Min(Projectile.ai[2] + GrowthRate, MaxRadius);
                Projectile.ai[0] += OrbitSpeed;
                // Anchor to player center (stars emanate from player)
                Player owner = Main.player[Projectile.owner];
                Projectile.Center = owner.Center + new Vector2(
                    System.MathF.Cos(Projectile.ai[0]),
                    System.MathF.Sin(Projectile.ai[0])
                ) * Projectile.ai[2];

                Projectile.velocity = Vector2.Zero;

                // Mark as ready when max radius reached
                if (Projectile.ai[2] >= MaxRadius)
                {
                    Projectile.localAI[0] = 1f;
                }
            }
            else
            {
                // Holding at max radius, wait for release signal
                Player owner = Main.player[Projectile.owner];
                Projectile.ai[0] += StandbyOrbitSpeed;
                Projectile.Center = owner.Center + new Vector2(
                    System.MathF.Cos(Projectile.ai[0]),
                    System.MathF.Sin(Projectile.ai[0])
                ) * MaxRadius;
                Projectile.velocity = Vector2.Zero;
                Projectile.timeLeft++;
            }

            Projectile.rotation += 0.15f;
            SpawnDust(fast: false);
            Lighting.AddLight(Projectile.Center, new Vector3(0.1f, 0.3f, 0.9f));
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 120);
            if (Released)
                Explode();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 120);
            if (Released)
                Explode();
        }

        public override void OnKill(int timeLeft)
        {
            if (Released)
                Explode();
        }

        private void Explode()
        {
            if (_exploded) return;
            _exploded = true;

            // Starburst of alternating astral dust
            for (int i = 0; i < 20; i++)
            {
                float angle = MathHelper.TwoPi * i / 20f;
                int dType = i % 2 == 0
                    ? ModContent.DustType<AstralBlue>()
                    : ModContent.DustType<AstralOrange>();
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center,
                    dType,
                    angle.ToRotationVector2() * Main.rand.NextFloat(4f, 9f),
                    0, default, 1.2f
                );
                d.noGravity = true;
            }
        }

        private void SpawnDust(bool fast)
        {
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            // Use a round texture instead of pixel
            Texture2D glowTexture = TextureAssets.Extra[ExtrasID.ThePerfectGlow].Value; // Falling star sparkle

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            // Alternating color star
            Color starColor = (Main.GameUpdateCount % 20 < 10) ? astralRed : astralCyan;

            // Draw star with bloom
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Vector2 origin = glowTexture.Size() / 2f;

            // Outer glow
            spriteBatch.Draw(glowTexture, drawPos, null, starColor * 0.5f, 0f, origin, 1.5f, SpriteEffects.None, 0f);
            spriteBatch.Draw(glowTexture, drawPos, null, starColor * 0.5f, MathHelper.PiOver2, origin, 1.5f, SpriteEffects.None, 0f);

            // Mid glow
            spriteBatch.Draw(glowTexture, drawPos, null, starColor * 0.8f, 0f, origin, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(glowTexture, drawPos, null, starColor * 0.8f, MathHelper.PiOver2, origin, 1f, SpriteEffects.None, 0f);

            // Bright core
            spriteBatch.Draw(glowTexture, drawPos, null, Color.White, 0f, origin, 0.5f, SpriteEffects.None, 0f);
            spriteBatch.Draw(glowTexture, drawPos, null, Color.White, MathHelper.PiOver2, origin, 0.5f, SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}