
using System;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using RagnarokMod.Utils;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Buffs.Healer;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Utilities;

namespace ThoriumMod.Projectiles.Scythe;

public abstract class ScythePro2 : ThoriumProjectile
{
    public float rotationSpeed = 0.25f;

    public int scytheCount = 2;

    public int dustCount = 1;

    public int dustType = -1;

    public int dustType2 = -1;

    public int doubleDustCounter = 0;

    public Vector2 dustOffset = Vector2.Zero;

    public bool CanGiveScytheCharge
    {
        get
        {
            return base.Projectile.localAI[0] == 0f;
        }
        set
        {
            base.Projectile.localAI[0] = (value ? 0f : 1f);
        }
    }

    public bool FirstHit
    {
        get
        {
            return base.Projectile.localAI[1] == 0f;
        }
        set
        {
            base.Projectile.localAI[1] = (value ? 0f : 1f);
        }
    }

    public Vector2 DustCenterBase => new Vector2(base.Projectile.width, -base.Projectile.height) / 2f;

    public Vector2 DustCenter => DustCenterBase + dustOffset;

    public int ScytheCharge => ScytheItem.GetScytheChargeFromPro(base.Projectile.type);

    public sealed override void SetStaticDefaults()
    {
        SafeSetStaticDefaults();
    }

    public virtual void SafeSetStaticDefaults()
    {
    }

    public sealed override void SetDefaults()
    {
        base.Projectile.aiStyle = 0;
        base.Projectile.light = 0.2f;
        base.Projectile.friendly = true;
        base.Projectile.tileCollide = false;
        base.Projectile.ownerHitCheck = true;
        base.Projectile.ignoreWater = true;
        base.Projectile.penetrate = -1;
        base.Projectile.timeLeft = 26;
        base.Projectile.usesIDStaticNPCImmunity = true;
        base.Projectile.idStaticNPCHitCooldown = 10;
        base.Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
        fadeOutTime = 10;
        fadeOutSpeed = 30;
        rotationSpeed = 0.25f;
        scytheCount = 2;
        dustCount = 1;
        dustType = -1;
        SafeSetDefaults();
    }

    public virtual void SafeSetDefaults()
    {
    }

    public sealed override void ModifyDamageHitbox(ref Rectangle hitbox)
    {
        hitbox.Inflate(8, 8);
        SafeModifyDamageHitbox(ref hitbox);
    }

    public virtual void SafeModifyDamageHitbox(ref Rectangle hitbox)
    {
    }

    public sealed override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        Player player = Main.player[base.Projectile.owner];
        modifiers.HitDirectionOverride = ((!(target.Center.X < player.Center.X)) ? 1 : (-1));
        SafeModifyHitNPC(target, ref modifiers);
    }

    public virtual void SafeModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
    }

    public sealed override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        Player player = Main.player[base.Projectile.owner];
        ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
        int scytheCharge = ScytheCharge;
        if (scytheCharge > 0 && target.IsHostile())
        {
            player.AddBuff(ModContent.BuffType<SoulEssence>(), 1800);
            CombatText.NewText(target.Hitbox, new Color(100, 255, 200), scytheCharge, dramatic: false, dot: true);
            thoriumPlayer.soulEssence += scytheCharge;
        }

        if (FirstHit)
        {
            FirstHit = false;
            OnFirstHit(target, hit, damageDone);
        }

        SafeOnHitNPC(target, hit, damageDone);
    }

    public virtual void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
    }

    public virtual void OnFirstHit(NPC target, NPC.HitInfo hit, int damageDone)
    {
    }

    public override bool ShouldUpdatePosition()
    {
        return false;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        return base.PreDraw(ref lightColor);
    }

    public override void AI()
    {
        Player player = Main.player[base.Projectile.owner];
        if (player.dead)
        {
            base.Projectile.Kill();
            return;
        }

        base.Projectile.rotation += (float)player.direction * rotationSpeed;
        base.Projectile.spriteDirection = player.direction;
        player.heldProj = base.Projectile.whoAmI;
        base.Projectile.Center = player.Center;
        base.Projectile.gfxOffY = player.gfxOffY;
        SpawnDust();
        SafeAI();
    }

    private void SpawnDust()
    {
        int num = dustCount;
        int num2 = scytheCount;
        int num3 = dustType;
        int num3again = dustType2;
        Vector2 dustCenter = DustCenter;
        if (num2 <= 0 || num <= 0 || num3 <= -1)
        {
            return;
        }

        for (int i = 0; i < num2; i++)
        {
            float num4 = (float)i * ((float)Math.PI * 2f / (float)num2);
            float rotation = base.Projectile.rotation;
            Vector2 spinningpoint = dustCenter;
            if (base.Projectile.spriteDirection < 0)
            {
                spinningpoint.X = 0f - spinningpoint.X;
            }

            spinningpoint = spinningpoint.RotatedBy(rotation + num4);
            Vector2 position = base.Projectile.Center + new Vector2(0f, base.Projectile.gfxOffY) + spinningpoint;
            if (num3again == -1)
            {
                for (int j = 0; j < num; j++)
                {
                    Dust dust = Dust.NewDustPerfect(position, num3, Vector2.Zero);
                    dust.noGravity = true;
                    dust.noLight = true;
                    ModifyDust(dust, position, i);
                }
            }
            if (num3again != -1)
            {
                for (int j = 0; j < num; j++)
                {
                    doubleDustCounter++;
                    if (doubleDustCounter == 2)
                    {
                        doubleDustCounter = 0;
                        Dust dust = Dust.NewDustPerfect(position, num3, Vector2.Zero);
                        dust.noGravity = true;
                        dust.noLight = true;
                        ModifyDust(dust, position, i);
                    }
                    else
                    {
                        Dust dust2 = Dust.NewDustPerfect(position, num3again, Vector2.Zero);
                        dust2.noGravity = true;
                        dust2.noLight = true;
                        ModifyDust(dust2, position, i);
                    }
                }
            }
        }
    }

    public virtual void SafeAI()
    {
    }

    public virtual void ModifyDust(Dust dust, Vector2 position, int scytheIndex)
    {
    }
}