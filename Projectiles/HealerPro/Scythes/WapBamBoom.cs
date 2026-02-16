using CalamityMod.Dusts;
using CalamityMod.NPCs.TownNPCs;
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
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Bard;
using ThoriumMod.Projectiles.Scythe;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class WapBamBoom : ModProjectile, ILocalizedModType
    {
        public int onFives = 0;
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 25;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.velocity = Vector2.Zero;
            Projectile.alpha = 255;
            Projectile.width = 128;
            Projectile.height = 128;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.Center = Projectile.position;
            Projectile.scale = 3;
        }
        public override void AI()
        {
            Projectile.damage = 10000;
            Projectile.knockBack = 10f;
            onFives++;
            if (onFives % 5 == 0)
            {
                Projectile.position = Projectile.Center;
                Projectile.width += 128;
                Projectile.height += 128;
                Projectile.Center = Projectile.position;
                for (int i = 0; i < 144; i++)
                {
                    Vector2 position = Projectile.Center + new Vector2(64 * (onFives / 5), 0).RotatedBy(MathHelper.ToRadians(2.5f * (i + 1)));
                    Dust dust = Dust.NewDustPerfect(position, DustID.OrangeStainedGlass, Vector2.Zero, default, default, 1.5f);
                    dust.noGravity = true;
                }
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(Sounds.RagnarokModSounds.TSS);
        }
    }
}
