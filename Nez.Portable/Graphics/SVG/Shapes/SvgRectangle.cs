﻿using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace Nez.Svg
{
    public class SvgRectangle : SvgElement
    {
        [XmlAttribute("height")] public float height;

        [XmlAttribute("width")] public float width;

        [XmlAttribute("x")] public float x;

        [XmlAttribute("y")] public float y;

        public Vector2 center => new Vector2(x + width / 2, y + height / 2);


        /// <summary>
        ///     gets the points for the rectangle with all transforms applied
        /// </summary>
        /// <returns>The transformed points.</returns>
        public Vector2[] getTransformedPoints()
        {
            var pts = new[]
            {
                new Vector2(x, y), new Vector2(x + width, y), new Vector2(x + width, y + height),
                new Vector2(x, y + height)
            };
            var mat = getCombinedMatrix();
            Vector2Ext.transform(pts, ref mat, pts);

            return pts;
        }
    }
}