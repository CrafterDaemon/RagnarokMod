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

namespace RagnarokMod.Items.BardItems.Electronic
{
    public class Arpeggiator : BardItem
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.Electronic;
        private int counter = 1;
        public override void SetStaticDefaults()
        {
            Empowerments.AddInfo<EmpowermentProlongation>(3, 0);
            Empowerments.AddInfo<AttackSpeed>(4, 0);
        }

        public override void SetBardDefaults()
        {
            Item.damage = 140;
            InspirationCost = 1;
            Item.width = 50;
            Item.height = 26;
            Item.scale = 1f;
            Item.useTime = 5;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.knockBack = 20f;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = ModContent.RarityType<CosmicPurple>();
            Item.UseSound = new SoundStyle?(ThoriumSounds.BlackMidi);
            Item.shoot = ModContent.ProjectileType<ArpeggiatorPro1>();
            Item.shootSpeed = 20;


        }
        public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 offset = velocity.RotatedBy(MathHelper.PiOver2);
            Vector2 offset1 = Vector2.Normalize(offset) * -30;
            Vector2 offset2 = Vector2.Normalize(offset) * -15;
            Vector2 offset3 = Vector2.Normalize(offset) * 0;
            Vector2 offset4 = Vector2.Normalize(offset) * 15;
            Vector2 offset5 = Vector2.Normalize(offset) * 30;
            if (counter == 1) {
                Projectile.NewProjectileDirect(source, position+offset5, velocity, type, damage, knockback, player.whoAmI);
                Projectile.NewProjectileDirect(source, position+offset1, velocity, ModContent.ProjectileType<ArpeggiatorPro2>(), damage, knockback, player.whoAmI);
            }
            if (counter == 2) {
                Projectile.NewProjectileDirect(source, position+offset4, velocity, type, damage, knockback, player.whoAmI);
                Projectile.NewProjectileDirect(source, position+offset2, velocity, ModContent.ProjectileType<ArpeggiatorPro2>(), damage, knockback, player.whoAmI);
            }
            if (counter == 3) {
                Projectile.NewProjectileDirect(source, position+offset3, velocity, type, damage, knockback, player.whoAmI);
                Projectile.NewProjectileDirect(source, position+offset3, velocity, ModContent.ProjectileType<ArpeggiatorPro2>(), damage, knockback, player.whoAmI);
            }
            if (counter == 4) {
                Projectile.NewProjectileDirect(source, position+offset2, velocity, type, damage, knockback, player.whoAmI);
                Projectile.NewProjectileDirect(source, position+offset4, velocity, ModContent.ProjectileType<ArpeggiatorPro2>(), damage, knockback, player.whoAmI);
            }
            if (counter == 5) {
                Projectile.NewProjectileDirect(source, position+offset1, velocity, type, damage, knockback, player.whoAmI);
                Projectile.NewProjectileDirect(source, position+offset5, velocity, ModContent.ProjectileType<ArpeggiatorPro2>(), damage, knockback, player.whoAmI);
            }
            if (counter == 6) {
                Projectile.NewProjectileDirect(source, position+offset2, velocity, type, damage, knockback, player.whoAmI);
                Projectile.NewProjectileDirect(source, position+offset4, velocity, ModContent.ProjectileType<ArpeggiatorPro2>(), damage, knockback, player.whoAmI);
            }
            if (counter == 7) {
                Projectile.NewProjectileDirect(source, position+offset3, velocity, ModContent.ProjectileType<ArpeggiatorPro2>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectileDirect(source, position+offset3, velocity, type, damage, knockback, player.whoAmI);
                
            }
            if (counter == 8) {
                Projectile.NewProjectileDirect(source, position+offset4, velocity, type, damage, knockback, player.whoAmI);
                Projectile.NewProjectileDirect(source, position+offset2, velocity, ModContent.ProjectileType<ArpeggiatorPro2>(), damage, knockback, player.whoAmI);
                counter = 0;
            }
            counter++;
            
            

            
            return false;

        }
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(Item.type);
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 12);
            recipe.AddIngredient(ModContent.ItemType<GrandPiano>(), 1);
            recipe.AddIngredient(ModContent.ItemType<BloodstoneCore>(), 8);
            recipe.AddIngredient(ModContent.ItemType<ExodiumCluster>(), 30);
            recipe.AddTile<CosmicAnvil>();
            recipe.Register();

        }
    }
}