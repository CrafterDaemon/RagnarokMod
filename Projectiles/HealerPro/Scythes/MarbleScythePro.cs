using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;
using ThoriumMod;
using ThoriumMod.Projectiles.Scythe;
using ThoriumMod.Buffs.Healer;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class MarbleScythePro : ScythePro
    {
        public override void SafeSetStaticDefaults()
        {
        }

        public override void SafeSetDefaults()
        {
            Projectile.aiStyle = 0;
            Projectile.light = 0.2f;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 26;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            fadeOutTime = 10;
            fadeOutSpeed = 30;
            rotationSpeed = 0.25f;
            scytheCount = 2;
            Projectile.Size = new Vector2(96f);
            dustOffset = new Vector2(-20f, 20f);
            dustCount = 4;
            dustType = DustID.YellowTorch;

            Projectile.scale = 1.75f;
        }

        public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<HolyGlare>(), 180);
        }

        public override void SafeAI()
        {
            if (Projectile.timeLeft <= 10)
            {
                float fadeProgress = 1f - (Projectile.timeLeft / 10f);
                Projectile.alpha = (int)MathHelper.Lerp(50f, 255f, fadeProgress);
            }
        }

        public override void ModifyDust(Dust dust, Vector2 position, int scytheIndex)
        {
            dust.scale = 1.5f;
            dust.noLight = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;

            Vector2 origin = texture.Size() / 2f;

            Color drawColor = Projectile.GetAlpha(lightColor);

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                drawColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }
    }
}
