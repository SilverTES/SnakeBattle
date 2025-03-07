using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mugen.Core;
using Mugen.GFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SnakeBattle
{
    class Arena : Node
    {
        public Point MapSize => _mapSize;
        static public Vector2 Cell = new Vector2(32, 32);
        Point _mapSize = new Point(20,20);

        Hero _hero;

        

        public Arena(int mapW, int mapH, int cellW = 32, int cellH = 32)
        {
            _hero = (Hero)new Hero(this).AppendTo(this);
            _hero.SetMapPosition(2, 2);

            SetMapSize(mapW, mapH, cellW, cellH);



        }
        public void SetMapSize(int mapW, int mapH, int cellW = 32, int cellH = 32)
        {
            _mapSize.X = mapW;
            _mapSize.Y = mapH;

            Cell.X = cellW;
            Cell.Y = cellH;

            _rect.Width = _mapSize.X * Cell.X;
            _rect.Height = _mapSize.Y * Cell.Y;
        }

        public override Node Update(GameTime gameTime)
        {
            

            UpdateChilds(gameTime);

            return base.Update(gameTime);
        }

        public override Node Draw(SpriteBatch batch, GameTime gameTime, int indexLayer)
        {
            if (indexLayer == (int)Game1.Layers.Main)
            {
                //batch.GraphicsDevice.Clear(Color.DarkSlateBlue * .5f);
                batch.FillRectangle(AbsRectF, Color.Black * .25f);
                //batch.Grid(Vector2.Zero, Game1.ScreenW, Game1.ScreenH, Cell.X, Cell.Y, Color.Black * 1f, 3f);
                batch.Grid(AbsXY, AbsRectF.Width, AbsRectF.Height, Cell.X, Cell.Y, Color.Black * .25f, 1f);

                batch.Rectangle(AbsRectF, Color.RoyalBlue, 3f);
            }

            DrawChilds(batch, gameTime, indexLayer);

            return base.Draw(batch, gameTime, indexLayer);
        }
    }
}
