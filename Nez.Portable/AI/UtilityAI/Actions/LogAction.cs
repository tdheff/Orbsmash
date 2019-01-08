﻿namespace Nez.AI.UtilityAI
{
	/// <summary>
	///     Action that logs text
	/// </summary>
	public class LogAction<T> : IAction<T>
    {
        private readonly string _text;


        public LogAction(string text)
        {
            _text = text;
        }


        void IAction<T>.execute(T context)
        {
            Debug.log(_text);
        }
    }
}