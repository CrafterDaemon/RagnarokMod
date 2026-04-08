using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Projectiles.BardPro.Riff;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;

namespace RagnarokMod.Projectiles.BardPro.String
{
    public class ToxicWavesPro : ModProjectile
    {
        public override string Texture => "CalamityMod/NPCs/OldDuke/SulphurousSharkron";


        // ai[0] = phase (0 = initial, 1 = aggressive homing)
        // ai[1] = phase timer

        private const float PhaseOneEnd = 30f;
        private const float PhaseTwoDie = 180f;
        private const float PhaseTwoFall = 130f;
        private const float MaxSpeed = 14f;
        private const float InitialSpeed = MaxSpeed * 0.67f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 8;
            ProjectileID.Sets.TrailingMode[Type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 480;
            Projectile.alpha = 255;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
        }

        private NPC FindTarget()
        {
            NPC closest = null;
            float closest_dist = 600f;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.active || npc.friendly || npc.lifeMax <= 5 || npc.dontTakeDamage) continue;
                float dist = Vector2.Distance(Projectile.Center, npc.Center);
                if (dist < closest_dist) { closest_dist = dist; closest = npc; }
            }
            return closest;
        }

        public override void AI()
        {
            // Fade in
            Projectile.alpha -= 20;
            if (Projectile.alpha < 0) Projectile.alpha = 0;

            // Sprite direction from velocity
            Projectile.spriteDirection = Projectile.velocity.X < 0f ? -1 : 1;
            Projectile.rotation = (float)Math.Atan2(
                Projectile.spriteDirection == 1 ? Projectile.velocity.Y : -Projectile.velocity.Y,
                Projectile.spriteDirection == 1 ? Projectile.velocity.X : -Projectile.velocity.X);

            NPC target = FindTarget();

            if (Projectile.ai[0] == 0f)
            {
                // Phase 1: fly in shot direction, then loosely home
                Projectile.ai[1]++;

                if (Projectile.ai[1] >= PhaseOneEnd * 0.5f && target != null)
                {
                    float speed = Projectile.velocity.Length();
                    Vector2 dir = Vector2.Normalize(target.Center - Projectile.Center);
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, dir * Math.Max(speed, MaxSpeed), 0.04f);
                    Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitY) * Math.Max(speed, MaxSpeed);
                }

                if (Projectile.ai[1] >= PhaseOneEnd && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                {
                    Projectile.ai[0] = 1f;
                    Projectile.ai[1] = 0f;
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                // Phase 2: aggressive homing with tile collision
                Projectile.tileCollide = true;
                Projectile.ai[1]++;

                if (target != null)
                {
                    float speed = Projectile.velocity.Length();
                    Vector2 dir = Vector2.Normalize(target.Center - Projectile.Center);
                    float inertia = 20f;
                    Projectile.velocity = (Projectile.velocity * (inertia - 1f) + dir * Math.Max(speed, MaxSpeed)) / inertia;
                    Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitY) * Math.Max(speed, MaxSpeed);
                }
                else
                {
                    Projectile.velocity.Y += 0.15f;
                    if (Projectile.velocity.Y > MaxSpeed) Projectile.velocity.Y = MaxSpeed;
                }

                if (Projectile.ai[1] >= PhaseTwoFall)
                    Projectile.velocity.Y += 0.3f;

                if (Projectile.ai[1] >= PhaseTwoDie)
                    Projectile.Kill();
            }

            // Same-type push separation
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile other = Main.projectile[i];
                if (i == Projectile.whoAmI || !other.active || other.type != Type) continue;
                if (Vector2.Distance(Projectile.Center, other.Center) < 60f)
                {
                    Projectile.velocity.X += Projectile.Center.X < other.Center.X ? -0.4f : 0.4f;
                    Projectile.velocity.Y += Projectile.Center.Y < other.Center.Y ? -0.4f : 0.4f;
                }
            }

            // Acid trail
            if (Main.rand.NextBool())
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.TintableDustLighted,
                    -Projectile.velocity * 0.2f + Main.rand.NextVector2Circular(0.5f, 0.5f),
                    0, new Color(55, 195, 0), Main.rand.NextFloat(0.8f, 1.4f));
                d.noGravity = true;
            }

            Lighting.AddLight(Projectile.Center, 0.2f, 0.5f, 0.1f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.TintableDustLighted,
                    Main.rand.NextVector2Circular(6f, 6f), 0, new Color(55, 195, 0), Main.rand.NextFloat(0.8f, 1.5f));
                d.noGravity = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath12 with { Volume = 0.5f }, Projectile.Center);

            for (int i = 0; i < 12; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.TintableDustLighted,
                    Main.rand.NextVector2Circular(6f, 6f), 0, new Color(55, 195, 0), Main.rand.NextFloat(0.8f, 1.5f));
                d.noGravity = true;
            }
            for (int i = 0; i < 8; i++)
            {
                Dust blood = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Blood, 0f, 0f, 100, default, 1.2f);
                blood.velocity.Y *= 4f;
                blood.velocity.X *= 2f;
            }

            // Spawn gore shards like SulphurousSharkron
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int spawnRadius = Projectile.width / 2;
                for (int i = 0; i < 6; i++)
                {
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center.X + Main.rand.Next(-spawnRadius, spawnRadius),
                        Projectile.Center.Y,
                        Main.rand.Next(-3, 4),
                        Main.rand.Next(-12, -6),
                        ModContent.ProjectileType<ToxicSharkronGore>(),
                        Projectile.damage / 3,
                        0f,
                        Projectile.owner
                    );
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            SpriteEffects spriteEffects = Projectile.spriteDirection == -1
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[Type].Value;
            int frameCount = Main.projFrames[Type];
            int frameHeight = texture.Height / frameCount;
            Rectangle frameRect = new Rectangle(0, Projectile.frame * frameHeight, texture.Width, frameHeight);
            Vector2 halfSize = frameRect.Size() / 2f;

            int afterimageAmt = Projectile.oldPos.Length;
            if (CalamityClientConfig.Instance.Afterimages)
            {
                for (int i = 1; i < afterimageAmt; i += 2)
                {
                    if (Projectile.oldPos[i] == Vector2.Zero) continue;
                    Color afterimageColor = Color.Lerp(lightColor, Color.Lime, 0.5f);
                    afterimageColor = Projectile.GetAlpha(afterimageColor);
                    afterimageColor *= (afterimageAmt - i) / (float)afterimageAmt;
                    afterimageColor.A = 0;
                    Vector2 afterimagePos = Projectile.oldPos[i] + new Vector2(Projectile.width, Projectile.height) / 2f - Main.screenPosition;
                    spriteBatch.Draw(texture, afterimagePos, frameRect, afterimageColor, Projectile.rotation, halfSize, Projectile.scale, spriteEffects, 0f);
                }
            }

            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            spriteBatch.Draw(texture, drawPos, frameRect, Projectile.GetAlpha(lightColor), Projectile.rotation, halfSize, Projectile.scale, spriteEffects, 0f);

            return false;
        }
    }
}