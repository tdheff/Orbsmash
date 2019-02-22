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

        private List<string> WinMessages = new List<string>() { "OUCH!", "OOOO THAT'S ROUGH!", "BETTER LUCK NEXT TIME!", "TRY PLAYING BETTER!" };

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
                switch(gameState.StateEnum)
                {
                    case GameStates.Service:
                        gameState.RightScore.setText(gameState.RightPoints.ToString());
                        gameState.LeftScore.setText(gameState.LeftPoints.ToString());
                        gameState.MainLabel.setText("SERVICE");
                        break;
                    case GameStates.Play:
                        gameState.MainLabel.setText("");
                        break;
                    case GameStates.PointScoredLeft:
                    case GameStates.PointScoredRight:
                        if(gameState.MainLabel.getText()== "")
                        {
                            gameState.MainLabel.setText(WinMessages.randomItem());
                        }
                        break;
                }
                
            }
        }

        private void FirstTimeSetup(Stage stage, GameState state)
        {
            state.MainLabel = new Label("Let's Roll!").setFontScale(10);
            var table = stage.addElement(new Table());
            state.Table = table;
            table.setFillParent(true).top().padTop(30);
            table.add(state.MainLabel).center();
            table.row().center().setPadTop(-40);
            var scoreTable = new Table();
            table.add(scoreTable);
            state.LeftScore = new Label("0").setFontScale(15);
            scoreTable.add(state.LeftScore);
            state.RightScore = new Label("0").setFontScale(15);
            scoreTable.add(state.RightScore).setPadLeft(1200);
        }
    }
}
