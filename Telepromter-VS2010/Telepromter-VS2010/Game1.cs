using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Collections.Generic;
namespace Telepromter_VS2010
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<AdvancedDrawString> Lines = new List<AdvancedDrawString>();
        string[] Words;
        SpriteFont font;
        Texture2D triangle;
        float startPos = 200;
        float middleX, middleY;
        bool hasLoaded = false;
        float fontSize, scale = 1;
        Vector2 halfHalf, leftTri, rightTri;
        Rectangle triRect = new Rectangle(0, 0, 400, 400);
        bool showCuts = false;
        Vector2 zero = Vector2.Zero;
        KeyboardState oldState;
        float StringSize = 0;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //this.graphics.ToggleFullScreen();
            middleY = graphics.PreferredBackBufferHeight / 2;
            middleX = graphics.PreferredBackBufferWidth / 2;
            IsMouseVisible = true;
            String dir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\Telepromter\";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        }
        protected override void Initialize()
        {
            Mouse.SetPosition(100, (int)middleY);
            fontSize = new Save().readScore();
            halfHalf = new Vector2(0.5f, 0.5f); leftTri = new Vector2(-20, 200); rightTri = new Vector2(Window.ClientBounds.Width - 95, 200);
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("pericles14");
            triangle = Content.Load<Texture2D>("triangle");
            loadFile();
        }
        protected void loadFile()
        {
            try
            {
                using (StreamReader reader = new StreamReader(File.OpenRead(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\Telepromter\" + "Prompt.txt")))
                    Words = reader.ReadToEnd().Split('\n');
            }
            catch (FileNotFoundException e)
            {
                Words = "UH OH: there appears to be an oopsie.\n I was unable to locate your text file.\n Please place a file named 'Prompt.txt' in the Teleprompter Desktop Folder".Split('\n');
            }
            load();
        }
        protected override void UnloadContent()
        {
            new Save().writeScore((int)fontSize);
            base.UnloadContent();
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Mouse.GetState().LeftButton == ButtonState.Pressed) smartScroll((middleY - Mouse.GetState().Y) / 10);
            else smartScroll((middleY - Mouse.GetState().Y) / 100);
            if (Mouse.GetState().RightButton == ButtonState.Pressed) Mouse.SetPosition(100, (int)middleY);
            else if (Keyboard.GetState().IsKeyDown(Keys.Down)) { Mouse.SetPosition(100, (int)middleY); smartScroll(-5); }
            else if (Keyboard.GetState().IsKeyDown(Keys.Up)) { Mouse.SetPosition(100, (int)middleY); smartScroll(5); }
            else if (KeyPressed(Keys.F1)) showCuts = !showCuts;
            else if (KeyPressed(Keys.F2)) { Mouse.SetPosition(100, (int)middleY); startPos = 225; }
            else if (KeyPressed(Keys.F3)) { Mouse.SetPosition(100, (int)middleY); startPos = 225; loadFile(); }
            else if (KeyPressed(Keys.OemPlus)) { fontSize++; load(); }
            else if (KeyPressed(Keys.OemMinus) && fontSize > 10) { fontSize--; load(); }
            else if (KeyPressed(Keys.H)) { Mouse.SetPosition(100, (int)middleY); startPos = 225; SuperSecret(); }
            oldState = Keyboard.GetState();
            base.Update(gameTime);
        }
        protected void smartScroll(float offset)
        {
            float newVal = startPos + offset;
            if (hasLoaded && newVal < 225 && newVal > 300 - Lines.Count * StringSize) startPos = newVal;
        }
        protected bool KeyPressed(Keys key)
        {
            return Keyboard.GetState().IsKeyUp(key) && oldState.IsKeyDown(key);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(triangle, leftTri, triRect, Color.White, 0f, halfHalf, .3f, SpriteEffects.None, 0f);
            spriteBatch.Draw(triangle, rightTri, triRect, Color.White, 0f, halfHalf, .3f, SpriteEffects.FlipHorizontally, 0f);
            for (int i = 0, temp = Lines.Count; i < temp; i++)
            {
                float yVal = startPos + (i * (StringSize));
                if (yVal > -70 && yVal < Window.ClientBounds.Height - 30)
                {
                    if (Math.Abs(yVal - 200) < StringSize / 2) Lines[i].draw(spriteBatch, yVal, true);
                    else Lines[i].draw(spriteBatch, yVal, false);
                }
            }
            DrawShortcuts(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
        protected void DrawShortcuts(SpriteBatch batch)
        {
            if (showCuts)
            {
                batch.DrawString(font, "F1 : Hide Shortcuts", new Vector2(10, 10), Color.Gray, 0f, zero, .17f, SpriteEffects.None, 0f);
                batch.DrawString(font, "F2 : Scroll to Top", new Vector2(10, 30), Color.Gray, 0f, zero, .17f, SpriteEffects.None, 0f);
                batch.DrawString(font, "F3 : Reload", new Vector2(10, 50), Color.Gray, 0f, zero, .17f, SpriteEffects.None, 0f);
                batch.DrawString(font, "+/- : Font Size", new Vector2(10, 70), Color.Gray, 0f, zero, .17f, SpriteEffects.None, 0f);
                batch.DrawString(font, "Right Click : Stop Scrolling", new Vector2(10, 90), Color.Gray, 0f, zero, .17f, SpriteEffects.None, 0f);
                batch.DrawString(font, "Left Click : Fast Scrolling", new Vector2(10, 110), Color.Gray, 0f, zero, .17f, SpriteEffects.None, 0f);
            }
            else batch.DrawString(font, "F1 : Show Shortcuts", new Vector2(10, 10), Color.Gray, 0f, zero, .17f, SpriteEffects.None, 0f);
        }
        protected void SuperSecret()
        {
            using (StreamReader reader = new StreamReader(File.OpenRead("SelfDestruct.txt")))
                Words = reader.ReadToEnd().Split('\n');
            load();
        }
        protected void load()
        {
            scale = fontSize / (70f);
            StringSize = font.MeasureString("H").Y * scale;
            Lines.Clear();
            int width = Window.ClientBounds.Width - 210;
            for (int i = 0; i < Words.Length; i++)
            {
                List<AdvancedDrawString> seperatedLines = new List<AdvancedDrawString>();
                String[] lineWords = Words[i].Split(' ');
                int ln = lineWords.Length;
                int wordNum = 0;
                bool carryImportant = false;
                while (wordNum < ln)
                {
                    String tempString = "";
                    while (wordNum < ln) if (getLineLength(tempString + lineWords[wordNum]) >= width) break;
                        else { tempString += (lineWords[wordNum] + " "); wordNum++; }
                    AdvancedDrawString advanced = new AdvancedDrawString(tempString, font, scale, middleX, carryImportant);
                    seperatedLines.Add(advanced);
                    carryImportant = advanced.important;
                }
                Lines.AddRange(seperatedLines);
            }
            hasLoaded = true;
        }
        protected float getLineLength(string line)
        {
            return font.MeasureString(line).X * scale;
        }
    }
}