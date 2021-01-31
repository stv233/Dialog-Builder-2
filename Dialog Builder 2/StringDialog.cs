using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dialog_Builder_2
{
    class StringDialog : Form
    {
        private TextBox tbString = new TextBox();
        private Button btOk = new Button();

        public string String
        {
            get
            {
                return tbString.Text;
            }
            set
            {
                tbString.Text = value;
            }
        }

        public bool CanBeEmpty { get; set; }

        public StringDialog()
        {
            this.Size = new System.Drawing.Size(300, 150);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.BackColor = frMain.ActiveForm.BackColor;
            this.Icon = Properties.Resources.ico1;

            tbString = new TextBox
            {
                BackColor = this.BackColor,
                Width = this.ClientSize.Width - 10,
                Left = 5,
                Parent = this
            };
            tbString.Top = this.ClientSize.Height / 3 - tbString.Height / 2;

            btOk = new Button
            {
                Text = "Ok",
                FlatStyle = FlatStyle.Popup,
                Cursor = Cursors.Hand,
                Top = tbString.Top + tbString.Height + 20,
                Parent = this
            };
            btOk.Left = this.Width / 2 - btOk.Width / 2;
            btOk.Click += (s, e) =>
            {
                if ((!CanBeEmpty) && (String == ""))
                {
                    MessageBox.Show("The string cannot be empty.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                this.DialogResult = DialogResult.OK;
            };

            this.Load += (s, e) =>
            {
                tbString.ForeColor = this.ForeColor;
                btOk.ForeColor = this.ForeColor;
            };
        }
    }
}
