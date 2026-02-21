using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class AuricGreatscytheSwing : ModProjectile
    {
        // Total swing duration in frames
        private int SwingDuration = 24;

        // How far through the swing to fire the bolt (0-1)
        private float BoltFirePoint = 0.6f;

        // Total arc of the swing in radians (~240 degrees)
        private float SwingArc = MathHelper.Pi * 1.33f;

        private int TrailLength = 10;

        private bool hasFiredBolt = false;
        private bool initialized = false;
        private Vector2 boltDirection;

        private Player Owner => Main.player[Projectile.owner];

        private float SwingProgress => 1f - (float)Projectile.timeLeft / SwingDuration;

        // Eased swing progress for dramatic acceleration
        private float EasedProgress
        {
            get
            {
                float t = SwingProgress;
                // Ease-in-out: slow start, fast middle, slow end
                return t < 0.5f
                    ? 4f * t * t * t
                    : 1f - MathF.Pow(-2f * t + 2f, 3f) / 2f;
            }
        }

        public override string Texture => "RagnarokMod/Items/HealerItems/Scythes/AuricGreatscythe";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = TrailLength;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 206;
            Projectile.height = 190;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = SwingDuration;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
            Projectile.ownerHitCheck = true;
            Projectile.alpha = 0;
        }

        public override bool ShouldUpdatePosition() => false;

        public override void AI()
        {
            Player player = Owner;

            if (player.dead)
            {
                Projectile.Kill();
                return;
            }

            // On first frame, capture the bolt direction from initial velocity and zero out movement
            if (!initialized)
            {
                initialized = true;
                boltDirection = Projectile.velocity.SafeNormalize(Vector2.UnitX * player.direction);
                Projectile.velocity = Vector2.Zero;
            }

            // Lock player animation
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;

            // Keep projectile centered on player
            Projectile.Center = player.Center;
            Projectile.gfxOffY = player.gfxOffY;

            // Calculate swing rotation
            // Start angle behind the player, sweep forward
            float startAngle = -SwingArc / 2f;
            float currentAngle = startAngle + SwingArc * EasedProgress;
            Projectile.rotation = currentAngle * player.direction;
            Projectile.spriteDirection = player.direction;

            // Lighting
            Lighting.AddLight(Projectile.Center, 1f, 1f, 1f);

            // Electric dust along the swing edge
            SpawnSwingDust(currentAngle, player.direction);

            // Fire the bolt at the right moment
            if (!hasFiredBolt && SwingProgress >= BoltFirePoint)
            {
                hasFiredBolt = true;
                FireBolt(player);
            }
        }

        /// <summary>
        /// Gets the position of the scythe blade tip in world space, based on current rotation.
        /// The blade tip is at the top-right of the sprite (opposite of the bottom-left handle).
        /// </summary>
        private Vector2 GetSwingTip()
        {
            Player player = Owner;
            // Distance from handle (player center) to the blade tip
            float reach = 90f; // approximate diagonal reach of the item sprite
            // Tip points toward top-right in local space: (+X, -Y) when facing right
            Vector2 tipOffset = new Vector2(reach * player.direction, -reach).RotatedBy(Projectile.rotation);
            return player.Center + tipOffset;
        }

        private void SpawnSwingDust(float angle, int direction)
        {
            Vector2 tipPos = GetSwingTip();

            for (int i = 0; i < 3; i++)
            {
                Vector2 pos = tipPos + Main.rand.NextVector2Circular(20f, 20f);
                Dust dust = Dust.NewDustDirect(pos, 1, 1, DustID.Electric, 0f, 0f, 100, default, 1.5f);
                dust.noGravity = true;
                // Velocity tangent to the swing arc
                Vector2 outward = (tipPos - Owner.Center).SafeNormalize(Vector2.Zero);
                dust.velocity = outward.RotatedBy(MathHelper.PiOver2 * direction) * 3f;
                dust.velocity += Main.rand.NextVector2Circular(1f, 1f);
            }
        }

        private void FireBolt(Player player)
        {
            if (Main.myPlayer != Projectile.owner)
                return;

            SoundEngine.PlaySound(SoundID.Item94, Projectile.Center);

            Vector2 velocity = boltDirection * 14f;
            int boltDamage = Projectile.damage; // already 3x from ModifyShootStats
            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                GetSwingTip() - new Vector2(0,50),
                velocity,
                ModContent.ProjectileType<AuricGreatscytheBolt>(),
                boltDamage,
                Projectile.knockBack,
                Projectile.owner
            );
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // Line collision from player center (handle) to blade tip
            Vector2 swingTip = GetSwingTip();
            float collisionPoint = 0f;

            return Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(),
                targetHitbox.Size(),
                Owner.Center,
                swingTip,
                40f,
                ref collisionPoint
            );
        }

        /// <summary>
        /// Gets the draw origin (pivot point) at the bottom-left of the texture (the handle).
        /// When flipped horizontally (facing left), the pivot mirrors to bottom-right.
        /// </summary>
        private Vector2 GetHandleOrigin(Texture2D texture)
        {
            // Bottom-left corner as the grip point (handle end of the scythe)
            return new Vector2(0f, texture.Height);
        }

        /// <summary>
        /// Gets the draw position. This is the player's hand position, which is where the handle sits.
        /// </summary>
        private Vector2 GetDrawPosition()
        {
            Player player = Owner;
            return player.Center - Main.screenPosition;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AuricRebuke>(), 60, false);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<AuricRebuke>(), 60, false);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 handleOrigin = GetHandleOrigin(texture);
            Vector2 drawPos = GetDrawPosition();
            SpriteEffects flip = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            // When flipped (facing left), mirror the origin X to the opposite side
            if (Projectile.spriteDirection == -1)
                handleOrigin.X = texture.Width;

            // Draw trail (previous rotations at same position)
            for (int k = 1; k < Projectile.oldPos.Length; k++)
            {
                if (Projectile.oldRot[k] == 0f && k > 1)
                    continue;

                float trailProgress = 1f - (float)k / Projectile.oldPos.Length;
                Color trailColor = lightColor * MathF.Pow(trailProgress, 1.5f) * 0.4f;
                float trailScale = Projectile.scale * (0.6f + 0.4f * trailProgress);

                Main.EntitySpriteDraw(
                    texture,
                    drawPos,
                    null,
                    trailColor,
                    Projectile.oldRot[k],
                    handleOrigin,
                    trailScale,
                    flip
                );
            }

            // Draw electric glow underneath
            Color glowColor = new Color(120, 180, 255) * 0.3f;
            Main.EntitySpriteDraw(texture, drawPos, null, glowColor, Projectile.rotation, handleOrigin, Projectile.scale * 1.15f, flip);

            // Draw main scythe sprite
            Main.EntitySpriteDraw(texture, drawPos, null, Color.White, Projectile.rotation, handleOrigin, Projectile.scale, flip);

            return false;
        }
    }
}
