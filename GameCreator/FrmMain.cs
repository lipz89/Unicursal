using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Unicursal.GameCore;
using System.Linq;

namespace Unicursal.GameCreator
{
    public partial class FrmMain : Form
    {
        private int waitSwitchIndex = -1;
        public FrmMain(string data = null)
        {
            InitializeComponent();

            this.tsbSpeedUp.Enabled = false;
            this.tsbSpeedDown.Enabled = false;
            this.tsbCancelSolve.Enabled = false;

            CheckForIllegalCrossThreadCalls = false;
            if (!string.IsNullOrWhiteSpace(data))
            {
                gamePanel.InitGame(data);
            }
            else
            {
                gamePanel.InitEmptyGame(6, 8);
            }
            gamePanel.OnSelected += gamePanel_OnSelected;
            cmsMain.Items.OfType<ToolStripMenuItem>().ToList().ForEach(x => x.Click += tsmi_Click);
        }

        private void gamePanel_OnSelected(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                if (waitSwitchIndex > -1)
                {
                    gamePanel.SetSwitch(waitSwitchIndex);
                    waitSwitchIndex = -1;
                }
                return;
            }

            var dir = gamePanel.SelectRestrictFrom();
            tsmiStartPoint.Checked = gamePanel.IsSelectStartPoint();
            tsmiWall.Checked = gamePanel.IsSelectWall();
            tsmiTeleporter.Checked = gamePanel.IsSelectTeleporter();
            tsmiSwitch.Checked = gamePanel.IsSelectSwitch();

            tsmiFromLeft.Checked = dir == Direction.Left;
            tsmiFromUp.Checked = dir == Direction.Up;
            tsmiFromRight.Checked = dir == Direction.Right;
            tsmiFromDown.Checked = dir == Direction.Down;

            tsmiWall.Enabled = !gamePanel.IsSelectSwitch();

            tsmiFromLeft.Enabled = !gamePanel.IsSelectLeftBorder();
            tsmiFromUp.Enabled = !gamePanel.IsSelectTopBorder();
            tsmiFromRight.Enabled = !gamePanel.IsSelectRightBorder();
            tsmiFromDown.Enabled = !gamePanel.IsSelectBottomBorder();

            cmsMain.Show(gamePanel, e.Location);
        }

