using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Nez
{
	/// <summary>
	///     A virtual input represented as a float between -1 and 1
	/// </summary>
	public class VirtualAxis : VirtualInput
    {
        public List<Node> nodes = new List<Node>();


        public VirtualAxis()
        {
        }


        public VirtualAxis(params Node[] nodes)
        {
            this.nodes.AddRange(nodes);
        }

        public float value
        {
            get
            {
                for (var i = 0; i < nodes.Count; i++)
                {
                    var val = nodes[i].value;
                    if (val != 0)
                        return val;
                }

                return 0;
            }
        }


        public override void update()
        {
            for (var i = 0; i < nodes.Count; i++)
                nodes[i].update();
        }


        public static implicit operator float(VirtualAxis axis)
        {
            return axis.value;
        }


        #region Node types

        public abstract class Node : VirtualInputNode
        {
            public abstract float value { get; }
        }


        public class GamePadLeftStickX : Node
        {
            public float deadzone;
            public int gamepadIndex;


            public GamePadLeftStickX(int gamepadIndex = 0, float deadzone = Input.DEFAULT_DEADZONE)
            {
                this.gamepadIndex = gamepadIndex;
                this.deadzone = deadzone;
            }

            public override float value =>
                Mathf.signThreshold(Input.gamePads[gamepadIndex].getLeftStick(deadzone).X, deadzone);
        }


        public class GamePadLeftStickY : Node
        {
            public float deadzone;
            public int gamepadIndex;

            /// <summary>
            ///     if true, pressing up will return -1 and down will return 1 matching GamePadDpadUpDown
            /// </summary>
            public bool invertResult = true;


            public GamePadLeftStickY(int gamepadIndex = 0, float deadzone = Input.DEFAULT_DEADZONE)
            {
                this.gamepadIndex = gamepadIndex;
                this.deadzone = deadzone;
            }

            public override float value
            {
                get
                {
                    var multiplier = invertResult ? -1 : 1;
                    return multiplier *
                           Mathf.signThreshold(Input.gamePads[gamepadIndex].getLeftStick(deadzone).Y, deadzone);
                }
            }
        }


        public class GamePadRightStickX : Node
        {
            public float deadzone;
            public int gamepadIndex;


            public GamePadRightStickX(int gamepadIndex = 0, float deadzone = Input.DEFAULT_DEADZONE)
            {
                this.gamepadIndex = gamepadIndex;
                this.deadzone = deadzone;
            }

            public override float value =>
                Mathf.signThreshold(Input.gamePads[gamepadIndex].getRightStick(deadzone).X, deadzone);
        }


        public class GamePadRightStickY : Node
        {
            public float deadzone;
            public int gamepadIndex;


            public GamePadRightStickY(int gamepadIndex = 0, float deadzone = Input.DEFAULT_DEADZONE)
            {
                this.gamepadIndex = gamepadIndex;
                this.deadzone = deadzone;
            }

            public override float value =>
                Mathf.signThreshold(Input.gamePads[gamepadIndex].getRightStick(deadzone).Y, deadzone);
        }


        public class GamePadDpadLeftRight : Node
        {
            public int gamepadIndex;


            public GamePadDpadLeftRight(int gamepadIndex = 0)
            {
                this.gamepadIndex = gamepadIndex;
            }


            public override float value
            {
                get
                {
                    if (Input.gamePads[gamepadIndex].DpadRightDown)
                        return 1f;
                    if (Input.gamePads[gamepadIndex].DpadLeftDown)
                        return -1f;
                    return 0f;
                }
            }
        }


        public class GamePadDpadUpDown : Node
        {
            public int gamepadIndex;


            public GamePadDpadUpDown(int gamepadIndex = 0)
            {
                this.gamepadIndex = gamepadIndex;
            }


            public override float value
            {
                get
                {
                    if (Input.gamePads[gamepadIndex].DpadDownDown)
                        return 1f;
                    if (Input.gamePads[gamepadIndex].DpadUpDown)
                        return -1f;
                    return 0f;
                }
            }
        }


        public class KeyboardKeys : Node
        {
            private bool _turned;

            private float _value;
            public Keys negative;
            public OverlapBehavior overlapBehavior;
            public Keys positive;


            public KeyboardKeys(OverlapBehavior overlapBehavior, Keys negative, Keys positive)
            {
                this.overlapBehavior = overlapBehavior;
                this.negative = negative;
                this.positive = positive;
            }


            public override float value => _value;


            public override void update()
            {
                if (Input.isKeyDown(positive))
                {
                    if (Input.isKeyDown(negative))
                    {
                        switch (overlapBehavior)
                        {
                            default:
                            case OverlapBehavior.CancelOut:
                                _value = 0;
                                break;

                            case OverlapBehavior.TakeNewer:
                                if (!_turned)
                                {
                                    _value *= -1;
                                    _turned = true;
                                }

                                break;
                            case OverlapBehavior.TakeOlder:
                                //value stays the same
                                break;
                        }
                    }
                    else
                    {
                        _turned = false;
                        _value = 1;
                    }
                }
                else if (Input.isKeyDown(negative))
                {
                    _turned = false;
                    _value = -1;
                }
                else
                {
                    _turned = false;
                    _value = 0;
                }
            }
        }

        #endregion
    }
}