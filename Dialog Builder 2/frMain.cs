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

        struct staticSetting
        {
            public int Width;
            public int Height;
            public int Left;
            public int Top;
            public Color Main;
            public Color Secondary;
            public Color Text;
        }

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
                foreach (ToolStripMenuItem item in msMain.Items)
                {
                    item.BackColor = value;

                    foreach(ToolStripMenuItem subItem in item.DropDownItems)
                    {
                        subItem.BackColor = value;

                        foreach (ToolStripMenuItem subSubItem in subItem.DropDownItems)
                        {
                            subSubItem.BackColor = value;
                        }
                    }
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
                this.ForeColor = value;
                tbName.ForeColor = value;
                rtbPageText.ForeColor = value;
                cbPages.ForeColor = value;
                btAddPage.ForeColor = value;
                btRemovePage.ForeColor = value;
                pnPageResponses.ForeColor = value;
                btAddResponse.ForeColor = value;
                lbText.ForeColor = value;
                lbResponses.ForeColor = value;
                btActions.ForeColor = value;
                foreach (ToolStripMenuItem item in msMain.Items)
                {
                    item.ForeColor = value;

                    foreach (ToolStripMenuItem subItem in item.DropDownItems)
                    {
                        subItem.ForeColor = value;
                        foreach (ToolStripMenuItem subSubItem in subItem.DropDownItems)
                        {
                            subSubItem.ForeColor = value;
                        }
                    }
                }
                LoadInfo();
            }
        }

        public bool Loaded { get; private set; }

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

        public frMain(string [] args)
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

            if (args.Length > 0)
            {
                Open(args[0]);
            }
            else if (args.Length > 1)
            {
                Open(args[0]);
                for(var i = 1; i < args.Length; i++)
                {
                    System.Diagnostics.Process.Start(Application.ExecutablePath, args[i]);
                }
            }
        }

        private void frMain_Load(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\Stv233\\Dialog builder\\usersett"))
            {
                staticSetting setting = Newtonsoft.Json.JsonConvert.DeserializeObject<staticSetting>(System.IO.File.ReadAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/Stv233/Dialog builder/usersett"));

                MainColor = setting.Main;
                SecondaryColor = setting.Secondary;
                TextColor = setting.Text;
                this.Width = setting.Width;
                this.Height = setting.Height;
                this.Top = setting.Top;
                this.Left = setting.Left;
            }
            else
            {
                MainColor = mainСolor;
                SecondaryColor = secondaryColor;
                TextColor = textColor;
                staticSetting setting = new staticSetting();
                setting.Main = MainColor;
                setting.Secondary = SecondaryColor;
                setting.Text = TextColor;
                setting.Width = this.Width;
                setting.Height = this.Height;
                setting.Left = this.Left;
                setting.Top = this.Top;

                System.IO.Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\Stv233\\Dialog builder");
                System.IO.File.WriteAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\Stv233\\Dialog builder\\usersett", Newtonsoft.Json.JsonConvert.SerializeObject(setting));
            }

            safeModeToolStripMenuItem.Checked = new Properties.Settings().SafeMode;
            safeModeToolStripMenuItem.CheckedChanged += safeModeToolStripMenuItem_CheckedChanged;
            Loaded = true;
            this.Text = this.Text + "   version " + new Properties.Settings().Version;
            this.Activate();
        }

        private void safeModeToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            var settings = new Properties.Settings();
            settings.SafeMode = safeModeToolStripMenuItem.Checked;
            settings.Save();
            MessageBox.Show("Safe mode status changed. For all changes to take effect, you must restart the program.", "Safe mode", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


            ContextMenuStrip contextMenu = new ContextMenuStrip();
            contextMenu.BackColor = MainColor;
            contextMenu.ForeColor = TextColor;

            var localVariable = new ToolStripMenuItem("Insert local variable");
            localVariable.Image = Properties.Resources.local;
            localVariable.ImageTransparentColor = Color.FromArgb(64, 64, 64);

            foreach (string variable in Dialog.LocalVariable.Split('\n'))
            {
                try
                {
                    var lVariable = new ToolStripMenuItem(variable.Split(':')[1].Split('=')[0]);
                    lVariable.BackColor = MainColor;
                    lVariable.ForeColor = textColor;
                    lVariable.Image = Properties.Resources.local;
                    lVariable.ImageTransparentColor = Color.FromArgb(64,64,64);
                    lVariable.Click += (s, e) =>
                    {
                        rtbPageText.SelectionLength = 0;
                        rtbPageText.SelectedText = "[variable:" + variable.Split(':')[1].Split('=')[0] + "]";
                    };
                    localVariable.DropDownItems.Add(lVariable);
                }
                catch(IndexOutOfRangeException) { }
            }
            contextMenu.Items.Add(localVariable);

            var globalVariable = new ToolStripMenuItem("Insert global variable");
            globalVariable.Image = Properties.Resources.global;
            globalVariable.ImageTransparentColor = Color.FromArgb(64, 64, 64);

            foreach (string variable in Dialog.GlobalVariable.Split('\n'))
            {
                try
                {
                    var gVariable = new ToolStripMenuItem(variable.Split(':')[1].Split('=')[0]);
                    gVariable.BackColor = MainColor;
                    gVariable.ForeColor = textColor;
                    gVariable.Image = Properties.Resources.global;
                    gVariable.ImageTransparentColor = Color.FromArgb(64, 64, 64);
                    gVariable.Click += (s, e) =>
                    {
                        rtbPageText.SelectionLength = 0;
                        rtbPageText.SelectedText = "[gvariable:" + variable.Split(':')[1].Split('=')[0] + "]";
                    };
                    globalVariable.DropDownItems.Add(gVariable);
                }
                catch (IndexOutOfRangeException) { }
            }
            contextMenu.Items.Add(globalVariable);

            rtbPageText.ContextMenuStrip = contextMenu;

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

                path = "";
                if (this.Text.Contains("-"))
                {
                    this.Text = this.Text.Substring(0, this.Text.IndexOf("-") - 1);
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog() { FileName = path, Filter = "Dialog builder file (*.dbf)|*.dbf|All files (*.*)|*.*" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Open(ofd.FileName);
                }
            }
        }

        private void Open(string filePath)
        {
            Dialog = Newtonsoft.Json.JsonConvert.DeserializeObject<Dialog>(System.IO.File.ReadAllText(filePath));
            cbPages.Items.Clear();

            foreach (Page page in Dialog.Pages)
            {
                cbPages.Items.Add(page.Name);
            }

            cbPages.SelectedIndex = 0;
            path = filePath;

            if (this.Text.Contains ("-"))
            {
                this.Text = this.Text.Substring(0,this.Text.IndexOf("-") - 1);
            }
            this.Text += " - " + System.IO.Path.GetFileNameWithoutExtension(path);
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

                    if (this.Text.Contains("-"))
                    {
                        this.Text = this.Text.Substring(0, this.Text.IndexOf("-") - 1);
                    }
                    this.Text += " - " + System.IO.Path.GetFileNameWithoutExtension(path);
                }
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllText(fbd.SelectedPath + "\\variables", Dialog.LocalVariable);
                    System.IO.File.WriteAllText(fbd.SelectedPath + "\\gvariables", Dialog.GlobalVariable);

                    foreach (Page page in Dialog.Pages)
                    {
                        System.IO.File.WriteAllText(fbd.SelectedPath + "\\" + page.Name + ".dat", page.Text);
                        System.IO.File.WriteAllText(fbd.SelectedPath + "\\" + page.Name + "_a.dat", page.Actions);

                        using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(fbd.SelectedPath + "\\" + page.Name + "_r.dat"))
                        {
                            foreach(Response response in page.Responses)
                            {
                                streamWriter.WriteLine(response.Text);
                            }
                        }

                        using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(fbd.SelectedPath + "\\" + page.Name + "_l.dat"))
                        {
                            foreach (Response response in page.Responses)
                            {
                                if (response.Link == "Exit")
                                {
                                    streamWriter.WriteLine("0");
                                }
                                else
                                {
                                    streamWriter.WriteLine(response.Link);
                                }
                            }
                        }

                        using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(fbd.SelectedPath + "\\" + page.Name + "_c.dat"))
                        {
                            foreach (Response response in page.Responses)
                            {
                                streamWriter.WriteLine(response.Condition);
                            }
                        }
                    }
                }
            }
        }

        private void localVariablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (VariablesWindow variablesWindow = new VariablesWindow(Dialog.LocalVariable) { ControlsColor = SecondaryColor, ForeColor = this.ForeColor})
            {
                variablesWindow.ShowDialog();
                Dialog.LocalVariable = variablesWindow.VariablesText;
                LoadInfo();
            }
        }

        private void globalVariablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (VariablesWindow variablesWindow = new VariablesWindow(Dialog.GlobalVariable) { ControlsColor = SecondaryColor, ForeColor = this.ForeColor })
            {
                variablesWindow.ShowDialog();
                Dialog.GlobalVariable = variablesWindow.VariablesText;
                LoadInfo();
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void color1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ColorDialog cd = new ColorDialog { Color = MainColor})
            {
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    MainColor = cd.Color;
                    staticSetting setting = new staticSetting();
                    setting.Main = MainColor;
                    setting.Secondary = SecondaryColor;
                    setting.Text = TextColor;
                    setting.Width = this.Width;
                    setting.Height = this.Height;
                    setting.Left = this.Left;
                    setting.Top = this.Top;
                    System.IO.File.WriteAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/Stv233/Dialog builder/usersett", Newtonsoft.Json.JsonConvert.SerializeObject(setting));
                }
            }
        }

        private void color2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ColorDialog cd = new ColorDialog { Color = SecondaryColor })
            {
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    SecondaryColor = cd.Color;
                    staticSetting setting = new staticSetting();
                    setting.Main = MainColor;
                    setting.Secondary = SecondaryColor;
                    setting.Text = TextColor;
                    setting.Width = this.Width;
                    setting.Height = this.Height;
                    setting.Left = this.Left;
                    setting.Top = this.Top;
                    System.IO.File.WriteAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/Stv233/Dialog builder/usersett", Newtonsoft.Json.JsonConvert.SerializeObject(setting));
                }
            }
        }

        private void textColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ColorDialog cd = new ColorDialog { Color = SecondaryColor })
            {
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    TextColor = cd.Color;
                    staticSetting setting = new staticSetting();
                    setting.Main = MainColor;
                    setting.Secondary = SecondaryColor;
                    setting.Text = TextColor;
                    setting.Width = this.Width;
                    setting.Height = this.Height;
                    setting.Left = this.Left;
                    setting.Top = this.Top;
                    System.IO.File.WriteAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/Stv233/Dialog builder/usersett", Newtonsoft.Json.JsonConvert.SerializeObject(setting));
                }
            }
        }

        private void frMain_SizeChanged(object sender, EventArgs e)
        {
            staticSetting setting = new staticSetting();
            setting.Main = MainColor;
            setting.Secondary = SecondaryColor;
            setting.Text = TextColor;
            setting.Width = this.Width;
            setting.Height = this.Height;
            setting.Left = this.Left;
            setting.Top = this.Top;
            System.IO.File.WriteAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/Stv233/Dialog builder/usersett", Newtonsoft.Json.JsonConvert.SerializeObject(setting));
        }

        private void frMain_Move(object sender, EventArgs e)
        {
            if (Loaded)
            {
                staticSetting setting = new staticSetting();
                setting.Main = MainColor;
                setting.Secondary = SecondaryColor;
                setting.Text = TextColor;
                setting.Width = this.Width;
                setting.Height = this.Height;
                setting.Left = this.Left;
                setting.Top = this.Top;
                System.IO.File.WriteAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/Stv233/Dialog builder/usersett", Newtonsoft.Json.JsonConvert.SerializeObject(setting));
            }
        }
    }
}
