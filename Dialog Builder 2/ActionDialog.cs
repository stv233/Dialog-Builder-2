using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dialog_Builder_2
{
    class ActionDialog : Form
    {
        public string Action { get; private set; }

        private Label lbAction = new Label();
        private Label lbName = new Label();
        private Label lbSign = new Label();
        private Label lbValue = new Label();

        private ComboBox cbAction = new ComboBox();
        private TextBox tbName = new TextBox();
        private ComboBox cbSign = new ComboBox();
        private TextBox tbValue = new TextBox();

        private Button btOk = new Button();
        private Button btCancel = new Button();

        public ActionDialog()
        {
            this.Size = new System.Drawing.Size(550, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.BackColor = frMain.ActiveForm.BackColor;

            cbAction = new ComboBox
            {
                FlatStyle = FlatStyle.Popup,
                BackColor = this.BackColor,
                Width = 100,
                Top = 100,
                Left = 30,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Parent = this
            };
            cbAction.Items.Add("Local variable");
            cbAction.Items.Add("Global variable");
            cbAction.Items.Add("Inventory item [Advanced inventory system]");
            cbAction.Items.Add("Skillpoints [Skill tree]");
            cbAction.SelectedIndex = 0;
            cbAction.SelectedIndexChanged += (s, e) =>
            {
                tbName.Visible = !(cbAction.SelectedIndex == cbAction.Items.IndexOf("Skillpoints [Skill tree]"));
                lbName.Visible = !(cbAction.SelectedIndex == cbAction.Items.IndexOf("Skillpoints [Skill tree]"));
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
            cbSign.Items.Add("+");
            cbSign.Items.Add("-");
            cbSign.Items.Add("=");
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
                string action = "";

                if (cbAction.SelectedIndex == 0)
                {
                    action += "variable";
                }
                else if (cbAction.SelectedIndex == 1)
                {
                    action += "gvariable";
                }
                else if (cbAction.SelectedIndex == 2)
                {
                    action += "inventoryitem";
                }
                else if (cbAction.SelectedIndex == 3)
                {
                    action += "skillpoint";
                    tbName.Text = "";
                }

                action += ":";
                action += tbName.Text;
                action += cbSign.Text;
                action += tbValue.Text;

                Action = action;

                this.DialogResult = DialogResult.OK;

            };

            lbAction = new Label
            {
                AutoSize = true,
                Text = "Action type",
                Top = 75,
                Parent = this
            };
            lbAction.Left = cbAction.Left + cbAction.Width / 2 - lbAction.Width / 2;

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
                cbAction.ForeColor = this.ForeColor;
                tbName.ForeColor = this.ForeColor;
                cbSign.ForeColor = this.ForeColor;
                tbValue.ForeColor = this.ForeColor;

                lbAction.ForeColor = this.ForeColor;
                lbName.ForeColor = this.ForeColor;
                lbSign.ForeColor = this.ForeColor;
                lbValue.ForeColor = this.ForeColor;
            };
        }

        public ActionDialog(string action) : this()
        {
            try
            {
                string type = action.Split(':')[0];
                string name = action.Split(':')[1].Split('+', '-', '=')[0];
                string value = action.Split(':')[1].Split('+', '-', '=')[1];
                string sign = action.Replace(type + ":" + name, "").Replace(value, "");

                if (type == "variable")
                {
                    cbAction.SelectedIndex = 0;
                }
                else if (type == "gvariable")
                {
                    cbAction.SelectedIndex = 1;
                }
                else if (type == "inventoryitem")
                {
                    cbAction.SelectedIndex = 2;
                }
                else if (type == "skillpoint")
                {
                    cbAction.SelectedIndex = 3;
                }

                tbName.Text = name;
                cbSign.Text = sign;
                tbValue.Text = value;
            }
            catch(IndexOutOfRangeException)
            {}
        }
    }
}
