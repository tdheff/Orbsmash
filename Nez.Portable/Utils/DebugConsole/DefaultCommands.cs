using System;
using System.Collections.Generic;
using System.Text;

namespace Nez.Console
{
	/// <summary>
	///     add this attribute to any static method
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        public string help;
        public string name;


        public CommandAttribute(string name, string help)
        {
            this.name = name;
            this.help = help;
        }
    }
}


#if DEBUG
namespace Nez.Console
{
    public partial class DebugConsole
    {
        private static ITimer _drawCallTimer;

        [Command("clear", "Clears the terminal")]
        private static void clear()
        {
            instance._drawCommands.Clear();
        }


        [Command("exit", "Exits the game")]
        private static void exit()
        {
            Core.exit();
        }


        [Command("inspect",
            "Inspects the Entity with the passed in name, or pass in 'pp' or 'postprocessors' to inspect all PostProccessors in the Scene. Pass in no name to close the inspector.")]
        private static void inspectEntity(string entityName = "")
        {
            // clean up no matter what
            if (instance._runtimeInspector != null)
            {
                instance._runtimeInspector.Dispose();
                instance._runtimeInspector = null;
            }

            if (entityName == "pp" || entityName == "postprocessors")
            {
                instance._runtimeInspector = new RuntimeInspector();
                instance.isOpen = false;
            }
            else if (entityName != "")
            {
                var entity = Core.scene.findEntity(entityName);
                if (entity == null)
                {
                    instance.log("could not find entity named " + entityName);
                    return;
                }

                instance._runtimeInspector = new RuntimeInspector(entity);
                instance.isOpen = false;
            }
        }


        [Command("console-scale", "Sets the scale that the console is rendered. Defaults to 1 and has a max of 5.")]
        private static void setScale(float scale = 1f)
        {
            renderScale = Mathf.clamp(scale, 0.2f, 5f);
        }


        [Command("assets", "Logs all loaded assets. Pass 's' for scene assets or 'g' for global assets")]
        private static void logLoadedAssets(string whichAssets = "s")
        {
            if (whichAssets == "s")
                instance.log(Core.scene.content.logLoadedAssets());
            else if (whichAssets == "g")
                instance.log(Core.content.logLoadedAssets());
            else
                instance.log("Invalid parameter");
        }


        [Command("vsync", "Enables or disables vertical sync")]
        private static void vsync(bool enabled = true)
        {
            Screen.synchronizeWithVerticalRetrace = enabled;
            instance.log("Vertical Sync " + (enabled ? "Enabled" : "Disabled"));
        }


        [Command("fixed", "Enables or disables fixed time step")]
        private static void fixedTimestep(bool enabled = true)
        {
            Core._instance.IsFixedTimeStep = enabled;
            instance.log("Fixed Time Step " + (enabled ? "Enabled" : "Disabled"));
        }


        [Command("framerate", "Sets the target framerate. Defaults to 60.")]
        private static void framerate(float target = 60f)
        {
            Core._instance.TargetElapsedTime = TimeSpan.FromSeconds(1.0 / target);
        }

        [Command("log-drawcalls",
            "Enables/disables logging of draw calls in the standard console. Call once to enable and again to disable. delay is how often they should be logged and defaults to 1s.")]
        private static void logDrawCalls(float delay = 1f)
        {
            if (_drawCallTimer != null)
            {
                _drawCallTimer.stop();
                _drawCallTimer = null;
                Debug.log("Draw call logging stopped");
            }
            else
            {
                _drawCallTimer = Core.schedule(delay, true, timer => { Debug.log("Draw Calls: {0}", Core.drawCalls); });
            }
        }


        [Command("entity-count",
            "Logs amount of Entities in the Scene. Pass a tagIndex to count only Entities with that tag")]
        private static void entityCount(int tagIndex = -1)
        {
            if (Core.scene == null)
            {
                instance.log("Current Scene is null!");
                return;
            }

            if (tagIndex < 0)
                instance.log("Total entities: " + Core.scene.entities.count);
            else
                instance.log("Total entities with tag [" + tagIndex + "] " +
                             Core.scene.findEntitiesWithTag(tagIndex).Count);
        }


