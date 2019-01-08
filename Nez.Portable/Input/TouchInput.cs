using System.Collections.Generic;
#if !FNA
using Microsoft.Xna.Framework.Input.Touch;

#endif


namespace Nez
{
	/// <summary>
	///     to enable touch input you must first call enableTouchSupport()
	/// </summary>
	public class TouchInput
    {
#pragma warning disable 0649
#pragma warning restore 0649


        private void onGraphicsDeviceReset()
        {
#if !FNA
            TouchPanel.DisplayWidth = Core.graphicsDevice.Viewport.Width;
            TouchPanel.DisplayHeight = Core.graphicsDevice.Viewport.Height;
            TouchPanel.DisplayOrientation = Core.graphicsDevice.PresentationParameters.DisplayOrientation;
#endif
        }


        internal void update()
        {
            if (!isConnected)
                return;

#if !FNA
            previousTouches = currentTouches;
            currentTouches = TouchPanel.GetState();

            previousGestures = currentGestures;
            currentGestures.Clear();
            while (TouchPanel.IsGestureAvailable)
                currentGestures.Add(TouchPanel.ReadGesture());
#endif
        }


        public void enableTouchSupport()
        {
#if !FNA
            isConnected = TouchPanel.GetCapabilities().IsConnected;
#endif

            if (isConnected)
            {
                Core.emitter.addObserver(CoreEvents.GraphicsDeviceReset, onGraphicsDeviceReset);
                Core.emitter.addObserver(CoreEvents.OrientationChanged, onGraphicsDeviceReset);
                onGraphicsDeviceReset();
            }
        }
#if !FNA
        public bool isConnected { get; private set; }

        public TouchCollection currentTouches { get; private set; }

        public TouchCollection previousTouches { get; private set; }

        public List<GestureSample> previousGestures { get; private set; } = new List<GestureSample>();

        public List<GestureSample> currentGestures { get; } = new List<GestureSample>();

#endif
    }
}