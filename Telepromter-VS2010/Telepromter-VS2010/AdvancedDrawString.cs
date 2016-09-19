using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Telepromter_VS2010
{
    class AdvancedDrawString
    {
        public static Dictionary<String, Color> nameColors = new Dictionary<string, Color>();
        public static List<Color> availableColors = new List<Color>();
        String[] words;
        byte[] colors;
        String Line;
        float length, spaceLength;
        bool custom = false;
        public bool important = false;
        public AdvancedDrawString(String line, bool carry)
        {
            this.important = carry;
            if (important || line.StartsWith("<") || line.StartsWith("!"))
            {
                words = line.Split(' ');
                colors = new byte[words.Length];
                for (int i = 0; i < words.Length; i++)
                    if (!important && words[i].StartsWith("!")) { words[i] = words[i].Substring(1); important = true; colors[i] = 2; }
                    else if (important) { colors[i] = 2; }
                    else if (Regex.IsMatch(words[i], @"<[\w]+>"))
                    { colors[i] = 1; if (!AdvancedDrawString.nameColors.ContainsKey(words[i])) AdvancedDrawString.nameColors.Add(words[i], lastCol()); }
                    else colors[i] = 0;
                length = Game1.font.MeasureString(line).X * Game1.scale;
                spaceLength = Game1.font.MeasureString(" ").X * Game1.scale;
                custom = true;
            }
            else Line = line;
        }
        public Color lastCol()
        {
            int last = AdvancedDrawString.availableColors.Count - 1;
            if (last == -1) { ColorGen(); return lastCol(); }
            Color col = AdvancedDrawString.availableColors[last];
            AdvancedDrawString.availableColors.RemoveAt(last);
            return col;
        }
        public void ColorGen()
        {
            AdvancedDrawString.availableColors = new System.Collections.Generic.List<Color>() { Color.Teal, Color.Tomato, Color.Pink, Color.SpringGreen, Color.Brown, Color.DarkOrchid, Color.IndianRed, Color.SkyBlue };
        }
        Vector2 halfHalf = new Vector2(0.5f, 0.5f);
        public void draw(SpriteBatch batch, float yVal, bool highlighted)
        {
            if (custom)
            {
                float tempWidth = Game1.middleX - (length / 2);
                for (int i = 0, t = words.Length; i < t; i++)
                {
                    Color drawColor;
                    switch (colors[i])
                    {
                        case 1: { drawColor = AdvancedDrawString.nameColors[words[i]]; break; }
                        case 2: { drawColor = Color.Gray; break; }
                        default: { if (highlighted) drawColor = Color.Yellow; else drawColor = Color.White; break; }
                    }
                    batch.DrawString(Game1.font, words[i], new Vector2(tempWidth, yVal), drawColor, 0f, halfHalf, Game1.scale, SpriteEffects.None, 0f);
                    tempWidth += (Game1.font.MeasureString(words[i]).X * Game1.scale) + spaceLength;
                }
            }
            else batch.DrawString(Game1.font, Line, new Vector2(Game1.middleX - (Game1.font.MeasureString(Line).X / 2 * Game1.scale), yVal), highlighted ? Color.Yellow : Color.White, 0f, halfHalf, Game1.scale, SpriteEffects.None, 0f);
        }
    }
}