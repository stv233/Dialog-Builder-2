using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dialog_Builder_2
{

    class VariablesWindow : Form
    {
        public string VariablesText
        {
            get
            {
                return rtbVariablesText.Text;
            }
            set
            {
                rtbVariablesText.Text = value;
            }
        }

        private Panel pnVariables = new Panel();
        private RichTextBox rtbVariablesText = new RichTextBox();
        private Button btAddVariable = new Button();
        private List<string> variablesString = new List<string>();

        public System.Drawing.Color ControlsColor { get; set; }

        public VariablesWindow(string variablesText)
        {
            this.Size = new System.Drawing.Size(800, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.BackColor = frMain.ActiveForm.BackColor;

            pnVariables = new Panel
            {
                Left = 5,
                Top = 5,
                Width = this.ClientSize.Width - 10,
                Height = this.Height / 2 - 30,
                BackColor = ControlsColor,
                BorderStyle = BorderStyle.FixedSingle,
                Parent = this
            };

            rtbVariablesText = new RichTextBox
            {
                Text = variablesText,
                Width = this.ClientSize.Width - 10,
                Height = this.ClientSize.Height / 2 - 30,
                Left = 5,
                Top = this.Height / 2 + 5,
                BackColor = ControlsColor,
                Parent = this
            };
            rtbVariablesText.ReadOnly = new Properties.Settings().SafeMode;
            rtbVariablesText.TextChanged += (s, e) =>
            {
                TextToPanel();
            };

            btAddVariable = new Button
            {
                AutoSize = true,
                Text = "Add variable",
                FlatStyle = FlatStyle.Popup,
                Top = pnVariables.Top + pnVariables.Height,
                Parent = this

            };
            btAddVariable.Left = pnVariables.Left + pnVariables.Width - btAddVariable.Width;
            btAddVariable.Click += (s, e) =>
            {
                using (VariableDialog variableDialog = new VariableDialog() { ForeColor = this.ForeColor })
                {
                    if (variableDialog.ShowDialog() == DialogResult.OK)
                    {
                        rtbVariablesText.Text += variableDialog.Variable + "\n";
                    }
                }
            };



            this.Load += (s, e) =>
            {
                pnVariables.BackColor = ControlsColor;
                rtbVariablesText.BackColor = ControlsColor;
                rtbVariablesText.ForeColor = this.ForeColor;
            };
        }

        void TextToPanel()
        {
            variablesString = new List<string>(rtbVariablesText.Text.Split('\n'));
            pnVariables.Controls.Clear();

            var i = 0;
            foreach(string variable in variablesString)
            {
                try
                {
                    if ((variable != "") && (!string.IsNullOrEmpty(variable.Split(':')[0])) && (!string.IsNullOrEmpty(variable.Split(':')[1].Split('=')[0])) && (variable.Split(':')[1].Split('=')[1] != null))
                    {

                        var pnVariable = new Panel
                        {
                            Height = 40,
                            Width = pnVariables.ClientSize.Width,
                            BackColor = this.BackColor,
                            ForeColor = this.ForeColor,
                            Parent = pnVariables
                        };
                        pnVariable.Top = pnVariable.Height * i;

                        var lbName = new Label
                        {
                            AutoSize = true,
                            Text = "Name",
                            Left = 5,
                            Parent = pnVariable
                        };
                        lbName.Top = pnVariable.Height / 2 - lbName.Height / 2;

                        var tbVariableName = new TextBox
                        {
                            ForeColor = this.ForeColor,
                            BackColor = this.BackColor,
                            Text = variable.Split(':')[1].Split('=')[0],
                            Width = 200,
                            Left = lbName.Left + lbName.Width,
                            Parent = pnVariable
                        };
                        tbVariableName.Top = pnVariable.Height / 2 - tbVariableName.Height / 2;
                        tbVariableName.Click += (s, e) =>
                        {
                            using (StringDialog stringDialog = new StringDialog() { CanBeEmpty = false,ForeColor = this.ForeColor, String = tbVariableName.Text})
                            {
                                invalidChar:
                                if (stringDialog.ShowDialog() == DialogResult.OK)
                                {
                                    if ((stringDialog.String.Contains("[")
                                        || stringDialog.String.Contains("]")
                                        || stringDialog.String.Contains(" ")
                                        || stringDialog.String.Contains("!")
                                        || stringDialog.String.Contains("+")
                                        || stringDialog.String.Contains("-")
                                        || stringDialog.String.Contains("=")
                                        || stringDialog.String.Contains(">")
                                        || stringDialog.String.Contains("<")))
                                    {
                                        MessageBox.Show("Variable name contains invalid characters.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        goto invalidChar;
                                    }
                                    variablesString[variablesString.IndexOf(variable)] = variable.Split(':')[0] + ":" + stringDialog.String + "=" + variable.Split(':')[1].Split('=')[1];
                                    PanelToText();
                                    return;
                                }
                            }
                            
                        };

                        var lbValue = new Label
                        {
                            AutoSize = true,
                            Text = "Value",
                            Left = tbVariableName.Left + tbVariableName.Width + 50,
                            Parent = pnVariable
                        };
                        lbValue.Top = pnVariable.Height / 2 - lbValue.Height / 2;

                        if (variable.Split(':')[0] == "number")
                        {
                            var nudVariableValue = new NumericUpDown
                            {
                                DecimalPlaces = 2,
                                ForeColor = this.ForeColor,
                                BackColor = this.BackColor,
                                Maximum = 99999999999,
                                Minimum = -99999999999,
                                Left = lbValue.Left + lbValue.Width + 5,
                                Width = 300,
                                Parent = pnVariable
                            };
                            nudVariableValue.Top = pnVariable.Height / 2 - nudVariableValue.Height / 2;
                            if (!String.IsNullOrEmpty(variable.Split(':')[1].Split('=')[1]))
                            {
                                nudVariableValue.Value = Convert.ToDecimal(variable.Split(':')[1].Split('=')[1]);
                            }
                            else
                            {
                                nudVariableValue.Value = 0;
                            }
                            nudVariableValue.ValueChanged += (s, e) =>
                            {
                                variablesString[variablesString.IndexOf(variable)] = variable.Split('=')[0] + "=" + nudVariableValue.Value.ToString();
                                PanelToText();
                                return;
                            };
                        }
                        else
                        {
                            var tbVariableValue = new TextBox
                            {
                                ForeColor = this.ForeColor,
                                BackColor = this.BackColor,
                                Text = variable.Split(':')[1].Split('=')[1],
                                Left = lbValue.Left + lbValue.Width + 5,
                                Width = 300,
                                Parent = pnVariable
                            };
                            tbVariableValue.Top = pnVariable.Height / 2 - tbVariableValue.Height / 2;
                            tbVariableValue.Click += (s, e) =>
                            {
                                using (StringDialog stringDialog = new StringDialog() { CanBeEmpty = true, ForeColor = this.ForeColor, String = tbVariableValue.Text })
                                {
                                    if (stringDialog.ShowDialog() == DialogResult.OK)
                                    {
                                        variablesString[variablesString.IndexOf(variable)] = variable.Split('=')[0] + "=" + stringDialog.String;
                                        PanelToText();
                                        return;
                                    }
                                }
                            };
                        }

                        var btRemove = new Button
                        {
                            Text = "Remove",
                            FlatStyle = FlatStyle.Popup,
                            Height = pnVariable.Height - 10,
                            Left = pnVariable.Controls[3].Left + pnVariable.Controls[3].Width + 50,
                            Parent = pnVariable
                        };
                        btRemove.Top = pnVariable.Height / 2 - btRemove.Height / 2;
                        btRemove.Click += (s, e) =>
                        {
                            variablesString.Remove(variable);
                            PanelToText();
                        };
                        i++;
                    }
                }
                catch (IndexOutOfRangeException) { }
            }
        }

        void PanelToText()
        {
            string text = "";

            foreach(string variable in variablesString)
            {
                if (variable != "")
                {
                    text += variable + "\n";
                }
            }

            rtbVariablesText.Text = text;
        }
    }
}
