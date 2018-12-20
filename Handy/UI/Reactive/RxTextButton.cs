using System;
using System.Reactive.Linq;
using Nez.UI;

namespace Handy.UI.Reactive
{
    public class RxTextButton : TextButton
    {
        public IObservable<bool> OnChanged_;
        public IObservable<Button> OnClick_;
        
        private void CreateObservables()
        {
            OnClick_ = Observable.FromEvent<Button>(action => onClicked += action, action => onClicked -= action);
            OnChanged_ = Observable.FromEvent<bool>(action => onChanged += action, action => onChanged -= action);
        }

        public RxTextButton(string text, TextButtonStyle style) : base(text, style)
        {
            CreateObservables();
        }

        public RxTextButton(string text, Skin skin, string styleName = null) : base(text, skin, styleName)
        {
            CreateObservables();
        }
    }
}