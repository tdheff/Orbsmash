namespace Nez.AI.FSM
{
    public abstract class State<T>
    {
        protected T _context;
        protected StateMachine<T> _machine;


        public void setMachineAndContext(StateMachine<T> machine, T context)
        {
            _machine = machine;
            _context = context;
            onInitialized();
        }


        /// <summary>
        ///     called directly after the machine and context are set allowing the state to do any required setup
        /// </summary>
        public virtual void onInitialized()
        {
        }


        /// <summary>
        ///     called when the state becomes the active state
        /// </summary>
        public virtual void begin()
        {
        }


        /// <summary>
        ///     called before update allowing the state to have one last chance to change state
        /// </summary>
        public virtual void reason()
        {
        }


        /// <summary>
        ///     called every frame this state is the active state
        /// </summary>
        /// <param name="deltaTime">Delta time.</param>
        public abstract void update(float deltaTime);


        /// <summary>
        ///     called when this state is no longer the active state
        /// </summary>
        public virtual void end()
        {
        }
    }
}