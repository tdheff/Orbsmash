using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Handy.Animation
{
    public class AnimationFrame
    {
        public int FrameNumber;
        public int Duration;
        public int? LastFrameLength;
    }
    public class EventFrame
    {
        public string EventName;
        public int Time;
    }
    public class AnimationDetail
    {
        public string AnimationName;
        public bool Repeats;
        public string NextAnimation;
        public IList<AnimationFrame> Frames;
        //public IList<EventFrame> Events;
    }

    public class AnimationDefinition
    {
        public SpriteDefinition SpriteDefinition;
        public IList<AnimationDetail> Animations;

        public static AnimationDefinition FromAsepriteJson(AsepriteJson asepriteJson, Texture2D texture)
        {
	        var frameWidth = asepriteJson.Frames[0].Frame.W;
	        var frameHeight = asepriteJson.Frames[0].Frame.H;
	        var hFrames = asepriteJson.Meta.Size.W / frameWidth;
	        var vFrames = asepriteJson.Meta.Size.H / frameHeight;

	        var spriteDefinition = new SpriteDefinition(texture, vFrames, hFrames);

	        List<AnimationDetail> animationDetails = new List<AnimationDetail>();
	        for (int i = 0; i < asepriteJson.Meta.FrameTags.Length; i++)
	        {
		        var detail = new AnimationDetail();
		        var frameTag = asepriteJson.Meta.FrameTags[i];

		        detail.AnimationName = frameTag.Name;
		        
		        List<AnimationFrame> animationFrames = new List<AnimationFrame>();
		        for (int j = frameTag.From; j < frameTag.To + 1; j++)
		        {
			        animationFrames.Add(new AnimationFrame
			        {
				        FrameNumber = j,
				        Duration = asepriteJson.Frames[j].Duration
			        });
		        }

		        detail.Frames = animationFrames;
		        animationDetails.Add(detail);
	        }

	        return new AnimationDefinition
	        {
		        Animations = animationDetails,
		        SpriteDefinition = spriteDefinition
	        };
        }
    }
}

/*
     {
	    "SpriteName": "Player0",
	    "Animations": [
		    {
			    "AnimationName": "PlayerIdle",
			    "Repeats": true,
			    "Frames": [
				    {
					    "FrameNumber": 0,
					    "Time": 0
				    },
				    {
					    "FrameNumber": 1,
					    "Time": 100
				    }
			    ]
		    }
	    ]
    }
*/
