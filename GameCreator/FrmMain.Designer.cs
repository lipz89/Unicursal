namespace Unicursal.GameCreator
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbNew = new System.Windows.Forms.ToolStripButton();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbQuickSolve = new System.Windows.Forms.ToolStripButton();
            this.tsbSolve = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSpeedUp = new System.Windows.Forms.ToolStripButton();
            this.tsbSpeedDown = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbCancelSolve = new System.Windows.Forms.ToolStripButton();
            this.cmsMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiStartPoint = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiWall = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTeleporter = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSwitch = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiFromLeft = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFromUp = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFromRight = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFromDown = new System.Windows.Forms.ToolStripMenuItem();
            this.gamePanel = new Unicursal.GameCore.GamePanel();
            this.toolStrip1.SuspendLayout();
            this.cmsMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNew,
            this.tsbSave,
            this.toolStripSeparator2,
            this.tsbQuickSolve,
            this.tsbSolve,
            this.toolStripSeparator3,
            this.tsbSpeedUp,
            this.tsbSpeedDown,
            this.toolStripSeparator4,
            this.tsbCancelSolve});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(330, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbNew
            // 
            this.tsbNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNew.Image = global::Unicursal.GameCreator.ToolImages.New;
            this.tsbNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNew.Name = "tsbNew";
            this.tsbNew.Size = new System.Drawing.Size(23, 22);
            this.tsbNew.Text = "新建地图";
            this.tsbNew.ToolTipText = "新建地图 (CTRL+N)";
            this.tsbNew.Click += new System.EventHandler(this.tsbNew_Click);
            // 
            // tsbSave
            // 
            this.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSave.Image = global::Unicursal.GameCreator.ToolImages.Save;
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(23, 22);
            this.tsbSave.Text = "保存地图";
            this.tsbSave.ToolTipText = "保存地图 (CTRL+S)";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbQuickSolve
            // 
            this.tsbQuickSolve.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbQuickSolve.Image = global::Unicursal.GameCreator.ToolImages.QuickSolve;
            this.tsbQuickSolve.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbQuickSolve.Name = "tsbQuickSolve";
            this.tsbQuickSolve.Size = new System.Drawing.Size(23, 22);
            this.tsbQuickSolve.Text = "快速求解";
            this.tsbQuickSolve.ToolTipText = "快速求解 (CTRL+Q)";
            this.tsbQuickSolve.Click += new System.EventHandler(this.tsbQuickSolve_Click);
            // 
            // tsbSolve
            // 
            this.tsbSolve.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSolve.Image = global::Unicursal.GameCreator.ToolImages.Solve;
            this.tsbSolve.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSolve.Name = "tsbSolve";
            this.tsbSolve.Size = new System.Drawing.Size(23, 22);
            this.tsbSolve.Text = "可视求解";
            this.tsbSolve.ToolTipText = "可视求解 (CTRL+A)";
            this.tsbSolve.Click += new System.EventHandler(this.tsbSolve_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbSpeedUp
            // 
            this.tsbSpeedUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSpeedUp.Image = global::Unicursal.GameCreator.ToolImages.Up;
            this.tsbSpeedUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSpeedUp.Name = "tsbSpeedUp";
            this.tsbSpeedUp.Size = new System.Drawing.Size(23, 22);
            this.tsbSpeedUp.Text = "加速";
            this.tsbSpeedUp.ToolTipText = "加速 (UP)";
            this.tsbSpeedUp.Click += new System.EventHandler(this.tsbSpeedUp_Click);
            // 
            // tsbSpeedDown
            // 
            this.tsbSpeedDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSpeedDown.Image = global::Unicursal.GameCreator.ToolImages.Down;
            this.tsbSpeedDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSpeedDown.Name = "tsbSpeedDown";
            this.tsbSpeedDown.Size = new System.Drawing.Size(23, 22);
            this.tsbSpeedDown.Text = "减速";
            this.tsbSpeedDown.ToolTipText = "减速 (DOWN)";
            this.tsbSpeedDown.Click += new System.EventHandler(this.tsbSpeedDown_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbCancelSolve
            // 
            this.tsbCancelSolve.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCancelSolve.Image = global::Unicursal.GameCreator.ToolImages.Stop;
            this.tsbCancelSolve.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCancelSolve.Name = "tsbCancelSolve";
            this.tsbCancelSolve.Size = new System.Drawing.Size(23, 22);
            this.tsbCancelSolve.Text = "取消求解";
            this.tsbCancelSolve.ToolTipText = "取消求解 (ESC)";
            this.tsbCancelSolve.Click += new System.EventHandler(this.tsbCancelSolve_Click);
            // 
            // cmsMain
            // 
            this.cmsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiStartPoint,
            this.tsmiWall,
            this.tsmiTeleporter,
            this.tsmiSwitch,
            this.toolStripSeparator1,
            this.tsmiFromLeft,
            this.tsmiFromUp,
            this.tsmiFromRight,
            this.tsmiFromDown});
            this.cmsMain.Name = "cmsMain";
            this.cmsMain.Size = new System.Drawing.Size(113, 186);
            // 
            // tsmiStartPoint
            // 
            this.tsmiStartPoint.Name = "tsmiStartPoint";
            this.tsmiStartPoint.Size = new System.Drawing.Size(112, 22);
            this.tsmiStartPoint.Text = "起点";
            // 
            // tsmiWall
            // 
            this.tsmiWall.Name = "tsmiWall";
            this.tsmiWall.Size = new System.Drawing.Size(112, 22);
            this.tsmiWall.Text = "障碍";
            // 
            // tsmiTeleporter
            // 
            this.tsmiTeleporter.Name = "tsmiTeleporter";
            this.tsmiTeleporter.Size = new System.Drawing.Size(112, 22);
            this.tsmiTeleporter.Text = "传送点";
            // 
            // tsmiSwitch
            // 
            this.tsmiSwitch.Name = "tsmiSwitch";
            this.tsmiSwitch.Size = new System.Drawing.Size(112, 22);
            this.tsmiSwitch.Text = "开关";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(109, 6);
            // 
            // tsmiFromLeft
            // 
            this.tsmiFromLeft.Name = "tsmiFromLeft";
            this.tsmiFromLeft.Size = new System.Drawing.Size(112, 22);
            this.tsmiFromLeft.Text = "左进";
            // 
            // tsmiFromUp
            // 
            this.tsmiFromUp.Name = "tsmiFromUp";
            this.tsmiFromUp.Size = new System.Drawing.Size(112, 22);
            this.tsmiFromUp.Text = "上进";
            // 
            // tsmiFromRight
            // 
            this.tsmiFromRight.Name = "tsmiFromRight";
            this.tsmiFromRight.Size = new System.Drawing.Size(112, 22);
            this.tsmiFromRight.Text = "右进";
            // 
            // tsmiFromDown
            // 
            this.tsmiFromDown.Name = "tsmiFromDown";
            this.tsmiFromDown.Size = new System.Drawing.Size(112, 22);
            this.tsmiFromDown.Text = "下进";
            // 
            // gamePanel
            // 
            this.gamePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gamePanel.IsDesignMode = true;
            this.gamePanel.Location = new System.Drawing.Point(0, 25);
            this.gamePanel.Name = "gamePanel";
            this.gamePanel.Size = new System.Drawing.Size(330, 436);
            this.gamePanel.TabIndex = 0;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(255)))), ((int)(((byte)(212)))));
            this.ClientSize = new System.Drawing.Size(330, 461);
            this.Controls.Add(this.gamePanel);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "地图求解";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmMain_KeyDown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.cmsMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Unicursal.GameCore.GamePanel gamePanel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbNew;
        private System.Windows.Forms.ToolStripButton tsbSolve;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.ContextMenuStrip cmsMain;
        private System.Windows.Forms.ToolStripMenuItem tsmiStartPoint;
        private System.Windows.Forms.ToolStripMenuItem tsmiWall;
        private System.Windows.Forms.ToolStripMenuItem tsmiTeleporter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiFromLeft;
        private System.Windows.Forms.ToolStripMenuItem tsmiFromUp;
        private System.Windows.Forms.ToolStripMenuItem tsmiFromRight;
        private System.Windows.Forms.ToolStripMenuItem tsmiFromDown;
        private System.Windows.Forms.ToolStripButton tsbQuickSolve;
        private System.Windows.Forms.ToolStripButton tsbCancelSolve;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsbSpeedUp;
        private System.Windows.Forms.ToolStripButton tsbSpeedDown;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem tsmiSwitch;




    }
}

