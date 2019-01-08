using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nez.Overlap2D.Runtime
{
    public class SceneVO
    {
        public float[] ambientColor = {1f, 1f, 1f, 1f};
        public CompositeVO composite;
        public List<float> horizontalGuides;
        public bool lightSystemEnabled = false;

        public PhysicsPropertiesVO physicsPropertiesVO;

        // this is a project property, not part of the SceneVO overlap spec
        public int pixelToWorld = 1;

        public string sceneName = string.Empty;
        public List<float> verticalGuides;


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}