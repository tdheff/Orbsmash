﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Nez.Spatial
{
	/// <summary>
	///     A QuadTree Object that provides fast and efficient storage of objects in a world space.
	/// </summary>
	/// <typeparam name="T">Any object implementing IQuadStorable.</typeparam>
	public class QuadTreeNode<T> where T : IQuadTreeStorable
    {
        // How many objects can exist in a QuadTree before it sub divides itself
        private const int maxObjectsPerNode = 2;

        // Bottom Left Child

        // Bottom Right Child

        // Top Left Child

        // Top Right Child


        // The objects in this QuadTree
        private List<QuadTreeObject<T>> _objects;

        // The parent of this quad

        // The area this QuadTree represents
        private Rectangle _rect;


        /// <summary>
        ///     Creates a QuadTree for the specified area.
        /// </summary>
        /// <param name="rect">The area this QuadTree object will encompass.</param>
        public QuadTreeNode(Rectangle rect)
        {
            _rect = rect;
        }


        /// <summary>
        ///     Creates a QuadTree for the specified area.
        /// </summary>
        /// <param name="x">The top-left position of the area rectangle.</param>
        /// <param name="y">The top-right position of the area rectangle.</param>
        /// <param name="width">The width of the area rectangle.</param>
        /// <param name="height">The height of the area rectangle.</param>
        public QuadTreeNode(int x, int y, int width, int height)
        {
            _rect = new Rectangle(x, y, width, height);
        }


        private QuadTreeNode(QuadTreeNode<T> parent, Rectangle rect) : this(rect)
        {
            this.parent = parent;
        }


        /// <summary>
        ///     The area this QuadTree represents.
        /// </summary>
        public Rectangle quadRect => _rect;

        /// <summary>
        ///     The top left child for this QuadTree
        /// </summary>
        public QuadTreeNode<T> topLeftChild { get; private set; }

        /// <summary>
        ///     The top right child for this QuadTree
        /// </summary>
        public QuadTreeNode<T> topRightChild { get; private set; }

        /// <summary>
        ///     The bottom left child for this QuadTree
        /// </summary>
        public QuadTreeNode<T> bottomLeftChild { get; private set; }

        /// <summary>
        ///     The bottom right child for this QuadTree
        /// </summary>
        public QuadTreeNode<T> bottomRightChild { get; private set; }

        /// <summary>
        ///     This QuadTree's parent
        /// </summary>
        public QuadTreeNode<T> parent { get; }

        /// <summary>
        ///     How many total objects are contained within this QuadTree (ie, includes children)
        /// </summary>
        public int count => objectCount();

        /// <summary>
        ///     Returns true if this is a empty leaf node
        /// </summary>
        public bool isEmptyLeaf => count == 0 && topLeftChild == null;


        /// <summary>
        ///     Add an item to the object list.
        /// </summary>
        /// <param name="item">The item to add.</param>
        private void add(QuadTreeObject<T> item)
        {
            if (_objects == null)
                _objects = new List<QuadTreeObject<T>>();

            item.owner = this;
            _objects.Add(item);
        }


        /// <summary>
        ///     Remove an item from the object list.
        /// </summary>
        /// <param name="item">The object to remove.</param>
        private void remove(QuadTreeObject<T> item)
        {
            if (_objects != null)
            {
                var removeIndex = _objects.IndexOf(item);
                if (removeIndex >= 0)
                {
                    _objects[removeIndex] = _objects[_objects.Count - 1];
                    _objects.RemoveAt(_objects.Count - 1);
                }
            }
        }


        /// <summary>
        ///     Get the total for all objects in this QuadTree, including children.
        /// </summary>
        /// <returns>The number of objects contained within this QuadTree and its children.</returns>
        private int objectCount()
        {
            var count = 0;

            // Add the objects at this level
            if (_objects != null) count += _objects.Count;

            // Add the objects that are contained in the children
            if (topLeftChild != null)
            {
                count += topLeftChild.objectCount();
                count += topRightChild.objectCount();
                count += bottomLeftChild.objectCount();
                count += bottomRightChild.objectCount();
            }

            return count;
        }


        /// <summary>
        ///     Subdivide this QuadTree and move it's children into the appropriate Quads where applicable.
        /// </summary>
        private void subdivide()
        {
            // We've reached capacity, subdivide...
            var size = new Point(_rect.Width / 2, _rect.Height / 2);
            var mid = new Point(_rect.X + size.X, _rect.Y + size.Y);

            topLeftChild = new QuadTreeNode<T>(this, new Rectangle(_rect.Left, _rect.Top, size.X, size.Y));
            topRightChild = new QuadTreeNode<T>(this, new Rectangle(mid.X, _rect.Top, size.X, size.Y));
            bottomLeftChild = new QuadTreeNode<T>(this, new Rectangle(_rect.Left, mid.Y, size.X, size.Y));
            bottomRightChild = new QuadTreeNode<T>(this, new Rectangle(mid.X, mid.Y, size.X, size.Y));

            // If they're completely contained by the quad, bump objects down
            for (var i = 0; i < _objects.Count; i++)
            {
                var destTree = getDestinationTree(_objects[i]);

                if (destTree != this)
                {
                    // Insert to the appropriate tree, remove the object, and back up one in the loop
                    destTree.insert(_objects[i]);
                    remove(_objects[i]);
                    i--;
                }
            }
        }


        /// <summary>
        ///     Get the child Quad that would contain an object.
        /// </summary>
        /// <param name="item">The object to get a child for.</param>
        /// <returns></returns>
        private QuadTreeNode<T> getDestinationTree(QuadTreeObject<T> item)
        {
            // If a child can't contain an object, it will live in this Quad
            var destTree = this;

            if (topLeftChild.quadRect.Contains(item.data.bounds))
                destTree = topLeftChild;
            else if (topRightChild.quadRect.Contains(item.data.bounds))
                destTree = topRightChild;
            else if (bottomLeftChild.quadRect.Contains(item.data.bounds))
                destTree = bottomLeftChild;
            else if (bottomRightChild.quadRect.Contains(item.data.bounds))
                destTree = bottomRightChild;

            return destTree;
        }


        private void relocate(QuadTreeObject<T> item)
        {
            // Are we still inside our parent?
            if (quadRect.Contains(item.data.bounds))
            {
                // Good, have we moved inside any of our children?
                if (topLeftChild != null)
                {
                    var dest = getDestinationTree(item);
                    if (item.owner != dest)
                    {
                        // Delete the item from this quad and add it to our child
                        // Note: Do NOT clean during this call, it can potentially delete our destination quad
                        var formerOwner = item.owner;
                        delete(item, false);
                        dest.insert(item);

                        // Clean up ourselves
                        formerOwner.cleanUpwards();
                    }
                }
            }
            else
            {
                // We don't fit here anymore, move up, if we can
                if (parent != null)
                    parent.relocate(item);
            }
        }


        private void cleanUpwards()
        {
            if (topLeftChild != null)
            {
                // If all the children are empty leaves, delete all the children
                if (topLeftChild.isEmptyLeaf && topRightChild.isEmptyLeaf && bottomLeftChild.isEmptyLeaf &&
                    bottomRightChild.isEmptyLeaf)
                {
                    topLeftChild = null;
                    topRightChild = null;
                    bottomLeftChild = null;
                    bottomRightChild = null;

                    if (parent != null && count == 0)
                        parent.cleanUpwards();
                }
            }
            else
            {
                // I could be one of 4 empty leaves, tell my parent to clean up
                if (parent != null && count == 0)
                    parent.cleanUpwards();
            }
        }


        /// <summary>
        ///     Clears the QuadTree of all objects, including any objects living in its children.
        /// </summary>
        internal void clear()
        {
            // Clear out the children, if we have any
            if (topLeftChild != null)
            {
                topLeftChild.clear();
                topRightChild.clear();
                bottomLeftChild.clear();
                bottomRightChild.clear();
            }

            // Clear any objects at this level
            if (_objects != null)
            {
                _objects.Clear();
                _objects = null;
            }

            // Set the children to null
            topLeftChild = null;
            topRightChild = null;
            bottomLeftChild = null;
            bottomRightChild = null;
        }


        /// <summary>
        ///     Deletes an item from this QuadTree. If the object is removed causes this Quad to have no objects in its children,
        ///     it's children will be removed as well.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <param name="clean">Whether or not to clean the tree</param>
        internal void delete(QuadTreeObject<T> item, bool clean)
        {
            if (item.owner != null)
            {
                if (item.owner == this)
                {
                    remove(item);
                    if (clean)
                        cleanUpwards();
                }
                else
                {
                    item.owner.delete(item, clean);
                }
            }
        }


        /// <summary>
        ///     Insert an item into this QuadTree object.
        /// </summary>
        /// <param name="item">The item to insert.</param>
        internal void insert(QuadTreeObject<T> item)
        {
            // If this quad doesn't contain the items rectangle, do nothing, unless we are the root
            if (!_rect.Contains(item.data.bounds))
            {
                Assert.isNull(parent, "We are not the root, and this object doesn't fit here. How did we get here?");
                if (parent == null)
                    add(item);
                else
                    return;
            }

            if (_objects == null || topLeftChild == null && _objects.Count + 1 <= maxObjectsPerNode)
            {
                // If there's room to add the object, just add it
                add(item);
            }
            else
            {
                // No quads, create them and bump objects down where appropriate
                if (topLeftChild == null)
                    subdivide();

                // Find out which tree this object should go in and add it there
                var destTree = getDestinationTree(item);
                if (destTree == this)
                    add(item);
                else
                    destTree.insert(item);
            }
        }


        /// <summary>
        ///     Get the objects in this tree that intersect with the specified rectangle.
        /// </summary>
        /// <param name="searchRect">The rectangle to find objects in.</param>
        internal List<T> getObjects(Rectangle searchRect)
        {
            var results = new List<T>();
            getObjects(searchRect, ref results);
            return results;
        }


        /// <summary>
        ///     Get the objects in this tree that intersect with the specified rectangle.
        /// </summary>
        /// <param name="searchRect">The rectangle to find objects in.</param>
        /// <param name="results">A reference to a list that will be populated with the results.</param>
        internal void getObjects(Rectangle searchRect, ref List<T> results)
        {
            // We can't do anything if the results list doesn't exist
            if (results != null)
            {
                if (searchRect.Contains(_rect))
                {
                    // If the search area completely contains this quad, just get every object this quad and all it's children have
                    getAllObjects(ref results);
                }
                else if (searchRect.Intersects(_rect))
                {
                    // Otherwise, if the quad isn't fully contained, only add objects that intersect with the search rectangle
                    if (_objects != null)
                        for (var i = 0; i < _objects.Count; i++)
                            if (searchRect.Intersects(_objects[i].data.bounds))
                                results.Add(_objects[i].data);

                    // Get the objects for the search rectangle from the children
                    if (topLeftChild != null)
                    {
                        topLeftChild.getObjects(searchRect, ref results);
                        topRightChild.getObjects(searchRect, ref results);
                        bottomLeftChild.getObjects(searchRect, ref results);
                        bottomRightChild.getObjects(searchRect, ref results);
                    }
                }
            }
        }


        /// <summary>
        ///     Get all objects in this Quad, and it's children.
        /// </summary>
        /// <param name="results">A reference to a list in which to store the objects.</param>
        internal void getAllObjects(ref List<T> results)
        {
            // If this Quad has objects, add them
            if (_objects != null)
                for (var i = 0; i < _objects.Count; i++)
                    results.Add(_objects[i].data);

            // If we have children, get their objects too
            if (topLeftChild != null)
            {
                topLeftChild.getAllObjects(ref results);
                topRightChild.getAllObjects(ref results);
                bottomLeftChild.getAllObjects(ref results);
                bottomRightChild.getAllObjects(ref results);
            }
        }


        /// <summary>
        ///     Moves the QuadTree object in the tree
        /// </summary>
        /// <param name="item">The item that has moved</param>
        internal void move(QuadTreeObject<T> item)
        {
            if (item.owner != null)
                item.owner.relocate(item);
            else
                relocate(item);
        }
    }
}