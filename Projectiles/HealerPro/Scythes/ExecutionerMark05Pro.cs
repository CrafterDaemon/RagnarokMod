using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Scythe;
using RagnarokMod.Utils;
using Terraria.DataStructures;
using Terraria.Audio;
using CalamityMod.Sounds;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class ExecutionerMark05Pro : ScythePro2
    {
        public double counter = 0;

        public override void SafeSetStaticDefaults()
        {
        }
        public override void SafeSetDefaults()
        {
            base.Projectile.Size = new Vector2(188, 212);
            dustOffset = new Vector2(-35f, 7f);
            dustCount = 24;
            dustType = DustID.GemSapphire;
            dustType2 = DustID.GemRuby;
            base.rotationSpeed = 0.025f;
            Projectile.light = 1f;
            scytheCount = 1;
            this.Projectile.idStaticNPCHitCooldown = 48;
            base.Projectile.timeLeft = 151;

        }
        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(Sounds.RagnarokModSounds.TSC);
            Projectile.TryGetOwner(out Player player);
            if (Projectile.direction == 1)
            {
                base.Projectile.rotation = MathHelper.PiOver2 * -player.direction;
            }
            else
            {
                base.Projectile.rotation = ((float)Math.PI + MathHelper.PiOver2) * -player.direction;
            }
        }
        public override void SafeAI()
        {
            counter++;
            base.rotationSpeed *= 1.025f;
            if (this.Projectile.idStaticNPCHitCooldown > 1 && (counter % 3) == 0)
            {
                this.Projectile.idStaticNPCHitCooldown--;
            }
            if (counter >= 150 && Projectile.TryGetOwner(out Player player))
            {
                Vector2 velocity = -45 * (player.Center - Main.MouseWorld).SafeNormalize(Vector2.UnitX);
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y - 10), velocity, ModContent.ProjectileType<ExecutionerMark05ThrowPro>(), Projectile.damage * 20, Projectile.knockBack, Main.myPlayer, 0, 1);
                SoundEngine.PlaySound(Sounds.RagnarokModSounds.TSL);
                Projectile.Kill();
            }
        }
    }
}
