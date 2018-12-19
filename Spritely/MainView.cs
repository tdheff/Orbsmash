using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;
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
            
            // Add button to tab group
            var addbutton = new TextButton("+", TextButtonStyle.create(Color.Black, Color.DarkGray, Color.Green));
            addbutton.getLabel().setFontScale(10);
            _tabGroup.addElement(addbutton);

            _entity.addComponent(_canvas);
            addEntity(_entity);
        }
    }
}