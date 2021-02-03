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
        private Label lbType = new Label();
        private Label lbName = new Label();
        private Label lbValue = new Label();

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
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.BackColor = frMain.ActiveForm.BackColor;
            this.Icon = Properties.Resources.ico1;

            cbType = new ComboBox
            {
                BackColor = this.BackColor,
                FlatStyle = FlatStyle.Popup,
                Left = 30,
                Top = 100,
                Width = 100,
                DropDownStyle = ComboBoxStyle.DropDownList,
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
                BackColor = this.BackColor,
                Left = 160,
                Top = 100,
                Width = 100,
                Parent = this
            };

            tbValue = new TextBox
            {
                BackColor = this.BackColor,
                Left = 290,
                Top = 100,
                Width = 100,
                Parent = this
            };

            nudValue = new NumericUpDown
            {
                BackColor = this.BackColor,
                Left = 290,
                Top = 100,
                Width = 100,
                DecimalPlaces = 2,
                Maximum = 99999999999,
                Minimum = -99999999999,
                Visible = false,
                Parent = this
            };

            btCancel = new Button
            {
                FlatStyle = FlatStyle.Popup,
                AutoSize = true,
                Text = "Cancel",
                Cursor = Cursors.Hand,
                Top = 200,
                Left = tbName.Left + tbName.Width,
                DialogResult = DialogResult.Cancel,
                Parent = this
            };

            btOk = new Button
            {
                FlatStyle = FlatStyle.Popup,
                AutoSize = true,
                Text = "Ok",
                Cursor = Cursors.Hand,
                Top = 200,
                Parent = this
            };
            btOk.Left = tbName.Left - btOk.Width;
            btOk.Click += (s, e) =>
            {
                if (tbName.Text == "")
                {
                    MessageBox.Show("Variable name cannot be empty.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if ((tbName.Text.Contains("[") 
                    || tbName.Text.Contains("]")
                    || tbName.Text.Contains(" ") 
                    || tbName.Text.Contains("!") 
                    || tbName.Text.Contains("+") 
                    || tbName.Text.Contains("-") 
                    || tbName.Text.Contains("=") 
                    || tbName.Text.Contains(">") 
                    || tbName.Text.Contains("<")))
                {
                    MessageBox.Show("Variable name contains invalid characters.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cbType.SelectedIndex == 0)
                {
                    Variable = cbType.Text.ToLower() + ":" + tbName.Text + "=" + tbValue.Text;
                }
                else
                {
                    Variable = cbType.Text.ToLower() + ":" + tbName.Text + "=" + nudValue.Value.ToString();
                }

                this.DialogResult = DialogResult.OK;

            };

            lbType = new Label
            {
                AutoSize = true,
                Text = "Variable type",
                Top = 75,
                Parent = this
            };
            lbType.Left = cbType.Left + cbType.Width / 2 - lbType.Width / 2;

            lbName = new Label
            {
                AutoSize = true,
                Text = "Name",
                Top = 75,
                Parent = this
            };
            lbName.Left = tbName.Left + tbName.Width / 2 - lbName.Width / 2;

            lbValue = new Label
            {
                AutoSize = true,
                Text = "Value",
                Top = 75,
                Parent = this
            };
            lbValue.Left = tbValue.Left + tbValue.Width / 2 - lbValue.Width / 2;

            this.Load += (s, e) =>
            {
                cbType.ForeColor = this.ForeColor;
                tbName.ForeColor = this.ForeColor;
                tbValue.ForeColor = this.ForeColor;
                nudValue.ForeColor = this.ForeColor;
            };

        }

    }
}
