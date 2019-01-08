using System;

namespace Nez.Timers
{
	/// <summary>
	///     private class hiding the implementation of ITimer
	/// </summary>
	internal class Timer : ITimer
    {
        private float _elapsedTime;
        private bool _isDone;
        private Action<ITimer> _onTime;
        private bool _repeats;

        private float _timeInSeconds;
        public object context { get; set; }


        public void stop()
        {
            _isDone = true;
        }


        public void reset()
        {
            _elapsedTime = 0f;
        }


        public T getContext<T>()
        {
            return (T) context;
        }


        internal bool tick()
        {
            // if stop was called before the tick then isDone will be true and we should not tick again no matter what
            if (!_isDone && _elapsedTime > _timeInSeconds)
            {
                _elapsedTime -= _timeInSeconds;
                _onTime(this);

                if (!_isDone && !_repeats)
                    _isDone = true;
            }

            _elapsedTime += Time.deltaTime;

            return _isDone;
        }


        internal void initialize(float timeInSeconds, bool repeats, object context, Action<ITimer> onTime)
        {
            _timeInSeconds = timeInSeconds;
            _repeats = repeats;
            this.context = context;
            _onTime = onTime;
        }


        /// <summary>
        ///     nulls out the object references so the GC can pick them up if needed
        /// </summary>
        internal void unload()
        {
            context = null;
            _onTime = null;
        }
    }
}