using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dialog_Builder_2
{
    class VariableDialog : Form
    {
        private ComboBox cbType;

        private TextBox tbName;

        private TextBox tbValue;

        private NumericUpDown nudValue;

        private Button btOk;

        private Button btCancel;

        public string Variable { get; private set; }

        public VariableDialog()
        {
            this.Size = new System.Drawing.Size(420, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            cbType = new ComboBox
            {
                Left = 30,
                Top = 100,
                Width = 100,
                Parent = this
            };
            cbType.Items.Add("String");
            cbType.Items.Add("Number");
            cbType.SelectedIndex = 0;
            cbType.SelectedIndexChanged += (s, e) =>
            {
                if (cbType.SelectedIndex == 0)
                {
                    nudValue.Visible = false;
                    tbValue.Visible = true;

                    try
                    {
                        tbValue.Text = nudValue.Value.ToString();
                    }
                    catch
                    {
                        tbValue.Text = "";
                    }
                }
                else if (cbType.SelectedIndex == 1)
                {
                    tbValue.Visible = false;
                    nudValue.Visible = true;

                    try
                    {
                        nudValue.Value = Convert.ToDecimal(tbValue.Text);
                    }
                    catch
                    {
                        nudValue.Value = 0;
                    }
                }
            };

            tbName = new TextBox
            {
                Left = 160,
                Top = 100,
                Width = 100,
                Parent = this
            };

            tbValue = new TextBox
            {
                Left = 290,
                Top = 100,
                Width = 100,
                Parent = this
            };

            nudValue = new NumericUpDown
            {
                Left = 290,
                Top = 100,
                Width = 100,
                DecimalPlaces = 2,
                Visible = false,
                Parent = this
            };

            btCancel = new Button
            {
                AutoSize = true,
                Text = "Cancel",
                Cursor = Cursors.Hand,
                Top = 200,
                Left = this.Width / 2 + 20,
                DialogResult = DialogResult.Cancel,
                Parent = this
            };

            btOk = new Button
            {
                AutoSize = true,
                Text = "Ok",
                Cursor = Cursors.Hand,
                Top = 200,
                Parent = this
            };
            btOk.Left = this.Width / 2 - btOk.Width - 20;
            btOk.Click += (s, e) =>
            {
                if (tbName.Text == "")
                {
                    MessageBox.Show("Variable name cannot be empty", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cbType.SelectedIndex == 0)
                {
                    Variable = cbType.Text.ToLower() + ":" + tbName.Text + "=" + tbValue;
                }
                else
                {
                    Variable = cbType.Text.ToLower() + ":" + tbName.Text + "=" + nudValue.Value.ToString();
                }

                this.DialogResult = DialogResult.OK;

            };

        }
    }
}
