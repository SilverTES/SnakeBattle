using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mugen.Animation;
using Mugen.Core;
using Mugen.GFX;
using Mugen.Physics;
using System;

namespace SnakeBattle
{
    internal class Body : Node
    {
        public int _numOrder = 0;

        public Color _color = Color.Black;

        public Point _mapPrevPosition;
        public Point _mapPosition;
        
        public Point _direction;
        //public Point _nextDirection;

        public bool _isMoveTo = false;

        public Vector2 _from;
        public Vector2 _goal;
        public float _ticMove = 0;
        public float _tempoMove = 10;


        public bool _isACorner = false;

        Arena _arena;

        public Body(Arena arena, Color color)
        {
            _type = UID.Get<Body>();
            _arena = arena;
            _color = color;

            SetSize(_arena.CellSize);
            SetPivot(Position.TOP_LEFT);
        }
        public void SetMapPosition(Point mapPosition)
        {
            _mapPosition = mapPosition;

            _mapPrevPosition.X = _mapPosition.X;
            _mapPrevPosition.Y = _mapPosition.Y;

            _x = mapPosition.X * _arena.CellSize.X;
            _y = mapPosition.Y * _arena.CellSize.Y;

            UpdateRect();
        }
        public void SetDirection(Point direction)
        {
            _direction = direction;

            MoveTo(_mapPosition + direction);
        }
        //public void SetNextDirection(int dx, int dy)
        //{
        //    _nextDirection.X = dx;
        //    _nextDirection.Y = dy;

        //    //MoveTo(_mapPosition.X + dx, _mapPosition.Y + dy);
        //}
        public void MoveTo(Point mapPosition)
        {
            _mapPrevPosition.X = _mapPosition.X;
            _mapPrevPosition.Y = _mapPosition.Y;

            _ticMove = 0;
            _isMoveTo = true;


            _from.X = _x;
            _from.Y = _y;

            _goal.X = mapPosition.X * _arena.CellSize.X;
            _goal.Y = mapPosition.Y * _arena.CellSize.Y;
        }

        public override Node Update(GameTime gameTime)
        {
            UpdateRect();

            _isACorner = false;

            _mapPosition.X = (int)Math.Round(_x / _arena.CellSize.X);
            _mapPosition.Y = (int)Math.Round(_y / _arena.CellSize.Y);

            if (_isMoveTo)
            {
                _x = Easing.GetValue(Easing.Linear, _ticMove, _from.X, _goal.X, _tempoMove);
                _y = Easing.GetValue(Easing.Linear, _ticMove, _from.Y, _goal.Y, _tempoMove);

                _ticMove++;
                if (_ticMove > _tempoMove)
                {
                    _isMoveTo = false;

                    _arena.SetGrid(_mapPrevPosition, Const.NoIndex, null);
                    _arena.SetGrid(_mapPosition, UID.Get<Hero>(), this);
                }
            }

            return base.Update(gameTime);
        }

        public void Draw(SpriteBatch batch)
        {
            var rect = new RectangleF(AbsX, AbsY, _arena.CellSize.X, _arena.CellSize.Y);
            //batch.FillRectangle(rect.Extend(-4), _isACorner ? Color.Red : Color.Orange);
            //batch.FillRectangle(rect.Extend(-6), Color.Orange);

            batch.Rectangle(rect.Extend(-6), _color, 3f);
            batch.Rectangle(rect.Extend(-2), _color * .75f, 2f);

            //batch.CenterStringXY(Game1._fontMain, $"{_index}", _position + _arena.CellSize / 2 + _arena.XY, Color.White);

            //rect = new RectangleF(_mapPrevPosition.X * ScreenPlay.CellW, _mapPrevPosition.Y * ScreenPlay.CellH, ScreenPlay.CellW, ScreenPlay.CellH);
            //batch.FillRectangle(rect.Extend(-8), Color.IndianRed * .5f);
        }

        public void DrawHead(SpriteBatch batch)
        {
            var rect = new RectangleF(AbsX, AbsY, _arena.CellSize.X, _arena.CellSize.Y);
            //batch.FillRectangle(rect.Extend(-4), _isACorner ? Color.Red : Color.Orange);
            //batch.FillRectangle(rect.Extend(-6), Color.Orange);

            batch.Rectangle(rect.Extend(-6), Color.Yellow, 2f);
            batch.Rectangle(rect.Extend(-2), Color.LightGoldenrodYellow, 2f);

            //batch.CenterStringXY(Game1._fontMain, $"{_index}", _position + _arena.CellSize / 2 + _arena.XY, Color.White);

            //rect = new RectangleF(_mapPrevPosition.X * ScreenPlay.CellW, _mapPrevPosition.Y * ScreenPlay.CellH, ScreenPlay.CellW, ScreenPlay.CellH);
            //batch.FillRectangle(rect.Extend(-8), Color.IndianRed * .5f);
        }
    }
}
