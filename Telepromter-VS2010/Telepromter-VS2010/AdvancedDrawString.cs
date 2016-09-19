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
        SpriteFont font;
        float scale, length, spaceLength, midX;
        bool custom = false;
        public bool important = false;
        public AdvancedDrawString(String line, SpriteFont font, float scale, float midX, bool carry)
        {
            this.important = carry;
            if (important || Regex.IsMatch(line, @"<[\w]+>") || line.Contains("!"))
            {
                words = line.Split(' ');
                colors = new byte[words.Length];
                for (int i = 0; i < words.Length; i++)
                    if (!important && words[i].StartsWith("!")) {words[i] = Regex.Replace(words[i], "!", ""); important = true; colors[i] = 2; }
                    else if (important) { colors[i] = 2; }
                    else if (Regex.IsMatch(words[i], @"<[\w]+>"))
                    { colors[i] = 1; if (!AdvancedDrawString.nameColors.ContainsKey(words[i])) AdvancedDrawString.nameColors.Add(words[i], lastCol()); }
                    else colors[i] = 0;
                length = font.MeasureString(line).X * scale;
                spaceLength = font.MeasureString(" ").X * scale;
                custom = true;
            }
            else Line = line;
            this.midX = midX;
            this.font = font;
            this.scale = scale;

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
                float tempWidth = midX - (length / 2);
                for (int i = 0, t = words.Length; i < t; i++)
                {
                    Color drawColor = Color.White;
                    if (colors[i] == 1) drawColor = AdvancedDrawString.nameColors[words[i]];
                    else if (colors[i] == 2) drawColor = Color.Gray;
                    else if (highlighted) drawColor = Color.Yellow;
                    batch.DrawString(font, words[i], new Vector2(tempWidth, yVal), drawColor, 0f, halfHalf, scale, SpriteEffects.None, 0f);
                    tempWidth += (font.MeasureString(words[i]).X * scale) + spaceLength;
                }
            }
            else batch.DrawString(font, Line, new Vector2(midX - (font.MeasureString(Line).X / 2 * scale), yVal), highlighted ? Color.Yellow : Color.White, 0f, halfHalf, scale, SpriteEffects.None, 0f);
        }
    }
}