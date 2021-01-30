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
        private Response response = new Response();
        private Label lbText = new Label();
        private Label lbLink = new Label();
        private Label lbCondition = new Label();

        private TextBox tbText = new TextBox();
        private ComboBox cbLink = new ComboBox();
        private ComboBox cbCondition = new ComboBox();

        private Button btOk = new Button();
        private Button btCancel = new Button();
        private Button btAdd = new Button();

        public Response Response
        { 
            get
            {
                return response;
            }
            private set
            {
                response = value;
            }
        }

        public ResponseDialog()
        {
            this.Size = new System.Drawing.Size(445, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);

            tbText = new TextBox
            {
                BackColor = this.BackColor,
                Width = 100,
                Top = 100,
                Left = 30,
                Parent = this
            };

            cbLink = new ComboBox
            {
                BackColor = this.BackColor,
                FlatStyle = FlatStyle.Popup,
                Width = 100,
                Top = 100,
                Left = 160,
                DropDownStyle = ComboBoxStyle.DropDown,
                Parent = this
            };
            cbLink.Items.Add("Exit");
            cbLink.Text = "Exit";

            cbCondition = new ComboBox
            {
                BackColor = this.BackColor,
                FlatStyle = FlatStyle.Popup,
                Width = 100,
                Top = 100,
                Left = 290,
                DropDownStyle = ComboBoxStyle.DropDown,
                Parent = this
            };
            cbCondition.Items.Add("true");
            cbCondition.Items.Add("false");
            cbCondition.Text = "true";

            btAdd = new Button
            {
                FlatStyle = FlatStyle.Popup,
                Text = "+",
                Width = 25,
                Top = 100,
                Left = 390,
                Parent = this
            };
            btAdd.Click += (s, e) =>
            {
                using (ConditionDialog conditionDialog = new ConditionDialog() { ForeColor = this.ForeColor })
                {

                    if (conditionDialog.ShowDialog() == DialogResult.OK)
                    {
                        if (!cbCondition.Items.Contains(conditionDialog.Condition))
                        {
                            cbCondition.Items.Add(conditionDialog.Condition);
                        }
                        cbCondition.Text = conditionDialog.Condition;
                    }
                }
            };

            btCancel = new Button
            {
                FlatStyle = FlatStyle.Popup,
                AutoSize = true,
                Text = "Cancel",
                Cursor = Cursors.Hand,
                Top = 200,
                Left = cbLink.Left + cbLink.Width,
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
            btOk.Left = cbLink.Left - btOk.Width;
            btOk.Click += (s, e) =>
            {
                Response.Text = tbText.Text;
                Response.Link = cbLink.Text;
                Response.Condition = cbCondition.Text;

                this.DialogResult = DialogResult.OK;

            };

            lbText = new Label
            {
                AutoSize = true,
                Text = "Text",
                Top = 75,    
                Parent = this
            };
            lbText.Left = tbText.Left + tbText.Width / 2 - lbText.Width / 2;

            lbLink = new Label
            {
                AutoSize = true,
                Text = "Link",
                Top = 75,
                Parent = this
            };
            lbLink.Left = cbLink.Left + cbLink.Width / 2 - lbLink.Width / 2;

            lbCondition = new Label
            {
                AutoSize = true,
                Text = "Condition",
                Top = 75,
                Parent = this
            };
            lbCondition.Left = cbCondition.Left + cbCondition.Width / 2 - lbCondition.Width / 2;

            this.Load += (s, e) =>
            {
                tbText.ForeColor = this.ForeColor;
                cbLink.ForeColor = this.ForeColor;
                cbCondition.ForeColor = this.ForeColor;
                lbText.ForeColor = this.ForeColor;
                lbLink.ForeColor = this.ForeColor;
                lbCondition.ForeColor = this.ForeColor;
            };
        }

        public ResponseDialog(List<Page> pages) : this()
        {
            foreach (Page page in pages)
            {
                cbLink.Items.Add(page.Name);
            }
        }

        public ResponseDialog(Response response) : this()
        {
            tbText.Text = response.Text;

            if ((!cbLink.Items.Contains(response.Link)) && (response.Link != ""))
            {
                cbLink.Items.Add(response.Link);
            }

            if (response.Link != "")
            {
                cbLink.Text = response.Link;
            }

            if ((!cbCondition.Items.Contains(response.Condition)) && (response.Condition != ""))
            {
                cbCondition.Items.Add(response.Condition);
            }

            if (response.Condition != "")
            {
                cbCondition.Text = response.Condition;
            }
        }

        public ResponseDialog(Response response, List<Page> pages) : this(response)
        {
            foreach (Page page in pages)
            {
                cbLink.Items.Add(page.Name);
            }
        }
    }
}
