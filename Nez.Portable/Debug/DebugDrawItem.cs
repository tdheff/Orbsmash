﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez.BitmapFonts;

namespace Nez
{
    internal class DebugDrawItem
    {
        public BitmapFont bitmapFont;

        // shared by multiple items
        public Color color;

        internal DebugDrawType drawType;
        public float duration;
        public Vector2 end;
        public Vector2 position;
        public Rectangle rectangle;
        public float scale;
        public int size;
        public NezSpriteFont spriteFont;

        // used for Line items
        public Vector2 start;

        // used for Text items
        public string text;

        // used for Pixel items
        public float x, y;


        public DebugDrawItem(Vector2 start, Vector2 end, Color color, float duration)
        {
            this.start = start;
            this.end = end;
            this.color = color;
            this.duration = duration;
            drawType = DebugDrawType.Line;
        }


        public DebugDrawItem(Rectangle rectangle, Color color, float duration)
        {
            this.rectangle = rectangle;
            this.color = color;
            this.duration = duration;
            drawType = DebugDrawType.HollowRectangle;
        }


        public DebugDrawItem(float x, float y, int size, Color color, float duration)
        {
            this.x = x;
            this.y = y;
            this.size = size;
            this.color = color;
            this.duration = duration;
            drawType = DebugDrawType.Pixel;
        }


        public DebugDrawItem(BitmapFont bitmapFont, string text, Vector2 position, Color color, float duration,
            float scale)
        {
            this.bitmapFont = bitmapFont;
            this.text = text;
            this.position = position;
            this.color = color;
            this.scale = scale;
            this.duration = duration;
            drawType = DebugDrawType.BitmapFontText;
        }


        public DebugDrawItem(NezSpriteFont spriteFont, string text, Vector2 position, Color color, float duration,
            float scale)
        {
            this.spriteFont = spriteFont;
            this.text = text;
            this.position = position;
            this.color = color;
            this.scale = scale;
            this.duration = duration;
            drawType = DebugDrawType.SpriteFontText;
        }


        public DebugDrawItem(string text, Color color, float duration, float scale)
        {
            bitmapFont = Graphics.instance.bitmapFont;
            this.text = text;
            this.color = color;
            this.scale = scale;
            this.duration = duration;
            drawType = DebugDrawType.ConsoleText;
        }


        /// <summary>
        ///     returns true if we are done with this debug draw item
        /// </summary>
        public bool draw(Graphics graphics)
        {
            switch (drawType)
            {
                case DebugDrawType.Line:
                    graphics.batcher.drawLine(start, end, color);
                    break;
                case DebugDrawType.HollowRectangle:
                    graphics.batcher.drawHollowRect(rectangle, color);
                    break;
                case DebugDrawType.Pixel:
                    graphics.batcher.drawPixel(x, y, color, size);
                    break;
                case DebugDrawType.BitmapFontText:
                    graphics.batcher.drawString(bitmapFont, text, position, color, 0f, Vector2.Zero, scale,
                        SpriteEffects.None, 0f);
                    break;
                case DebugDrawType.SpriteFontText:
                    graphics.batcher.drawString(spriteFont, text, position, color, 0f, Vector2.Zero, new Vector2(scale),
                        SpriteEffects.None, 0f);
                    break;
                case DebugDrawType.ConsoleText:
                    graphics.batcher.drawString(bitmapFont, text, position, color, 0f, Vector2.Zero, scale,
                        SpriteEffects.None, 0f);
                    break;
            }

            duration -= Time.deltaTime;

            return duration < 0f;
        }


        public float getHeight()
        {
            switch (drawType)
            {
                case DebugDrawType.Line:
                    return (end - start).Y;
                case DebugDrawType.HollowRectangle:
                    return rectangle.Height;
                case DebugDrawType.Pixel:
                    return size;
                case DebugDrawType.BitmapFontText:
                case DebugDrawType.ConsoleText:
                    return bitmapFont.measureString(text).Y * scale;
                case DebugDrawType.SpriteFontText:
                    return spriteFont.measureString(text).Y * scale;
            }

            return 0;
        }

        internal enum DebugDrawType
        {
            Line,
            HollowRectangle,
            Pixel,
            BitmapFontText,
            SpriteFontText,
            ConsoleText
        }
    }
}