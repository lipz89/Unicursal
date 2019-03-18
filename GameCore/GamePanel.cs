using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Unicursal.GameCore
{
    public sealed class GamePanel : Panel
    {
        public GamePanel()
        {
            this.DoubleBuffered = true;
            this.PathColor = Color.DarkBlue;
            this.NormalBlockColor = Color.LightGray;
            this.StartBlockColor = Color.LightSeaGreen;
            this.WallBlockColor = Color.Red;
            this.PathBlockColor = Color.SkyBlue;
            this.FromColor = Color.Blue;
            this.SelectedIndex = -1;

            this.Size = new Size(200, 300);
            this.Paint += GamePanel_Paint;
            this.Resize += GamePanel_Resize;
            this.MouseClick += GamePanel_MouseClick;
        }

        public event MouseEventHandler OnSelected;
        private readonly AutoSolver autoSolver = new AutoSolver();

        private void GamePanel_MouseClick(object sender, MouseEventArgs e)
        {
            var lastSelect = SelectedIndex;
            SelectedIndex = -1;
            if (new Rectangle(offset.X, offset.Y, gWidth, gHeight).Contains(e.Location))
            {
                foreach (var block in Game.Blocks)
                {
                    var index = block.Index;
                    var i = index % Game.Width;
                    var j = index / Game.Width;
                    var sx = i * unit + offset.X + 1;
                    var sy = j * unit + offset.Y + 1;
                    var rect = new Rectangle(sx, sy, unit - 2, unit - 2);
                    if (rect.Contains(e.Location))
                    {
                        SelectedIndex = block.Index;
                        break;
                    }
                }
            }
            if (IsDesignMode && lastSelect != SelectedIndex)
            {
                if (lastSelect != -1)
                {
                    var i = lastSelect % Game.Width;
                    var j = lastSelect / Game.Width;
                    var sx = i * unit + offset.X;
                    var sy = j * unit + offset.Y;
                    this.Invalidate(new Rectangle(sx, sy, unit, unit));
                }

                if (SelectedIndex != -1)
                {
                    var i = SelectedIndex % Game.Width;
                    var j = SelectedIndex / Game.Width;
                    var sx = i * unit + offset.X;
                    var sy = j * unit + offset.Y;
                    this.Invalidate(new Rectangle(sx, sy, unit, unit));
                }
            }

            if (SelectedIndex != -1)
            {
                if (OnSelected != null)
                {
                    OnSelected(this, e);
                }
            }
        }

        [Browsable(true), DefaultValue(false), Category("Behavior"), Description("编辑地图模式")]
        public bool IsDesignMode { get; set; }

        public int SelectedIndex { get; private set; }

        private Point offset;

        private int gWidth;
        private int gHeight;

        private GameData Game { get; set; }

        private int unit = 40;
        private int stepSpace = 50;

        private void GamePanel_Resize(object sender, EventArgs e)
        {
            if (Game != null)
            {
                CalcUnit();
            }
        }

        private void CalcUnit()
        {
            var un = Math.Min((this.Width - 8) / Game.Width, (this.Height - 8) / Game.Height);
            unit = un / 2 * 2;
            gWidth = Game.Width * unit;
            gHeight = Game.Height * unit;
            offset = new Point((this.Width - gWidth) / 2 - 1, (this.Height - gHeight) / 2 - 1);
        }

        public void InitEmptyGame(Size size)
        {
            InitEmptyGame(size.Width, size.Height);
        }

        public void InitEmptyGame(int width, int height)
        {
            Game = new GameData { Width = width, Height = height };
            Game.InitBlocks();
            Game.StartPoint = -1;
        }

        public void ReInitGame()
        {
            Game.ReInit();
        }

        public void InitGame(string data)
        {
            Game = GameData.From(data);
        }

        #region 游戏逻辑

        /// <summary>
        /// 检验游戏是否已经完成
        /// </summary>
        /// <returns></returns>
        public bool IsFinished()
        {
            return Game.IsFinished();
        }

        /// <summary>
        /// 将路径移动到某个块上
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool Goto(int index)
        {
            var flag = Game.GoTo(index);
            this.Invalidate();
            return flag;
        }

        /// <summary>
        /// 将路径移动想某个方向
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public bool Goto(Direction direction)
        {
            var flag = Game.GoTo(direction);
            this.Invalidate();
            return flag;
        }

        public bool Back()
        {
            var count = Game.Path.Count;
            if (count > 1)
            {
                var index = Game.Path[count - 2];
                return CutTo(index);
            }
            return false;
        }
        public bool CutTo(int index)
        {
            var flag = Game.CutTo(index);
            this.Invalidate();
            return flag;
        }

        /// <summary>
        /// 保存游戏到字符串中（不包含游戏进度）
        /// </summary>
        /// <returns></returns>
        public string SaveData()
        {
            return Game.ZipData();
        }

        public List<int> Solve(bool show = false)
        {
            SelectedIndex = -1;
            this.Invalidate();
            autoSolver.ShowAction = show ? (Action<StepArgs>)StepAction : null;
            return autoSolver.TrySolve(Game, show);
        }

        private void StepAction(StepArgs stepInfo)
        {
            if (stepInfo.To != Direction.None)
            {
                Game.GoTo(stepInfo.To);
            }
            else
            {
                Game.CutTo(stepInfo.Index);
            }
            this.Invalidate();
            Thread.Sleep(stepSpace);
        }

        public void SpeedUp()
        {
            if (stepSpace > 10)
                stepSpace /= 2;
        }

        public bool IsSpeedMax()
        {
            return stepSpace <= 20;
        }

        public void SpeedDown()
        {
            if (stepSpace < 1000)
                stepSpace *= 2;
        }

        public bool IsSpeedMin()
        {
            return stepSpace >= 1000;
        }

        #endregion

        #region 设置游戏数据

        private Rectangle GetBlockArea(int index)
        {
            var i = index % Game.Width;
            var j = index / Game.Width;
            var sx = i * unit + offset.X + 1;
            var sy = j * unit + offset.Y + 1;
            return new Rectangle(sx, sy, unit - 2, unit - 2);
        }

        private Rectangle GetBlockAreaWidthBorder(int index)
        {
            var i = index % Game.Width;
            var j = index / Game.Width;
            var sx = i * unit + offset.X + 1;
            var sy = j * unit + offset.Y + 1;
            return new Rectangle(sx - 2, sy - 2, unit + 2, unit + 2);
        }

        public void RefreshBlock(int index, bool withBorder = false)
        {
            var rect = withBorder ? GetBlockAreaWidthBorder(index) : GetBlockArea(index);

            this.Invalidate(rect);
        }

        public string ValidData()
        {
            if (Game.StartPoint < 0)
            {
                return "请选择起点。";
            }
            var cntTeleporter = Game.Blocks.Count(x => x.IsTeleporter);
            if (cntTeleporter == 1 || cntTeleporter > 2)
            {
                return "如果使用传送点，则传送点数目必须是两个。";
            }
            if (Game.GetCornerCount() >= 2)
            {
                return "游戏地图的死角数目不能多于一个。";
            }
            if (!Game.CheckRestricts())
            {
                return "不能限制从地图外面或障碍进入块。";
            }
            if (!Game.IsUnPassInOneArea())
            {
                return "游戏地图的未开拓区域不是一个连续区域。";
            }
            return string.Empty;
        }

        private void CancelStartPoint(int index)
        {
            if (index == Game.StartPoint)
            {
                Game.StartPoint = -1;
                Game.CurrentPoint = -1;
                Game.Get(index).IsPass = false;
            }
        }

        public void SetStartPoint(bool isChecked)
        {
            if (SelectedIndex >= 0 && SelectedIndex < Game.Width * Game.Height)
            {
                var lastStart = Game.StartPoint;
                if (!isChecked)
                {
                    Game.StartPoint = -1;
                    Game.CurrentPoint = -1;
                }
                else
                {
                    Game.StartPoint = SelectedIndex;
                    Game.CurrentPoint = SelectedIndex;
                    Game.Get(SelectedIndex).IsPass = true;
                    Game.Get(SelectedIndex).IsWall = false;
                    Game.Get(SelectedIndex).IsTeleporter = false;
                }
                if (lastStart >= 0 && lastStart < Game.Width * Game.Height)
                {
                    Game.Get(lastStart).IsPass = false;
                    this.RefreshBlock(lastStart, true);
                }
                this.RefreshBlock(SelectedIndex, true);
            }
        }

        public bool IsSelectStartPoint()
        {
            return Game.Get(SelectedIndex).Index == Game.StartPoint;
        }

        public void SetWall()
        {
            if (SelectedIndex >= 0 && SelectedIndex < Game.Width * Game.Height)
            {
                var block = Game.Get(SelectedIndex);
                if (block.BySwitch != -1 && block.IsWall || block.Switch != -1 && !block.IsWall)
                {
                    return;
                }
                block.IsWall = !block.IsWall;
                block.IsTeleporter = false;
                CancelStartPoint(SelectedIndex);
                this.RefreshBlock(SelectedIndex, true);
            }
        }

        public bool IsSelectWall()
        {
            return Game.Get(SelectedIndex).IsWall;
        }

        public void SetTeleporter()
        {
            if (SelectedIndex >= 0 && SelectedIndex < Game.Width * Game.Height)
            {
                var block = Game.Get(SelectedIndex);
                block.IsTeleporter = !block.IsTeleporter;
                block.IsWall = false;
                CancelStartPoint(SelectedIndex);
                this.RefreshBlock(SelectedIndex, true);
            }
        }

        public bool IsSelectTeleporter()
        {
            return Game.Get(SelectedIndex).IsTeleporter;
        }

        public void SetSwitch(int index)
        {
            if (SelectedIndex >= 0 && SelectedIndex < Game.Width * Game.Height)
            {
                if (index == -1)
                {
                    var block = Game.Get(SelectedIndex);
                    if (block.Switch != -1)
                    {
                        Game.Get(block.Switch).BySwitch = -1;
                        this.RefreshBlock(block.Switch, true);
                        block.Switch = -1;
                    }
                    else if (block.BySwitch != -1)
                    {
                        Game.Get(block.BySwitch).Switch = -1;
                        this.RefreshBlock(block.BySwitch, true);
                        block.BySwitch = -1;
                    }
                }
                else
                {
                    var block = Game.Get(index);
                    block.Switch = SelectedIndex;
                    block.IsWall = false;
                    CancelStartPoint(SelectedIndex);
                    var sw = Game.Get(SelectedIndex);
                    sw.BySwitch = index;
                    sw.IsWall = true;

                    this.RefreshBlock(index, true);
                }
                this.RefreshBlock(SelectedIndex, true);
            }
        }

        public bool IsSelectSwitch()
        {
            var block = Game.Get(SelectedIndex);
            return block.Switch >= 0 || block.BySwitch >= 0;
        }

        public void SetFromDirection(Direction direction)
        {
            if (SelectedIndex >= 0 && SelectedIndex < Game.Width * Game.Height)
            {
                var block = Game.Get(SelectedIndex);
                block.RestrictFrom = direction;
                block.IsWall = false;
                CancelStartPoint(SelectedIndex);
                this.RefreshBlock(SelectedIndex, true);

                if (direction == Direction.Left)
                {
                    Game.Get(SelectedIndex - 1).RestrictTo = Direction.Right;
                }
                else if (direction == Direction.Up)
                {
                    Game.Get(SelectedIndex - GameWidth).RestrictTo = Direction.Down;
                }
                else if (direction == Direction.Right)
                {
                    Game.Get(SelectedIndex + 1).RestrictTo = Direction.Left;
                }
                else if (direction == Direction.Down)
                {
                    Game.Get(SelectedIndex + GameWidth).RestrictTo = Direction.Up;
                }
            }
        }

        public Direction SelectRestrictFrom()
        {
            return Game.Get(SelectedIndex).RestrictFrom;
        }

        public bool IsSelectLeftBorder()
        {
            return SelectedIndex % GameWidth == 0;
        }

        public bool IsSelectTopBorder()
        {
            return SelectedIndex < GameWidth;
        }

        public bool IsSelectRightBorder()
        {
            return (SelectedIndex + 1) % GameWidth == 0;
        }

        public bool IsSelectBottomBorder()
        {
            return SelectedIndex + GameWidth >= GameWidth * GameHeight;
        }

        public bool IsSelectPath()
        {
            return Game.Get(SelectedIndex).IsPass;
        }

        public bool HasWall()
        {
            return Game.Blocks.Any(x => x.IsWall);
        }

        #endregion

        #region 游戏绘制

        #region 外观设置

        [Browsable(true), DefaultValue(typeof(Color), "DarkBlue"), Category("颜色"), Description("路径颜色")]
        public Color PathColor { get; set; }
        [Browsable(true), DefaultValue(typeof(Color), "LightGray"), Category("颜色"), Description("普通格子颜色")]
        public Color NormalBlockColor { get; set; }
        [Browsable(true), DefaultValue(typeof(Color), "LightSeaGreen"), Category("颜色"), Description("起始格子颜色")]
        public Color StartBlockColor { get; set; }
        [Browsable(true), DefaultValue(typeof(Color), "Red"), Category("颜色"), Description("障碍格子颜色")]
        public Color WallBlockColor { get; set; }
        [Browsable(true), DefaultValue(typeof(Color), "SkyBlue"), Category("颜色"), Description("已经过格子颜色")]
        public Color PathBlockColor { get; set; }
        [Browsable(true), DefaultValue(typeof(Color), "Blue"), Category("颜色"), Description("限制方向格子颜色")]
        public Color FromColor { get; set; }

        public int GameWidth
        {
            get { return Game.Width; }
        }

        public int GameHeight
        {
            get { return Game.Height; }
        }

        #endregion

        private void GamePanel_Paint(object sender, PaintEventArgs e)
        {
            if (DesignMode)
            {
                InitGame(@"5*8;35;0,13,14,17,19,32;10-4;6,33");
            }
            CalcUnit();

            var gp = e.Graphics;

            var penLine = new Pen(this.BackColor, 2);
            var penPath = new Pen(PathColor) { DashStyle = DashStyle.Dot };
            var brushNormal = new SolidBrush(NormalBlockColor);
            var brushStart = new SolidBrush(StartBlockColor);
            var brushWall = new SolidBrush(WallBlockColor);
            var brushPath = new SolidBrush(PathBlockColor);
            var brushFrom = new SolidBrush(FromColor);

            for (int i = 0; i <= Game.Width; i++)
            {
                gp.DrawLine(penLine, offset.X + i * unit, offset.Y, offset.X + i * unit, offset.Y + gHeight);
            }
            for (int i = 0; i <= Game.Height; i++)
            {
                gp.DrawLine(penLine, offset.X, offset.Y + i * unit, offset.X + gWidth, offset.Y + i * unit);
            }

            foreach (var block in Game.Blocks)
            {
                var rect = GetBlockArea(block.Index);
                if (block.IsWall)
                {
                    gp.FillRectangle(brushWall, rect);
                }
                else if (block.Index == Game.StartPoint)
                {
                    gp.FillRectangle(brushStart, rect);
                }
                else if (block.IsPass)
                {
                    gp.FillRectangle(brushPath, rect);
                }
                else
                {
                    gp.FillRectangle(brushNormal, rect);
                }
                if (block.Index == SelectedIndex && IsDesignMode)
                {
                    var penSelect = new Pen(Color.Blue) { DashStyle = DashStyle.Dash };
                    gp.DrawRectangle(penSelect, rect.X - 1, rect.Y - 1, rect.Width + 1, rect.Width + 1);
                }

                if (block.IsTeleporter)
                {
                    var cx = rect.X + unit / 4 - 1;
                    var cy = rect.Y + unit / 4 - 1;
                    for (int k = unit / 4, d = 255; k > 0; k -= 2, d -= 20)
                    {
                        Color color = Color.FromArgb(d, 0, d);
                        gp.DrawEllipse(new Pen(color), cx + k, cy + k, unit / 2 - 1 - k * 2, unit / 2 - 1 - k * 2);
                    }
                }
            }

            foreach (var block in Game.Blocks.Where(x => x.RestrictFrom != Direction.None))
            {
                var index = block.Index;
                var i = index % Game.Width;
                var j = index / Game.Width;
                var sx = i * unit + offset.X + 1;
                var sy = j * unit + offset.Y + 1;
                var cx = sx + unit / 2 - 1;
                var cy = sy + unit / 2 - 1;
                var direction = block.RestrictFrom;
                var points = new Point[3];
                if (direction == Direction.Right)
                {
                    points[0] = new Point(sx + unit - 2, cy - 5);
                    points[1] = new Point(sx + unit - 2, cy + 5);
                    points[2] = new Point(sx + unit - 7, cy);
                }
                else if (direction == Direction.Down)
                {
                    points[0] = new Point(cx - 5, sy + unit - 2);
                    points[1] = new Point(cx + 5, sy + unit - 2);
                    points[2] = new Point(cx, sy + unit - 8);
                }
                else if (direction == Direction.Left)
                {
                    points[0] = new Point(sx, cy - 5);
                    points[1] = new Point(sx, cy + 5);
                    points[2] = new Point(sx + 5, cy);
                }
                else if (direction == Direction.Up)
                {
                    points[0] = new Point(cx - 4, sy);
                    points[1] = new Point(cx + 5, sy);
                    points[2] = new Point(cx, sy + 5);
                }
                gp.FillPolygon(brushFrom, points);
            }

            foreach (var block in Game.Blocks.Where(x => x.IsPass))
            {
                var index = block.Index;
                var i = index % Game.Width;
                var j = index / Game.Width;
                var sx = i * unit + offset.X + 1;
                var sy = j * unit + offset.Y + 1;
                DrawPath(block.FromDirection, gp, brushPath, sx, sy, unit, penPath);
            }

            if (IsDesignMode)
            {
                char ch = 'A';
                foreach (var block in Game.Blocks.Where(x => x.Switch >= 0))
                {
                    var str = ch.ToString();
                    SizeF size = gp.MeasureString(str, this.Font);
                    var strOffset = new Point((unit - (int)size.Width) / 2, (int)(unit - size.Height) / 2);
                    var rect = GetBlockArea(block.Index);
                    gp.DrawString(str, this.Font, brushWall, rect.X + strOffset.X, rect.Y + strOffset.Y);
                    var rect2 = GetBlockArea(block.Switch);
                    gp.DrawString(str, this.Font, Game.Get(block.Switch).IsWall ? brushNormal : brushWall, rect2.X + strOffset.X, rect2.Y + strOffset.Y);
                    ch++;
                }
            }
        }

        private static void DrawPath(Direction direction, Graphics gp, Brush brushPass, int sx, int sy, int unit, Pen penPath)
        {
            var cx = sx + unit / 2 - 1;
            var cy = sy + unit / 2 - 1;
            if (direction == Direction.Right)
            {
                gp.FillRectangle(brushPass, sx + unit - 2, sy, 2, unit - 2);
                gp.DrawLine(penPath, cx, cy, cx + unit, cy);
            }
            else if (direction == Direction.Down)
            {
                gp.FillRectangle(brushPass, sx, sy + unit - 2, unit - 2, 2);
                gp.DrawLine(penPath, cx, cy, cx, cy + unit);
            }
            else if (direction == Direction.Left)
            {
                gp.FillRectangle(brushPass, sx - 2, sy, 2, unit - 2);
                gp.DrawLine(penPath, cx, cy, cx - unit - 1, cy);
            }
            else if (direction == Direction.Up)
            {
                gp.FillRectangle(brushPass, sx, sy - 2, unit - 2, 2);
                gp.DrawLine(penPath, cx, cy, cx, cy - unit - 1);
            }
        }

        #endregion

        //public void SelectWall(GamePanel from)
        //{
        //    this.Dock = DockStyle.Fill;

        //    InitEmptyGame(from.GameWidth, from.GameHeight);
        //    from.Game.Blocks
        //        .Where(x => x.IsWall)
        //        .Select(x => x.Index)
        //        .ToList()
        //        .ForEach(x => Game.Get(x).IsWall = true);

        //    this.OnSelected = (o, e) =>
        //    {
        //        if (e.Button == MouseButtons.Left)
        //        {
        //            if (Game.Get(this.SelectedIndex).IsWall)
        //            {
        //                from.SetSwitch(this.SelectedIndex);
        //                this.Visible = false;
        //            }
        //        }
        //    };
        //    this.Visible = true;
        //}
    }
}
