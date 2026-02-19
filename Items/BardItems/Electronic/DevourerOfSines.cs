using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Projectiles.BardPro.Electronic;
using RagnarokMod.Projectiles.BardPro.String;
using RagnarokMod.Riffs;
using RagnarokMod.Riffs.RiffTypes;
using RagnarokMod.Sounds;
using RagnarokMod.Utils;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Empowerments;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Utilities;

namespace RagnarokMod.Items.BardItems.Electronic
{
    public class DevourerOfSines : RiffInstrumentBase
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.Electronic;
        public override SoundStyle RiffSound => RagnarokModSounds.devourerriff;
        public override SoundStyle NormalSound => RagnarokModSounds.devourersine;
        public override byte RiffType => RiffLoader.RiffType<DevourerRiff>();

        public override void SetStaticDefaults()
        {
            Empowerments.AddInfo<FlightTime>(5, 0);
            Empowerments.AddInfo<EmpowermentProlongation>(4, 0);
        }

        public override void SetBardDefaults()
        {
            Item.damage = 1771;
            InspirationCost = 1;
            Item.width = 88;
            Item.height = 88;
            Item.useTime = 23;
            Item.useAnimation = 23;
            Item.useStyle = ItemUseStyleID.Guitar;
            Item.holdStyle = 5;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.knockBack = 3f;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = ModContent.RarityType<CosmicPurple>();
            Item.shoot = ModContent.ProjectileType<DevourerSineBeam>();
            Item.shootSpeed = 11f;
            Item.crit = 24;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.altFunctionUse == 2)
                Item.channel = false;
            else
                Item.channel = true;

        }

        public override Vector2? HoldoutOffset() => new Vector2(-18, 20);

        public override void HoldItemFrame(Player player)
        {
            player.itemLocation += new Vector2(-2, 10) * -player.Directions;
            player.HeldItem.scale = 0.5f;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation += new Vector2(-2, 10) * -player.Directions;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            return DrawHelper.DrawItemInWorldScaled(Item, spriteBatch, lightColor, ref rotation, ref scale, 0.5f);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient<CosmiliteBar>(12)
            .AddTile<CosmicAnvil>()
            .Register();
        }
    }
}