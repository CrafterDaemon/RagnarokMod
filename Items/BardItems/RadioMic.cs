using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Empowerments;
using ThoriumMod.Projectiles.Bard;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using Terraria.DataStructures;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Rarities;
using RagnarokMod.Projectiles.BardPro;
using RagnarokMod.Sounds;
using ThoriumMod.Items.BardItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace RagnarokMod.Items.BardItems
{
    public class RadioMic : BardItem
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.Electronic;

        public override void SetStaticDefaults()
        {
            this.Empowerments.AddInfo<EmpowermentProlongation>(4, 0);
            this.Empowerments.AddInfo<Damage>(3, 0);
        }

        public override void SetBardDefaults()
        {
            Item.damage = 120;
            base.InspirationCost = 4;
            base.Item.width = 32;
            base.Item.height = 118;
            base.Item.scale = 0.6f;
            base.Item.useTime = 48;
            base.Item.useAnimation = 48;
            base.Item.useStyle = ItemUseStyleID.RaiseLamp;
            base.Item.noMelee = true;
            base.Item.autoReuse = true;
            base.Item.knockBack = 20f;
            Item.value = 30000;
            Item.rare = ModContent.RarityType<CalamityRed>();
            Item.UseSound = SoundID.DD2_BookStaffCast;
            base.Item.shoot = ModContent.ProjectileType<TendrilStrike>();
            base.Item.shootSpeed = 10f;

        }
    }
}