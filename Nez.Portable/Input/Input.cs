using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez.Systems;
#if !FNA

#endif


namespace Nez
{
    public static class Input
    {
        public const float DEFAULT_DEADZONE = 0.1f;
        public static Emitter<InputEventType, InputEvent> emitter;

        public static GamePadData[] gamePads;

        /// <summary>
        ///     set by the Scene and used to scale mouse input
        /// </summary>
        internal static Vector2 _resolutionScale;

        /// <summary>
        ///     set by the Scene and used to scale input
        /// </summary>
        internal static Point _resolutionOffset;

        private static KeyboardState _previousKbState;
        private static KeyboardState _currentKbState;
        private static MouseState _currentMouseState;
        internal static FastList<VirtualInput> _virtualInputs = new FastList<VirtualInput>();
        private static int _maxSupportedGamePads;

        public static TouchInput touch;


        static Input()
        {
            emitter = new Emitter<InputEventType, InputEvent>();
            touch = new TouchInput();

            _previousKbState = new KeyboardState();
            _currentKbState = Keyboard.GetState();

            previousMouseState = new MouseState();
            _currentMouseState = Mouse.GetState();

            maxSupportedGamePads = 1;
        }


        public static int maxSupportedGamePads
        {
            get => _maxSupportedGamePads;
            set
            {
                _maxSupportedGamePads = Mathf.clamp(value, 1, GamePad.MaximumGamePadCount);
                gamePads = new GamePadData[_maxSupportedGamePads];
                for (var i = 0; i < _maxSupportedGamePads; i++)
                    gamePads[i] = new GamePadData((PlayerIndex) i);
            }
        }


        public static void update()
        {
            touch.update();

            _previousKbState = _currentKbState;
            _currentKbState = Keyboard.GetState();

            previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            for (var i = 0; i < _maxSupportedGamePads; i++)
                gamePads[i].update();

            for (var i = 0; i < _virtualInputs.length; i++)
                _virtualInputs.buffer[i].update();
        }

