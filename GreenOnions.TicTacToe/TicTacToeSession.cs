﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace GreenOnions.TicTacToe
{
    public class TicTacToeSession
    {
        private int[,] data = null;  //井字棋棋子位置数据, 0为未下子, 1为玩家下子, -1为机器人下子
        private Bitmap lastStepBmp = null;

        ~TicTacToeSession()
        {
            try
            {
                lastStepBmp?.Dispose();
            }
            catch{ }
        }

        public int IsBitmapSizeSame(int width, int height)
        {
            if (lastStepBmp.Width == width && lastStepBmp.Height == height)  //和原始图尺寸一样
                return 1;
            if (width == height)  //长宽比一样, IOS端涂鸦后尺寸会等比例缩放到320*320, 需要先压缩回300*300
                return 0;
            return -1;
        }

        public Bitmap StartNewSession()
        {
            data = new int[3, 3];
            lastStepBmp = CreateChessboard();

            return lastStepBmp;
        }

        private Bitmap CreateChessboard()
        {
            Bitmap Chessboard = new Bitmap(300, 300, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(Chessboard);
            g.Clear(Color.White);

            g.DrawLine(Pens.Black, 0, 100, 300, 100);
            g.DrawLine(Pens.Black, 0, 200, 300, 200);

            g.DrawLine(Pens.Black, 100, 0, 100, 300);
            g.DrawLine(Pens.Black, 200, 0, 200, 300);
            return Chessboard;
        }

        private void DrawPiece(Bitmap Chessboard)
        {
            using (Graphics g = Graphics.FromImage(Chessboard))
            {
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        switch (data[x, y])
                        {
                            case -1:
                                DrawO(x * 100 + 50, y * 100 + 50);
                                break;
                            case 1:
                                DrawX(x * 100 + 50, y * 100 + 50);
                                break;
                        }
                    }
                }
                void DrawX(int loactionX, int locationY)
                {
                    g.DrawLine(new Pen(Brushes.Black, 4), loactionX - 30, locationY - 30, loactionX + 30, locationY + 30);
                    g.DrawLine(new Pen(Brushes.Black, 4), loactionX - 30, locationY + 30, loactionX + 30, locationY - 30);
                }

                void DrawO(int loactionX, int locationY)
                {
                    g.DrawEllipse(new Pen(Brushes.Black, 4), loactionX - 30, locationY - 30, 60, 60);
                }
            }
        }

        public Dictionary<System.Drawing.Point, int> PlayerMoveByBitmap(Bitmap playerStepBmp)
        {
#if DEBUG
            playerStepBmp.Save("接收到的图片.bmp", playerStepBmp.RawFormat);
#endif

            if (playerStepBmp.PixelFormat == PixelFormat.Format32bppArgb)
            {
#if DEBUG
                playerStepBmp.Save("转换通道前图片.bmp", playerStepBmp.RawFormat);
#endif

                Bitmap bmp24 = new Bitmap(playerStepBmp.Width, playerStepBmp.Height, PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(bmp24);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
                g.DrawImageUnscaled(playerStepBmp, 0, 0);
                playerStepBmp.Dispose();
                playerStepBmp = bmp24;

#if DEBUG
                playerStepBmp.Save("转换通道后图片.bmp", playerStepBmp.RawFormat);
#endif
            }

            Mat playStep = BitmapConverter.ToMat(playerStepBmp);

            if (playStep.Width != lastStepBmp.Width || playStep.Height != playStep.Height)
            {
                Bitmap before = BitmapConverter.ToBitmap(playStep, PixelFormat.Format24bppRgb);

#if DEBUG
                before.Save("缩放前图片.bmp", before.RawFormat);
#endif

                Mat temp = new Mat();
                Cv2.Resize(playStep, temp, new OpenCvSharp.Size(300, 300), 0, 0, InterpolationFlags.Lanczos4);
                Bitmap after = BitmapConverter.ToBitmap(temp, PixelFormat.Format24bppRgb);
#if DEBUG
                after.Save($"缩放后图片.bmp", after.RawFormat);
#endif

                playStep.Dispose();
                playStep = temp;
            }

            using (Mat lastStep = BitmapConverter.ToMat(lastStepBmp))
            {
                using (Mat nowStepTemp = lastStep - playStep)
                {
                    Dictionary<System.Drawing.Point, int> weight = new Dictionary<System.Drawing.Point, int>();

                    for (int x = 0; x < 300; x++)
                    {
                        if (x > 98 && x < 102)
                            continue;
                        if (x > 198 && x < 202)
                            continue;
                        for (int y = 0; y < 300; y++)
                        {
                            if (y >98 && y < 102)
                                continue;
                            if (y > 198 && y < 202)
                                continue;
                            byte i = nowStepTemp.At<byte>(y, x);
                            if (i > 100)
                            {
                                System.Drawing.Point point = new System.Drawing.Point(x / 100, y / 100);
                                if (weight.ContainsKey(point))
                                    weight[point]++;
                                else
                                    weight.Add(point, 1);
                            }
                        }
                    }

                    Bitmap bmpNewStep = BitmapConverter.ToBitmap(nowStepTemp, PixelFormat.Format24bppRgb);

#if DEBUG
                    bmpNewStep.Save($"相减后图片.bmp", bmpNewStep.RawFormat);
#endif

                    playStep.Dispose();

                    weight = weight.Where(kv => kv.Value > 300).ToDictionary(kv => kv.Key, kv => kv.Value);  //过滤一下保留大于300个像素修改的格子, 防止不小心画过线和图片缩放导致的相减误差

                    return weight;
                }
            }
        }

        /// <summary>
        /// 玩家下子
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="winOrLostType">-1为机器人获胜, 0为平局, 1为玩家获胜, null为对局尚未结束</param>
        /// <returns></returns>
        public Bitmap PlayerMove(int x, int y, out int? winOrLostType)  //玩家下子
        {
            winOrLostType = null;
            if (data[x, y] == 0)
            {
                data[x, y] = 1;

                var computerStep = ComputerMove();
                data[computerStep.X, computerStep.Y] = -1;

                lastStepBmp = CreateChessboard();
                DrawPiece(lastStepBmp);

                int camp;
                int resultX = CheckWinX(out camp);
                if (resultX > -1)
                {
                    DrawWinLine(resultX, -1);
                    if (camp != 0)
                        winOrLostType = camp;
                    return lastStepBmp;
                }

                int resultY = CheckWinY(out camp);
                if (resultY > -1)
                {
                    DrawWinLine(-1, resultY);
                    if (camp != 0)
                        winOrLostType = camp;
                    return lastStepBmp;
                }

                int resultXY = CheckWinXY(out camp);
                if (resultXY != 0)
                {
                    DrawWinLine(resultXY, resultXY);
                    if (camp != 0)
                        winOrLostType = camp;
                    return lastStepBmp;
                }

                if (OnlyLeftOneGrid(out int lastOneX, out int lastOneY))
                {
                    if (!IsPlayerWin(lastOneX, lastOneY))
                    {
                        winOrLostType = 0;
                        return lastStepBmp; //平局
                    }
                }
                return lastStepBmp;
            }
            else
            {
                return null;
            }
        }

        private (int X, int Y) ComputerMove()
        {
            var computerChance = FindChance(true);
            if (computerChance != null)
                return (computerChance.Value.X, computerChance.Value.Y);  //优先查找电脑获胜的机会

            var playerChance = FindChance(false);
            if (playerChance != null)
                return (playerChance.Value.X, playerChance.Value.Y);  //随后查找阻止玩家获胜的机会

            if (data[1,1] == 0)  //优先抢中间
                return (1, 1);

            while (true)  //都不符合条件的时候随机下子
            {
                Random random = new Random(Guid.NewGuid().GetHashCode());
                int x = random.Next(0, 3);
                int y = random.Next(0, 3);
                if (data[x, y] == 0)
                    return (x, y);
            }
        }

        private (int X, int Y)? FindChance(bool computerChance)
        {
            int[] row = new int[3];
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                    row[x] = data[x, y];
                if (Chance(row, computerChance))
                    return (Array.IndexOf(row, 0), y);
            }

            int[] column = new int[3];
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                    column[y] = data[x, y];
                if (Chance(column, computerChance))
                    return (x, Array.IndexOf(column, 0));
            }

            int[] slash = new int[3]; // /
            for (int i = 0; i < 3; i++)
            {
                int x = 2 - i;
                slash[i] = data[x, i];
            }
            if (Chance(slash, computerChance))
            {
                int index = Array.IndexOf(slash, 0);
                return (2 - index, index);
            }

            int[] backslash = new int[3]; // \
            for (int i = 0; i < 3; i++)
                backslash[i] = data[i, i];
            if (Chance(backslash, computerChance))
            {
                int index = Array.IndexOf(backslash, 0);
                return (index, index);
            }

            return null;

            bool Chance(int[] sourece, bool isComputer)
            {
                if (sourece.Contains(0))  //还有空格子
                {
                    int sum = sourece.Sum();
                    if (isComputer)
                    {
                        if (sum == -2)
                            return true;
                    }
                    else
                    {
                        if (sum == 2)
                            return true;
                    }
                }
                return false;
            }
        }

        private bool OnlyLeftOneGrid(out int lastOneX, out int lastOneY)
        {
            int zeroCount = 0;
            lastOneX = -1;
            lastOneY = -1;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (data[x, y] == 0)
                    {
                        zeroCount++;
                        lastOneX = x;
                        lastOneY = y;
                    }
                }
            }
            return zeroCount == 1;  //只剩一个位置下子, 可以直接判断是否结束
        }

        private bool IsPlayerWin(int lastOneX, int lastOneY)
        {
            data[lastOneX, lastOneY] = 1;
            int resultX = CheckWinX(out _);
            if (resultX > -1)
            {
                DrawWinLine(resultX, -1);
            }

            int resultY = CheckWinY(out _);
            if (resultY > -1)
            {
                DrawWinLine(-1, resultY);
            }

            int resultXY = CheckWinXY(out _);
            if (resultXY != 0)
            {
                DrawWinLine(resultXY, resultXY);
            }
            return false;
        }

        private void DrawWinLine(int x, int y)
        {
            using (Graphics g = Graphics.FromImage(lastStepBmp))
            {
                Pen pen = new Pen(Brushes.Red, 6);
                if (x == -1 && y == -1) //画/
                    g.DrawLine(pen, 280, 20, 20, 280);
                else if (x == -1)  //画竖线
                    g.DrawLine(pen, y * 100 + 50, 10, y * 100 + 50, 290);
                else if (y == -1)  //画横线
                    g.DrawLine(pen, 10, x * 100 + 50, 290, x * 100 + 50);
                else  //画\
                    g.DrawLine(pen, 20, 20, 280, 280);
            }
        }

        private int CheckWinX(out int camp)
        {
            camp = 0;
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    if (x == 0)
                        camp = data[x, y];
                    else if (data[x, y] == 0 || data[x, y] != camp)
                        goto IL_Next;
                }
                return y;
            IL_Next:;
            }
            return -1;
        }

        private int CheckWinY(out int camp)
        {
            camp = 0;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (y == 0)
                        camp = data[x, y];
                    else if (data[x, y] == 0 || data[x, y] != camp)
                        goto IL_Next;
                }
                return x;
            IL_Next:;
            }
            return -1;
        }

        private int CheckWinXY(out int camp)
        {
            camp = 0;
            for (int i = 0; i < 3; i++)
            {
                if (i == 0)
                    camp = data[i, i];
                else if (data[i, i] == 0 || data[i, i] != camp)
                    goto IL_Next;
            }
            return 1;
        IL_Next:;
            for (int i = 0; i < 3; i++)
            {
                int x = 2 - i;
                if (i == 0)
                    camp = data[x, i];
                else if (data[x, i] == 0 || data[x, i] != camp)
                    goto IL_0;
            }
            return -1;
        IL_0:;
            return 0;
        }
    }
}
