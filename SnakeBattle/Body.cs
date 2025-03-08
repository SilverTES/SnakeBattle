using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mugen.Animation;
using Mugen.Core;
using Mugen.GFX;
using Mugen.Physics;

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
        public bool _onGoal = false;

        public Vector2 _from;
        public Vector2 _goal;
        public float _ticMove = 0;
        public float _tempoMove = 12;


        public bool _isACorner = false;
        public bool _isAttack = false;

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

            _arena.SetGrid(_mapPrevPosition, Const.NoIndex, null);
        }

        public override Node Update(GameTime gameTime)
        {
            UpdateRect();

            _isACorner = false;
            _isAttack = false;

            var cell = _arena.GetGrid(_mapPosition + new Point(1, 0));

            if (cell != null)
                if (cell._type == Const.NoIndex)
                {
                    _isAttack = true;
                }


            _onGoal = false;

            if (_isMove)
            {
                _x = Easing.GetValue(Easing.Linear, _ticMove, _from.X, _goal.X, _tempoMove);
                _y = Easing.GetValue(Easing.Linear, _ticMove, _from.Y, _goal.Y, _tempoMove);

                _ticMove ++;
                if (_ticMove > _tempoMove)
                {
                    _isMove = false;
                    _onGoal = true;

                    _mapPosition = _mapNextPosition;
                    
                    _arena.SetGrid(_mapPosition, UID.Get<Body>(), this);
                }
            }

            return base.Update(gameTime);
        }

        public void Draw(SpriteBatch batch)
        {
            //if (_isAttack)
            //    _arena.DrawLine(batch, _mapPosition,  new Point(_arena.MapSize.X, _mapPosition.Y), _color * .25f);


            var rect = new RectangleF(AbsX, AbsY, _arena.CellSize.X, _arena.CellSize.Y);
            //batch.FillRectangle(rect.Extend(-4), _isACorner ? Color.Red : Color.Orange);
            //batch.FillRectangle(rect.Extend(-4), _isAttack ? _color : Color.Transparent);
            //batch.FillRectangle(rect.Extend(-6), Color.Orange);

            batch.Rectangle(rect.Extend(-12), _color, 3f);
            batch.Rectangle(rect.Extend(-8), _color * .75f, 2f);

            //batch.CenterStringXY(Game1._fontMain, $"{_index}", _position + _arena.CellSize / 2 + _arena.XY, Color.White);

            //rect = new RectangleF(_mapPrevPosition.X * ScreenPlay.CellW, _mapPrevPosition.Y * ScreenPlay.CellH, ScreenPlay.CellW, ScreenPlay.CellH);
            //batch.FillRectangle(rect.Extend(-8), Color.IndianRed * .5f);
        }

        public void DrawHead(SpriteBatch batch, Color color)
        {
            var rect = new RectangleF(AbsX, AbsY, _arena.CellSize.X, _arena.CellSize.Y);
            //batch.FillRectangle(rect.Extend(-4), _isACorner ? Color.Red : Color.Orange);
            batch.FillRectangle(rect.Extend(-10), color);

            batch.Rectangle(rect.Extend(-8), color * .75f, 2f);
            batch.Rectangle(rect.Extend(-4), color * .5f, 2f);

            //batch.CenterStringXY(Game1._fontMain, $"{_index}", _position + _arena.CellSize / 2 + _arena.XY, Color.White);

            //rect = new RectangleF(_mapPrevPosition.X * ScreenPlay.CellW, _mapPrevPosition.Y * ScreenPlay.CellH, ScreenPlay.CellW, ScreenPlay.CellH);
            //batch.FillRectangle(rect.Extend(-8), Color.IndianRed * .5f);
        }

    }
}
