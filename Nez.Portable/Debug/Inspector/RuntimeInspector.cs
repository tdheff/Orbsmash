using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Nez.Console;
using Nez.UI;

#if DEBUG
namespace Nez
{
    public class RuntimeInspector : IDisposable
    {
        private const float kLeftCellWidth = 100;
        private ScreenSpaceCamera _camera;
        private Entity _entity;
        private readonly List<InspectorList> _inspectors = new List<InspectorList>();
        private ScrollPane _scrollPane;

        // ui fields
        private Skin _skin;
        private Table _table;

        private UICanvas ui;


        /// <summary>
        ///     creates a PostProcessor inspector
        /// </summary>
        public RuntimeInspector()
        {
            initialize();
        }


        /// <summary>
        ///     creates an Entity inspector
        /// </summary>
        /// <param name="entity">Entity.</param>
        public RuntimeInspector(Entity entity)
        {
            _entity = entity;
            initialize();
            cacheTransformInspector();
        }


        private void initialize()
        {
            prepCanvas();
            _camera = new ScreenSpaceCamera();
            Core.emitter.addObserver(CoreEvents.GraphicsDeviceReset, onGraphicsDeviceReset);
            Core.emitter.addObserver(CoreEvents.SceneChanged, onSceneChanged);
        }


        private void onGraphicsDeviceReset()
        {
            _scrollPane.setHeight(Screen.height);
        }


        private void onSceneChanged()
        {
            DebugConsole.instance._runtimeInspector = null;
            Dispose();
        }


        public void update()
        {
            // if we have an Entity this is an Entity inspector else it is a PostProcessor inspector
            if (_entity != null)
            {
                // update transform, which has a null Component
                getOrCreateInspectorList(null).update();

                for (var i = 0; i < _entity.components.count; i++)
                    getOrCreateInspectorList(_entity.components[i]).update();
            }
            else
            {
                for (var i = 0; i < Core.scene._postProcessors.length; i++)
                    getOrCreateInspectorList(Core.scene._postProcessors.buffer[i]).update();
            }
        }


        public void render()
        {
            // manually start a fresh batch and call the UICanvas Component lifecycle methods since it isnt attached to the Scene
            Graphics.instance.batcher.begin();
            (ui as IUpdatable).update();
            ui.render(Graphics.instance, _camera);
            Graphics.instance.batcher.end();
        }


        /// <summary>
        ///     attempts to find a cached version of the InspectorList and if it cant find one it will create a new one
        /// </summary>
        /// <returns>The or create inspector list.</returns>
        /// <param name="comp">Comp.</param>
        private InspectorList getOrCreateInspectorList(object comp)
        {
            var inspector = _inspectors.Where(i => i.target == comp).FirstOrDefault();
            if (inspector == null)
            {
                inspector = new InspectorList(comp);
                inspector.initialize(_table, _skin, kLeftCellWidth);
                _inspectors.Add(inspector);
            }

            return inspector;
        }


        private void cacheTransformInspector()
        {
            // add Transform separately
            var transformInspector = new InspectorList(_entity.transform);
            transformInspector.initialize(_table, _skin, kLeftCellWidth);
            _inspectors.Add(transformInspector);
        }


        private void prepCanvas()
        {
            _skin = Skin.createDefaultSkin();

            // modify some of the default styles to better suit our needs
            var tfs = _skin.get<TextFieldStyle>();
            tfs.background.leftWidth = tfs.background.rightWidth = 4;
            tfs.background.bottomHeight = 0;
            tfs.background.topHeight = 3;

            var checkbox = _skin.get<CheckBoxStyle>();
            checkbox.checkboxOn.minWidth = checkbox.checkboxOn.minHeight = 15;
            checkbox.checkboxOff.minWidth = checkbox.checkboxOff.minHeight = 15;
            checkbox.checkboxOver.minWidth = checkbox.checkboxOver.minHeight = 15;

            // since we arent using this as a Component on an Entity we'll fake it here
            ui = new UICanvas();
            ui.onAddedToEntity();
            ui.stage.isFullScreen = true;

            _table = new Table();
            _table.top().left();
            _table.defaults().setPadTop(4).setPadLeft(4).setPadRight(0).setAlign(Align.left);
            _table.setBackground(new PrimitiveDrawable(new Color(40, 40, 40)));

            // wrap up the table in a ScrollPane
            _scrollPane = ui.stage.addElement(new ScrollPane(_table, _skin));
            // force a validate which will layout the ScrollPane and populate the proper scrollBarWidth
            _scrollPane.validate();
            _scrollPane.setSize(295 + _scrollPane.getScrollBarWidth(), Screen.height);
        }


        #region IDisposable Support

        private bool _disposedValue;

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                Core.emitter.removeObserver(CoreEvents.GraphicsDeviceReset, onGraphicsDeviceReset);
                Core.emitter.removeObserver(CoreEvents.SceneChanged, onSceneChanged);
                _entity = null;
                _disposedValue = true;
            }
        }


        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
#endif