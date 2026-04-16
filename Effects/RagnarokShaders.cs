using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace RagnarokMod.Core
{
    /// <summary>
    /// Loads and registers all RagnarokMod shaders on startup.
    /// Mirrors the pattern used by CalamityShaders.cs.
    ///
    /// All shaders live in RagnarokMod/Effects/
    ///
    /// Usage:
    ///   MiscShaderData shaders -> GameShaders.Misc["RagnarokMod:ShaderName"]
    ///   Raw Effect shaders     -> RagnarokShaders.ShaderName.Value
    /// </summary>
    [Autoload(Side = ModSide.Client)]
    public sealed class RagnarokShaders : ModSystem
    {
        private const string ShaderPath = "Effects/";
        internal const string Prefix = "RagnarokMod:";

        // Raw Effect assets
        // These are used directly via .Value rather than through GameShaders.Misc,
        // because they use custom parameter names rather than the vanilla
        // uColor/uOpacity/etc. uniform convention.

        /// <summary>AphelionSmear.xnb rotational motion smear for the scythe blade.</summary>
        public static Asset<Effect> AphelionSmear { get; private set; }

        /// <summary>AphelionSun.xnb procedural sun shader for the orbiting suns.</summary>
        public static Asset<Effect> AphelionSun { get; private set; }

        /// <summary>AphelionNovaCore.xnb — destabilizing stellar core for the supernova.</summary>
        public static Asset<Effect> AphelionNovaCore { get; private set; }

        /// <summary>AphelionNovaRing.xnb — expanding shockwave ring for the supernova.</summary>
        public static Asset<Effect> AphelionNovaRing { get; private set; }

        // Helpers

        private Asset<Effect> LoadShader(string name) =>
            Mod.Assets.Request<Effect>($"{ShaderPath}{name}", AssetRequestMode.ImmediateLoad);

        private static void RegisterMiscShader(Asset<Effect> shader, string passName, string registrationName) =>
            GameShaders.Misc[$"{Prefix}{registrationName}"] =
                new MiscShaderData(shader, passName);


        public override void PostSetupContent()
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            AphelionSmear = LoadShader("AphelionSmear");

            AphelionSun = LoadShader("AphelionSun");
            AphelionNovaCore = LoadShader("AphelionNovaCore");
            AphelionNovaRing = LoadShader("AphelionNovaRing");
        }

        public override void Unload()
        {
            AphelionSmear = null;
            AphelionSun = null;
            AphelionNovaCore = null;
            AphelionNovaRing = null;
        }
    }
}