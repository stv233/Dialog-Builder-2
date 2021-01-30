using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dialog_Builder_2
{
    class ResponseDialog : Form
    {
        private TextBox tbText = new TextBox();
        private ComboBox cbLink = new ComboBox();
        private TextBox tbCondition = new TextBox();

        private Button btOk = new Button();
        private Button btCancel = new Button();
        private Button btAdd = new Button();

        public Response Response { get; private set; }

        public ResponseDialog()
        {
            this.Size = new System.Drawing.Size(430, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);

            tbText = new TextBox
            {
                Width = 100,
                Top = 100,
                Left = 30,
                Parent = this
            };

            cbLink = new ComboBox
            {
                Width = 100,
                Top = 100,
                Left = 160,
                DropDownStyle = ComboBoxStyle.DropDown,
                Parent = this
            };
            cbLink.Items.Add("Exit");

            tbCondition = new TextBox
            {
                Width = 100,
                Top = 100,
                Left = 290,
                Parent = this
            };

            btAdd = new Button
            {
                AutoSize = true,
                Text = "+",
                Width = 100,
                Top = 100,
                Left = 390,
                Parent = this
            };
            btAdd.Click += (s, e) =>
            {
                using (ConditionDialog conditionDialog = new ConditionDialog())
                {

                    if (conditionDialog.ShowDialog() == DialogResult.OK)
                    {
                        tbCondition.Text = conditionDialog.Condition;
                    }
                }
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
                Response.Text = tbText.Text;
                Response.Link = cbLink.Text;
                Response.Condition = tbCondition.Text;

                this.DialogResult = DialogResult.OK;

            };
        }

        ResponseDialog(List<Page> pages) : this()
        {
            foreach (Page page in pages)
            {
                cbLink.Items.Add(page.Name);
            }
        }

        ResponseDialog(Response response) : this()
        {
            tbText.Text = response.Text;

            if (!cbLink.Items.Contains(response.Link))
            {
                cbLink.Items.Add(response.Link);
            }
            cbLink.Text = response.Link;

            tbCondition.Text = response.Condition;
        }

        ResponseDialog(Response response, List<Page> pages) : this(response)
        {
            foreach (Page page in pages)
            {
                cbLink.Items.Add(page.Name);
            }
        }
    }
}
