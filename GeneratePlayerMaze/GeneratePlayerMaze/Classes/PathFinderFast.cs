﻿//
//  THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
//  PURPOSE. IT CAN BE DISTRIBUTED FREE OF CHARGE AS LONG AS THIS HEADER 
//  REMAINS UNCHANGED.
//
//  Email:  gustavo_franco@hotmail.com
//
//  Copyright (C) 2006 Franco, Gustavo 
//

using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using GeneratePlayerMaze.Classes;

namespace Algorithms
{
    #region Enum
    public enum HeuristicFormula
    {
        Manhattan = 1,
        MaxDXDY = 2,
        DiagonalShortCut = 3,
        Euclidean = 4,
        EuclideanNoSQR = 5,
        Custom1 = 6
    }
    #endregion

    public struct Location
    {
        public Location(int xy, int z)
        {
            this.xy = xy;
            this.z = z;
        }

        public int xy;
        public int z;
    }

    public class PathFinderFast
    {

        #region Structs
        internal struct PathFinderNodeFast
        {
            #region Variables Declaration
            public int F; // f = gone + heuristic
            public int G;
            public ushort PX; // Parent
            public ushort PY;
            public byte Status;
            public byte PZ;
            public short JumpLength;
            #endregion

            public PathFinderNodeFast UpdateStatus(byte newStatus)
            {
                PathFinderNodeFast newNode = this;
                newNode.Status = newStatus;
                return newNode;
            }
        }

        #endregion

        private List<PathFinderNodeFast>[] nodes;
        private Stack<int> touchedLocations;

        #region Variables Declaration
        // Heap variables are initializated to default, but I like to do it anyway
        private byte[,] mGrid = null;
        private PriorityQueueB<Location> mOpen = null;
        private List<Point> mClose = null;
        private bool mStop = false;
        private bool mStopped = true;
        private HeuristicFormula mFormula = HeuristicFormula.Manhattan;
        private bool mDiagonals = true;
        private int mHEstimate = 2;
        private bool mPunishChangeDirection = false;
        private bool mTieBreaker = false;
        private bool mHeavyDiagonals = false;
        private int mSearchLimit = 2000;
        private double mCompletedTime = 0;
        private bool mDebugProgress = false;
        private bool mDebugFoundPath = false;
        private byte mOpenNodeValue = 1;
        private byte mCloseNodeValue = 2;

