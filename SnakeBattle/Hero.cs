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
    internal class TileCase
    {
        public Vector2 _position;

        public Point _mapPrevPosition;
        public Point _mapPosition;
        public Point _direction;
        public bool _isMoveTo = false;
        public bool _isGoal = false;
        public Vector2 _from;
        public Vector2 _goal;
        public float _ticMove = 0;
        public float _tempoMove = 5;
        public void SetMapPosition(int mapX, int mapY)
        {
            _mapPosition.X = mapX;
            _mapPosition.Y = mapY;
            _position.X = mapX * ScreenPlay.CellW;
            _position.Y = mapY * ScreenPlay.CellH;
        }
        public void SetDirection(int dx, int dy)
        {
            _direction.X = dx;
            _direction.Y = dy;

            MoveTo(_mapPosition.X + dx, _mapPosition.Y + dy);
        }
        public void MoveTo(int mapX, int mapY)
        {
            _mapPrevPosition.X = _mapPosition.X;
            _mapPrevPosition.Y = _mapPosition.Y;

            _ticMove = 0;
            _isMoveTo = true;
            _isGoal = false;


            _from.X = _position.X;
            _from.Y = _position.Y;
            
            _goal.X = mapX * ScreenPlay.CellW;
            _goal.Y = mapY * ScreenPlay.CellH;
        }

        public void Update()
        {
            _mapPosition.X = (int)Math.Round(_position.X / ScreenPlay.CellW);
            _mapPosition.Y = (int)Math.Round(_position.Y / ScreenPlay.CellH);

            if (_isMoveTo)
            {
                _position.X = Easing.GetValue(Easing.BounceEaseOut, _ticMove, _from.X, _goal.X, _tempoMove);
                _position.Y = Easing.GetValue(Easing.BounceEaseOut, _ticMove, _from.Y, _goal.Y, _tempoMove);

                if (_position.X == _goal.X && _position.Y == _goal.Y)
                {
                    _isGoal = true;
                }

                _ticMove++;
                if (_ticMove > _tempoMove)
                {
                    _isMoveTo = false;
                }
            }
        }

        public void Draw(SpriteBatch batch)
        {
            var rect = new RectangleF(_position.X, _position.Y, ScreenPlay.CellW, ScreenPlay.CellH);
            batch.FillRectangle(rect.Extend(-8), Color.Yellow);

            batch.Rectangle(rect.Extend(-4), Color.Red, 2f);
            batch.Rectangle(rect.Extend(-2), Color.DarkRed, 2f);

            //rect = new RectangleF(_mapPrevPosition.X * ScreenPlay.CellW, _mapPrevPosition.Y * ScreenPlay.CellH, ScreenPlay.CellW, ScreenPlay.CellH);
            //batch.FillRectangle(rect.Extend(-8), Color.IndianRed * .5f);
        }
    }

    internal class Hero : Node
    {
        GamePadState _pad;
        KeyboardState _key;

        Vector2 _stickLeft;
        Vector2 _stickRight;

        TileCase _tileCase;

        List<TileCase> _tileCases = [];

        public Hero()
        {
            _tileCase = new TileCase();
            _tileCase._mapPosition = new Point();

            SetSize(ScreenPlay.CellW, ScreenPlay.CellH);
            SetPivot(Position.CENTER);

            for (int i = 0; i < 10; i++)
            {
                var tileCase = new TileCase();
                _tileCases.Add(tileCase);
            }

        }
        public void SetMapPosition(int mapX, int mapY)
        {
            _tileCase.SetMapPosition(mapX, mapY);
        }

        public void SetDirection(int dx, int dy)
        {
            _tileCase.SetDirection(dx, dy);

            _tileCases[0].MoveTo(_tileCase._mapPrevPosition.X, _tileCase._mapPrevPosition.Y);
            
            for (int i = 1; i < _tileCases.Count; i++)
            {
                _tileCases[i].MoveTo(_tileCases[i-1]._mapPrevPosition.X, _tileCases[i-1]._mapPrevPosition.Y);
            }
        }

        public override Node Init()
        {
            return base.Init();
        }


        public override Node Update(GameTime gameTime)
        {
            UpdateRect();

            _tileCase.Update();

            for (int i = 0; i < _tileCases.Count; i++)
            {
                _tileCases[i].Update();
            }

            _pad = GamePad.GetState(PlayerIndex.One);
            _key = Keyboard.GetState();

            _stickLeft = _pad.ThumbSticks.Left * -Vector2.UnitY;
            _stickRight = _pad.ThumbSticks.Right * -Vector2.UnitY;

            if (!_tileCase._isMoveTo)
            {
                if (_pad.DPad.Up == ButtonState.Pressed || _key.IsKeyDown(Keys.Up) || _stickLeft.Y < 0) SetDirection(0,-1);
                if (_pad.DPad.Down == ButtonState.Pressed || _key.IsKeyDown(Keys.Down) || _stickLeft.Y > 0) SetDirection(0, 1);

                if (_pad.DPad.Left == ButtonState.Pressed || _key.IsKeyDown(Keys.Left) || _stickLeft.X < 0) SetDirection(-1, 0);
                if (_pad.DPad.Right == ButtonState.Pressed || _key.IsKeyDown(Keys.Right) || _stickLeft.X > 0) SetDirection(1, 0);

                //if (_direction.X != 0 || _direction.Y != 0)
                //    SetDirection(_direction.X, _direction.Y);
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
                for (int i = 0; i < _tileCases.Count; i++)
                {
                    _tileCases[i].Draw(batch);
                }

                _tileCase.Draw(batch);


            }

            if (indexLayer == (int)Game1.Layers.Debug)
            {

                //batch.Line(_from + OXY, AbsXY + OXY, Color.GreenYellow, 4f);
                //batch.CenterStringXY(Game1._fontMain, $"{_direction}", AbsXY + OXY - Vector2.UnitY * 24, Color.White);
                //batch.CenterStringXY(Game1._fontMain, $"{_mapPosition}", AbsXY + OXY - Vector2.UnitY * 24, Color.White);

            }

            return base.Draw(batch, gameTime, indexLayer);
        }
    }
}
