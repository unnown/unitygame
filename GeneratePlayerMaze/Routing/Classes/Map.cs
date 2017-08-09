using Algorithms;
using Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routing.Classes
{
    public class Map
    {
        public TileType[,] tiles;

        public PathFinderFast mPathFinder;

        public byte[,] mGrid;

        public int mWidth = 0;

        public int mHeight = 0;

        public Map() { }
        public Map(int width, int height)
        {
            this.mWidth = width;
            this.mHeight = height;
            tiles = new TileType[mWidth, mHeight];

            mGrid = new byte[NextPowerOfTwo((int)mWidth), NextPowerOfTwo((int)mHeight)];
            InitPathFinder();
        }

        public void SetTile(int x, int y, TileType type)
        {
            if (x < 0 || x > mWidth || y < 0 || y > mHeight)
                return;

            tiles[x, y] = type;

            if (type == TileType.Block)
            {
                mGrid[x, y] = 0;
            }
            else if (type == TileType.Deadly)
            {
                mGrid[x, y] = 2;
            }
            else
            {
                mGrid[x, y] = 1;
            }
        }

        public TileType GetTile(int x, int y)
        {
            if (x < 0 || x >= mWidth
                || y < 0 || y >= mHeight)
                return TileType.Block;

            return tiles[x, y];
        }

        public bool IsGround(int x, int y)
        {
            if (x < 0 || x >= mWidth
               || y < 0 || y >= mHeight)
                return false;

            return (tiles[x, y] == TileType.Block);
        }

        public bool IsObstacle(int x, int y)
        {
            if (x < 0 || x >= mWidth
                || y < 0 || y >= mHeight)
                return true;

            return (tiles[x, y] == TileType.Block || tiles[x, y] == TileType.Deadly);
        }

        public bool IsNotEmpty(int x, int y)
        {
            if (x < 0 || x >= mWidth
                || y < 0 || y >= mHeight)
                return true;

            return (tiles[x, y] != TileType.Empty);
        }

        private void InitPathFinder()
        {
            mPathFinder = new PathFinderFast(mGrid, this);

            mPathFinder.Formula = HeuristicFormula.Manhattan;
            //if false then diagonal movement will be prohibited
            mPathFinder.Diagonals = false;
            //if true then diagonal movement will have higher cost
            mPathFinder.HeavyDiagonals = false;
            //estimate of path length
            mPathFinder.HeuristicEstimate = 20;
            mPathFinder.PunishChangeDirection = false;
            mPathFinder.TieBreaker = false;
            mPathFinder.SearchLimit = 20000;
            mPathFinder.DebugProgress = true;
            mPathFinder.DebugFoundPath = true;
        }

        private static int NextPowerOfTwo(int x)
        {
            var result = 2;
            while (result < x)
            {
                result <<= 1;
            }
            return result;
        }
    }
}
