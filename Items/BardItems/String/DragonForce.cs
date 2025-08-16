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
using CalamityMod.Items.Materials;
using RagnarokMod.Sounds;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using CalamityMod.Tiles.Furniture.CraftingStations;
using static Mono.CompilerServices.SymbolWriter.CodeBlockEntry;
using RagnarokMod.Projectiles.BardPro.Electronic;
using CalamityMod.Items.Placeables.Ores;
using RagnarokMod.Projectiles.BardPro.Wind;
using RagnarokMod.Projectiles.BardPro.String;
using RagnarokMod.Projectiles.HealerPro.Other;


namespace RagnarokMod.Items.BardItems.String
{
    public class DragonForce : BardItem
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.String;
        public override void SetStaticDefaults()
        {
            Empowerments.AddInfo<Damage>(6);
            Empowerments.AddInfo<AttackSpeed>(6);
        }

        public override void SetBardDefaults()
        {
            Item.damage = 275;
            InspirationCost = 1;
            Item.width = 86;
            Item.height = 92;
            Item.scale = 1f;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.knockBack = 20f;
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.rare = ModContent.RarityType<DarkOrange>();
            Item.UseSound = new SoundStyle?(SoundID.Item47);
            Item.shoot = ModContent.ProjectileType<DragonForcePro1>();
            Item.shootSpeed = 20;


        }
        public virtual Vector2? HoldoutOffset() => new Vector2?(new Vector2(-12f, 0.0f));
        public override bool BardShoot(
            Player player,
            EntitySource_ItemUse_WithAmmo source,
            Vector2 position,
            Vector2 velocity,
            int type,
            int damage,
            float knockback)
        {
            return true;
        }
    }
}