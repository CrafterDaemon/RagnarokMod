using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Riffs;
using RagnarokMod.Riffs.RiffTypes;
using RagnarokMod.Utils;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Projectiles.Bard;

namespace RagnarokMod.Projectiles.BardPro.Percussion
{
    public class FungalCastanetClick : BardProjectile
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;

        private const int TotalLifetime = 15;

        // ai[0] = hand side: 0=left, 1=right
        // ai[1] = heavy flag: 0=light, 1=heavy
        private bool IsRightHand => Projectile.ai[0] == 1f;
        private bool IsHeavy => Projectile.ai[1] == 1f;
        private float Progress => 1f - Projectile.timeLeft / (float)TotalLifetime;

        private bool IsRiffing
        {
            get
            {
                Player owner = Main.player[Projectile.owner];
                return owner.active && owner.GetRagnarokModPlayer().activeRiffType == RiffLoader.RiffType<InfestationRiff>();
            }
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetBardDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = TotalLifetime;
            Projectile.alpha = 0;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            bool isRiffing = IsRiffing;

            Projectile.spriteDirection = owner.direction;

            if (++Projectile.frameCounter >= 10)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
            }

            float sideOffset = IsRightHand ? 12f : -12f;
            float startDist = isRiffing ? -10f : -5f;
            float endDist = IsHeavy
                ? (isRiffing ? 100f : 50f)
                : (isRiffing ? 80f : 40f);

            float thrustT = EaseOutQuad(Math.Min(Progress * 2f, 1f));
            float thrustDist = MathHelper.Lerp(startDist, endDist, thrustT);

            float arcT = MathF.Sin(Progress * MathHelper.Pi);
            float arcMagnitude = IsHeavy ? 22f : 16f;
            if (isRiffing) arcMagnitude *= 1.4f;
            float arcDist = arcT * (IsRightHand ? -arcMagnitude : arcMagnitude);

            Vector2 handBase = owner.Center + new Vector2(sideOffset * owner.direction, -4f);
            Projectile.Center = handBase + new Vector2(owner.direction * thrustDist, arcDist);

            float rotAmount = IsHeavy ? 0.7f : 0.5f;
            if (isRiffing) rotAmount *= 1.3f;
            Projectile.rotation = owner.direction * arcT * (IsRightHand ? -rotAmount : rotAmount);

