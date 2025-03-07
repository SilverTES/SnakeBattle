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

        Arena _arena;

        Addon.Loop _loop;

        public ScreenPlay(Game1 game)
        {
            _game = game;

            _arena = (Arena)new Arena(32,32,32,32).AppendTo(this);
            
            _arena.SetPosition(440, 20);

            SetPosition(0, 0);
            SetSize(Game1.ScreenW, Game1.ScreenH);

            _loop = new Addon.Loop(this);
            _loop.SetLoop(0f, 0f, 2f, .05f, Mugen.Animation.Loops.PINGPONG);
            _loop.Start();

        }
        public override Node Init()
        {
            return base.Init();
        }
        public override Node Update(GameTime gameTime)
        {
            _loop.Update(gameTime);

            _mouse = WindowManager.GetMousePosition();

            UpdateChilds(gameTime);

            return base.Update(gameTime);   
        }
        public override Node Draw(SpriteBatch batch, GameTime gameTime, int indexLayer)
        {
            batch.GraphicsDevice.Clear(Color.Transparent);

            if (indexLayer == (int)Game1.Layers.Main)
            {
                batch.Draw(Game1._texBG, AbsXY + Vector2.UnitY * _loop._current, Color.White);
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
