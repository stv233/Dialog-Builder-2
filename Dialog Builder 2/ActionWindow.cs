using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dialog_Builder_2
{
    class ActionWindow : Form
    {
        public string ActionsText
        {
            get
            {
                return rtbActionsText.Text;
            }
            set
            {
                rtbActionsText.Text = value;
            }
        }

        private Panel pnActions = new Panel();
        private RichTextBox rtbActionsText = new RichTextBox();
        private Button btAddAction = new Button();
        private List<string> actionString = new List<string>();

        public System.Drawing.Color ControlsColor { get; set; }

        public ActionWindow(string actionsText)
        {
            this.Size = new System.Drawing.Size(800, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.BackColor = frMain.ActiveForm.BackColor;
            this.Icon = Properties.Resources.ico1;

            pnActions = new Panel
            {
                Left = 5,
                Top = 5,
                Width = this.ClientSize.Width - 10,
                Height = this.Height / 2 - 30,
                BackColor = ControlsColor,
                BorderStyle = BorderStyle.FixedSingle,
                Parent = this
            };

            rtbActionsText = new RichTextBox
            {
                Text = actionsText,
                Width = this.ClientSize.Width - 10,
                Height = this.ClientSize.Height / 2 - 30,
                Left = 5,
                Top = this.Height / 2 + 5,
                BackColor = ControlsColor,
                Parent = this
            };
            rtbActionsText.ReadOnly = new Properties.Settings().SafeMode;
            rtbActionsText.TextChanged += (s, e) =>
            {
                TextToPanel();
            };

            btAddAction = new Button
            {
                AutoSize = true,
                Text = "Add action",
                FlatStyle = FlatStyle.Popup,
                Top = pnActions.Top + pnActions.Height,
                Parent = this

            };
            btAddAction.Left = pnActions.Left + pnActions.Width - btAddAction.Width;
            btAddAction.Click += (s, e) =>
            {
                using (ActionDialog actionDialog = new ActionDialog() { ForeColor = this.ForeColor })
                {
                    if (actionDialog.ShowDialog() == DialogResult.OK)
                    {
                        rtbActionsText.Text += actionDialog.Action + "\n";
                    }
                }
            };

            this.Load += (s, e) =>
            {
                pnActions.BackColor = ControlsColor;
                rtbActionsText.BackColor = ControlsColor;
                rtbActionsText.ForeColor = this.ForeColor;
            };
        }

        void TextToPanel()
        {
            actionString = new List<string>(rtbActionsText.Text.Split('\n'));
            pnActions.Controls.Clear();

            var i = 0;
            foreach (string action in actionString)
            {
                try
                {
                    string type = action.Split(':')[0];

                    string name;
                    string value;
                    string sign;
                    if (type != "skillpoint")
                    {
                        name = action.Split(':')[1].Split(new string[] { "=", "+", "-" },StringSplitOptions.None)[0];
                        value = action.Split(':')[1].Split(new string[] { "=", "+", "-" },StringSplitOptions.None)[1];
                        
                        if (string.IsNullOrEmpty(name))
                        {
                            if (string.IsNullOrEmpty(value))
                            {
                                sign = action.Replace(type + ":", "");
                            }
                            else
                            {
                                sign = action.Replace(type + ":", "").Replace(value, "");
                            }
                        }
                        else if (string.IsNullOrEmpty(value))
                        {
                            sign = action.Replace(type + ":" + name, "");
                        }
                        else
                        {
                            sign = action.Replace(type + ":" + name, "").Replace(value, "");
                        }
                        
                    }
                    else 
                    {
                        name = "";
                        value = action.Split(':')[1].Split(new string[] { "=", "+", "-" }, StringSplitOptions.None)[1];
                        sign = action.Replace(type + ":", "").Replace(value, "");
                    }

                   

                    if (type == "variable")
                    {
                        type = "Local variable";
                    }
                    else if (type == "gvariable")
                    {
                        type = "Global variable";
                    }
                    else if (type == "inventoryitem")
                    {
                        type = "Inventory item [Advanced inventory system]";
                    }
                    else if (type == "skillpoint")
                    {
                        type = "Skillpoints [Skill tree]";
                    }

                    var pnVariable = new Panel
                    {
                        AutoScroll = true,
                        Height = 40,
                        Width = pnActions.ClientSize.Width,
                        BackColor = this.BackColor,
                        ForeColor = this.ForeColor,
                        Parent = pnActions
                    };
                    pnVariable.Top = pnVariable.Height * i;

                    var lbText = new Label
                    {
                        AutoSize = true,
                        Text = type + ": " + name + " " + sign + " " + value,
                        Left = 5,
                        Parent = pnVariable
                    };
                    lbText.Top = pnVariable.Height / 2 - lbText.Height / 2;

                    var btEdit = new Button
                    {
                        Cursor = Cursors.Hand,
                        Text = "Edit",
                        FlatStyle = FlatStyle.Popup,
                        Height = pnVariable.Height - 10,
                        Left = lbText.Left + lbText.Width + 20,
                        Parent = pnVariable
                    };
                    btEdit.Top = pnVariable.Height / 2 - btEdit.Height / 2;
                    btEdit.Click += (s, e) =>
                    {
                        using (ActionDialog actionDialog = new ActionDialog(action) { ForeColor = this.ForeColor })
                        {
                            if (actionDialog.ShowDialog() == DialogResult.OK)
                            {
                                actionString[actionString.IndexOf(action)] = actionDialog.Action;
                                PanelToText();
                                return;
                            }
                        }
                    };

                    var btRemove = new Button
                    {
                        Cursor = Cursors.Hand,
                        Text = "Remove",
                        FlatStyle = FlatStyle.Popup,
                        Height = pnVariable.Height - 10,
                        Left = btEdit.Left + btEdit.Width + 20,
                        Parent = pnVariable
                    };
                    btRemove.Top = pnVariable.Height / 2 - btRemove.Height / 2;
                    btRemove.Click += (s, e) =>
                    {
                        actionString.Remove(action);
                        PanelToText();
                    };
                    i++;
                }
                catch (IndexOutOfRangeException) { }
            }
        }

        void PanelToText()
        {
            string text = "";

            foreach (string variable in actionString)
            {
                if (variable != "")
                {
                    text += variable + "\n";
                }
            }

            rtbActionsText.Text = text;
        }
    }
}
