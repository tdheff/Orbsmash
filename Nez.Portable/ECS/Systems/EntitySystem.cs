using System.Collections.Generic;

namespace Nez
{
    public class EntitySystem
    {
        protected List<Entity> _entities = new List<Entity>();

        protected Matcher _matcher;
        protected Scene _scene;


        public EntitySystem()
        {
            _matcher = Matcher.empty();
        }


        public EntitySystem(Matcher matcher)
        {
            _matcher = matcher;
        }

        public Matcher matcher => _matcher;

        public Scene scene
        {
            get => _scene;
            set
            {
                _scene = value;
                _entities = new List<Entity>();
            }
        }


        public virtual void onChange(Entity entity)
        {
            var contains = _entities.Contains(entity);
            var interest = _matcher.isInterested(entity);

            if (interest && !contains)
                add(entity);
            else if (!interest && contains)
                remove(entity);
        }


        public virtual void add(Entity entity)
        {
            _entities.Add(entity);
            onAdded(entity);
        }


        public virtual void remove(Entity entity)
        {
            _entities.Remove(entity);
            onRemoved(entity);
        }


        public virtual void onAdded(Entity entity)
        {
        }


        public virtual void onRemoved(Entity entity)
        {
        }


        protected virtual void process(List<Entity> entities)
        {
        }


        protected virtual void lateProcess(List<Entity> entities)
        {
        }


        protected virtual void begin()
        {
        }


        public void update()
        {
            begin();
            process(_entities);
        }


        public void lateUpdate()
        {
            lateProcess(_entities);
            end();
        }


        protected virtual void end()
        {
        }
    }
}