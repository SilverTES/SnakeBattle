using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mugen.Core;
using Mugen.GFX;
using Mugen.Physics;
using System.Collections.Generic;
using System.Linq;

namespace SnakeBattle
{
    internal class Hero : Node
    {
        GamePadState _pad;
        KeyboardState _key;

        Vector2 _stickLeft;
        Vector2 _stickRight;

        List<Body> _bodys = [];

        public Point _directionHead;

        Arena _arena;
        public Hero(Arena arena, Point mapPosition, int size = 4)
        {
            _type = UID.Get<Hero>();

            _arena = arena;

            SetSize(_arena.CellSize.X, _arena.CellSize.Y);
            SetPivot(Position.CENTER);

            for (int i = 0; i < size; i++)
            {
                var body = new Body(_arena, Color.Gray);

                AddBody(body, mapPosition);
            }

            _arena = arena;
        }
        public void SetMapPosition(Point mapPosition)
        {
            for (int i = 0; i < _bodys.Count; i++)
            {
                _bodys[i].SetMapPosition(mapPosition);
            }
        }
        public void SetDirection(Point direction)
        {
            Point nextPosition = _bodys[0]._mapPosition + direction;
            // check if position + direction is in Arena
            if (!_arena.IsInArena(nextPosition))
                return;


            var cell = _arena.GetGrid(nextPosition);

            if (cell == null)
                return;

            // if touch item
            if (cell._type == UID.Get<Item>())
            {
                if (cell._owner != null)
                {
                    var item = (Item)cell._owner;
                    
                    AddBody(new Body(_arena, item._color), LastBody()._mapPosition);
                    
                    new FxExplose(_arena.AbsXY + nextPosition.ToVector2() * _arena.CellSize + _arena.CellSize/2, item._color, 16, 40).AppendTo(_parent);

                    cell._owner.KillMe();

                    _arena.SetGrid(nextPosition, Const.NoIndex, null);
                }
                return;
            }

            if (cell._type != Const.NoIndex)
                return;

            _bodys[0].MoveTo(nextPosition);

            _directionHead = direction;

            for (int i = 1; i < _bodys.Count; i++)
            {
                _bodys[i].MoveTo(_bodys[i-1]._mapPrevPosition);
            }

        }
        public void AddBody(Body body, Point mapPosition)
        {
            _bodys.Add(body);
            body.AppendTo(_arena);
            body.SetMapPosition(mapPosition);
        }
        public void DeleteBody()
        {
            for (int i = 1; i < _bodys.Count; i++)
            {
                if (_bodys[i] != null)
                    _bodys[i].KillMe();
            }

            if (_bodys.Count > 1)
                _bodys.RemoveRange(1, _bodys.Count - 1);
        }
        public Body LastBody()
        {
            return _bodys.Last();
        }
        public override Node Init()
        {
            return base.Init();
        }
        public override Node Update(GameTime gameTime)
        {
            UpdateRect();


            for (int i = 0; i < _bodys.Count; i++)
            {
                _bodys[i].Update(gameTime);
                _bodys[i]._numOrder = i;

                // Check if before and after are not aligne = Corner
                if (i > 0 && i < _bodys.Count - 1)
                {
                    if ((_bodys[i - 1]._mapPosition.X != _bodys[i + 1]._mapPosition.X) &&
                        (_bodys[i - 1]._mapPosition.Y != _bodys[i + 1]._mapPosition.Y))
                    {
                        _bodys[i]._isACorner = true;
                    }
                }
            }

            _pad = GamePad.GetState(PlayerIndex.One);
            _key = Keyboard.GetState();

            _stickLeft = _pad.ThumbSticks.Left;
            _stickRight = _pad.ThumbSticks.Right;

            if (!_bodys[0]._isMove)
            {
                Point direction = new Point();

                bool btnUp = _pad.DPad.Up == ButtonState.Pressed || _key.IsKeyDown(Keys.Z) || _key.IsKeyDown(Keys.Up) || _stickLeft.Y > 0;
                bool btnDown = _pad.DPad.Down == ButtonState.Pressed || _key.IsKeyDown(Keys.S) || _key.IsKeyDown(Keys.Down) || _stickLeft.Y < 0;
                bool btnLeft = _pad.DPad.Left == ButtonState.Pressed || _key.IsKeyDown(Keys.Q) || _key.IsKeyDown(Keys.Left) || _stickLeft.X < 0;
                bool btnRight = _pad.DPad.Right == ButtonState.Pressed || _key.IsKeyDown(Keys.D) || _key.IsKeyDown(Keys.Right) || _stickLeft.X > 0;

                if (btnUp && !_arena.Is<Body>(_bodys[0]._mapPosition + new Point(0, -1))) direction = new Point(0, -1);
                if (btnDown && !_arena.Is<Body>(_bodys[0]._mapPosition + new Point(0, 1))) direction = new Point(0, 1);

                if (btnLeft && !_arena.Is<Body>(_bodys[0]._mapPosition + new Point(-1, 0)))  direction = new Point(-1, 0);
                if (btnRight && !_arena.Is<Body>(_bodys[0]._mapPosition + new Point(1, 0))) direction = new Point(1, 0);

                // Result of 4 directions
                //if (direction.X != 0 ^ direction.Y != 0)
                {
                    SetDirection(direction);
                }

                // Continue until touch wall
                //if (_directionHead.X != 0 || _directionHead.Y != 0) SetDirection(_directionHead);
            }

            return base.Update(gameTime);
        }
        public override Node Draw(SpriteBatch batch, GameTime gameTime, int indexLayer)
        {

            if (indexLayer == (int)Game1.Layers.Main)
            {
                for (int i = _bodys.Count -1; i >= 0; i--)
                {
                    _bodys[i].Draw(batch);

                    if (i > 0)
                    {
                        //batch.Line(_bodys[i].AbsXY + _arena.CellSize/2, _bodys[i-1].AbsXY + _arena.CellSize/2, Color.Yellow * .5f, 5f);
                    }
                }

                _bodys[0].DrawHead(batch);
            }

            if (indexLayer == (int)Game1.Layers.Debug)
            {

                //batch.Line(_from + OXY, AbsXY + OXY, Color.GreenYellow, 4f);
                //batch.CenterStringXY(Game1._fontMain, $"{_cells[0]._nextDirection}", _cells[0]._position + OXY - Vector2.UnitY * 24, Color.White);
                //batch.CenterStringXY(Game1._fontMain, $"{_mapPosition}", AbsXY + OXY - Vector2.UnitY * 24, Color.White);

                batch.LeftTopString(Game1._fontMain, $"{_arena.GetGrid(_bodys[0]._mapPosition)._type}", Vector2.One * 20, Color.White);

            }

            return base.Draw(batch, gameTime, indexLayer);
        }
    }
}
