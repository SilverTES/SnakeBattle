using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mugen.Animation;
using Mugen.Core;
using Mugen.GFX;
using Mugen.Input;
using Mugen.Physics;
using System;
using System.Collections.Generic;

namespace SnakeBattle
{
    internal class Hero : Node
    {
        GamePadState _pad;
        KeyboardState _key;

        Vector2 _stickLeft;
        Vector2 _stickRight;

        List<Cell> _cells = [];

        Arena _arena;
        public Hero(Arena arena)
        {
            _arena = arena;

            SetSize(Arena.Cell.X, Arena.Cell.Y);
            SetPivot(Position.CENTER);

            for (int i = 0; i < 6; i++)
            {
                var cell = new Cell(_arena);
                _cells.Add(cell);
            }

            _arena = arena;
        }
        public void SetMapPosition(int mapX, int mapY)
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                _cells[i].SetMapPosition(mapX, mapY);
            }
        }

        public void SetDirection(int dx, int dy)
        {
            _cells[0].SetDirection(dx, dy);

            for (int i = 1; i < _cells.Count; i++)
            {
                _cells[i].MoveTo(_cells[i-1]._mapPrevPosition.X, _cells[i-1]._mapPrevPosition.Y);
            }
        }
        public void SetNextDirection(int dx, int dy)
        {
            _cells[0].SetNextDirection(dx, dy);
        }

        public override Node Init()
        {
            return base.Init();
        }


        public override Node Update(GameTime gameTime)
        {
            UpdateRect();

            for (int i = 0; i < _cells.Count; i++)
            {
                _cells[i].Update();
                _cells[i]._index = i;

                // Check if before and after are not aligne = Corner
                if (i>0 && i< _cells.Count - 1)
                {
                    if ((_cells[i - 1]._mapPosition.X != _cells[i + 1]._mapPosition.X) &&
                        (_cells[i - 1]._mapPosition.Y != _cells[i + 1]._mapPosition.Y))
                    {
                        _cells[i]._isACorner = true;
                    }
                }
            }

            _pad = GamePad.GetState(PlayerIndex.One);
            _key = Keyboard.GetState();

            _stickLeft = _pad.ThumbSticks.Left;
            _stickRight = _pad.ThumbSticks.Right;

            if (!_cells[0]._isMoveTo)
            {
                if (_pad.DPad.Up == ButtonState.Pressed || _key.IsKeyDown(Keys.Up) || _stickLeft.Y > 0) SetDirection(0,-1);
                if (_pad.DPad.Down == ButtonState.Pressed || _key.IsKeyDown(Keys.Down) || _stickLeft.Y < 0) SetDirection(0, 1);

                if (_pad.DPad.Left == ButtonState.Pressed || _key.IsKeyDown(Keys.Left) || _stickLeft.X < 0) SetDirection(-1, 0);
                if (_pad.DPad.Right == ButtonState.Pressed || _key.IsKeyDown(Keys.Right) || _stickLeft.X > 0) SetDirection(1, 0);

                //if (_direction.X != 0 || _direction.Y != 0)
                //    SetDirection(_direction.X, _direction.Y);
            }
            else
            {
                if (_pad.DPad.Up == ButtonState.Pressed || _key.IsKeyDown(Keys.Up) || _stickLeft.Y > 0) SetNextDirection(0, -1);
                if (_pad.DPad.Down == ButtonState.Pressed || _key.IsKeyDown(Keys.Down) || _stickLeft.Y < 0) SetNextDirection(0, 1);

                if (_pad.DPad.Left == ButtonState.Pressed || _key.IsKeyDown(Keys.Left) || _stickLeft.X < 0) SetNextDirection(-1, 0);
                if (_pad.DPad.Right == ButtonState.Pressed || _key.IsKeyDown(Keys.Right) || _stickLeft.X > 0) SetNextDirection(1, 0);

                // Auto Next Direction if different than current direction
                if (_cells[0]._nextDirection.X != _cells[0]._direction.X || _cells[0]._nextDirection.Y != _cells[0]._direction.Y)
                {
                    //SetDirection(_cells[0]._nextDirection.X, _cells[0]._nextDirection.Y);
                }
            }

                return base.Update(gameTime);
        }
        public override Node Draw(SpriteBatch batch, GameTime gameTime, int indexLayer)
        {

            if (indexLayer == (int)Game1.Layers.Main)
            {
                //var rect = new RectangleF(AbsX, AbsY, ScreenPlay.CellW, ScreenPlay.CellH);
                //batch.FillRectangle(rect.Extend(-8), Color.Yellow);

                //batch.Rectangle(rect.Extend(-4), Color.Red, 1f);
                //batch.Rectangle(rect.Extend(-2), Color.DarkRed, 1f);
                //for (int i = 0; i < _cells.Count; i++)
                //{
                //    _cells[i].Draw(batch);
                //}

                for (int i = _cells.Count -1; i >= 0; i--)
                {
                    _cells[i].Draw(batch);
                }


            }

            if (indexLayer == (int)Game1.Layers.Debug)
            {

                //batch.Line(_from + OXY, AbsXY + OXY, Color.GreenYellow, 4f);
                //batch.CenterStringXY(Game1._fontMain, $"{_cells[0]._nextDirection}", _cells[0]._position + OXY - Vector2.UnitY * 24, Color.White);
                //batch.CenterStringXY(Game1._fontMain, $"{_mapPosition}", AbsXY + OXY - Vector2.UnitY * 24, Color.White);

            }

            return base.Draw(batch, gameTime, indexLayer);
        }
    }
}
