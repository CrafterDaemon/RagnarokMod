using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using CalamityMod.Items.Accessories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Bard;
using ThoriumMod.Projectiles.Scythe;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class ExecutionerMark05ThrowPro : ModProjectile, ILocalizedModType
    {
        public bool LockedIn = false;

        public Vector2 boomSpot = Vector2.Zero;
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 150;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.Size = new Vector2(50f, 50f);
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 0;
        }
        public override void AI()
        {
            if (Projectile.direction == 1)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
                Projectile.spriteDirection = Projectile.direction;
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI - MathHelper.ToRadians(45f);
                Projectile.spriteDirection = Projectile.direction;
            }
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.SnowSpray, 0f, 0f, 100, default, 1f);
            dust.scale = 0.1f + Main.rand.Next(5);
            dust.fadeIn = 1.5f + Main.rand.Next(5) * 0.1f;
            dust.noGravity = true;
            dust.position = Projectile.Center + new Vector2(1, 0).RotatedBy(Projectile.rotation - 2.1f, default) * 10f;
            NPC npc = Projectile.FindNearestNPC(500);
            if (npc != null)
            {
                Projectile.HomeInOnTarget(npc, 22f, 0.15f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);
        }

        public override void OnKill(int timeLeft)
        {
            Mod calamity = ModLoader.GetMod("CalamityMod");
            int supernovaBoomType = calamity.Find<ModProjectile>("SupernovaBoom")?.Type ?? -1;

            if (supernovaBoomType != -1)
            {
                int damage = (int)(Projectile.damage * 2.25);

                int index = Projectile.NewProjectile(
                    Projectile.GetSource_Death(),
                    Projectile.Center,
                    Vector2.Zero,
                    supernovaBoomType,
                    damage,
                    Projectile.knockBack,
                    Projectile.owner
                );

                if (index >= 0 && index < Main.maxProjectiles)
                {
                    Projectile newProj = Main.projectile[index];
                    newProj.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
                    newProj.localAI[0] = 1337f;

                    // --- SCALE THE PROJECTILE ---
                    float scaleFactor = 1.5f;
                    Vector2 originalSize = new Vector2(newProj.width, newProj.height);
                    Vector2 oldCenter = newProj.Center;

                    newProj.scale *= scaleFactor;
                    newProj.width = (int)(originalSize.X * scaleFactor);
                    newProj.height = (int)(originalSize.Y * scaleFactor);
                    newProj.Center = oldCenter;
                }
            }

            SoundEngine.PlaySound(new SoundStyle("CalamityMod/Sounds/Item/SupernovaBoom", (SoundType)0));
        }
    }
}
