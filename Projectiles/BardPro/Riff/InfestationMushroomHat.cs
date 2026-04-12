using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Riffs;
using RagnarokMod.Riffs.RiffTypes;
using RagnarokMod.Utils;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Projectiles.Bard;
using ThoriumMod.Utilities;

namespace RagnarokMod.Projectiles.BardPro.Riff
{
    public class InfestationMushroomHat : BardProjectile
    {
        public override string Texture => "RagnarokMod/Projectiles/NoProj";
        public byte RiffType => RiffLoader.RiffType<InfestationRiff>();

        private int mushroomTimer = 0;
        private const int MushroomInterval = 4;

        public override void SetBardDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.timeLeft = 20;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            if (owner.GetRagnarokModPlayer().activeRiffType == RiffType)
                Projectile.timeLeft++;

            // Sit on the player's head
            Projectile.Center = owner.Top + new Vector2(0f, -14f);

            // Fade in
            if (Projectile.alpha > 0 && Projectile.timeLeft > 19)
            {
                Projectile.alpha -= 10;
                if (Projectile.alpha < 0)
                    Projectile.alpha = 0;
            }

            // Fade out on death
            if (Projectile.timeLeft < 19)
            {
                Projectile.alpha += 15;
                if (Projectile.alpha > 255)
                    Projectile.alpha = 255;
            }

            // Subtle spore dust
            if (Main.rand.NextBool(6) && Projectile.alpha < 100)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(10f, 6f),
                    DustID.GlowingMushroom, new Vector2(0f, -0.5f), 0, default, 0.8f);
                d.noGravity = true;
            }

            // Green glow
            float lightAmt = 1f - Projectile.alpha / 255f;
            Lighting.AddLight(Projectile.Center, 0.2f * lightAmt, 0.6f * lightAmt, 0.2f * lightAmt);

            // Spawn rain mushrooms
            mushroomTimer++;
            if (mushroomTimer >= MushroomInterval && Projectile.alpha < 100)
            {
                mushroomTimer = 0;
                SpawnMushroomRain();
            }
        }

        private void SpawnMushroomRain()
        {
            for (int i = 3; i > 0; i--) {
                Vector2 spawnPos = Projectile.Center + new Vector2(Main.rand.NextFloat(-Main.screenWidth/2, Main.screenWidth/2), -Main.screenHeight * 0.5f);
                Vector2 velocity = new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), 2f);

                Player owner = Main.player[Projectile.owner];
                int damage = 1;
                if (owner.GetThoriumPlayer() != null)
                    damage = (int)(owner.HeldItem.damage * owner.GetTotalDamage(ThoriumDamageBase<BardDamage>.Instance).Additive * 0.5f);

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPos, velocity,
                    ModContent.ProjectileType<InfestationMushroomRain>(), damage, 2f, Projectile.owner); 
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(4f, 4f);
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.GlowingMushroom, velocity, 0, default, Main.rand.NextFloat(1f, 1.5f));
                d.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            float alphaLerp = 1f - Projectile.alpha / 255f;
            Color drawColor = Color.White * alphaLerp;

            Main.EntitySpriteDraw(tex, drawPos, tex.Bounds, drawColor,
                0f, tex.Size() / 2f, 0.25f, SpriteEffects.None, 0);

            return false;
        }
    }
}
