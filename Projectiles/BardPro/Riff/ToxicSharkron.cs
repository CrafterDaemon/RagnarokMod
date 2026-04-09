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
using ThoriumMod.Projectiles.Bard;

namespace RagnarokMod.Projectiles.BardPro.Riffs
{
    public class ToxicSharkron : BardProjectile
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.String;

        public override string Texture => "CalamityMod/NPCs/OldDuke/SulphurousSharkron";

        // ai[0] = phase (0 = initial, 1 = aggressive homing)
        // ai[1] = phase timer
        // ai[2] = target NPC index

        private const float PhaseOneEnd = 40f;
        private const float PhaseTwoDie = 200f;
        private const float PhaseTwoFall = 150f;
        private const float MaxSpeed = 16f;
        private const float InitialSpeed = MaxSpeed * 0.67f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 1;
        }

        public override void SetBardDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600;
            Projectile.alpha = 255;
            Projectile.scale = 0.8f;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
        }

        public override void AI()
        {
            // Fade in
            Projectile.alpha -= 15;
            if (Projectile.alpha < 0) Projectile.alpha = 0;

            // Sprite direction from velocity (like SulphurousSharkron)
            Projectile.spriteDirection = Projectile.velocity.X < 0f ? -1 : 1;
            Projectile.rotation = (float)Math.Atan2(
                Projectile.spriteDirection == 1 ? Projectile.velocity.Y : -Projectile.velocity.Y,
                Projectile.spriteDirection == 1 ? Projectile.velocity.X : -Projectile.velocity.X);

            int targetIndex = (int)Projectile.ai[2];
            NPC target = (targetIndex >= 0 && targetIndex < Main.maxNPCs
                && Main.npc[targetIndex].active && !Main.npc[targetIndex].friendly)
                ? Main.npc[targetIndex] : null;

            if (Projectile.ai[0] == 0f)
            {
                if (Projectile.ai[1] == 0f)
                {
                    Projectile.velocity = target != null
                        ? Vector2.Normalize(target.Center - Projectile.Center) * InitialSpeed
                        : Projectile.velocity.SafeNormalize(Vector2.UnitY) * InitialSpeed;
                    SoundEngine.PlaySound(SoundID.NPCDeath19, Projectile.Center);
                    Projectile.netUpdate = true;
                }

                Projectile.ai[1]++;

                if (Projectile.ai[1] >= PhaseOneEnd * 0.5f && target != null)
                {
                    float speed = Projectile.velocity.Length();
                    Vector2 dir = Vector2.Normalize(target.Center - Projectile.Center);
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, dir * Math.Max(speed, MaxSpeed), 0.04f);
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
                Projectile.tileCollide = true;
                Projectile.ai[1]++;

                if (target != null)
                {
                    float speed = Projectile.velocity.Length();
                    Vector2 dir = Vector2.Normalize(target.Center - Projectile.Center);
                    float inertia = 22f;
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
                if (Vector2.Distance(Projectile.Center, other.Center) < 80f)
                {
                    Projectile.velocity.X += Projectile.Center.X < other.Center.X ? -0.5f : 0.5f;
                    Projectile.velocity.Y += Projectile.Center.Y < other.Center.Y ? -0.5f : 0.5f;
                }
            }

            if (Main.rand.NextBool())
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.TintableDustLighted,
                    -Projectile.velocity * 0.3f + Main.rand.NextVector2Circular(1f, 1f),
                    0, new Color(55, 195, 0), Main.rand.NextFloat(1f, 2f));
                d.noGravity = true;
            }

            Lighting.AddLight(Projectile.Center, 0.3f, 0.7f, 0.2f);
        }

        public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 16; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.TintableDustLighted,
                    Main.rand.NextVector2Circular(8f, 8f), 0, new Color(55, 195, 0), Main.rand.NextFloat(1f, 2f));
                d.noGravity = true;
            }
            for (int i = 0; i < 10; i++)
            {
                Dust blood = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Blood, 0f, 0f, 100, default, 1.5f);
                blood.velocity *= 3f;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath12 with { Volume = 0.5f }, Projectile.Center);

            for (int i = 0; i < 4; i++)
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
                for (int i = 0; i < 2; i++)
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

            int afterimageAmt = 10;
            if (CalamityClientConfig.Instance.Afterimages)
            {
                for (int i = 1; i < afterimageAmt; i += 2)
                {
                    if (Projectile.oldPos[i] == Vector2.Zero) continue;
                    Color afterimageColor = Color.Lerp(lightColor, Color.Lime, 0.5f);
                    afterimageColor = Projectile.GetAlpha(afterimageColor);
                    afterimageColor *= (afterimageAmt - i) / 15f;
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