using Microsoft.Xna.Framework.Graphics;
using Nez.Textures;

namespace Nez
{
	/// <summary>
	///     used in conjunction with the ReflectionRenderer
	/// </summary>
	public class ReflectionMaterial : Material<ReflectionEffect>
    {
        private RenderTarget2D _renderTarget;
        public RenderTexture renderTexture;


        public ReflectionMaterial(ReflectionRenderer reflectionRenderer) : base(new ReflectionEffect())
        {
            renderTexture = reflectionRenderer.renderTexture;
        }


        public override void onPreRender(Camera camera)
        {
            // only update the Shader when the renderTarget changes. it will be swapped out whenever the GraphicsDevice resets.
            if (_renderTarget == null || _renderTarget != renderTexture.renderTarget)
            {
                _renderTarget = renderTexture.renderTarget;
                effect.renderTexture = renderTexture.renderTarget;
            }

            effect.matrixTransform = camera.viewProjectionMatrix;
        }
    }
}