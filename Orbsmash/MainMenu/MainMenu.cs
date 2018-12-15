using System.CodeDom;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;
using Orbsmash.Game;
using Scene = Handy.Scene;

namespace Orbsmash.MainMenu
{
    public class MainMenu : Scene
    {
        private Skin _skin;
        private UICanvas _canvas;
        
        public MainMenu() : base()
        {
            var menuEntity = new Entity();
            var menuComponent = new MainMenuComponent();
            menuEntity.addComponent(menuComponent);
            
            _skin = Skin.createDefaultSkin();
            _canvas = new UICanvas();
            
            // tables are very flexible and make good candidates to use at the root of your UI. They work much like HTML tables but with more flexibility.
            var table = _canvas.stage.addElement( new Table() );

            // tell the table to fill all the available space. In this case that would be the entire screen.
            table.setFillParent( true );

            // Play button
            var button = new TextButton("Play", _skin );
            table.add( button ).setMinWidth( 100 ).setMinHeight( 30 );
            button.getLabel().setFontScale(10);
            button.onClicked += menuComponent.ClickPlay;
            _canvas.stage.setGamepadFocusElement(button);
            table.row();
            
            // Play button
            button = new TextButton("Quit", _skin );
            button.getLabel().setFontScale(10);
            table.add(button).setMinWidth( 100 ).setMinHeight( 30 );;
            button.onClicked += menuComponent.ClickQuit;
            table.row();
            

            menuEntity.addComponent(_canvas);
            addEntity(menuEntity);
        }
        
        protected override EntitySystem[] Systems()
        {
            return new EntitySystem[] { new MainMenuSystem() };
        }
    }
}