        //Promoted local variables to member variables to avoid recreation between calls
        private int mH = 0;
        private Location mLocation;
        private int mNewLocation = 0;
        private PathFinderNodeFast mNode;
        private ushort mLocationX = 0;
        private ushort mLocationY = 0;
        private ushort mNewLocationX = 0;
        private ushort mNewLocationY = 0;
        private int mCloseNodeCounter = 0;
        private ushort mGridX = 0;
        private ushort mGridY = 0;
        private ushort mGridXMinus1 = 0;
        private ushort mGridXLog2 = 0;
        private bool mFound = false;
        private sbyte[,] mDirection = new sbyte[8, 2] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 }, { 1, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 } };
        private int mEndLocation = 0;
        private int mNewG = 0;

        public Map mMap;
        #endregion

        #region Constructors
        public PathFinderFast(byte[,] grid, Map map)
        {
            if (map == null)
                throw new Exception("Map cannot be null");
            if (grid == null)
                throw new Exception("Grid cannot be null");

            mMap = map;
            mGrid = grid;
            mGridX = (ushort)(mGrid.GetUpperBound(0) + 1);
            mGridY = (ushort)(mGrid.GetUpperBound(1) + 1);
            mGridXMinus1 = (ushort)(mGridX - 1);
            mGridXLog2 = (ushort)Math.Log(mGridX, 2);

            if (Math.Log(mGridX, 2) != (int)Math.Log(mGridX, 2) ||
                Math.Log(mGridY, 2) != (int)Math.Log(mGridY, 2))
                throw new Exception("Invalid Grid, size in X and Y must be power of 2");

            if (nodes == null || nodes.Length != (mGridX * mGridY))
            {
                nodes = new List<PathFinderNodeFast>[mGridX * mGridY];
                touchedLocations = new Stack<int>(mGridX * mGridY);
                mClose = new List<Point>(mGridX * mGridY);
            }

            for (var i = 0; i < nodes.Length; ++i)
                nodes[i] = new List<PathFinderNodeFast>(1);

            mOpen = new PriorityQueueB<Location>(new ComparePFNodeMatrix(nodes));
        }
        #endregion

        #region Properties
        public bool Stopped
        {
            get { return mStopped; }
        }

        public HeuristicFormula Formula
        {
            get { return mFormula; }
            set { mFormula = value; }
        }

        public bool Diagonals
        {
            get { return mDiagonals; }
            set
            {
                mDiagonals = value;
                if (mDiagonals)
                    mDirection = new sbyte[8, 2] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 }, { 1, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 } };
                else
                    mDirection = new sbyte[4, 2] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 } };
            }
        }

        public bool HeavyDiagonals
        {
            get { return mHeavyDiagonals; }
            set { mHeavyDiagonals = value; }
        }

        public int HeuristicEstimate
        {
            get { return mHEstimate; }
            set { mHEstimate = value; }
        }

        public bool PunishChangeDirection
        {
            get { return mPunishChangeDirection; }
            set { mPunishChangeDirection = value; }
        }

        public bool TieBreaker
        {
            get { return mTieBreaker; }
            set { mTieBreaker = value; }
        }

        public int SearchLimit
        {
            get { return mSearchLimit; }
            set { mSearchLimit = value; }
        }

        public double CompletedTime
        {
            get { return mCompletedTime; }
            set { mCompletedTime = value; }
        }

        public bool DebugProgress
        {
            get { return mDebugProgress; }
            set { mDebugProgress = value; }
        }

        public bool DebugFoundPath
        {
            get { return mDebugFoundPath; }
            set { mDebugFoundPath = value; }
        }
        #endregion

        #region Methods
        public void FindPathStop()
        {
            mStop = true;
        }

        public List<Point> FindPath(Point start, Point end, short maxCharacterJumpHeight)
        {
            lock (this)
            {
                while (touchedLocations.Count > 0)
                    nodes[touchedLocations.Pop()].Clear();

                if (mGrid[end.X, end.Y] == 0)
                    return null;

                mFound = false;
                mStop = false;
                mStopped = false;
                mCloseNodeCounter = 0;
                mOpenNodeValue += 2;
                mCloseNodeValue += 2;
                mOpen.Clear();

                mLocation.xy = (start.Y << mGridXLog2) + start.X;
                mLocation.z = 0;
                mEndLocation = (end.Y << mGridXLog2) + end.X;

                PathFinderNodeFast firstNode = new PathFinderNodeFast();
                firstNode.G = 0;
                firstNode.F = mHEstimate;
                firstNode.PX = (ushort)start.X;
                firstNode.PY = (ushort)start.Y;
                firstNode.PZ = 0;
                firstNode.Status = mOpenNodeValue;

                if (mMap.IsGround(start.X, start.Y - 1))
                    firstNode.JumpLength = 0;
                else
                    firstNode.JumpLength = (short)(maxCharacterJumpHeight * 2);

                nodes[mLocation.xy].Add(firstNode);
                touchedLocations.Push(mLocation.xy);

                mOpen.Push(mLocation);

                while (mOpen.Count > 0 && !mStop)
                {
                    mLocation = mOpen.Pop();

                    //Is it in closed list? means this node was already processed
                    if (nodes[mLocation.xy][mLocation.z].Status == mCloseNodeValue)
                        continue;

                    mLocationX = (ushort)(mLocation.xy & mGridXMinus1);
                    mLocationY = (ushort)(mLocation.xy >> mGridXLog2);

                    if (mLocation.xy == mEndLocation)
                    {
                        nodes[mLocation.xy][mLocation.z] = nodes[mLocation.xy][mLocation.z].UpdateStatus(mCloseNodeValue);
                        mFound = true;
                        break;
                    }

                    if (mCloseNodeCounter > mSearchLimit)
                    {
                        mStopped = true;
                        return null;
                    }

                    //Lets calculate each successors
                    for (var i = 0; i < (mDiagonals ? 8 : 4); i++)
                    {
                        mNewLocationX = (ushort)(mLocationX + mDirection[i, 0]);
                        mNewLocationY = (ushort)(mLocationY + mDirection[i, 1]);
                        mNewLocation = (mNewLocationY << mGridXLog2) + mNewLocationX;

                        var onGround = false;
                        var atCeiling = false;

                        if (mGrid[mNewLocationX, mNewLocationY] == 0 || mGrid[mNewLocationX, mNewLocationY] == 2)
                            goto CHILDREN_LOOP_END;

                        if (mMap.IsGround(mNewLocationX, mNewLocationY - 1))
                            onGround = true;
                        else if (mGrid[mNewLocationX, mNewLocationY + 1] == 0)
                            atCeiling = true;
                        else if (mGrid[mNewLocationX, mNewLocationY + 1] == 2)
                            goto CHILDREN_LOOP_END;

                        //calculate a proper jumplength value for the successor

                        var jumpLength = nodes[mLocation.xy][mLocation.z].JumpLength;
                        short newJumpLength = jumpLength;

                        if (atCeiling)
                        {
                            if (mNewLocationX != mLocationX)                              
                                newJumpLength = (short)Math.Max(maxCharacterJumpHeight * 2 + 1, jumpLength + 1);
                            else
                                newJumpLength = (short)Math.Max(maxCharacterJumpHeight * 2, jumpLength + 2);
                        }
                        else if (onGround)
                            newJumpLength = 0;
                        else if (mNewLocationY > mLocationY)
                        {
                            if (jumpLength < 2) //first jump is always two block up instead of one up and optionally one to either right or left
                                newJumpLength = 3;
                            else if (jumpLength % 2 == 0)
                                newJumpLength = (short)(jumpLength + 2);
                            else
                                newJumpLength = (short)(jumpLength + 1);
                        }
                        else if (mNewLocationY < mLocationY)
                        {
                            if (jumpLength % 2 == 0)
                                newJumpLength = (short)Math.Max(maxCharacterJumpHeight * 2, jumpLength + 2);
                            else
                                newJumpLength = (short)Math.Max(maxCharacterJumpHeight * 2, jumpLength + 1);
                        }
                        else if (!onGround && mNewLocationX != mLocationX)
                            newJumpLength = (short)(jumpLength + 1);

                        if (jumpLength >= 0 && jumpLength % 2 != 0 && mLocationX != mNewLocationX)
                            continue;

                        //if we're falling and succeor's height is bigger than ours, skip that successor
                        if (jumpLength >= maxCharacterJumpHeight * 2 && mNewLocationY > mLocationY)
                            continue;

                        if (newJumpLength >= maxCharacterJumpHeight * 2 + 6 && mNewLocationX != mLocationX && (newJumpLength - (maxCharacterJumpHeight * 2 + 6)) % 8 != 3)
                            continue;


                        mNewG = nodes[mLocation.xy][mLocation.z].G + mGrid[mNewLocationX, mNewLocationY] + newJumpLength / 4;

                        if (nodes[mNewLocation].Count > 0)
                        {
                            int lowestJump = short.MaxValue;
                            bool couldMoveSideways = false;
                            for (int j = 0; j < nodes[mNewLocation].Count; ++j)
                            {
                                if (nodes[mNewLocation][j].JumpLength < lowestJump)
                                    lowestJump = nodes[mNewLocation][j].JumpLength;

                                if (nodes[mNewLocation][j].JumpLength % 2 == 0 && nodes[mNewLocation][j].JumpLength < maxCharacterJumpHeight * 2 + 6)
                                    couldMoveSideways = true;
                            }

                            if (lowestJump <= newJumpLength && (newJumpLength % 2 != 0 || newJumpLength >= maxCharacterJumpHeight * 2 + 6 || couldMoveSideways))
                                continue;
                        }

                        switch (mFormula)
                        {
                            default:
                            case HeuristicFormula.Manhattan:
                                mH = mHEstimate * (Math.Abs(mNewLocationX - end.X) + Math.Abs(mNewLocationY - end.Y));
                                break;
                            case HeuristicFormula.MaxDXDY:
                                mH = mHEstimate * (Math.Max(Math.Abs(mNewLocationX - end.X), Math.Abs(mNewLocationY - end.Y)));
                                break;
                            case HeuristicFormula.DiagonalShortCut:
                                var h_diagonal = Math.Min(Math.Abs(mNewLocationX - end.X), Math.Abs(mNewLocationY - end.Y));
                                var h_straight = (Math.Abs(mNewLocationX - end.X) + Math.Abs(mNewLocationY - end.Y));
                                mH = (mHEstimate * 2) * h_diagonal + mHEstimate * (h_straight - 2 * h_diagonal);
                                break;
                            case HeuristicFormula.Euclidean:
                                mH = (int)(mHEstimate * Math.Sqrt(Math.Pow((mNewLocationY - end.X), 2) + Math.Pow((mNewLocationY - end.Y), 2)));
                                break;
                            case HeuristicFormula.EuclideanNoSQR:
                                mH = (int)(mHEstimate * (Math.Pow((mNewLocationX - end.X), 2) + Math.Pow((mNewLocationY - end.Y), 2)));
                                break;
                            case HeuristicFormula.Custom1:
                                var dxy = new Point(Math.Abs(end.X - mNewLocationX), Math.Abs(end.Y - mNewLocationY));
                                var Orthogonal = Math.Abs(dxy.X - dxy.Y);
                                var Diagonal = Math.Abs(((dxy.X + dxy.Y) - Orthogonal) / 2);
                                mH = mHEstimate * (Diagonal + Orthogonal + dxy.X + dxy.Y);
                                break;
                        }

                        PathFinderNodeFast newNode = new PathFinderNodeFast();
                        newNode.JumpLength = newJumpLength;
                        newNode.PX = mLocationX;
                        newNode.PY = mLocationY;
                        newNode.PZ = (byte)mLocation.z;
                        newNode.G = mNewG;
                        newNode.F = mNewG + mH;
                        newNode.Status = mOpenNodeValue;

                        if (nodes[mNewLocation].Count == 0)
                            touchedLocations.Push(mNewLocation);

                        nodes[mNewLocation].Add(newNode);
                        mOpen.Push(new Location(mNewLocation, nodes[mNewLocation].Count - 1));

                        CHILDREN_LOOP_END:
                        continue;
                    }

                    nodes[mLocation.xy][mLocation.z] = nodes[mLocation.xy][mLocation.z].UpdateStatus(mCloseNodeValue);
                    mCloseNodeCounter++;
                }

                if (mFound)
                {
                    mClose.Clear();
                    var posX = end.X;
                    var posY = end.Y;

                    var fPrevNodeTmp = new PathFinderNodeFast();
                    var fNodeTmp = nodes[mEndLocation][0];

                    var fNode = end;
                    var fPrevNode = end;

                    var loc = (fNodeTmp.PY << mGridXLog2) + fNodeTmp.PX;

                    while (fNode.X != fNodeTmp.PX || fNode.Y != fNodeTmp.PY)
                    {
                        var fNextNodeTmp = nodes[loc][fNodeTmp.PZ];

                        if ((mClose.Count == 0)
                            || (fNodeTmp.JumpLength == 3)
                            || (fNextNodeTmp.JumpLength != 0 && fNodeTmp.JumpLength == 0)                                                                                                       //mark jumps starts
                            || (fNodeTmp.JumpLength == 0 && fPrevNodeTmp.JumpLength != 0)                                                                                                       //mark landings
                            || (fNode.Y > mClose[mClose.Count - 1].Y && fNode.Y > fNodeTmp.PY)
                            || (fNode.Y < mClose[mClose.Count - 1].Y && fNode.Y < fNodeTmp.PY)
                            || ((mMap.IsGround(fNode.X - 1, fNode.Y) || mMap.IsGround(fNode.X + 1, fNode.Y))
                                && fNode.Y != mClose[mClose.Count - 1].Y && fNode.X != mClose[mClose.Count - 1].X))
                            mClose.Add(fNode);

                        fPrevNode = fNode;
                        posX = fNodeTmp.PX;
                        posY = fNodeTmp.PY;
                        fPrevNodeTmp = fNodeTmp;
                        fNodeTmp = fNextNodeTmp;
                        loc = (fNodeTmp.PY << mGridXLog2) + fNodeTmp.PX;
                        fNode = new Point(posX, posY);
                    }

                    mClose.Add(fNode);

                    mStopped = true;

                    return mClose;
                }
                mStopped = true;
                return null;
            }
        }
        #endregion

        #region Inner Classes
        internal class ComparePFNodeMatrix : IComparer<Location>
        {
            #region Variables Declaration
            List<PathFinderNodeFast>[] mMatrix;
            #endregion

            #region Constructors
            public ComparePFNodeMatrix(List<PathFinderNodeFast>[] matrix)
            {
                mMatrix = matrix;
            }
            #endregion

            #region IComparer Members
            public int Compare(Location a, Location b)
            {
                if (mMatrix[a.xy][a.z].F > mMatrix[b.xy][b.z].F)
                    return 1;
                else if (mMatrix[a.xy][a.z].F < mMatrix[b.xy][b.z].F)
                    return -1;
                return 0;
            }
            #endregion
        }
        #endregion
    }
}