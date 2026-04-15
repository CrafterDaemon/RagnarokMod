using CalamityMod.Items;
using Microsoft.Xna.Framework;
using RagnarokMod.Projectiles;
using RagnarokMod.Projectiles.BardPro.Percussion;
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

namespace RagnarokMod.Items.BardItems.Percussion
{
    public class FungalCastanets : RiffInstrumentBase
    {
        // Full 16-step sequence:
        // Steps 0-4:   8th 8th 8th 8th rest   (round 1)
        // Steps 5-9:   8th 8th 8th 8th rest   (round 2)
        // Steps 10-13: 8th 8th 8th 8th        (round 3)
        // Steps 14-15: 4th 4th                (heavy)

        private static readonly bool[] StepFires =
        {
            true,  true,  true,  true,  false,  // round 1
            true,  true,  true,  true,  false,  // round 2
            true,  true,  true,  true,  false,  // round 3
            true,  true                         // heavy
        };

        private static readonly bool[] StepIsHeavy =
        {
            false, false, false, false, false,
            false, false, false, false, false,
            false, false, false, false, false,
            true,  true
        };

        public int RhythmStep = 0;
        public int FireCount = 0;

        public bool CurrentStepFires => StepFires[RhythmStep];
        public bool IsHeavyStep => StepIsHeavy[RhythmStep];
        public int CurrentSide => FireCount % 2;
        public override bool PlayDuringRiff => true;
        public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;
        public override SoundStyle RiffSound => RagnarokModSounds.InfestationRiff;
        public override SoundStyle NormalSound => (RhythmStep % 5 == 0 && !IsHeavyStep) ? RagnarokModSounds.none : RagnarokModSounds.castanetClick;
        public override byte RiffType => RiffLoader.RiffType<InfestationRiff>();

        public override void SetStaticDefaults()
        {
            Empowerments.AddInfo<MovementSpeed>(2, 0);
            Empowerments.AddInfo<JumpHeight>(1, 0);
            Empowerments.AddInfo<Defense>(1, 0);
        }

        public override void SetBardDefaults()
        {
            Item.damage = 20;
            InspirationCost = 0;
            Item.width = 44;
            Item.height = 42;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.useStyle = ItemUseStyleID.MowTheLawn;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.knockBack = 4f;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;
            Item.shoot = ModContent.ProjectileType<NoProj>();
            Item.shootSpeed = 8f;
        }
        public override void SafeBardHoldItem(Player player)
        {
            if (player.itemAnimation == 0)
            {
                RhythmStep = 0;
                FireCount = 0;
            }
            InspirationCost = (RhythmStep % 5 == 0 || IsHeavyStep) ? 1 : 0;
        }
        public override float PlayingSpeedMultiplier(Player player)
        {
            if (IsHeavyStep) return 0.33f;
            else if (RhythmStep == 14) return 0.3f;
            else if (!CurrentStepFires) return 0.5f;
            else return 1f;
        }
        public override bool SafeRiffBardShoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int sporeType = ModContent.ProjectileType<FungalSpore>();
            int clickType = ModContent.ProjectileType<FungalCastanetClick>();
            FireCount++;

            if (CurrentStepFires)
            {
                if (IsHeavyStep)
                {
                    // Heavy click: both hands fire simultaneously
                    Projectile.NewProjectile(source, position, Vector2.Zero, clickType,
                        (int)(damage * 1.5f), knockback * 1.5f, player.whoAmI, 0f, 1f); // left, heavy
                    Projectile.NewProjectile(source, position, Vector2.Zero, clickType,
                        (int)(damage * 1.5f), knockback * 1.5f, player.whoAmI, 1f, 1f); // right, heavy

                    // Spore spread from both sides
                    for (int side = -1; side <= 1; side += 2)
                    {
                        for (int i = -2; i <= 2; i++)
                        {
                            float angle = MathHelper.ToRadians(i * 15f);
                            Vector2 dir = new Vector2(player.direction * side, -0.3f)
                                .RotatedBy(angle)
                                .SafeNormalize(Vector2.UnitX) * 14f;
                            Projectile.NewProjectile(source, position, dir,
                                sporeType, (int)(damage * 0.7f), 1f, player.whoAmI);
                        }
                    }
                }
                else
                {
                    // Light click: single alternating hand
                    Projectile.NewProjectile(source, position, Vector2.Zero, clickType,
                        damage, knockback, player.whoAmI, CurrentSide, 0f);
                }
            }
            if (RhythmStep > 15) RhythmStep = 0;
            else RhythmStep++;
            return false;
        }
    }
}