using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dialog_Builder_2
{
    class ConditionDialog : Form
    {
        private Label lbCondition = new Label();
        private Label lbName = new Label();
        private Label lbSign = new Label();
        private Label lbValue = new Label();

        private ComboBox cbCondition = new ComboBox();
        private TextBox tbName = new TextBox();
        private ComboBox cbSign = new ComboBox();
        private TextBox tbValue = new TextBox();

        private Button btOk = new Button();
        private Button btCancel = new Button();

        public string Condition { get; private set; }

        public ConditionDialog()
        {
            this.Size = new System.Drawing.Size(550, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.BackColor = frMain.ActiveForm.BackColor;
            this.Icon = Properties.Resources.ico1;

            cbCondition = new ComboBox
            {
                FlatStyle = FlatStyle.Popup,
                BackColor = this.BackColor,
                Width = 100,
                Top = 100,
                Left = 30,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Parent = this
            };
            cbCondition.Items.Add("Local variable");
            cbCondition.Items.Add("Global variable");
            cbCondition.Items.Add("Inventory item [Advanced inventory system]");
            cbCondition.Items.Add("Task status [Task list]");
            cbCondition.SelectedIndex = 0;
            cbCondition.SelectedIndexChanged += (s, e) =>
            {
                if (cbCondition.SelectedIndex == cbCondition.Items.IndexOf("Task status [Task list]"))
                {
                    cbSign.SelectedIndex = cbSign.Items.IndexOf("=");
                    cbSign.Enabled = false;
                }
                else
                {
                    cbSign.Enabled = true;
                }
            };

            tbName = new TextBox
            {
                BackColor = this.BackColor,
                Width = 100,
                Top = 100,
                Left = 160,
                Parent = this
            };

            cbSign = new ComboBox
            {
                FlatStyle = FlatStyle.Popup,
                BackColor = this.BackColor,
                Width = 100,
                Top = 100,
                Left = 290,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Parent = this
            };
            cbSign.Items.Add("=");
            cbSign.Items.Add("!=");
            cbSign.Items.Add(">");
            cbSign.Items.Add("<");
            cbSign.Items.Add(">=");
            cbSign.Items.Add("<=");
            cbSign.SelectedIndex = 0;

            tbValue = new TextBox
            {
                BackColor = this.BackColor,
                Width = 100,
                Top = 100,
                Left = 420,
                Parent = this
            };

            btCancel = new Button
            {
                FlatStyle = FlatStyle.Popup,
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
                FlatStyle = FlatStyle.Popup,
                AutoSize = true,
                Text = "Ok",
                Cursor = Cursors.Hand,
                Top = 200,
                Parent = this
            };
            btOk.Left = this.Width / 2 - btOk.Width - 20;
            btOk.Click += (s, e) =>
            {
                string condition = "";

                if (cbCondition.SelectedIndex == cbCondition.Items.IndexOf("Local variable"))
                {
                    condition += "variable";
                }
                else if (cbCondition.SelectedIndex == cbCondition.Items.IndexOf("Global variable"))
                {
                    condition += "gvariable";
                }
                else if (cbCondition.SelectedIndex == cbCondition.Items.IndexOf("Inventory item [Advanced inventory system]"))
                {
                    condition += "inventoryitem";
                }
                else if (cbCondition.SelectedIndex == cbCondition.Items.IndexOf("Task status [Task list]"))
                {
                    condition += ("taskstatus");
                    cbSign.Text = "=";
                }

                condition += ":";
                condition += tbName.Text;
                condition += cbSign.Text;
                condition += tbValue.Text;

                Condition = condition;

                this.DialogResult = DialogResult.OK;

            };

            lbCondition = new Label
            {
                AutoSize = true,
                Text = "Condition type",
                Top = 75,
                Parent = this
            };
            lbCondition.Left = cbCondition.Left + cbCondition.Width / 2 - lbCondition.Width / 2;

            lbName = new Label
            {
                AutoSize = true,
                Text = "Name",
                Top = 75,
                Parent = this
            };
            lbName.Left = tbName.Left + tbName.Width / 2 - lbName.Width / 2;

            lbSign = new Label
            {
                AutoSize = true,
                Text = "Sign",
                Top = 75,
                Parent = this
            };
            lbSign.Left = cbSign.Left + cbSign.Width / 2 - lbSign.Width / 2;

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
                cbCondition.ForeColor = this.ForeColor;
                tbName.ForeColor = this.ForeColor;
                cbSign.ForeColor = this.ForeColor;
                tbValue.ForeColor = this.ForeColor;

                lbCondition.ForeColor = this.ForeColor;
                lbName.ForeColor = this.ForeColor;
                lbSign.ForeColor = this.ForeColor;
                lbValue.ForeColor = this.ForeColor;
            };
        }
    }
}
