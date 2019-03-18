using System;
using System.Drawing;
using System.Windows.Forms;

namespace Unicursal.GameCreator
{
    public partial class FrmInit : Form
    {
        public Size TheSize { get; private set; }
        public string MapData { get; private set; }

        public FrmInit(string data)
        {
            InitializeComponent();

            txtData.Text = data;
            rdoEmptyMap.Checked = true;

            var datas = data.Split(";".ToCharArray());
            var wh = datas[0].Split('*');
            nudWidth.Value = int.Parse(wh[0]);
            nudHeight.Value = int.Parse(wh[1]);

            this.TheSize = new Size((int)nudWidth.Value, (int)nudHeight.Value);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.TheSize = new Size((int)nudWidth.Value, (int)nudHeight.Value);
            if (rdoMapFromData.Checked)
            {
                this.MapData = txtData.Text.Trim();
            }
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void rdoMapFromData_CheckedChanged(object sender, EventArgs e)
        {
            gpEmptyMap.Enabled = rdoEmptyMap.Checked;
            gpMapFromData.Enabled = rdoMapFromData.Checked;
        }
    }
}
