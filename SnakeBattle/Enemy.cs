using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mugen.Core;
using Mugen.GFX;
using Mugen.Physics;

namespace SnakeBattle
{
    class Enemy : Node
    {
        Arena _arena;

        Point _mapPosition = new Point();

        public Color _color;
        public Enemy(Arena arena, Point mapPosition, Color color)
        {
            _type = UID.Get<Enemy>();

            _arena = arena;
            _mapPosition.X = mapPosition.X;
            _mapPosition.Y = mapPosition.Y;

            _color = color;

            SetMapPosition(_mapPosition);
        }
        public void SetMapPosition(Point mapPosition)
        {
            _mapPosition = mapPosition;

            _x = mapPosition.X * _arena.CellSize.X;
            _y = mapPosition.Y * _arena.CellSize.Y;

            _arena.SetGrid(mapPosition, _type, this);

        }
        public override Node Update(GameTime gameTime)
        {
            UpdateRect();

            return base.Update(gameTime);
        }
        public override Node Draw(SpriteBatch batch, GameTime gameTime, int indexLayer)
        {
            if (indexLayer == (int)Game1.Layers.Main)
            {
                var rect = new RectangleF(AbsX, AbsY, _arena.CellSize.X, _arena.CellSize.Y);

                batch.Rectangle(rect.Extend(-6), _color, 2f);
                batch.Rectangle(rect.Extend(-2), _color * .75f, 2f);

                batch.Circle(AbsXY + _arena.CellSize / 2, _arena.CellSize.X / 2 - 8, 6, _color, 2f);
                batch.Circle(AbsXY + _arena.CellSize / 2, _arena.CellSize.X / 2 - 4, 6, _color, 2f);

                //batch.CenterStringXY(Game1._fontMain, $"{_index}", AbsXY + _arena.CellSize / 2, Color.White);
            }
            return base.Draw(batch, gameTime, indexLayer);
        }
    }
}
