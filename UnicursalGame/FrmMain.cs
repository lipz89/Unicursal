using System.Threading;
using System.Windows.Forms;
using Unicursal.GameCore;

namespace Unicursal.Game
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            gamePanel.InitEmptyGame(5, 6);
            gamePanel.OnSelected += gamePanel_OnSelected;
            pnlLevel.InitLevel();

            pnlLevel.MouseEnter += pnlLevel_MouseEnter;
        }

        void pnlLevel_MouseEnter(object sender, System.EventArgs e)
        {
            pnlLevel.Focus();
        }

        private void gamePanel_OnSelected(object sender, MouseEventArgs e)
        {
            if (gamePanel.Visible && gamePanel.Enabled && !gamePanel.IsFinished())
            {
                if (gamePanel.IsSelectPath())
                {
                    gamePanel.CutTo(gamePanel.SelectedIndex);
                }
                else
                {
                    gamePanel.Goto(gamePanel.SelectedIndex);
                }
                CheckFinished();
            }
        }

        private int currentLevel;

        private void FrmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (isSolving)
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        if (thSolver != null)
                        {
                            this.thSolver.Abort();
                            isSolving = false;
                            this.gamePanel.Enabled = true;
                            this.gamePanel.ReInitGame();
                            this.gamePanel.Invalidate();
                        }
                        break;
                    case Keys.Up:
                        if (isSolving)
                        {
                            this.gamePanel.SpeedUp();
                        }
                        break;
                    case Keys.Down:
                        if (isSolving)
                        {
                            this.gamePanel.SpeedDown();
                        }
                        break;
                }
            }
            else if (gamePanel.Visible && gamePanel.Enabled && !gamePanel.IsFinished())
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        gamePanel.Goto(Direction.Up);
                        break;
                    case Keys.Left:
                        gamePanel.Goto(Direction.Left);
                        break;
                    case Keys.Down:
                        gamePanel.Goto(Direction.Down);
                        break;
                    case Keys.Right:
                        gamePanel.Goto(Direction.Right);
                        break;
                    case Keys.Back:
                        gamePanel.Back();
                        break;
                    case Keys.Escape:
                        this.Text = @"连线大师";
                        gamePanel.SendToBack();
                        break;
                    case Keys.Q:
                        if (e.Control && e.Alt)
                        {
                            this.Solver();
                        }
                        break;
                }

                if (e.KeyCode != Keys.Q && e.KeyCode != Keys.Back && e.KeyCode != Keys.Escape)
                {
                    CheckFinished();
                }
            }
        }

        private void CheckFinished()
        {
            if (gamePanel.IsFinished())
            {
                this.Text = @"连线大师";
                gamePanel.SendToBack();
                pnlLevel.FinishGame(currentLevel);
            }
        }

        private void FrmMain_Shown(object sender, System.EventArgs e)
        {
            pnlLevel.VerticalScroll.Value = pnlLevel.CurrentOffsetHeight();
            pnlLevel.Visible = false;
            pnlLevel.Visible = true;
            pnlLevel.ChooseLevel = (level, data) =>
            {
                currentLevel = level;
                this.Text = @"连线大师 - " + currentLevel;
                gamePanel.BringToFront();
                gamePanel.InitGame(data);
            };
        }

        private void Solver()
        {
            if (!isSolving)
            {
                isSolving = true;
                thSolver = new Thread(() =>
                {
                    this.gamePanel.Enabled = false;
                    gamePanel.Solve(true);
                    this.gamePanel.Enabled = true;
                    isSolving = false;
                    CheckFinished();
                });
                thSolver.Start();
            }
        }

        private bool isSolving;
        private Thread thSolver;

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSolving && thSolver != null)
            {
                thSolver.Abort();
            }
        }
    }
}
