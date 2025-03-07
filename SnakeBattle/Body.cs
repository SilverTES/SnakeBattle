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
        public Point _mapNextPosition;

        public bool _isMove = false;

        public Vector2 _from;
        public Vector2 _goal;
        public float _ticMove = 0;
        public float _tempoMove = 12;


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
        public void MoveTo(Point mapPosition)
        {
            _mapPrevPosition.X = _mapPosition.X;
            _mapPrevPosition.Y = _mapPosition.Y;

            _ticMove = 0;
            _isMove = true;


            _from.X = _x;
            _from.Y = _y;

            _goal.X = mapPosition.X * _arena.CellSize.X;
            _goal.Y = mapPosition.Y * _arena.CellSize.Y;

            _mapNextPosition.X = mapPosition.X;
            _mapNextPosition.Y = mapPosition.Y;

            _arena.SetGrid(_mapPosition, Const.NoIndex, null);
        }

        public override Node Update(GameTime gameTime)
        {
            UpdateRect();

            _isACorner = false;

            if (_isMove)
            {
                _x = Easing.GetValue(Easing.Linear, _ticMove, _from.X, _goal.X, _tempoMove);
                _y = Easing.GetValue(Easing.Linear, _ticMove, _from.Y, _goal.Y, _tempoMove);

                //_x = MathHelper.LerpPrecise(_from.X, _goal.X, _ticMove);
                //_y = MathHelper.LerpPrecise(_from.Y, _goal.Y, _ticMove);

                _ticMove += 1f;
                if (_ticMove > _tempoMove)
                {
                    _isMove = false;

                    _mapPosition = _mapNextPosition;
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

            batch.Rectangle(rect.Extend(-8), _color, 3f);
            batch.Rectangle(rect.Extend(-4), _color * .75f, 2f);

            //batch.CenterStringXY(Game1._fontMain, $"{_index}", _position + _arena.CellSize / 2 + _arena.XY, Color.White);

            //rect = new RectangleF(_mapPrevPosition.X * ScreenPlay.CellW, _mapPrevPosition.Y * ScreenPlay.CellH, ScreenPlay.CellW, ScreenPlay.CellH);
            //batch.FillRectangle(rect.Extend(-8), Color.IndianRed * .5f);
        }

        public void DrawHead(SpriteBatch batch)
        {
            var rect = new RectangleF(AbsX, AbsY, _arena.CellSize.X, _arena.CellSize.Y);
            //batch.FillRectangle(rect.Extend(-4), _isACorner ? Color.Red : Color.Orange);
            batch.FillRectangle(rect.Extend(-10), Color.Orange);

            batch.Rectangle(rect.Extend(-8), Color.Yellow, 2f);
            batch.Rectangle(rect.Extend(-4), Color.LightGoldenrodYellow, 2f);

            //batch.CenterStringXY(Game1._fontMain, $"{_index}", _position + _arena.CellSize / 2 + _arena.XY, Color.White);

            //rect = new RectangleF(_mapPrevPosition.X * ScreenPlay.CellW, _mapPrevPosition.Y * ScreenPlay.CellH, ScreenPlay.CellW, ScreenPlay.CellH);
            //batch.FillRectangle(rect.Extend(-8), Color.IndianRed * .5f);
        }
    }
}
