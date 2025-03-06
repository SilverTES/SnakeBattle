using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mugen.Core;
using Mugen.GFX;

namespace SnakeBattle
{
    internal class ScreenPlay : Node    
    {
        Game1 _game;

        Vector2 _mouse;

        static public Vector2 Cell = new Vector2(32,32);

        Hero _hero;

        public ScreenPlay(Game1 game)
        {
            _game = game;

            _hero = (Hero)new Hero().AppendTo(this);
            _hero.SetMapPosition(10, 10);
        }
        public override Node Init()
        {
            return base.Init();
        }
        public override Node Update(GameTime gameTime)
        {
            _mouse = WindowManager.GetMousePosition();

            

            UpdateChilds(gameTime);

            return base.Update(gameTime);   
        }
        public override Node Draw(SpriteBatch batch, GameTime gameTime, int indexLayer)
        {
            batch.GraphicsDevice.Clear(Color.Transparent);

            if (indexLayer == (int)Game1.Layers.Main)
            {
                batch.GraphicsDevice.Clear(Color.DarkSlateBlue * .5f);
                batch.Grid(Vector2.Zero, Game1.ScreenW, Game1.ScreenH, Cell.X, Cell.Y, Color.Black * .6f);

            }

            if (indexLayer == (int)Game1.Layers.Debug)
            {
                //batch.Sight(_mouse, Game1.ScreenW, Game1.ScreenH, Color.Gray * .8f, 1);

            }

            DrawChilds(batch, gameTime, indexLayer);

            return base.Draw(batch, gameTime, indexLayer);
        }
    }
}
