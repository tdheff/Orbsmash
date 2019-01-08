using System.Collections.Generic;

namespace Handy.Animation
{
    public class AnimationFrame
    {
        public int FrameNumber;
        public int? LastFrameLength;
        public int Time;
    }

    public class EventFrame
    {
        public string EventName;
        public int Time;
    }

    public class AnimationDetail
    {
        public string AnimationName;
        public IList<EventFrame> Events;
        public IList<AnimationFrame> Frames;
        public string NextAnimation;
        public bool Repeats;
    }

    public class SpriteDescriptor
    {
        public int HFrames;
        public string SpriteName;
        public int VFrames;
    }

    public class AnimationDefinition
    {
        public IList<AnimationDetail> Animations;
        public string Context;
        public SpriteDescriptor SpriteDescriptor;
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