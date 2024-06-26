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

namespace RagnarokMod.Items.BardItems.Electronic
{
    public class Korobeiniki : BardItem
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.Electronic;

        public override void SetStaticDefaults()
        {
            Empowerments.AddInfo<EmpowermentProlongation>(4, 0);
        }

        public override void SetBardDefaults()
        {
            Item.damage = 705;
            InspirationCost = 1;
            Item.width = 31;
            Item.height = 16;
            Item.scale = 1.5f;
            Item.useTime = 7;
            Item.useAnimation = 7;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.knockBack = 20f;
            Item.value = 15000;
            Item.rare = ModContent.RarityType<DarkOrange>();
            Item.UseSound = new SoundStyle?(ThoriumSounds.Didgeridoo);
            Item.shoot = ModContent.ProjectileType<KorobeinikiPro1>();
            Item.shootSpeed = 0.1f;


        }
        public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            for (int i = 0; i < 3; i++)
            {

                float num = velocity.Length();
                float fallspeedmult = 300;
                float projSpeed = num;
                float x = Main.MouseWorld.X + Main.rand.NextFloat(0f - 50f, 50f);
                float y = Main.MouseWorld.Y - 850f + i * 20f;

                Vector2 vector = new Vector2(x, y);
                Vector2 vel = Main.MouseWorld - vector;
                vel.X += Main.rand.NextFloat(0f - 130f, 130f);
                vel.Y += Main.rand.NextFloat(0f - 260f, 260f);
                float n = vel.Length();
                n = projSpeed / n;
                vel.X *= n;
                vel.Y *= n * fallspeedmult;
                int type2 = Main.rand.Next(new int[] { type, ModContent.ProjectileType<KorobeinikiPro2>(), ModContent.ProjectileType<KorobeinikiPro3>(), ModContent.ProjectileType<KorobeinikiPro4>(), ModContent.ProjectileType<KorobeinikiPro5>(), ModContent.ProjectileType<KorobeinikiPro6>(), ModContent.ProjectileType<KorobeinikiPro7>() });
                Projectile.NewProjectileDirect(source, vector, vel, type2, damage, knockback, player.whoAmI);

            }
            return false;

        }
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(Item.type);
            recipe.AddIngredient(ModContent.ItemType<AuricBar>(), 12);
            recipe.AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 12);
            recipe.AddIngredient(ModContent.ItemType<DubiousPlating>(), 12);
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 12);
            recipe.AddTile<CosmicAnvil>();
            recipe.Register();

        }
    }
}