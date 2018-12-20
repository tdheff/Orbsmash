using System;
using System.Reactive.Linq;
using Nez.UI;

namespace Handy.UI.Reactive
{
    public class RxButton : Button
    {
        public IObservable<bool> OnChanged_;
        public IObservable<Button> OnClick_;
        
        public RxButton(ButtonStyle style) : base(style)
        {
            CreateObservables();
        }

        public RxButton(Skin skin, string styleName = null) : base(skin, styleName)
        {
            CreateObservables();
        }

        public RxButton(IDrawable up) : base(up)
        {
            CreateObservables();
        }

        public RxButton(IDrawable up, IDrawable down) : base(up, down)
        {
            CreateObservables();
        }

        public RxButton(IDrawable up, IDrawable down, IDrawable checked_) : base(up, down, checked_)
        {
            CreateObservables();
        }

        private void CreateObservables()
        {
            OnClick_ = Observable.FromEvent<Button>(action => onClicked += action, action => onClicked -= action);
            OnChanged_ = Observable.FromEvent<bool>(action => onChanged += action, action => onChanged -= action);
        }
    }
}