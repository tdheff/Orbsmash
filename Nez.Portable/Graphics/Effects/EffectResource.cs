using System;
using System.IO;
using Microsoft.Xna.Framework;

namespace Nez
{
    public static class EffectResource
    {
        // sprite effects
        internal static byte[] spriteBlinkEffectBytes =>
            getFileResourceBytes("Content/nez/effects/SpriteBlinkEffect.mgfxo");

        internal static byte[] spriteLinesEffectBytes => getFileResourceBytes("Content/nez/effects/SpriteLines.mgfxo");

        internal static byte[] spriteAlphaTestBytes =>
            getFileResourceBytes("Content/nez/effects/SpriteAlphaTest.mgfxo");

        internal static byte[] crosshatchBytes => getFileResourceBytes("Content/nez/effects/Crosshatch.mgfxo");
        internal static byte[] noiseBytes => getFileResourceBytes("Content/nez/effects/Noise.mgfxo");
        internal static byte[] twistBytes => getFileResourceBytes("Content/nez/effects/Twist.mgfxo");
        internal static byte[] dotsBytes => getFileResourceBytes("Content/nez/effects/Dots.mgfxo");
        internal static byte[] dissolveBytes => getFileResourceBytes("Content/nez/effects/Dissolve.mgfxo");

        // post processor effects
        internal static byte[] bloomCombineBytes => getFileResourceBytes("Content/nez/effects/BloomCombine.mgfxo");
        internal static byte[] bloomExtractBytes => getFileResourceBytes("Content/nez/effects/BloomExtract.mgfxo");
        internal static byte[] gaussianBlurBytes => getFileResourceBytes("Content/nez/effects/GaussianBlur.mgfxo");
        internal static byte[] vignetteBytes => getFileResourceBytes("Content/nez/effects/Vignette.mgfxo");
        internal static byte[] letterboxBytes => getFileResourceBytes("Content/nez/effects/Letterbox.mgfxo");
        internal static byte[] heatDistortionBytes => getFileResourceBytes("Content/nez/effects/HeatDistortion.mgfxo");

        internal static byte[] spriteLightMultiplyBytes =>
            getFileResourceBytes("Content/nez/effects/SpriteLightMultiply.mgfxo");

        internal static byte[] pixelGlitchBytes => getFileResourceBytes("Content/nez/effects/PixelGlitch.mgfxo");

        // deferred lighting
        internal static byte[] deferredSpriteBytes => getFileResourceBytes("Content/nez/effects/DeferredSprite.mgfxo");
        internal static byte[] deferredLightBytes => getFileResourceBytes("Content/nez/effects/DeferredLighting.mgfxo");

        // forward lighting
        internal static byte[] forwardLightingBytes =>
            getFileResourceBytes("Content/nez/effects/ForwardLighting.mgfxo");

        internal static byte[] polygonLightBytes => getFileResourceBytes("Content/nez/effects/PolygonLight.mgfxo");

        // scene transitions
        internal static byte[] squaresTransitionBytes =>
            getFileResourceBytes("Content/nez/effects/transitions/Squares.mgfxo");

        // sprite or post processor effects
        internal static byte[] spriteEffectBytes =>
            getMonoGameEmbeddedResourceBytes(
                "Microsoft.Xna.Framework.Graphics.Effect.Resources.SpriteEffect.ogl.mgfxo");

        internal static byte[] multiTextureOverlayBytes =>
            getFileResourceBytes("Content/nez/effects/MultiTextureOverlay.mgfxo");

        internal static byte[] scanlinesBytes => getFileResourceBytes("Content/nez/effects/Scanlines.mgfxo");
        internal static byte[] reflectionBytes => getFileResourceBytes("Content/nez/effects/Reflection.mgfxo");
        internal static byte[] grayscaleBytes => getFileResourceBytes("Content/nez/effects/Grayscale.mgfxo");
        internal static byte[] sepiaBytes => getFileResourceBytes("Content/nez/effects/Sepia.mgfxo");
        internal static byte[] paletteCyclerBytes => getFileResourceBytes("Content/nez/effects/PaletteCycler.mgfxo");


        /// <summary>
        ///     gets the raw byte[] from an EmbeddedResource
        /// </summary>
        /// <returns>The embedded resource bytes.</returns>
        /// <param name="name">Name.</param>
        private static byte[] getEmbeddedResourceBytes(string name)
        {
            var assembly = ReflectionUtils.getAssembly(typeof(EffectResource));
            using (var stream = assembly.GetManifestResourceStream(name))
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }


        internal static byte[] getMonoGameEmbeddedResourceBytes(string name)
        {
#if FNA
			name = name.Replace( ".ogl.mgfxo", ".fxb" );
#endif

            var assembly = ReflectionUtils.getAssembly(typeof(MathHelper));
            using (var stream = assembly.GetManifestResourceStream(name))
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }


        /// <summary>
        ///     fetches the raw byte data of a file from the Content folder. Used to keep the Effect subclass code simple and clean
        ///     due to the Effect
        ///     constructor requiring the byte[].
        /// </summary>
        /// <returns>The file resource bytes.</returns>
        /// <param name="path">Path.</param>
        public static byte[] getFileResourceBytes(string path)
        {
#if FNA
			path = path.Replace( ".mgfxo", ".fxb" );
#endif

            byte[] bytes;
            try
            {
                using (var stream = TitleContainer.OpenStream(path))
                {
                    if (stream.CanSeek)
                    {
                        bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, bytes.Length);
                    }
                    else
                    {
                        using (var ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            bytes = ms.ToArray();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var txt = string.Format(
                    "OpenStream failed to find file at path: {0}. Did you add it to the Content folder and set its properties to copy to output directory?",
                    path);
                throw new Exception(txt, e);
            }

            return bytes;
        }
    }
}