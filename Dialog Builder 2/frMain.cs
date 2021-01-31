using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft;

namespace Dialog_Builder_2
{
    public partial class frMain : Form
    {
        private string path = "";
        private Dialog dialog = new Dialog();
        private Color mainСolor = Color.FromArgb(64, 64, 64);
        private Color secondaryColor = Color.FromArgb(55, 55, 55);
        private Color textColor = Color.FromName("MediumSpringGreen");

        public Dialog Dialog
        {
            get
            {
                return dialog;
            }
            set
            {
                dialog = value;
                //LoadInfo();
            }
        }

        public Color MainColor
        {
            get
            {
                return mainСolor;
            }
            set
            {
                mainСolor = value;
                this.BackColor = value;
                tbName.BackColor = value;
                cbPages.BackColor = value;
                LoadInfo();
            }
        }

        public Color SecondaryColor
        {
            get
            {
                return secondaryColor;
            }
            set
            {
                secondaryColor = value;
                rtbPageText.BackColor = value;
                pnPageResponses.BackColor = value;
                msMain.BackColor = value;
                foreach (Control control in msMain.Items)
                {
                    control.ForeColor = value;
                }
                LoadInfo();
            }
        }

        public Color TextColor
        {
            get
            {
                return textColor;
            }
            set
            {
                textColor = value;
                tbName.ForeColor = value;
                rtbPageText.ForeColor = value;
                cbPages.ForeColor = value;
                btAddPage.ForeColor = value;
                btRemovePage.ForeColor = value;
                pnPageResponses.ForeColor = value;
                btAddResponse.ForeColor = value;
                foreach (Control control in msMain.Items)
                {
                    control.BackColor = value;
                }
                LoadInfo();
            }
        }

        private Label lbText = new Label();
        private Label lbResponses = new Label();

        private TextBox tbName = new TextBox();
        private RichTextBox rtbPageText = new RichTextBox();
        private ComboBox cbPages = new ComboBox();
        private Button btAddPage = new Button();
        private Button btRemovePage = new Button();
        private Panel pnPageResponses = new Panel();
        private Button btAddResponse = new Button();
        private Button btActions = new Button();

