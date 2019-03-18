using System;
using System.Collections.Generic;
using System.Linq;

namespace Unicursal.GameCore
{
    class StepArgs
    {
        public Direction To { get; set; }
        public int Index { get; set; }
    }

    class AutoSolver
    {
        private Random rd;
        private GameData thisGame;
        private List<int> thisPath;
        private bool isSolve;

        public Action<StepArgs> ShowAction;

        private void CallShowAction(StepArgs setpInfo)
        {
            if (ShowAction != null)
            {
                ShowAction(setpInfo);
            }
        }

        private void GoTo(Direction direction)
        {
            CallShowAction(new StepArgs { To = direction });
        }

        private void CutTo(int index)
        {
            CallShowAction(new StepArgs { Index = index });
        }

        private void Log(string info)
        {
#if DEBUG
            //LogUtil.Info(info);
#endif
        }

        public List<int> TrySolve(GameData game, bool show = false)
        {
            thisPath = new List<int>();
            thisGame = GameData.From(game.ZipData());
            rd = new Random();
            isSolve = false;
            Log("开始自动求解");
            Step();
            Log("结束自动求解");
            if (thisPath.Any())
            {
                foreach (var block in thisGame.Blocks)
                {
                    var tg = game.Get(block.Index);
                    tg.IsPass = block.IsPass;
                    tg.FromDirection = block.FromDirection;
                    tg.IsWall = block.IsWall;
                }
                game.Path = thisGame.Path;
            }
            return thisPath;
        }

        private void Step()
        {
            //取当前位置
            var current = thisGame.CurrentPoint;
            Log("当前位置：" + current);

            var thisBlockTo = thisGame.Get(current).RestrictTo;

            //从随机的方向开始尝试
            int rdInt;
            int i = 1;
            int max = 4;
            if (thisBlockTo == Direction.None)
            {
                rdInt = rd.Next(100);
            }
            else
            {
                rdInt = (int)thisBlockTo;
                i = 3;
                max = 3;
            }

            //把四个方向都尝试一遍
            for (; i <= max; i++)
            {
                //要走的方向
                var iDir = (rdInt + i) % 4 + 1;
                var dir = (Direction)iDir;
                //走一步
                Log("尝试方向：" + dir);
                if (thisGame.GoTo(dir))
                {
                    GoTo(dir);
                    //如果完成游戏，复制游戏路径，标记解决
                    if (thisGame.IsFinished())
                    {
                        isSolve = true;
                        thisPath = thisGame.Path.ToList();
                        Log("求解成功：" + string.Join(",", thisPath));
                    }
                    //如果游戏被解决，直接退出求解
                    if (isSolve)
                    {
                        break;
                    }
                    //在求解的过程中出现不超过1个死角，未开拓区域是连续区域，限制进入的块，继续尝试下一步
                    var corner = thisGame.GetCornerCount();
                    if (corner < 2 && thisGame.IsUnPassInOneArea() && thisGame.CheckCornerRestricts())
                    {
                        Log("尝试下一步");
                        Step();
                        //如果游戏被解决，从递归中退出
                        if (isSolve)
                        {
                            break;
                        }
                    }
                    //取消这一步尝试
                    Log("求解失败，返回路径到：" + current);
                    thisGame.CutTo(current);
                    CutTo(current);
                }
            }
        }
    }
}
