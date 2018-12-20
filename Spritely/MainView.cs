using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Forms.VisualStyles;
using Handy.UI.Reactive;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;

namespace Spritely
{
    public class MainView : Scene
    {
        // Layout items
        private UICanvas _canvas = new UICanvas();
        private Entity _entity = new Entity();
        private Stage _stage;
        private Table _rootTable;
        private HorizontalGroup _tabGroup;
        
        // Subviews
        private List<AnimationView> _animationViews = new List<AnimationView>();

        public MainView()
        {
            addRenderer(new DefaultRenderer());
        }
        
        public override void initialize()
        {
            _stage = new Stage();
            _canvas.stage = _stage;

            // Add root table
            _rootTable = _stage.addElement(new Table());
            _rootTable.setFillParent( true );
            
            // Add tab bar
            _tabGroup = new HorizontalGroup();
            _rootTable.add(_tabGroup);
            
            
            // Example using event pattern (that i hacked into button)
            var buttonOne = new TextButton("One", TextButtonStyle.create(Color.Black, Color.DarkGray, Color.Green));
            buttonOne.getLabel().setFontScale(10);
            _tabGroup.addElement(buttonOne);
            var observableOne = Observable.FromEventPattern<EventArgs>(buttonOne, "onClickedBetter");
            observableOne.Subscribe(evt =>
            {
                Console.WriteLine("Button One");
            }) ;
            
            // Example using non-standard event (like are actually in Nez)
            var buttonTwo = new TextButton("Two", TextButtonStyle.create(Color.Black, Color.DarkGray, Color.Green));
            buttonTwo.getLabel().setFontScale(10);
            _tabGroup.addElement(buttonTwo);
            var observableTwo = Observable.FromEvent<Button>(
                handler => buttonTwo.onClicked += handler,
                handler => buttonTwo.onClicked -= handler
            );
            observableTwo.Subscribe(evt =>
            {
                Console.WriteLine("Button Two");
            }) ;
            
            // Example using my own custom thingy
            var buttonThree = new RxTextButton("Three", TextButtonStyle.create(Color.Black, Color.DarkGray, Color.Green));
            buttonThree.getLabel().setFontScale(10);
            _tabGroup.addElement(buttonThree);
            buttonThree.OnClick_.Subscribe(evt => { Console.WriteLine("Button Three"); });

            _tabGroup._spacing = 20.0f;
            
            _entity.addComponent(_canvas);
            addEntity(_entity);
        }
    }
}