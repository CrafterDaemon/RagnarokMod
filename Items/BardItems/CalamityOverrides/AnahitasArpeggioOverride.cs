﻿using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Empowerments;
using ThoriumMod;
using ThoriumMod.Items;
using CalamityMod.Items;

namespace RagnarokMod.Items
{
    [LegacyName("SirensSong")]
    public class AnahitasArpeggioOverride : BardItem, ILocalizedModType
    {
        //Anahita's Arpeggio, but a bard weapon
        public override string Texture => "CalamityMod/Items/Weapons/Magic/AnahitasArpeggio";
        public override BardInstrumentType InstrumentType => BardInstrumentType.String;

        public override void SetStaticDefaults()
        {
            Empowerments.AddInfo<AquaticAbility>(3);
            Empowerments.AddInfo<MovementSpeed>(2);
        }
        public override void SetBardDefaults()
        {
            Item.damage = 92;
            InspirationCost = 2;
            Item.width = 56;
            Item.height = 50;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 6.5f;
            Item.value = CalamityGlobalItem.Rarity7BuyPrice;
            Item.rare = ItemRarityID.Lime;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AnahitasArpeggioNote>();
            Item.shootSpeed = 13f;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

        public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float xDist = Main.mouseX + Main.screenPosition.X - position.X;
            float yDist = Main.mouseY + Main.screenPosition.Y - position.Y;
            Vector2 mouseDist = new Vector2(xDist, yDist);
            float soundMult = mouseDist.Length() / (Main.screenHeight / 2f);
            if (soundMult > 1f)
                soundMult = 1f;
            float soundPitch = soundMult * 2f - 1f;
            soundPitch = MathHelper.Clamp(soundPitch, -1f, 1f);

            velocity.X += Main.rand.NextFloat(-0.75f, 0.75f);
            velocity.Y += Main.rand.NextFloat(-0.75f, 0.75f);
            velocity.X *= soundMult + 0.25f;
            velocity.Y *= soundMult + 0.25f;

            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, soundPitch, 0f);
            return false;
        }
    }
}