        [Command("renderable-count",
            "Logs amount of Renderables in the Scene. Pass a renderLayer to count only Renderables in that layer")]
        private static void renderableCount(int renderLayer = int.MinValue)
        {
            if (Core.scene == null)
            {
                instance.log("Current Scene is null!");
                return;
            }

            if (renderLayer != int.MinValue)
                instance.log("Total renderables with tag [" + renderLayer + "] " +
                             Core.scene.renderableComponents.componentsWithRenderLayer(renderLayer).length);
            else
                instance.log("Total renderables: " + Core.scene.renderableComponents.count);
        }


        [Command("renderable-log",
            "Logs the Renderables in the Scene. Pass a renderLayer to log only Renderables in that layer")]
        private static void renderableLog(int renderLayer = int.MinValue)
        {
            if (Core.scene == null)
            {
                instance.log("Current Scene is null!");
                return;
            }

            var builder = new StringBuilder();
            for (var i = 0; i < Core.scene.renderableComponents.count; i++)
            {
                var renderable = Core.scene.renderableComponents[i];
                if (renderLayer == int.MinValue || renderable.renderLayer == renderLayer)
                    builder.AppendFormat("{0}\n", renderable);
            }

            instance.log(builder.ToString());
        }


        [Command("entity-list", "Logs all entities")]
        private static void logEntities(string whichAssets = "s")
        {
            if (Core.scene == null)
            {
                instance.log("Current Scene is null!");
                return;
            }

            var builder = new StringBuilder();
            for (var i = 0; i < Core.scene.entities.count; i++)
                builder.AppendLine(Core.scene.entities[i].ToString());

            instance.log(builder.ToString());
        }


        [Command("timescale", "Sets the timescale. Defaults to 1")]
        private static void tilescale(float timeScale = 1)
        {
            Time.timeScale = timeScale;
        }


        [Command("physics", "Logs the total Collider count in the spatial hash")]
        private static void physics(float secondsToDisplay = 5f)
        {
            // store off the current state so we can reset it when we are done
            var debugRenderState = Core.debugRenderEnabled;
            Core.debugRenderEnabled = true;

            var ticker = 0f;
            Core.schedule(0f, true, null, timer =>
            {
                Physics.debugDraw(0f);
                ticker += Time.deltaTime;
                if (ticker >= secondsToDisplay)
                {
                    timer.stop();
                    Core.debugRenderEnabled = debugRenderState;
                }
            });

            instance.log("Physics system collider count: " + ((HashSet<Collider>) Physics.getAllColliders()).Count);
        }


        [Command("debug-render", "enables/disables debug rendering")]
        private static void debugRender()
        {
            Core.debugRenderEnabled = !Core.debugRenderEnabled;
            instance.log(string.Format("Debug rendering {0}", Core.debugRenderEnabled ? "enabled" : "disabled"));
        }


        [Command("help", "Shows usage help for a given command")]
        private static void help(string command)
        {
            if (instance._sorted.Contains(command))
            {
                var c = instance._commands[command];
                var str = new StringBuilder();

                //Title
                str.Append(":: ");
                str.Append(command);

                //Usage
                if (!string.IsNullOrEmpty(c.usage))
                {
                    str.Append(" ");
                    str.Append(c.usage);
                }

                instance.log(str.ToString());

                //Help
                if (string.IsNullOrEmpty(c.help))
                    instance.log("No help info set");
                else
                    instance.log(c.help);
            }
            else
            {
                var str = new StringBuilder();
                str.Append("Commands list: ");
                str.Append(string.Join(", ", instance._sorted));
                instance.log(str.ToString());
                instance.log("Type 'help command' for more info on that command!");
            }
        }
    }
}
#endif