namespace Unicursal.GameCreator
{
    partial class FrmInit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nudHeight = new System.Windows.Forms.NumericUpDown();
            this.nudWidth = new System.Windows.Forms.NumericUpDown();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gpEmptyMap = new System.Windows.Forms.GroupBox();
            this.gpMapFromData = new System.Windows.Forms.GroupBox();
            this.txtData = new System.Windows.Forms.TextBox();
            this.rdoEmptyMap = new System.Windows.Forms.RadioButton();
            this.rdoMapFromData = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
            this.gpEmptyMap.SuspendLayout();
            this.gpMapFromData.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(150, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "高度";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "宽度";
            // 
            // nudHeight
            // 
            this.nudHeight.Location = new System.Drawing.Point(194, 27);
            this.nudHeight.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.nudHeight.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nudHeight.Name = "nudHeight";
            this.nudHeight.Size = new System.Drawing.Size(44, 21);
            this.nudHeight.TabIndex = 6;
            this.nudHeight.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // nudWidth
            // 
            this.nudWidth.Location = new System.Drawing.Point(66, 27);
            this.nudWidth.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.nudWidth.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nudWidth.Name = "nudWidth";
            this.nudWidth.Size = new System.Drawing.Size(44, 21);
            this.nudWidth.TabIndex = 5;
            this.nudWidth.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(60, 190);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "确定(&O)";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(199, 190);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "取消(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gpEmptyMap
            // 
            this.gpEmptyMap.Controls.Add(this.label1);
            this.gpEmptyMap.Controls.Add(this.nudWidth);
            this.gpEmptyMap.Controls.Add(this.nudHeight);
            this.gpEmptyMap.Controls.Add(this.label2);
            this.gpEmptyMap.Location = new System.Drawing.Point(36, 12);
            this.gpEmptyMap.Name = "gpEmptyMap";
            this.gpEmptyMap.Size = new System.Drawing.Size(269, 61);
            this.gpEmptyMap.TabIndex = 11;
            this.gpEmptyMap.TabStop = false;
            // 
            // gpMapFromData
            // 
            this.gpMapFromData.Controls.Add(this.txtData);
            this.gpMapFromData.Location = new System.Drawing.Point(36, 79);
            this.gpMapFromData.Name = "gpMapFromData";
            this.gpMapFromData.Size = new System.Drawing.Size(269, 101);
            this.gpMapFromData.TabIndex = 12;
            this.gpMapFromData.TabStop = false;
            // 
            // txtData
            // 
            this.txtData.Location = new System.Drawing.Point(24, 22);
            this.txtData.Multiline = true;
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(214, 68);
            this.txtData.TabIndex = 0;
            // 
            // rdoEmptyMap
            // 
            this.rdoEmptyMap.AutoSize = true;
            this.rdoEmptyMap.Location = new System.Drawing.Point(47, 13);
            this.rdoEmptyMap.Name = "rdoEmptyMap";
            this.rdoEmptyMap.Size = new System.Drawing.Size(59, 16);
            this.rdoEmptyMap.TabIndex = 13;
            this.rdoEmptyMap.Text = "空地图";
            this.rdoEmptyMap.UseVisualStyleBackColor = true;
            this.rdoEmptyMap.CheckedChanged += new System.EventHandler(this.rdoMapFromData_CheckedChanged);
            // 
            // rdoMapFromData
            // 
            this.rdoMapFromData.AutoSize = true;
            this.rdoMapFromData.Location = new System.Drawing.Point(47, 79);
            this.rdoMapFromData.Name = "rdoMapFromData";
            this.rdoMapFromData.Size = new System.Drawing.Size(71, 16);
            this.rdoMapFromData.TabIndex = 14;
            this.rdoMapFromData.Text = "数据地图";
            this.rdoMapFromData.UseVisualStyleBackColor = true;
            this.rdoMapFromData.CheckedChanged += new System.EventHandler(this.rdoMapFromData_CheckedChanged);
            // 
            // FrmInit
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(340, 226);
            this.Controls.Add(this.rdoMapFromData);
            this.Controls.Add(this.rdoEmptyMap);
            this.Controls.Add(this.gpMapFromData);
            this.Controls.Add(this.gpEmptyMap);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmInit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "新建地图";
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
            this.gpEmptyMap.ResumeLayout(false);
            this.gpEmptyMap.PerformLayout();
            this.gpMapFromData.ResumeLayout(false);
            this.gpMapFromData.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudHeight;
        private System.Windows.Forms.NumericUpDown nudWidth;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox gpEmptyMap;
        private System.Windows.Forms.GroupBox gpMapFromData;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.RadioButton rdoEmptyMap;
        private System.Windows.Forms.RadioButton rdoMapFromData;
    }
}