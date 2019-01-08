﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Nez
{
	/// <summary>
	///     moves taking collision into account only for reporting to any ITriggerListeners. The object will always move the
	///     full amount so it is up
	///     to the caller to destroy it on impact if desired.
	/// </summary>
	public class ProjectileMover : Component
    {
        private Collider _collider;
        private readonly List<ITriggerListener> _tempTriggerList = new List<ITriggerListener>();


        public override void onAddedToEntity()
        {
            _collider = entity.getComponent<Collider>();
            Assert.isNotNull(_collider, "null Collider. ProjectilMover requires a Collider!");
        }


        /// <summary>
        ///     moves the entity taking collisions into account
        /// </summary>
        /// <returns><c>true</c>, if move actor was newed, <c>false</c> otherwise.</returns>
        /// <param name="motion">Motion.</param>
        public bool move(Vector2 motion)
        {
            var didCollide = false;

            // fetch anything that we might collide with at our new position
            entity.transform.position += motion;

            // fetch anything that we might collide with us at our new position
            var neighbors = Physics.boxcastBroadphase(_collider.bounds, _collider.collidesWithLayers);
            foreach (var neighbor in neighbors)
                if (_collider.overlaps(neighbor))
                {
                    didCollide = true;
                    notifyTriggerListeners(_collider, neighbor);
                }

            return didCollide;
        }


        private void notifyTriggerListeners(Collider self, Collider other)
        {
            // notify any listeners on the Entity of the Collider that we overlapped
            other.entity.getComponents(_tempTriggerList);
            for (var i = 0; i < _tempTriggerList.Count; i++)
                _tempTriggerList[i].onTriggerEnter(self, other);

            _tempTriggerList.Clear();

            // notify any listeners on this Entity
            entity.getComponents(_tempTriggerList);
            for (var i = 0; i < _tempTriggerList.Count; i++)
                _tempTriggerList[i].onTriggerEnter(other, self);

            _tempTriggerList.Clear();
        }
    }
}