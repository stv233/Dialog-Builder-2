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
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);

            cbCondition = new ComboBox
            {
                Width = 100,
                Top = 100,
                Left = 30,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Parent = this
            };
            cbCondition.Items.Add("Local variable");
            cbCondition.Items.Add("Global variable");
            cbCondition.Items.Add("[Advanced inventory system] Inventory item");
            cbCondition.SelectedIndex = 0;

            tbName = new TextBox
            {
                Width = 100,
                Top = 100,
                Left = 160,
                Parent = this
            };

            cbSign = new ComboBox
            {
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
                Width = 100,
                Top = 100,
                Left = 420,
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
                string condition = "";

                if (cbCondition.SelectedIndex == 0)
                {
                    condition += "variable";
                }
                else if (cbCondition.SelectedIndex == 1)
                {
                    condition += "gvariable";
                }
                else if (cbCondition.SelectedIndex == 2)
                {
                    condition += "inventoryitem";
                }

                condition += ":";
                condition += tbName.Text;
                condition += cbSign.Text;
                condition += tbValue;

                Condition = condition;

                this.DialogResult = DialogResult.OK;

            };
        }
    }
}
