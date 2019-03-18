using System;
using System.Collections.Generic;
using System.Linq;

namespace Unicursal.GameCore
{
    class GameData
    {
        /// <summary>
        /// 游戏区域的宽度
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 游戏区域的高度
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// 游戏的起始点
        /// </summary>
        public int StartPoint { get; set; }
        /// <summary>
        /// 游戏路径的当前块的索引
        /// </summary>
        public int CurrentPoint { get; set; }
        /// <summary>
        /// 游戏中的块数据
        /// </summary>
        public List<Block> Blocks { get; private set; }
        /// <summary>
        /// 游戏路线经过的块索引
        /// </summary>
        public List<int> Path { get; internal set; }
        /// <summary>
        /// 初始化块
        /// </summary>
        public void InitBlocks()
        {
            Blocks = new List<Block>();
            Path = new List<int>();

            for (int i = 0; i < Width * Height; i++)
            {
                Blocks.Add(new Block { Index = i, Switch = -1, BySwitch = -1 });
            }
        }
        /// <summary>
        /// 取位于指定索引上的块
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Block Get(int index)
        {
            return Blocks.FirstOrDefault(x => x.Index == index);
        }
        /// <summary>
        /// 将路径切断到某块上
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool CutTo(int index)
        {
            if (Path.Contains(index))
            {
                var i = Path.IndexOf(index);
                var count = Path.Count - i - 1;
                if (count > 0)
                {
                    for (int j = Path.Count - 1; j > i; j--)
                    {
                        var cu = Get(Path[j]);
                        cu.IsPass = false;
                        cu.FromDirection = Direction.None;
                        if (cu.Switch >= 0)
                        {
                            Get(cu.Switch).IsWall = true;
                        }
                        Path.RemoveAt(j);
                    }
                }
                if (Path.Count(x => Get(x).IsTeleporter) == 1)
                {
                    var last = Get(Path.Last());
                    last.IsPass = false;
                    last.FromDirection = Direction.None;
                    Path.RemoveAt(Path.Count - 1);
                }
                CurrentPoint = Path.Last();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 将路径朝某个方向移动
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public bool GoTo(Direction direction)
        {
            int index = CurrentPoint;
            if (direction == Direction.Left)
            {
                index = CurrentPoint - 1;
            }
            else if (direction == Direction.Up)
            {
                index = CurrentPoint - Width;
            }
            else if (direction == Direction.Right)
            {
                index = CurrentPoint + 1;
            }
            else if (direction == Direction.Down)
            {
                index = CurrentPoint + Width;
            }
            if (index != CurrentPoint)
            {
                return GoTo(index);
            }
            return false;
        }
        /// <summary>
        /// 将路径移动到某个块上
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool GoTo(int index)
        {
            if (Path.Count == 0)
            {
                Path.Add(StartPoint);
            }

            if (index < 0 || index >= Width * Height)
            {
                return false;
            }
            if (Path.Contains(index))
            {
                return false;
            }
            if (Get(index).IsWall)
            {
                return false;
            }

            var d = index - CurrentPoint;
            if (d == 1 && index % Width != 0)
            {
                if (Get(index).RestrictFrom == Direction.Left || Get(index).RestrictFrom == Direction.None)
                {
                    CurrentPoint = index;
                    Path.Add(CurrentPoint);
                    Get(CurrentPoint).IsPass = true;
                    Get(CurrentPoint).FromDirection = Direction.Left;
                    CheckAndCrossTeleporter();
                    return true;
                }
            }
            if (d == -1 && CurrentPoint % Width != 0)
            {
                if (Get(index).RestrictFrom == Direction.Right || Get(index).RestrictFrom == Direction.None)
                {
                    CurrentPoint = index;
                    Path.Add(CurrentPoint);
                    Get(CurrentPoint).IsPass = true;
                    Get(CurrentPoint).FromDirection = Direction.Right;
                    CheckAndCrossTeleporter();
                    return true;
                }
            }
            if (d == Width)
            {
                if (Get(index).RestrictFrom == Direction.Up || Get(index).RestrictFrom == Direction.None)
                {
                    CurrentPoint = index;
                    Path.Add(CurrentPoint);
                    Get(CurrentPoint).IsPass = true;
                    Get(CurrentPoint).FromDirection = Direction.Up;
                    CheckAndCrossTeleporter();
                    return true;
                }
            }
            if (d == -Width)
            {
                if (Get(index).RestrictFrom == Direction.Down || Get(index).RestrictFrom == Direction.None)
                {
                    CurrentPoint = index;
                    Path.Add(CurrentPoint);
                    Get(CurrentPoint).IsPass = true;
                    Get(CurrentPoint).FromDirection = Direction.Down;
                    CheckAndCrossTeleporter();
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 检查当前块是否传送点，是的话直接传送
        /// </summary>
        private void CheckAndCrossTeleporter()
        {
            var cu = Get(CurrentPoint);
            if (cu.Switch >= 0)
            {
                Get(cu.Switch).IsWall = false;
            }
            if (!cu.IsTeleporter)
            {
                return;
            }
            var other = Blocks.FirstOrDefault(x => x.IsTeleporter && x.Index != CurrentPoint);
            if (other != null)
            {
                CurrentPoint = other.Index;
                other.IsPass = true;
                Path.Add(CurrentPoint);
            }
            else
            {
                throw new Exception("游戏中的传送点必须是两个。");
            }
        }
        /// <summary>
        /// 检查限制进入的块的合法性
        /// </summary>
        /// <returns></returns>
        public bool CheckRestricts()
        {
            var restricts = Blocks.Where(x => x.RestrictFrom != Direction.None).ToList();
            foreach (var restrict in restricts)
            {
                if (restrict.RestrictFrom == Direction.Left)
                {
                    if (restrict.Index % Width == 0)
                        return false;
                    if (Get(restrict.Index - 1).IsWall)
                        return false;
                }
                else if (restrict.RestrictFrom == Direction.Up)
                {
                    if (restrict.Index - Width < 0)
                        return false;
                    if (Get(restrict.Index - Width).IsWall)
                        return false;
                }
                else if (restrict.RestrictFrom == Direction.Right)
                {
                    if ((restrict.Index + 1) % Width == 0)
                        return false;
                    if (Get(restrict.Index + 1).IsWall)
                        return false;
                }
                else if (restrict.RestrictFrom == Direction.Down)
                {
                    if (restrict.Index + Width >= Height * Width)
                        return false;
                    if (Get(restrict.Index + Width).IsWall)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 游戏中死角的个数
        /// </summary>
        /// <returns></returns>
        public int GetCornerCount()
        {
            var spares = GetAllUnPassIndeics();
            var corners = spares.Where(x => GetAroundIndex(x).Count <= 1 && !AbuttToCurrent(x)).ToList();
            var cnt = corners.Count;
            return cnt;
        }
        /// <summary>
        /// 检查所有的限制进入的块，是否是死角
        /// </summary>
        /// <returns></returns>
        public bool CheckCornerRestricts()
        {
            var restricts = Blocks.Where(x => x.RestrictFrom != Direction.None && !x.IsPass).ToList();
            foreach (var restrict in restricts)
            {
                if (restrict.RestrictFrom == Direction.Left)
                {
                    if (restrict.Index % Width != 0)
                        if (Get(restrict.Index - 1).IsPass && restrict.Index - 1 != CurrentPoint)
                            return false;
                }
                else if (restrict.RestrictFrom == Direction.Up)
                {
                    if (restrict.Index - Width >= 0)
                        if (Get(restrict.Index - Width).IsPass && restrict.Index - Width != CurrentPoint)
                            return false;
                }
                else if (restrict.RestrictFrom == Direction.Right)
                {
                    if ((restrict.Index + 1) % Width != 0)
                        if (Get(restrict.Index + 1).IsPass && restrict.Index + 1 != CurrentPoint)
                            return false;
                }
                else if (restrict.RestrictFrom == Direction.Down)
                {
                    if (restrict.Index + Width < Height * Width)
                        if (Get(restrict.Index + Width).IsPass && restrict.Index + Width != CurrentPoint)
                            return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 判断一个索引是否贴近当前索引
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool AbuttToCurrent(int index)
        {
            var around = GetAroundIndex(CurrentPoint);
            return around.Contains(index);
        }
        /// <summary>
        /// 取游戏中所有未开拓索引
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllUnPassIndeics()
        {
            return Blocks.Select(x => x.Index).Where(UnPass).ToList();
        }
        /// <summary>
        /// 搜索指定索引周围不包括自身的未开拓的块索引
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private List<int> GetAroundIndex(int index)
        {
            var rtn = new List<int>();
            if (index % Width > 0)
                rtn.Add(index - 1);
            if (index >= Width)
                rtn.Add(index - Width);
            if ((index + 1) % Width > 0)
                rtn.Add(index + 1);
            if (index < (Height - 1) * Width)
                rtn.Add(index + Width);
            if (Get(index).IsTeleporter)
            {
                var other = Blocks.FirstOrDefault(x => x.IsTeleporter && x.Index != index);
                if (other != null) rtn.Add(other.Index);
            }
            return rtn.Where(UnPass).ToList();
        }
        /// <summary>
        /// 判断一个索引是未开拓块的索引
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool UnPass(int index)
        {
            var block = Get(index);
            return (!block.IsPass && !block.IsWall) || (!block.IsPass && block.BySwitch != -1);
        }
        /// <summary>
        /// 判断所有为开拓区域是否是一块连接的区域
        /// </summary>
        /// <returns></returns>
        public bool IsUnPassInOneArea()
        {
            var spares = GetAllUnPassIndeics();
            var cnt = spares.Count();
            var index = spares.First();
            var unPass = new List<int> { index };
            CollectUnPass(unPass, index);
            return unPass.Count == cnt;
        }
        /// <summary>
        /// 收集指定索引周围未开拓的索引
        /// </summary>
        /// <param name="unPass"></param>
        /// <param name="index"></param>
        private void CollectUnPass(List<int> unPass, int index)
        {
            var around = GetAroundIndex(index);
            foreach (var i in around)
            {
                if (!unPass.Contains(i))
                {
                    unPass.Add(i);
                    CollectUnPass(unPass, i);
                }
            }
        }
        /// <summary>
        /// 检验游戏是否已经完成
        /// </summary>
        /// <returns></returns>
        public bool IsFinished()
        {
            var spares = GetAllUnPassIndeics();
            return !spares.Any();
        }

        /// <summary>
        /// 重新初始化地图
        /// </summary>
        internal void ReInit()
        {
            if (StartPoint >= 0)
            {
                foreach (var block in Blocks)
                {
                    block.FromDirection = Direction.None;
                    block.IsPass = false;
                }
                CurrentPoint = StartPoint;
                Path.Clear();
                Get(StartPoint).IsPass = true;
                Path.Add(StartPoint);
                Blocks.Where(x => x.BySwitch != -1).ToList().ForEach(x => x.IsWall = true);
            }
        }

        /// <summary>
        /// 压缩游戏块数据到字符串
        /// </summary>
        /// <returns></returns>
        public string ZipData()
        {
            var datas = new List<string>
            {
                //数据区域的大小，宽度*高度
                string.Format("{0}*{1}", Width, Height),
                //游戏的起始点
                string.Format("{0}", StartPoint),
                //游戏中的墙，用,分割
                string.Format("{0}", string.Join(",",
                    Blocks.Where(x => x.IsWall).Select(x => x.Index))),
                //游戏中限制进入块的数据，每一项格式为index-direction，多项用,分割
                string.Format("{0}", string.Join(",",
                    Blocks.Where(x => x.RestrictFrom != Direction.None)
                        .Select(x => x.Index + "-" + (int) x.RestrictFrom))),
                //游戏中的传送点，必须为两项，用,分割
                string.Format("{0}", string.Join(",",
                    Blocks.Where(x => x.IsTeleporter).Select(x => x.Index))),
                //控制开关数据，每一项格式为switch-index，多项用,分割
                string.Format("{0}", string.Join(",",
                    Blocks.Where(x => x.Switch >= 0).Select(x => x.Index + "-" + x.Switch)))
                //预留空间
            };

            return string.Join(";", datas);
        }
        /// <summary>
        /// 通过压缩的字符串数据加载游戏数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static GameData From(string data)
        {
            var game = new GameData();
            var datas = data.Split(";".ToCharArray());

            try
            {
                //数据区域的大小，宽度*高度
                if (datas.Length > 0 && !string.IsNullOrWhiteSpace(datas[0]))
                {
                    var wh = datas[0].Split('*');
                    game.Width = int.Parse(wh[0]);
                    game.Height = int.Parse(wh[1]);
                    game.InitBlocks();
                }
                //游戏的起始点
                if (datas.Length > 1 && !string.IsNullOrWhiteSpace(datas[1]))
                {
                    var start = int.Parse(datas[1]);
                    game.Get(start).IsPass = true;
                    game.StartPoint = start;
                    game.CurrentPoint = start;
                    game.Path.Add(start);
                }
                //游戏中的墙，用,分割
                if (datas.Length > 2 && !string.IsNullOrWhiteSpace(datas[2]))
                {
                    var ws = datas[2].Split(',').Select(int.Parse).ToList();
                    var walls = game.Blocks.Where(x => ws.Contains(x.Index)).ToList();
                    walls.ForEach(x => x.IsWall = true);
                }
                //游戏中限制进入块的数据，每一项格式为index-direction，多项用,分割
                if (datas.Length > 3 && !string.IsNullOrWhiteSpace(datas[3]))
                {
                    var fs = datas[3].Split(',').Select(x => x.Split('-')).ToDictionary(x => int.Parse(x[0]), x => int.Parse(x[1]));
                    var froms = game.Blocks.Where(x => fs.Keys.Contains(x.Index)).ToList();
                    froms.ForEach(x => x.RestrictFrom = (Direction)fs[x.Index]);

                    foreach (var block in froms)
                    {
                        if (block.RestrictFrom == Direction.Left)
                        {
                            game.Get(block.Index - 1).RestrictTo = Direction.Right;
                        }
                        else if (block.RestrictFrom == Direction.Up)
                        {
                            game.Get(block.Index - game.Width).RestrictTo = Direction.Down;
                        }
                        else if (block.RestrictFrom == Direction.Right)
                        {
                            game.Get(block.Index + 1).RestrictTo = Direction.Left;
                        }
                        else if (block.RestrictFrom == Direction.Down)
                        {
                            game.Get(block.Index + game.Width).RestrictTo = Direction.Up;
                        }
                    }
                }
                //游戏中的传送点，必须为两项，用,分割
                if (datas.Length > 4 && !string.IsNullOrWhiteSpace(datas[4]))
                {
                    var cs = datas[4].Split(',').Select(int.Parse).ToList();
                    var teleporters = game.Blocks.Where(x => cs.Contains(x.Index)).ToList();
                    teleporters.ForEach(x => x.IsTeleporter = true);
                }
                //控制开关数据，每一项格式为switch-index，多项用,分割
                if (datas.Length > 5 && !string.IsNullOrWhiteSpace(datas[5]))
                {
                    var ss = datas[5].Split(',').Select(x => x.Split('-')).ToDictionary(x => int.Parse(x[0]), x => int.Parse(x[1]));
                    var switchs = game.Blocks.Where(x => ss.Keys.Contains(x.Index)).ToList();
                    switchs.ForEach(x =>
                    {
                        x.Switch = ss[x.Index];
                        game.Get(ss[x.Index]).BySwitch = x.Index;
                    });
                    //game.Blocks.Where(x => ss.Values.Contains(x.Index)).ToList().ForEach(x => x.BySwitch = x.Index);
                }
            }
            catch
            {
                throw new Exception("错误的地图格式。");
            }

            return game;
        }
    }
}