        /// <summary>
        ///     this takes into account the SceneResolutionPolicy and returns the value scaled to the RenderTargets coordinates
        /// </summary>
        /// <value>The scaled position.</value>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 scaledPosition(Vector2 position)
        {
            var scaledPos = new Vector2(position.X - _resolutionOffset.X, position.Y - _resolutionOffset.Y);
            return scaledPos * _resolutionScale;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 scaledPosition(Point position)
        {
            return scaledPosition(new Vector2(position.X, position.Y));
        }

        #region Keyboard

        public static KeyboardState previousKeyboardState => _previousKbState;

        public static KeyboardState currentKeyboardState => _currentKbState;


        /// <summary>
        ///     only true if down this frame
        /// </summary>
        /// <returns><c>true</c>, if key pressed was gotten, <c>false</c> otherwise.</returns>
        public static bool isKeyPressed(Keys key)
        {
            return _currentKbState.IsKeyDown(key) && !_previousKbState.IsKeyDown(key);
        }


        /// <summary>
        ///     true the entire time the key is down
        /// </summary>
        /// <returns><c>true</c>, if key down was gotten, <c>false</c> otherwise.</returns>
        public static bool isKeyDown(Keys key)
        {
            return _currentKbState.IsKeyDown(key);
        }


        /// <summary>
        ///     true only the frame the key is released
        /// </summary>
        /// <returns><c>true</c>, if key up was gotten, <c>false</c> otherwise.</returns>
        public static bool isKeyReleased(Keys key)
        {
            return !_currentKbState.IsKeyDown(key) && _previousKbState.IsKeyDown(key);
        }


        /// <summary>
        ///     only true if one of the keys is down this frame
        /// </summary>
        /// <returns><c>true</c>, if key pressed was gotten, <c>false</c> otherwise.</returns>
        public static bool isKeyPressed(Keys keyA, Keys keyB)
        {
            return isKeyPressed(keyA) || isKeyPressed(keyB);
        }


        /// <summary>
        ///     true while either of the keys are down
        /// </summary>
        /// <returns><c>true</c>, if key down was gotten, <c>false</c> otherwise.</returns>
        public static bool isKeyDown(Keys keyA, Keys keyB)
        {
            return isKeyDown(keyA) || isKeyDown(keyB);
        }


        /// <summary>
        ///     true only the frame one of the keys are released
        /// </summary>
        /// <returns><c>true</c>, if key up was gotten, <c>false</c> otherwise.</returns>
        public static bool isKeyReleased(Keys keyA, Keys keyB)
        {
            return isKeyReleased(keyA) || isKeyReleased(keyB);
        }

        #endregion


        #region Mouse

        /// <summary>
        ///     returns the previous mouse state. Use with caution as it only contains raw data and does not take camera scaling
        ///     into affect like
        ///     Input.mousePosition does.
        /// </summary>
        /// <value>The state of the previous mouse.</value>
        public static MouseState previousMouseState { get; private set; }

        /// <summary>
        ///     only true if down this frame
        /// </summary>
        public static bool leftMouseButtonPressed => _currentMouseState.LeftButton == ButtonState.Pressed &&
                                                     previousMouseState.LeftButton == ButtonState.Released;

        /// <summary>
        ///     true while the button is down
        /// </summary>
        public static bool leftMouseButtonDown => _currentMouseState.LeftButton == ButtonState.Pressed;

        /// <summary>
        ///     true only the frame the button is released
        /// </summary>
        public static bool leftMouseButtonReleased => _currentMouseState.LeftButton == ButtonState.Released &&
                                                      previousMouseState.LeftButton == ButtonState.Pressed;

        /// <summary>
        ///     only true if pressed this frame
        /// </summary>
        public static bool rightMouseButtonPressed => _currentMouseState.RightButton == ButtonState.Pressed &&
                                                      previousMouseState.RightButton == ButtonState.Released;

        /// <summary>
        ///     true while the button is down
        /// </summary>
        public static bool rightMouseButtonDown => _currentMouseState.RightButton == ButtonState.Pressed;

        /// <summary>
        ///     true only the frame the button is released
        /// </summary>
        public static bool rightMouseButtonReleased => _currentMouseState.RightButton == ButtonState.Released &&
                                                       previousMouseState.RightButton == ButtonState.Pressed;

        /// <summary>
        ///     only true if down this frame
        /// </summary>
        public static bool middleMouseButtonPressed => _currentMouseState.MiddleButton == ButtonState.Pressed &&
                                                       previousMouseState.MiddleButton == ButtonState.Released;

        /// <summary>
        ///     true while the button is down
        /// </summary>
        public static bool middleMouseButtonDown => _currentMouseState.MiddleButton == ButtonState.Pressed;

        /// <summary>
        ///     true only the frame the button is released
        /// </summary>
        public static bool middleMouseButtonReleased => _currentMouseState.MiddleButton == ButtonState.Released &&
                                                        previousMouseState.MiddleButton == ButtonState.Pressed;

        /// <summary>
        ///     gets the raw ScrollWheelValue
        /// </summary>
        /// <value>The mouse wheel.</value>
        public static int mouseWheel => _currentMouseState.ScrollWheelValue;

        /// <summary>
        ///     gets the delta ScrollWheelValue
        /// </summary>
        /// <value>The mouse wheel delta.</value>
        public static int mouseWheelDelta => _currentMouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue;

        /// <summary>
        ///     unscaled mouse position. This is the actual screen space value
        /// </summary>
        /// <value>The raw mouse position.</value>
        public static Point rawMousePosition => new Point(_currentMouseState.X, _currentMouseState.Y);

        /// <summary>
        ///     alias for scaledMousePosition
        /// </summary>
        /// <value>The mouse position.</value>
        public static Vector2 mousePosition => scaledMousePosition;

        /// <summary>
        ///     this takes into account the SceneResolutionPolicy and returns the value scaled to the RenderTargets coordinates
        /// </summary>
        /// <value>The scaled mouse position.</value>
        public static Vector2 scaledMousePosition =>
            scaledPosition(new Vector2(_currentMouseState.X, _currentMouseState.Y));

        public static Point mousePositionDelta => new Point(_currentMouseState.X, _currentMouseState.Y) -
                                                  new Point(previousMouseState.X, previousMouseState.Y);

        public static Vector2 scaledMousePositionDelta
        {
            get
            {
                var pastPos = new Vector2(previousMouseState.X - _resolutionOffset.X,
                    previousMouseState.Y - _resolutionOffset.Y);
                pastPos *= _resolutionScale;
                return scaledMousePosition - pastPos;
            }
        }

        #endregion
    }


    public enum InputEventType
    {
        GamePadConnected,
        GamePadDisconnected
    }


    public struct InputEvent
    {
        public int gamePadIndex;
    }


    /// <summary>
    ///     comparer that should be passed to a dictionary constructor to avoid boxing/unboxing when using an enum as a key
    ///     on Mono
    /// </summary>
    public struct InputEventTypeComparer : IEqualityComparer<InputEventType>
    {
        public bool Equals(InputEventType x, InputEventType y)
        {
            return x == y;
        }


        public int GetHashCode(InputEventType obj)
        {
            return (int) obj;
        }
    }
}