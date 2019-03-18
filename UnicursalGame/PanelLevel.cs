using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Unicursal.Game
{
    class PanelLevel : Panel
    {
        public PanelLevel()
        {
            CurrentLevel = 1;

            Init();
        }

        private Dictionary<int, Label> buttons = new Dictionary<int, Label>();

        private List<string> gameDatas = new List<string>();
        public int CurrentLevel { get; private set; }

        public Action<int, string> ChooseLevel;

        private void Init()
        {
            ReadData();
            LoadUserInfo();
        }

        private void ReadData()
        {
            try
            {
                if (File.Exists("data.txt"))
                {
                    gameDatas.Clear();
                    using (var fs = File.Open("data.txt", FileMode.Open))
                    {
                        using (var sw = new StreamReader(fs))
                        {
                            var line = sw.ReadLine();
                            while (!string.IsNullOrWhiteSpace(line))
                            {
                                gameDatas.Add(line.Trim());
                                line = sw.ReadLine();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, @"出现错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public string GetData(int index)
        {
            if (gameDatas.Count >= index)
            {
                return gameDatas[index - 1];
            }
            return string.Empty;
        }

        public void FinishGame(int level)
        {
            if (level == CurrentLevel && level <= gameDatas.Count)
            {
                var current = buttons[CurrentLevel];
                current.ForeColor = Color.Green;

                CurrentLevel++;

                var next = buttons[CurrentLevel];
                next.Enabled = true;
                next.BackColor = Color.BurlyWood;
                next.ForeColor = Color.Red;

                try
                {
                    using (var fs = File.Open("userdata.txt", FileMode.OpenOrCreate))
                    {
                        using (var sw = new StreamWriter(fs))
                        {
                            sw.WriteLine("currentlevel={0}", CurrentLevel);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, @"出现错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadUserInfo()
        {
            try
            {
                if (File.Exists("data.txt"))
                {
                    using (var fs = File.Open("userdata.txt", FileMode.Open))
                    {
                        using (var sw = new StreamReader(fs))
                        {
                            var line = sw.ReadLine();
                            while (!string.IsNullOrWhiteSpace(line))
                            {
                                if (line.Trim().StartsWith("currentlevel="))
                                {
                                    var lv = line.Trim().Replace("currentlevel=", string.Empty);
                                    CurrentLevel = int.Parse(lv);
                                }
                                line = sw.ReadLine();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, @"出现错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void InitLevel()
        {
            Font font = new Font(@"微软雅黑", 12, FontStyle.Bold);

            buttons.Clear();
            var rd = new Random();
            var padding = 15;
            var len = 40;
            var left = (this.Width - len) / 2 - 10;
            var top = padding;
            var dir = 0;//0下，1左，2右
            var wcnt = (this.Width - padding * 2) / len - 1;
            var step = rd.Next(wcnt) + 1;
            for (int i = 1; i <= gameDatas.Count; i++)
            {
                var lbl = new Label
                {
                    Text = i.ToString(),
                    Width = len,
                    Height = len,
                    BorderStyle = BorderStyle.Fixed3D,
                    Enabled = i <= CurrentLevel,
                    Left = left,
                    Top = top,
                    BackColor = i <= CurrentLevel ? Color.BurlyWood : SystemColors.Control,
                    Font = font,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = i == CurrentLevel ? Color.Red : Color.Green,
                };
                lbl.Click += Label_Click;

                this.Controls.Add(lbl);
                buttons.Add(i, lbl);

                if (i == gameDatas.Count)
                {
                    var lblEnd = new Label
                    {
                        Left = left,
                        Top = top + len,
                        Text = string.Empty,
                        Height = padding,
                        Width = len
                    };
                    this.Controls.Add(lblEnd);

                    this.Height = top + len;
                    break;
                }

                step--;
                if (dir == 0)
                {
                    top += len;
                }
                else if (dir == 1)
                {
                    left -= len;
                }
                else if (dir == 2)
                {
                    left += len;
                }
                while (step == 0)
                {
                    step = rd.Next(wcnt) + 1;
                    if (dir != 0)
                    {
                        dir = 0;
                        step = step / 2 + 1;
                    }
                    else
                    {
                        var l = step * len + padding;
                        dir = l > left ? 2 : 1;
                        step = Math.Abs(l - left) / len;
                        if (dir == 1)
                            step++;
                    }
                }
            }
        }

        public int CurrentOffsetHeight()
        {
            var lbl = buttons[CurrentLevel];
            var top = lbl.Top;
            return Math.Max(0, top - lbl.Height);
        }

        private void Label_Click(object sender, EventArgs e)
        {
            if (ChooseLevel == null)
                return;
            var lbl = sender as Label;
            var index = int.Parse(lbl.Text);
            var data = gameDatas[index - 1];
            ChooseLevel(index, data);
        }
    }
}