        public frMain()
        {
            InitializeComponent();

            tbName = new TextBox
            {
                BackColor = this.BackColor,
                ForeColor = this.ForeColor,
                Left = msMain.Width + (this.Width - msMain.Width) / 3 / 4,
                Top = 5,
                Parent = this
            };
            tbName.TextChanged += (s, e) =>
            {
                Dialog.Pages[cbPages.SelectedIndex].Name = tbName.Text;
                cbPages.Items[cbPages.SelectedIndex] = tbName.Text;
            };

            rtbPageText = new RichTextBox
            {
                BackColor = SecondaryColor,
                ForeColor = this.ForeColor,
                Width = (int)((this.Width - msMain.Width) / 3 * 1.5),
                Height = this.Height / 3 * 2,
                Left = tbName.Left,
                Top = this.Height / 3 / 2,
                Parent = this
            };
            rtbPageText.TextChanged += (s, e) =>
            {
                Dialog.Pages[cbPages.SelectedIndex].Text = rtbPageText.Text;
            };

            lbText = new Label
            {
                AutoSize = true,
                Text = "Text",
                Left = rtbPageText.Left,
                Parent = this
            };
            lbText.Top = rtbPageText.Top - lbText.Height;

            cbPages = new ComboBox
            {
                BackColor = this.BackColor,
                ForeColor = this.ForeColor,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Popup,
                Top = tbName.Top,
                Left = tbName.Left + tbName.Width + 5,
                Parent = this
            };
            cbPages.SelectedIndexChanged += (s, e) =>
            {
                LoadInfo();
                if (new Properties.Settings().SafeMode)
                {
                    tbName.ReadOnly = ((cbPages.SelectedIndex == 0));
                }
                if (new Properties.Settings().SafeMode)
                {
                    btRemovePage.Enabled = (!(cbPages.SelectedIndex == 0));
                }
            };

            btAddPage = new Button
            {
                Text = "Add",
                FlatStyle = FlatStyle.Popup,
                Cursor = Cursors.Hand,
                Top = tbName.Top,
                Left = cbPages.Left + cbPages.Width + 5,
                Parent = this

            };
            btAddPage.Click += (s, e) =>
            {
                cbPages.Items.Add((Dialog.Pages.Count + 1).ToString());
                Dialog.Pages.Add(new Page((Dialog.Pages.Count + 1).ToString()));
                cbPages.SelectedIndex = cbPages.Items.Count - 1;
            };

            btRemovePage = new Button
            {
                Text = "Remove",
                FlatStyle = FlatStyle.Popup,
                Cursor = Cursors.Hand,
                Top = tbName.Top,
                Left = btAddPage.Left + btAddPage.Width + 5,
                Parent = this
            };
            btRemovePage.Click += (s, e) =>
            {
                if (MessageBox.Show("Are you sure you want to delete the page?", "Deleting a page",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var index = cbPages.SelectedIndex;
                    Dialog.Pages.RemoveAt(index);
                    cbPages.Items.RemoveAt(index);
                    cbPages.SelectedIndex = index - 1;
                };
            };

            pnPageResponses = new Panel
            {
                AutoScroll = true,
                BackColor = SecondaryColor,
                BorderStyle = BorderStyle.FixedSingle,
                Width = this.Width / 4,
                Height = rtbPageText.Height / 5 * 4 - 3,
                Top = rtbPageText.Top,
                Parent = this
            };
            pnPageResponses.Left = this.Width - pnPageResponses.Width - (this.Width - msMain.Width) / 3 / 4;

            lbResponses = new Label
            {
                AutoSize = true,
                Text = "Player responses",
                Left = pnPageResponses.Left,
                Parent = this
            };
            lbResponses.Top = pnPageResponses.Top - lbResponses.Height;

            btAddResponse = new Button
            {
                Text = "Add",
                FlatStyle = FlatStyle.Popup,
                Cursor = Cursors.Hand,
                Width = pnPageResponses.Width,
                Height = rtbPageText.Height / 5 - 3,
                Top = pnPageResponses.Top + pnPageResponses.Height + 6,
                Left = pnPageResponses.Left,
                Parent = this
            };
            btAddResponse.Click += (s, e) =>
            {
                using (ResponseDialog responseDialog = new ResponseDialog(Dialog.Pages) { ForeColor = this.ForeColor})
                {
                    if (responseDialog.ShowDialog() == DialogResult.OK)
                    {
                        Dialog.Pages[cbPages.SelectedIndex].Responses.Add(responseDialog.Response);
                        LoadInfo();
                    }
                }
            };

            btActions = new Button
            {
                Text = "Actions",
                FlatStyle = FlatStyle.Popup,
                Cursor = Cursors.Hand,
                Parent = this
            };
            btActions.Left = rtbPageText.Left + rtbPageText.Width - btActions.Width;
            btActions.Top = rtbPageText.Top - btActions.Height - 3;
            btActions.Click += (s, e) =>
            {
                using (ActionWindow actionWindow = new ActionWindow(Dialog.Pages[cbPages.SelectedIndex].Actions) { ForeColor = this.ForeColor, ControlsColor =  SecondaryColor})
                {
                    actionWindow.ShowDialog();
                    Dialog.Pages[cbPages.SelectedIndex].Actions = actionWindow.ActionsText;
                }
            };

            this.Resize += (s, e) =>
            {
                tbName.Left = msMain.Width + (this.Width - msMain.Width) / 3 / 4;
                tbName.Top = 5;
                rtbPageText.Width = (int)((this.Width - msMain.Width) / 3 * 1.5);
                rtbPageText.Height = this.Height / 3 * 2;
                rtbPageText.Left = tbName.Left;
                rtbPageText.Top = this.Height / 3 / 2;
                lbText.Left = rtbPageText.Left;
                lbText.Top = rtbPageText.Top - lbText.Height;
                cbPages.Top = tbName.Top;
                cbPages.Left = tbName.Left + tbName.Width + 5;
                btAddPage.Top = tbName.Top;
                btAddPage.Left = cbPages.Left + cbPages.Width + 5;
                btRemovePage.Top = tbName.Top;
                btRemovePage.Left = btAddPage.Left + btAddPage.Width + 5;
                pnPageResponses.Width = this.Width / 4;
                pnPageResponses.Height = rtbPageText.Height - btAddResponse.Height - 6;
                pnPageResponses.Top = rtbPageText.Top;
                pnPageResponses.Left = this.Width - pnPageResponses.Width - (this.Width - msMain.Width) / 3 / 4;
                lbResponses.Left = pnPageResponses.Left;
                lbResponses.Top = pnPageResponses.Top - lbResponses.Height;
                btAddResponse.Width = pnPageResponses.Width;
                btAddResponse.Top = pnPageResponses.Top + pnPageResponses.Height + 6;
                btAddResponse.Left = pnPageResponses.Left;
                btActions.Left = rtbPageText.Left + rtbPageText.Width - btActions.Width;
                btActions.Top = rtbPageText.Top - btActions.Height - 3;
            };
            Dialog.Pages.Add(new Page("1"));
            cbPages.Items.Add("1");
            cbPages.SelectedIndex = 0;
        }

        private void frMain_Load(object sender, EventArgs e)
        {

        }

        private void LoadInfo()
        {
            rtbPageText.Text = Dialog.Pages[cbPages.SelectedIndex].Text;
            tbName.Text = Dialog.Pages[cbPages.SelectedIndex].Name;
            pnPageResponses.Controls.Clear();
            var maxWidth = pnPageResponses.ClientSize.Width;
            var i = 0;

            foreach (Response response in Dialog.Pages[cbPages.SelectedIndex].Responses)
            {
                var pnResponse = new Panel
                {
                    Height = 40,
                    Width = pnPageResponses.ClientSize.Width,
                    BackColor = MainColor,
                    ForeColor = TextColor,
                    Parent = pnPageResponses
                };
                pnResponse.Top = pnResponse.Height * i;

                var lbResponse = new Label
                {
                    AutoSize = true,
                    Text = response.Text + " -> " + response.Link + " (" + response.Condition + ")",
                    Left = 5,
                    Parent = pnResponse
                };
                lbResponse.Top = pnResponse.Height / 2 - lbResponse.Height / 2;

                var btEdit = new Button
                {
                    Text = "Edit",
                    FlatStyle = FlatStyle.Popup,
                    Height = pnResponse.Height - 10,
                    Left = lbResponse.Left + lbResponse.Width + 5,
                    Parent = pnResponse
                };
                btEdit.Top = pnResponse.Height / 2 - btEdit.Height / 2;
                btEdit.Click += (s, e) =>
                {
                    using (ResponseDialog responseDialog = new ResponseDialog(response, Dialog.Pages) { ForeColor = this.ForeColor})
                    {
                        if (responseDialog.ShowDialog() == DialogResult.OK)
                        {
                            Dialog.Pages[cbPages.SelectedIndex].Responses[Dialog.Pages[cbPages.SelectedIndex].Responses.IndexOf(response)] = responseDialog.Response;
                            LoadInfo();
                            return;
                        }
                    }
                };

                var btRemove = new Button
                {
                    Text = "Remove",
                    FlatStyle = FlatStyle.Popup,
                    Height = pnResponse.Height - 10,
                    Left = btEdit.Left + btEdit.Width + 5,
                    Parent = pnResponse
                };
                btRemove.Top = pnResponse.Height / 2 - btRemove.Height / 2;
                btRemove.Click += (s, e) =>
                {
                    if (MessageBox.Show("Are you sure you want to delete player response?", "Delete player response",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Dialog.Pages[cbPages.SelectedIndex].Responses.Remove(response);
                        LoadInfo();
                        return;
                    }
                };


                pnResponse.Width = btRemove.Left + btRemove.Width + 5;
                if (maxWidth <= pnResponse.Width) { maxWidth = pnResponse.Width; };
                i++;
            }

            foreach (Control control in pnPageResponses.Controls)
            {
                control.Width = maxWidth;
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to create a new dialogue?", "New",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Dialog = new Dialog();
                Dialog.Pages.Add(new Page("1"));
                cbPages.Items.Clear();
                cbPages.Items.Add("1");
                cbPages.SelectedIndex = 0;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog() { FileName = path, Filter = "Dialog builder file (*.dbf)|*.dbf|All files (*.*)|*.*" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Dialog = Newtonsoft.Json.JsonConvert.DeserializeObject<Dialog>(System.IO.File.ReadAllText(ofd.FileName));
                    cbPages.Items.Clear();

                    foreach(Page page in Dialog.Pages)
                    {
                        cbPages.Items.Add(page.Name);
                    }

                    cbPages.SelectedIndex = 0;
                    path = ofd.FileName;
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(path))
            {
                saveAsToolStripMenuItem.PerformClick();
            }
            else
            {
                System.IO.File.WriteAllText(path, Newtonsoft.Json.JsonConvert.SerializeObject(Dialog));
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog() { FileName = path, Filter = "Dialog builder file (*.dbf)|*.dbf|All files (*.*)|*.*" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllText(sfd.FileName, Newtonsoft.Json.JsonConvert.SerializeObject(Dialog));
                    path = sfd.FileName;
                }
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void localVariablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (VariablesWindow variablesWindow = new VariablesWindow(Dialog.LocalVariable) { ControlsColor = SecondaryColor, ForeColor = this.ForeColor})
            {
                variablesWindow.ShowDialog();
                Dialog.LocalVariable = variablesWindow.VariablesText;
            }
        }

        private void globalVariablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (VariablesWindow variablesWindow = new VariablesWindow(Dialog.GlobalVariable) { ControlsColor = SecondaryColor, ForeColor = this.ForeColor })
            {
                variablesWindow.ShowDialog();
                Dialog.GlobalVariable = variablesWindow.VariablesText;
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
