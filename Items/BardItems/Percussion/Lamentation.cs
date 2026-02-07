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
using RagnarokMod.Sounds;
using ThoriumMod.Items.BardItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using RagnarokMod.Projectiles.BardPro.Percussion;

namespace RagnarokMod.Items.BardItems.Percussion
{
    public class Lamentation : BardItem
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;

        public override void SetStaticDefaults()
        {
            Empowerments.AddInfo<MovementSpeed>(4, 0);
            Empowerments.AddInfo<FlightTime>(4, 0);
            Empowerments.AddInfo<JumpHeight>(4, 0);
        }

        public override void SetBardDefaults()
        {
            Item.damage = 2250;
            InspirationCost = 3;
            Item.width = 65;
            Item.height = 65;
            Item.useTime = 29;
            Item.useAnimation = 29;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.knockBack = 2f;
			Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
			Item.rare = ModContent.RarityType<BurnishedAuric>();
            Item.UseSound = RagnarokModSounds.calamitybell;
            Item.shoot = ModContent.ProjectileType<LamentationPro>();
            Item.shootSpeed = 20f;
        }
    }
}