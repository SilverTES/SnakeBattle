using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Mugen.Core;
using Mugen.Input;
using System.Data;

namespace SnakeBattle;

public class Game1 : Game
{
    public enum Layers
    {
        Main,
        Debug,
        FX,
        HUD,
    }

    static public readonly int ScreenW = 1920;
    static public readonly int ScreenH = 1080;

    static public SpriteFont _fontMain;
    static public Texture2D _texBG;

    ScreenPlay _screenPlay;

    public Game1()
    {
        WindowManager.Init(this, ScreenW, ScreenH);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
        //IsFixedTimeStep = false;
    }

    protected override void Initialize()
    {

        _screenPlay = new ScreenPlay(this);
        ScreenManager.Init(_screenPlay, Enums.Count<Layers>(), [(int)Layers.Main, (int)Layers.FX, (int)Layers.HUD, (int)Layers.Debug]);
        //ScreenManager.SetLayersOrder([(int)Layers.Main, (int)Layers.FX, (int)Layers.HUD, (int)Layers.Debug]);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _fontMain = Content.Load<SpriteFont>("Fonts/fontMain");
        _texBG = Content.Load<Texture2D>("Images/background00");
    }

    protected override void Update(GameTime gameTime)
    {
        WindowManager.Update(gameTime);

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (ButtonControl.OnePress("ToggleFullScreen", Keyboard.GetState().IsKeyDown(Keys.F11)))
            WindowManager.ToggleFullscreen();

        ScreenManager.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        ScreenManager.DrawScreen(gameTime, SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap);

        ScreenManager.ShowScreen(gameTime, SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap);

        base.Draw(gameTime);
    }
}
