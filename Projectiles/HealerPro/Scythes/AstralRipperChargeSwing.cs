using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class AstralRipperChargeSwing : ModProjectile, ILocalizedModType
    {
        public override string Texture => "RagnarokMod/Items/HealerItems/Scythes/AstralRipper";

        private const int MaxCharge = 180; // 3 s full charge
        private const int SwingDuration = 25; // enough frames for a smooth 270° arc
        private const float ArmLength = 72f;
        private const float SweepArc = MathHelper.Pi * 1.5f; // 270°

        // Hit window as fraction of swing
        private const float WindupEnd = 0.28f;
        private const float FollowthroughStart = 0.82f;

        // ai[0] = chargeTimer
        // ai[1] = swingTimer
        // ai[2] = swingBaseAngle (angle to mouse at release)

        // localAI[0]: 0 = charging, 1 = swinging
        private bool IsCharging => Projectile.localAI[0] == 0f;
        private bool chargedSwing = false;
        private bool playedFullChargeSound = false;

        private bool CanHit
        {
            get => Projectile.friendly;
            set => Projectile.friendly = value;
        }

        private int StarCount => 3 + (int)(Projectile.ai[0] / (MaxCharge / 9f));
        private float PercentCharge = 0f;
        private Player Owner => Main.player[Projectile.owner];

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 90;
            Projectile.height = 90;
            Projectile.friendly = false;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = MaxCharge + SwingDuration + 20;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }

        public override void AI()
        {
            Player owner = Owner;
            if (!owner.active || owner.dead) { Projectile.Kill(); return; }
            owner.heldProj = Projectile.whoAmI;

            if (IsCharging) HandleCharge(owner);
            else HandleSwing(owner);
        }

        private void HandleCharge(Player owner)
        {
            CanHit = false;

            bool mouseHeld = Main.mouseRight;

            if (mouseHeld)
            {
                if (Projectile.ai[0] < MaxCharge)
                    Projectile.ai[0]++;
                PercentCharge = Projectile.ai[0] / MaxCharge;
                Projectile.timeLeft++;
                Projectile.netUpdate = true;
            }

            float chargeRatio = Projectile.ai[0] / (float)MaxCharge;

            if (Projectile.ai[0] >= MaxCharge && !chargedSwing)
            {
                chargedSwing = true;
                if (!playedFullChargeSound)
                {
                    SoundEngine.PlaySound(SoundID.Item162 with { Volume = 0.9f, Pitch = 0.3f }, Projectile.Center);
                    playedFullChargeSound = true;
                }
            }

            // Scythe tracks toward mouse (visible during charge like Hellkite)
            Vector2 toMouse = owner.whoAmI == Main.myPlayer
                ? (Main.MouseWorld - owner.Center).SafeNormalize(Vector2.UnitX)
                : Projectile.velocity.SafeNormalize(Vector2.UnitX);

            owner.direction = toMouse.X >= 0f ? 1 : -1;

            // Windup visual snaps to held position quickly (~20 frames), independent of the
            // charge timer so the blade locks into place fast while stars keep accumulating.
            float windupVisual = MathHelper.Clamp(Projectile.ai[0] / 20f, 0f, 1f);
            float mouseAngle = toMouse.ToRotation();
            float windupPull = windupVisual * MathHelper.PiOver2 * owner.direction;
            float holdAngle = mouseAngle - windupPull;

            Projectile.Center = owner.Center + holdAngle.ToRotationVector2() * ArmLength;
            Projectile.velocity = Vector2.Zero;
            Projectile.rotation = owner.direction == 1
                ? holdAngle + MathHelper.PiOver4
                : holdAngle - MathHelper.PiOver4 - MathHelper.Pi;

            owner.itemRotation = (Projectile.Center - owner.MountedCenter).ToRotation();
            if (owner.direction == -1) owner.itemRotation += MathHelper.Pi;
            owner.itemTime = 2;
            owner.itemAnimation = 2;

            // Charge aura dust ring
            if (Main.rand.NextBool(chargeRatio > 0.66f ? 1 : chargeRatio > 0.33f ? 2 : 3))
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                float radius = 28f + 36f * chargeRatio;
                int dType = Main.rand.NextBool()
                    ? ModContent.DustType<AstralBlue>()
                    : ModContent.DustType<AstralOrange>();
                Dust d = Dust.NewDustPerfect(
                    owner.Center + angle.ToRotationVector2() * radius,
                    dType, Vector2.Zero, 0, default, 0.55f + chargeRatio * 0.65f);
                d.noGravity = true;
                d.velocity = angle.ToRotationVector2() * (0.8f + chargeRatio * 2.2f);
            }

            if (!mouseHeld)
                BeginSwing(owner);
        }

        private void BeginSwing(Player owner)
        {
            Projectile.damage = (int)(Projectile.damage * (1f + PercentCharge * 2));
            Vector2 toMouse = owner.whoAmI == Main.myPlayer
                ? (Main.MouseWorld - owner.Center).SafeNormalize(Vector2.UnitX)
                : Vector2.UnitX * owner.direction;

            Projectile.ai[2] = toMouse.ToRotation();
            Projectile.ai[1] = 0f;
            Projectile.localAI[0] = 1f;
            Projectile.netUpdate = true;

            for (int i = 0; i < 14; i++)
            {
                float a = MathHelper.TwoPi * i / 14f;
                int dType = i % 2 == 0
                    ? ModContent.DustType<AstralBlue>()
                    : ModContent.DustType<AstralOrange>();
                Dust d = Dust.NewDustPerfect(owner.Center, dType,
                    a.ToRotationVector2() * Main.rand.NextFloat(4f, 8f), 0, default, 1.3f);
                d.noGravity = true;
            }
            SoundEngine.PlaySound(SoundID.Item71, Projectile.Center);
        }

        private void HandleSwing(Player owner)
        {
            int effectiveDuration = chargedSwing
                ? (int)(SwingDuration * 0.65f)
                : SwingDuration;

            Projectile.ai[1]++;
            float progress = Projectile.ai[1] / effectiveDuration;
            float easedT = MathHelper.SmoothStep(0f, 1f, progress);

            float baseAngle = Projectile.ai[2];
            float pullBack = MathHelper.PiOver2 * owner.direction;
            float startAngle = baseAngle - pullBack;
            float endAngle = baseAngle + (SweepArc * owner.direction - pullBack);
            float curAngle = MathHelper.Lerp(startAngle, endAngle, easedT);

            Projectile.Center = owner.Center + curAngle.ToRotationVector2() * ArmLength;
            Projectile.rotation = owner.direction == 1
                ? curAngle + MathHelper.PiOver4
                : curAngle - MathHelper.PiOver4 - MathHelper.Pi;

            owner.direction = Projectile.ai[2].ToRotationVector2().X >= 0f ? 1 : -1;
            owner.itemRotation = (Projectile.Center - owner.MountedCenter).ToRotation();
            if (owner.direction == -1) owner.itemRotation += MathHelper.Pi;
            owner.itemTime = 2;
            owner.itemAnimation = 2;

            // Active hit window: windup -> followthrough (like Hellkite's CanHit gating)
            CanHit = progress >= WindupEnd && progress <= FollowthroughStart;

            if (CanHit && Main.rand.NextBool(2))
            {
                int dType = Main.rand.NextBool()
                    ? ModContent.DustType<AstralBlue>()
                    : ModContent.DustType<AstralOrange>();
                Dust d = Dust.NewDustPerfect(Projectile.Center, dType,
                    Main.rand.NextVector2Circular(2f, 2f), 0, default, 1.1f);
                d.noGravity = true;
            }

            Lighting.AddLight(Projectile.Center, CanHit
                ? new Vector3(0.7f, 0.35f, 0.9f)
                : new Vector3(0.2f, 0.1f, 0.3f));

            if (Projectile.ai[1] >= effectiveDuration)
                Projectile.Kill();
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (chargedSwing)
                modifiers.SetCrit();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 240);
            if (Main.netMode != NetmodeID.Server)
                SoundEngine.PlaySound(SoundID.Item62 with { Volume = 1f + Projectile.ai[0]/MaxCharge, Pitch = -0.6f }, Projectile.Center);
            RainStars(target.Center);
        }

        private void RainStars(Vector2 targetCenter)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;

            int count = StarCount;
            for (int i = 0; i < count; i++)
            {
                float xOffset = Main.rand.NextFloat(-600f, 600f) * (i + 1) / count;
                Vector2 spawnPos = targetCenter + new Vector2(xOffset, -600f);
                // Aim above the target to compensate for gravity accumulating over the fall.
                // extraUpdates=1 means 2 AI ticks/frame; ~23 frames of fall produces ~160px overshoot.
                Vector2 aimPoint = targetCenter - new Vector2(0f, 160f);
                Vector2 toTarget = (aimPoint - spawnPos).SafeNormalize(Vector2.UnitY);
                Vector2 vel = toTarget * Main.rand.NextFloat(16f, 22f);
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(), spawnPos, vel,
                    ModContent.ProjectileType<AstralRipperStarfall>(),
                    Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player owner = Owner;
            SpriteBatch sb = Main.spriteBatch;
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;

            float chargeRatio = Projectile.ai[0] / (float)MaxCharge;
            Color astralCyan = new Color(66, 189, 181);
            Color astralRed = new Color(237, 93, 83);
            Color chargeColor = Color.Lerp(astralCyan, astralRed, chargeRatio);

            SpriteEffects fx = owner.direction == -1
                ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, owner.gfxOffY);
            Vector2 origin = tex.Size() / 2f;
            float scale = 1f + (chargedSwing ? 0.12f : chargeRatio * 0.08f);

            // Additive glow pass
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            // Swing afterimage trail
            if (!IsCharging)
            {
                for (int i = 1; i < Projectile.oldPos.Length; i++)
                {
                    float alpha = (1f - i / (float)Projectile.oldPos.Length) * 0.5f;
                    Color trail = Color.Lerp(astralCyan, astralRed, i / (float)Projectile.oldPos.Length) * alpha;
                    Vector2 tPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition + new Vector2(0f, owner.gfxOffY);
                    sb.Draw(tex, tPos, null, trail, Projectile.oldRot[i], origin, scale * (1f - alpha * 0.4f), fx, 0f);
                }
            }

            // Rotating charge aura (like Hellkite's orbit of ghost sprites)
            float auraStrength = IsCharging
                ? chargeRatio
                : System.Math.Max(0f, 1f - Projectile.ai[1] / (float)SwingDuration);
            if (auraStrength > 0f)
            {
                for (int i = 0; i < 8; i++)
                {
                    Vector2 offset = (MathHelper.TwoPi * i / 8f + Main.GlobalTimeWrappedHourly * 4f)
                        .ToRotationVector2() * (5f * auraStrength);
                    sb.Draw(tex, drawPos + offset, null,
                        chargeColor with { A = 0 } * 0.18f * auraStrength,
                        Projectile.rotation, origin, scale, fx, 0f);
                }
            }

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            // Normal sprite — tints toward charge color as charge builds
            Color drawColor = Color.Lerp(lightColor, chargeColor, chargeRatio * 0.5f);
            sb.Draw(tex, drawPos, null, drawColor, Projectile.rotation, origin, scale, fx, 0f);

            return false;
        }
    }
}