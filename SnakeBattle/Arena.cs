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
    class Cell
    {
        public int _type = Const.NoIndex;
        public Node _owner = null;
    }

    struct ArenaSet
    {
        public int _mapW;
        public int _mapH;
        public int _cellW;
        public int _cellH;

        public ArenaSet(int mapW, int mapH, int cellW = 32 , int cellH = 32)
        {
            _mapW = mapW;
            _mapH = mapH;
            _cellW = cellW;
            _cellH = cellH;
        }
    }

    class Arena : Node
    {
        public Point MapSize => _mapSize;
        Point _mapSize = new Point(20,20);
        public Vector2 CellSize => _cellSize;
        Vector2 _cellSize = new Vector2(32, 32);



        List2D<Cell> _grid = new List2D<Cell>(20,20);

        public static Color[] Colors = [
            Color.Red,
            Color.Blue,
            Color.Green,
            Color.Yellow,
            Color.Violet,
            ];

        public Arena(ArenaSet arenaSet)
        {
            SetMapSize(arenaSet);
            InitGrid();

        }
        public void AddRandomItem(int nbItems)
        {
            for (int i = 0; i < nbItems; i++)
            {
                bool canSpawn = false;
                while(!canSpawn)
                {
                    int x = Misc.Rng.Next(0, MapSize.X);
                    int y = Misc.Rng.Next(0, MapSize.Y);

                    Point pos = new Point(x, y);

                    if (GetGrid(pos)._type != Const.NoIndex)
                        continue;

                    Color color = Colors[Misc.Rng.Next(0, Colors.Length - 1)];

                    new Item(this, pos, color).AppendTo(this);
                    canSpawn = true;
                }
            }
        }
        public void ClearGrid(int type = Const.NoIndex, Node owner = null)
        {
            for (int i = 0; i < _grid._width; i++)
            {
                for (int j = 0; j < _grid._height; j++)
                {
                    var cell = _grid.Get(i, j);
                    
                    if (cell != null)
                    {
                        cell._type = type;
                        cell._owner = owner;
                    }
                }
            }
        }
        public void InitGrid()
        {
            for (int i = 0; i < _grid._width; i++)
            {
                for (int j = 0; j < _grid._height; j++)
                {
                    _grid.Put(i, j, new Cell());
                }
            }
        }
        public void SetMapSize(ArenaSet arenaSet)
        {
            _mapSize.X = arenaSet._mapW;
            _mapSize.Y = arenaSet._mapH;

            _cellSize.X = arenaSet._cellW;
            _cellSize.Y = arenaSet._cellH;

            _rect.Width = _mapSize.X * CellSize.X;
            _rect.Height = _mapSize.Y * CellSize.Y;

            _grid.ResizeVecObject2D(arenaSet._mapW, arenaSet._mapH);
        }
        public bool IsInArena(Point mapPosition)
        {
            if (mapPosition.X < 0 || mapPosition.X >= MapSize.X ||
                mapPosition.Y < 0 || mapPosition.Y >= MapSize.Y)
                return false;
            else
                return true;
        }
        public bool IsVoid(Point mapPosition)
        {
            if (!IsInArena(mapPosition))
                return false;

            if (GetGrid(mapPosition)._type != Const.NoIndex)
                return false;

            return true;
        }
        public bool Is<T>(Point mapPosition)
        {
            if (!IsInArena(mapPosition))
                return false;

            if (GetGrid(mapPosition)._type == UID.Get<T>())
                return true;

            return false;
        }
        public void SetGrid(Point mapPosition, int type, Node owner)
        {
            if (IsInArena(mapPosition))
            {
                var cell = _grid.Get(mapPosition.X, mapPosition.Y);
                
                if (cell != null)
                {
                    cell._type = type;
                    cell._owner = owner;
                }
            }
        }
        public Cell GetGrid(Point mapPosition)
        {
            if (IsInArena(mapPosition))
                return _grid.Get(mapPosition.X, mapPosition.Y);

            return null;
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
                batch.Grid(AbsXY, AbsRectF.Width, AbsRectF.Height, CellSize.X, CellSize.Y, Color.Black * .25f, 1f);

                batch.Rectangle(AbsRectF.Extend(2f), Color.RoyalBlue, 3f);
            }

            if (indexLayer == (int)Game1.Layers.Debug)
            {
                for (int i = 0; i < _grid._width; i++)
                {
                    for (int j = 0; j < _grid._height; j++)
                    {

                        var cell = _grid.Get(i, j);


                        if (cell != null)
                        {
                            if (cell._type == Const.NoIndex)
                                continue;

                            Vector2 pos = Vector2.UnitX * i * CellSize.X + Vector2.UnitY * j * CellSize.Y;
                            batch.CenterStringXY(Game1._fontMain, $"{_grid.Get(i,j)._type}", pos + AbsXY + CellSize / 2, Color.White * .8f);

                        }
                    }
                }
            }

            DrawChilds(batch, gameTime, indexLayer);

            return base.Draw(batch, gameTime, indexLayer);
        }
    }
}
