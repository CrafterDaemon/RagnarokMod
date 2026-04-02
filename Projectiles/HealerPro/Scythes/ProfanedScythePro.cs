using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Scythe;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class ProfanedScythePro : ScythePro
    {
        public override void SafeSetDefaults()
        {
            base.Projectile.Size = new Vector2(362, 384);
            dustOffset = new Vector2(-80f, 32f);
            dustCount = 4;
            dustType = 87;
            base.rotationSpeed = 0.25f;
            Projectile.light = 1f;
            fadeOutSpeed = 30;

            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 6;
        }

        public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 300);
        }

        public override bool PreAI()
        {
            Vector2 DustCenterBase = new Vector2(Projectile.width / 2f, Projectile.height / 2f);

            if (Projectile.ai[0] != 0f) // Thrown mode
            {
                Player player = Main.player[Projectile.owner];
                player.heldProj = Projectile.whoAmI;

                // Face the correct way based on aim
                player.ChangeDir(Projectile.velocity.X < 0f ? 1 : -1);
                Projectile.spriteDirection = player.direction;

                // Spin faster while thrown
                Projectile.rotation += 0.25f * Projectile.spriteDirection * 2.5f;

                // Keep Projectile alive for full ai[1] duration
                if (Projectile.ai[1] - Projectile.ai[2] > Projectile.timeLeft)
                    Projectile.timeLeft++;

                float attackTime = ++Projectile.ai[2] / Projectile.ai[1];
                float v = Projectile.velocity.Length();

                // Orbit multipliers (width = horizontal, height = vertical)
                float widthScale = 1.4f;
                float heightScale = 1.5f;

                // Base orbit circle
                Vector2 orbit = Vector2.UnitX.RotatedBy(attackTime * MathHelper.TwoPi);

                // Apply scales: velocity length × custom × Projectile.scale
                orbit *= new Vector2(
                    v * widthScale,
                    v * heightScale * Math.Max(Projectile.ai[0], 0.001f) * Projectile.spriteDirection
                );

                orbit = orbit.RotatedBy(Projectile.velocity.ToRotation());
                Projectile.Center = player.MountedCenter + orbit - Projectile.velocity;

                // Play sound when rotation loops
                if (Projectile.rotation > Math.PI)
                {
                    SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                    Projectile.rotation -= MathHelper.TwoPi;
                }
                else if (Projectile.rotation < -Math.PI)
                {
                    SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                    Projectile.rotation += MathHelper.TwoPi;
                }

                SpawnDust(Projectile);

                // Increment localAI counter
                Projectile.localAI[1]++;

                // Spawn MoltenBlob every 4 ticks
                if (Projectile.localAI[1] >= 4f)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        float speed = 24f;

                        float randomAngle = Main.rand.NextFloat(0f, MathHelper.TwoPi);

                        Vector2 randomDirection = new Vector2((float)Math.Cos(randomAngle), (float)Math.Sin(randomAngle));

                        Vector2 randomVelocity = randomDirection * speed;

                        if (Projectile.owner == Main.myPlayer)
                        {
                            int proj = Projectile.NewProjectile(
                                Projectile.GetSource_FromThis(),
                                Projectile.Center,
                                randomVelocity,
                                ModContent.ProjectileType<CalamityMod.Projectiles.Boss.MoltenBlob>(),
                                (int)(Projectile.damage * 0.25f),
                                1f,
                                Projectile.owner
                            );

                            if (proj >= 0 && Main.projectile.IndexInRange(proj))
                            {
                                Main.projectile[proj].DamageType = ThoriumDamageBase<HealerDamage>.Instance;
                                Main.projectile[proj].friendly = true;
                                Main.projectile[proj].hostile = false;
                                Main.projectile[proj].tileCollide = true;
                                Main.projectile[proj].timeLeft = 90;

                                Main.projectile[proj].usesLocalNPCImmunity = true;
                                Main.projectile[proj].localNPCHitCooldown = 20;

                                Main.projectile[proj].usesIDStaticNPCImmunity = false;
                            }
                        }
                    }

                    // Reset the counter
                    Projectile.localAI[1] = 0f;
                }

                return false; // Skip normal scythe AI
            }

            return true; // Normal swing AI for left-click
        }

        // -------- Dust spawning (unchanged except for using new DustCenterBase) --------
        private void SpawnDust(Projectile Projectile)
        {
            int num = dustCount;
            int num2 = scytheCount;
            int num3 = dustType;
            Vector2 dustCenter = DustCenter;
            if (num2 <= 0 || num <= 0 || num3 <= -1)
                return;

            for (int i = 0; i < num2; i++)
            {
                float num4 = (float)i * ((float)Math.PI * 2f / (float)num2);
                float rotation = Projectile.rotation;
                Vector2 val = dustCenter;
                if (Projectile.spriteDirection < 0)
                    val.X = 0f - val.X;

                val = Terraria.Utils.RotatedBy(val, (double)(rotation + num4), default);
                Vector2 val2 = Projectile.Center + new Vector2(0f, Projectile.gfxOffY) + val;
                for (int j = 0; j < num; j++)
                {
                    Dust val3 = Dust.NewDustPerfect(val2, num3, (Vector2)Vector2.Zero, 0, default(Color), 1f);
                    val3.noGravity = true;
                    val3.noLight = true;
                    ModifyDust(Projectile, val3, val2, i);
                }
            }
        }

        public void ModifyDust(Projectile Projectile, Dust dust, Vector2 position, int scytheIndex)
        {
            if (Projectile.ModProjectile is ScythePro scythePro)
                scythePro.ModifyDust(dust, position, scytheIndex);
        }
    }
}
