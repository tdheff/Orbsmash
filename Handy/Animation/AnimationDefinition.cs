﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Handy.Animation
{
    public class AnimationFrame
    {
        public int FrameNumber;
        public int Time;
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
    }

    public class SpriteDescriptor
    {
        public string SpriteName;
        public int VFrames;
        public int HFrames;
    }

    public class AnimationDefinition
    {
        public string Context;
        public SpriteDescriptor SpriteDescriptor;
        public IList<AnimationDetail> Animations;
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
