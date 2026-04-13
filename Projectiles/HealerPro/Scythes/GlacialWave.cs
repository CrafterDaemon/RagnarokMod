using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class GlacialWave : ModProjectile
    {
        private bool spawnedChild = false;

        private int Timer
        {
            get => (int)Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }

        private const int GrowTime = 8;
        private const int BaseWidth = 128;
        private const int BaseHeight = 80;
        private const float MaxScale = 3f;

        public override void SetDefaults()
        {
            Projectile.width = BaseWidth;
            Projectile.height = BaseHeight;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.scale = 0.01f; // avoid zero scale
            Projectile.tileCollide = false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.direction = (int)Projectile.ai[1];
            Projectile.spriteDirection = Projectile.direction;

            if (Main.netMode != NetmodeID.Server)
                SoundEngine.PlaySound(SoundID.Item62 with
                {
                    Volume = 1f - (Projectile.ai[0] / 10f),
                    Pitch = -0.6f
                }, Projectile.Center);
        }

        public override void AI()
        {
            Timer++;

            // Save center before any hitbox changes
            Vector2 center = Projectile.Center;

            if (Timer <= GrowTime)
            {
                float rate = (1f / GrowTime) * ((Projectile.ai[0] + 1) * 0.3f);
                Projectile.scale = MathHelper.Min(Projectile.scale + rate, MaxScale);
            }

            if (Timer >= GrowTime && !spawnedChild && Projectile.ai[0] < 6)
            {
                Projectile.NewProjectileDirect(
                    Projectile.GetSource_FromThis(),
                    center + new Vector2(Projectile.ai[1] * (Projectile.width * 0.66f), 0),
                    Projectile.velocity,
                    Type,
                    Projectile.damage,
                    Projectile.knockBack,
                    Projectile.owner,
                    Projectile.ai[0] + 1,
                    Projectile.ai[1]); // pass direction as ai[1]
                spawnedChild = true;
            }

            if (Projectile.timeLeft < GrowTime)
            {
                Projectile.scale -= MaxScale / GrowTime;
                if (Projectile.scale <= 0f)
                    Projectile.Kill();
            }

            // Resize hitbox around the stable center
            int scaledW = (int)(BaseWidth * Projectile.scale);
            int scaledH = (int)(BaseHeight * Projectile.scale);
            Projectile.width = scaledW;
            Projectile.height = scaledH;
            Projectile.Center = center;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Vector2 origin = tex.Size() / 2f;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            SpriteEffects fx = Projectile.spriteDirection == 1
                ? SpriteEffects.FlipHorizontally
                : SpriteEffects.None;

            Main.EntitySpriteDraw(tex, drawPos, null, lightColor,
                Projectile.rotation, origin, Projectile.scale, fx, 0);

            return false;
        }
    }
}
