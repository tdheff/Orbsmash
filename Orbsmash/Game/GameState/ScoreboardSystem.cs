using Nez;
using Nez.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbsmash.Game
{
    public class ScoreboardSystem : EntitySystem
    {
        public ScoreboardSystem() : base(new Matcher().all(typeof(UICanvas), typeof(GameStateComponent))) { }

        protected override void process(List<Entity> entities)
        {
            foreach(var entity in entities)
            {
                var gameState = entity.getComponent<GameStateComponent>().State;
                var scoreboard = entity.getComponent<UICanvas>("scoreboard");
                var stage = scoreboard.stage;

                if (!gameState.ScoreboardSetup)
                {
                    FirstTimeSetup(stage, gameState);
                    gameState.ScoreboardSetup = true;
                }
                // keep the scores up to date
                if(gameState.StateEnum == GameStates.Service)
                {
                    gameState.RightScore.setText(gameState.RightPoints.ToString());
                    gameState.LeftScore.setText(gameState.LeftPoints.ToString());
                }
                
            }
        }

        private void FirstTimeSetup(Stage stage, GameState state)
        {
            state.MainLabel = new Label("Let's Roll!").setFontScale(8);
            var table = stage.addElement(new Table());
            state.Table = table;
            table.setFillParent(true).top().padTop(30);
            table.add(state.MainLabel).center();
            table.row().center().setPadTop(20);
            var scoreTable = new Table();
            table.add(scoreTable);
            state.LeftScore = new Label("0").setFontScale(5);
            scoreTable.add(state.LeftScore);
            state.RightScore = new Label("0").setFontScale(5);
            scoreTable.add(state.RightScore).setPadLeft(200);
        }
    }
}
