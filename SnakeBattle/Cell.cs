using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mugen.Animation;
using Mugen.GFX;
using Mugen.Physics;
using System;

namespace SnakeBattle
{
    internal class Cell
    {
        public int _index = 0;

        public Vector2 _position;

        public Point _mapPrevPosition;
        public Point _mapPosition;
        
        public Point _direction;
        //public Point _nextDirection;

        public bool _isMoveTo = false;

        public Vector2 _from;
        public Vector2 _goal;
        public float _ticMove = 0;
        public float _tempoMove = 4;


        public bool _isACorner = false;

        Arena _arena;

        public Cell(Arena arena)
        {
            _arena = arena;
        }
        public void SetMapPosition(int mapX, int mapY)
        {
            _mapPosition.X = mapX;
            _mapPosition.Y = mapY;

            _mapPrevPosition.X = _mapPosition.X;
            _mapPrevPosition.Y = _mapPosition.Y;

            _position.X = mapX * Arena.Cell.X;
            _position.Y = mapY * Arena.Cell.Y;
        }
        public void SetDirection(int dx, int dy)
        {
            _direction.X = dx;
            _direction.Y = dy;

            MoveTo(_mapPosition.X + dx, _mapPosition.Y + dy);
        }
        //public void SetNextDirection(int dx, int dy)
        //{
        //    _nextDirection.X = dx;
        //    _nextDirection.Y = dy;

        //    //MoveTo(_mapPosition.X + dx, _mapPosition.Y + dy);
        //}
        public void MoveTo(int mapX, int mapY)
        {
            _mapPrevPosition.X = _mapPosition.X;
            _mapPrevPosition.Y = _mapPosition.Y;

            _ticMove = 0;
            _isMoveTo = true;


            _from.X = _position.X;
            _from.Y = _position.Y;

            _goal.X = mapX * Arena.Cell.X;
            _goal.Y = mapY * Arena.Cell.Y;
        }

        public void Update()
        {
            _isACorner = false;

            _mapPosition.X = (int)Math.Round(_position.X / Arena.Cell.X);
            _mapPosition.Y = (int)Math.Round(_position.Y / Arena.Cell.Y);

            if (_isMoveTo)
            {
                _position.X = Easing.GetValue(Easing.Linear, _ticMove, _from.X, _goal.X, _tempoMove);
                _position.Y = Easing.GetValue(Easing.Linear, _ticMove, _from.Y, _goal.Y, _tempoMove);

                _ticMove++;
                if (_ticMove > _tempoMove)
                {
                    _isMoveTo = false;
                }
            }

        }

        public void Draw(SpriteBatch batch)
        {
            var rect = new RectangleF(_position.X + _arena._x, _position.Y + _arena._y, Arena.Cell.X, Arena.Cell.Y);
            batch.FillRectangle(rect.Extend(-4), _isACorner ? Color.Red : Color.Orange);
            //batch.FillRectangle(rect.Extend(-6), Color.Orange);

            batch.Rectangle(rect.Extend(-6), Color.Red, 2f);
            batch.Rectangle(rect.Extend(-2), Color.DarkRed, 2f);

            batch.CenterStringXY(Game1._fontMain, $"{_index}", _position + Arena.Cell / 2 + _arena.XY, Color.White);

            //rect = new RectangleF(_mapPrevPosition.X * ScreenPlay.CellW, _mapPrevPosition.Y * ScreenPlay.CellH, ScreenPlay.CellW, ScreenPlay.CellH);
            //batch.FillRectangle(rect.Extend(-8), Color.IndianRed * .5f);
        }
    }
}
