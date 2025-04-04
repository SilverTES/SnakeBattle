﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mugen.Core;
using Mugen.GFX;
using Mugen.Input;

namespace SnakeBattle
{
    internal class ScreenPlay : Node
    {
        Game1 _game;

        KeyboardState _key;

        Vector2 _mouse;

        Arena _arena;

        Hero _hero;

        Addon.Loop _loop;

        ArenaSet[] _arenaSets = [
            new ArenaSet(32,32,32,32),
            new ArenaSet(24,24,40,40),
            new ArenaSet(16,16,64,64),
            new ArenaSet(12,12,80,80),
            ];

        int _playerLife = 100;
        int _enemyLife = 80;


        public ScreenPlay(Game1 game)
        {
            _game = game;

            SetPosition(0, 0);
            SetSize(Game1.ScreenW, Game1.ScreenH);

            _arena = (Arena)new Arena(_arenaSets[2]).AppendTo(this);
            _arena.SetPosition(440, 20);

            _loop = new Addon.Loop(this);
            _loop.SetLoop(0f, 0f, 2f, .05f, Mugen.Animation.Loops.PINGPONG);
            _loop.Start();

            _hero = (Hero)new Hero(_arena, new Point(_arena.MapSize.X / 2, _arena.MapSize.Y / 2), 5).AppendTo(_arena);

            Init();
        }
        public override Node Init()
        {
            _arena.KillAll(UID.Get<Item>());
            _arena.KillAll(UID.Get<Enemy>());
            _arena.ClearGrid();

            _arena.AddRandomItem(16);
            _arena.AddRandomEnemy(8);

            _hero.DeleteBody();
            _hero.SetMapPosition(new Point(_arena.MapSize.X / 2, _arena.MapSize.Y / 2));

            return base.Init();
        }

        public override Node Update(GameTime gameTime)
        {
            _key = Keyboard.GetState();
            _loop.Update(gameTime);

            _mouse = WindowManager.GetMousePosition();

            // Debug
            if (ButtonControl.OnePress("Reset", _key.IsKeyDown(Keys.F5)))
            {
                Init();
            }

            UpdateChilds(gameTime);

            if (_arena.GroupOf(UID.Get<Item>()).Count <= 0 && !_hero._isMove)
                _arena.AddRandomItem(16);

            if (_hero._onChainLose)
            {
                _enemyLife -= _hero.NbBody();
            }


            return base.Update(gameTime);   
        }
        public override Node Draw(SpriteBatch batch, GameTime gameTime, int indexLayer)
        {
            batch.GraphicsDevice.Clear(Color.Transparent);

            if (indexLayer == (int)Game1.Layers.Main)
            {
                batch.Draw(Game1._texBG, AbsXY + Vector2.UnitY * _loop._current, Color.White);

                batch.FillRectangle(AbsRectF, Color.Black * .5f);
            }

            if (indexLayer == (int)Game1.Layers.Debug)
            {
                //batch.Sight(_mouse, Game1.ScreenW, Game1.ScreenH, Color.Gray * .8f, 1);

                //_arena.DrawCell(batch, new Point(2, 2), Color.Red * .5f);
                //_arena.DrawLine(batch, new Point(2, 2), new Point(8,8), Color.Red * .5f);

                batch.CenterStringXY(Game1._fontMain, $"Player {_playerLife}", new Vector2(80, Game1.ScreenH/2), Color.Yellow);
                batch.CenterStringXY(Game1._fontMain, $"Enemy {_enemyLife}", new Vector2(Game1.ScreenW - 80, Game1.ScreenH/2), Color.Yellow);

            }

            DrawChilds(batch, gameTime, indexLayer);

            return base.Draw(batch, gameTime, indexLayer);
        }
    }
}