            float maxScale = IsHeavy
                ? (isRiffing ? 2.0f : 1.5f)
                : (isRiffing ? 1.5f : 1.1f);
            Projectile.scale = MathHelper.Lerp(0.5f, maxScale, EaseOutQuad(Math.Min(Progress * 4f, 1f)));

            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
                if (!IsHeavy)
                    SpawnSpores(owner);
            }

            if (Projectile.timeLeft == (int)(TotalLifetime * 0.6f))
            {
                int dustCount = IsHeavy ? 16 : 8;
                float dustSpeed = IsHeavy ? 5f : 3f;
                float dustScale = IsHeavy ? 1.4f : 1f;
                for (int i = 0; i < dustCount; i++)
                {
                    Vector2 vel = new Vector2(
                        owner.direction * Main.rand.NextFloat(1f, dustSpeed),
                        Main.rand.NextFloat(-dustSpeed, dustSpeed));
                    Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.GlowingMushroom,
                        vel, 0, default, dustScale);
                    d.noGravity = true;
                }

                if (IsHeavy)
                {
                    PunchCameraModifier modifier = new PunchCameraModifier(owner.Center, new Vector2(-owner.direction), 4f, 12f, 6);
                    Main.instance.CameraModifiers.Add(modifier);
                }
                else if (isRiffing){
                    PunchCameraModifier modifier = new PunchCameraModifier(owner.Center, new Vector2(-owner.direction), 4f, 12f, 20);
                    Main.instance.CameraModifiers.Add(modifier);
                }
            }

            if (Projectile.timeLeft < 5)
            {
                Projectile.alpha = Math.Min(Projectile.alpha + 60, 255);
            }
        }

        // Drawing is handled entirely by the PlayerDrawLayers below
        public override bool PreDraw(ref Color lightColor) => false;

        /// <summary>
        /// Shared draw logic for both hand layers.
        /// Swaps to Crabulon claw texture in riff mode.
        /// </summary>
        public static void DrawForLayer(ref PlayerDrawSet drawInfo, Projectile proj)
        {
            Player owner = drawInfo.drawPlayer;
            bool isRiffing = owner.GetRagnarokModPlayer().activeRiffType == RiffLoader.RiffType<InfestationRiff>();

            Texture2D tex;
            Rectangle sourceRect;

            if (isRiffing)
            {
                tex = ModContent.Request<Texture2D>("CalamityMod/Gores/Crabulon6").Value;
                sourceRect = tex.Bounds;
            }
            else
            {
                tex = TextureAssets.Projectile[proj.type].Value;
                int frameHeight = tex.Height / Main.projFrames[proj.type];
                sourceRect = new Rectangle(0, frameHeight * proj.frame, tex.Width, frameHeight);
            }

            Vector2 drawPos = proj.Center - Main.screenPosition;
            Vector2 origin = sourceRect.Size() / 2f;
            float opacity = (255 - proj.alpha) / 255f;
            Color color = Lighting.GetColor(proj.Center.ToTileCoordinates()) * opacity;
            SpriteEffects effects = proj.spriteDirection == -1
                ? SpriteEffects.FlipHorizontally
                : SpriteEffects.None;

            drawInfo.DrawDataCache.Add(new DrawData(
                tex, drawPos, sourceRect, color,
                proj.rotation, origin,
                isRiffing ? proj.scale * 0.5f : proj.scale,
                effects, 0));
        }

        private static float EaseOutQuad(float t) => 1f - (1f - t) * (1f - t);

        private void SpawnSpores(Player owner)
        {
            int sporeCount = 2 + Main.rand.Next(2);
            float baseAngle = IsRightHand
                ? MathHelper.ToRadians(-30f) * owner.direction
                : MathHelper.ToRadians(30f) * owner.direction;
            int sporeDamage = (int)(Projectile.damage * 0.4f);

            for (int i = 0; i < sporeCount; i++)
            {
                float spread = MathHelper.ToRadians(Main.rand.NextFloat(-25f, 25f));
                float speed = Main.rand.NextFloat(6f, 18f);
                Vector2 dir = new Vector2(owner.direction, -0.5f)
                    .RotatedBy(baseAngle + spread)
                    .SafeNormalize(Vector2.UnitX) * speed;

                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(), Projectile.Center, dir,
                    ModContent.ProjectileType<FungalSpore>(), sporeDamage, 1f, Projectile.owner);
            }
        }
    }

    // Right-hand layer: renders above the body, below the front arm
    public class FungalClickRightHandLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() =>
            new BeforeParent(PlayerDrawLayers.ArmOverItem);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) =>
            FindClick(drawInfo.drawPlayer, rightHand: true) != null;

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Projectile proj = FindClick(drawInfo.drawPlayer, rightHand: true);
            if (proj != null)
                FungalCastanetClick.DrawForLayer(ref drawInfo, proj);
        }

        private static Projectile FindClick(Player player, bool rightHand)
        {
            int type = ModContent.ProjectileType<FungalCastanetClick>();
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.type == type && p.owner == player.whoAmI
                    && (p.ai[0] == 1f) == rightHand)
                    return p;
            }
            return null;
        }
    }

    // Left-hand layer: renders above wings/accessories, below the body
    public class FungalClickLeftHandLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() =>
            new BeforeParent(PlayerDrawLayers.Torso);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) =>
            FindClick(drawInfo.drawPlayer, rightHand: false) != null;

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Projectile proj = FindClick(drawInfo.drawPlayer, rightHand: false);
            if (proj != null)
                FungalCastanetClick.DrawForLayer(ref drawInfo, proj);
        }

        private static Projectile FindClick(Player player, bool rightHand)
        {
            int type = ModContent.ProjectileType<FungalCastanetClick>();
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.type == type && p.owner == player.whoAmI
                    && (p.ai[0] == 1f) == rightHand)
                    return p;
            }
            return null;
        }
    }
}