        private void tsmi_Click(object sender, EventArgs e)
        {
            if (sender == tsmiStartPoint)
            {
                gamePanel.SetStartPoint(!tsmiStartPoint.Checked);
            }
            else if (sender == tsmiWall)
            {
                gamePanel.SetWall();
            }
            else if (sender == tsmiTeleporter)
            {
                gamePanel.SetTeleporter();
            }
            else if (sender == tsmiSwitch)
            {
                if (tsmiSwitch.Checked)
                {
                    gamePanel.SetSwitch(-1);
                }
                else
                {
                    waitSwitchIndex = gamePanel.SelectedIndex;
                }
            }
            else if (sender == tsmiFromLeft)
            {
                gamePanel.SetFromDirection(tsmiFromLeft.Checked ? Direction.None : Direction.Left);
            }
            else if (sender == tsmiFromUp)
            {
                gamePanel.SetFromDirection(tsmiFromUp.Checked ? Direction.None : Direction.Up);
            }
            else if (sender == tsmiFromRight)
            {
                gamePanel.SetFromDirection(tsmiFromRight.Checked ? Direction.None : Direction.Right);
            }
            else if (sender == tsmiFromDown)
            {
                gamePanel.SetFromDirection(tsmiFromDown.Checked ? Direction.None : Direction.Down);
            }
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            var dlg = new FrmInit(gamePanel.SaveData());
            var dlgRtn = dlg.ShowDialog();
            if (dlgRtn == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(dlg.MapData))
                {
                    var size = dlg.TheSize;
                    gamePanel.InitEmptyGame(size.Width, size.Height);
                }
                else
                {
                    gamePanel.InitGame(dlg.MapData);
                }
                gamePanel.Enabled = true;
                gamePanel.Invalidate();
            }
        }

        private bool ValidMap()
        {
            gamePanel.ReInitGame();
            var valid = gamePanel.ValidData();
            if (!string.IsNullOrWhiteSpace(valid))
            {
                MessageBox.Show(this, valid, @"提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool isSolving;
        private Thread thSolver;
        private void tsbSolve_Click(object sender, EventArgs e)
        {
            if (!isSolving && ValidMap())
            {
                AutoSolve(true);
            }
        }

        private void tsbQuickSolve_Click(object sender, EventArgs e)
        {
            if (!isSolving && ValidMap())
            {
                AutoSolve();
            }
        }

        private void tsbCancelSolve_Click(object sender, EventArgs e)
        {
            if (isSolving)
            {
                AbortSlove();
            }
        }

        private void AbortSlove()
        {
            if (isSolving)
            {
                if (thSolver != null)
                {
                    thSolver.Abort();
                    AfterSoler();
                    isSolving = false;
                }
            }
        }

        private void BeforeSolve()
        {
            this.gamePanel.Enabled = false;
            this.tsbNew.Enabled = false;
            this.tsbSolve.Enabled = false;
            this.tsbQuickSolve.Enabled = false;
            this.tsbCancelSolve.Enabled = true;
            this.Text += @" - 自动求解中...";
        }

        private void AfterSoler()
        {
            isSolving = false;
            this.Text = this.Text.Replace(@" - 自动求解中...", string.Empty);
            this.tsbNew.Enabled = true;
            this.tsbSolve.Enabled = true;
            this.tsbQuickSolve.Enabled = true;
            this.tsbCancelSolve.Enabled = false;
            tsbSpeedUp.Enabled = false;
            tsbSpeedDown.Enabled = false;
        }

        private void AutoSolve(bool show = false)
        {
            isSolving = true;
            thSolver = new Thread(() =>
            {
                BeforeSolve();

                this.tsbSpeedUp.Enabled = show && !gamePanel.IsSpeedMax();
                this.tsbSpeedDown.Enabled = show && !gamePanel.IsSpeedMin();
                var lst = gamePanel.Solve(show);
                AfterSoler();
                if (!lst.Any())
                {
                    MessageBox.Show(this, @"很抱歉，该地图无解。", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    gamePanel.ReInitGame();
                    foreach (var i in lst)
                    {
                        gamePanel.Goto(i);
                    }
                    this.Activate();
                    //string path = string.Join(",", lst);
                    //MessageBox.Show(this, string.Format("恭喜，该地图被解出，路线索引如下：\r\n{0}", path), @"提示",
                    //    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            });
            thSolver.Start();
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (ValidMap())
            {
                var data = gamePanel.SaveData();
                if (Save(data))
                {
                    MessageBox.Show(this, @"保存成功。", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private bool Save(string data)
        {
            try
            {
                using (var fs = File.Open("data.txt", FileMode.Append))
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(data);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, @"出现错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            AbortSlove();
        }

        private void tsbSpeedUp_Click(object sender, EventArgs e)
        {
            gamePanel.SpeedUp();
            tsbSpeedUp.Enabled = !gamePanel.IsSpeedMax();
            tsbSpeedDown.Enabled = !gamePanel.IsSpeedMin();
        }

        private void tsbSpeedDown_Click(object sender, EventArgs e)
        {
            gamePanel.SpeedDown();
            tsbSpeedUp.Enabled = !gamePanel.IsSpeedMax();
            tsbSpeedDown.Enabled = !gamePanel.IsSpeedMin();
        }

        private void FrmMain_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up: tsbSpeedUp.PerformClick(); break;
                case Keys.Down: tsbSpeedDown.PerformClick(); break;
                case Keys.Escape: tsbCancelSolve.PerformClick(); break;
            }
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.N: tsbNew.PerformClick(); break;
                    case Keys.S: tsbSave.PerformClick(); break;
                    case Keys.Q: tsbQuickSolve.PerformClick(); break;
                    case Keys.A: tsbSolve.PerformClick(); break;
                }
            }
        }
    }
}
