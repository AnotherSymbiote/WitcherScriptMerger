using System.Drawing;
using System.Windows.Forms;
using WitcherScriptMerger.LoadOrder;

namespace WitcherScriptMerger.Forms
{
    public class NumericPrompt : Form
    {
        const int Spacing = 5;

        public int? ShowDialog(string caption, int value = 0)
        {
            var inputField = new NumericUpDown
            {
                Left = Spacing,
                Top = Spacing,
                Width = 50,
                Increment = 1,
                DecimalPlaces = 0,
                Minimum = CustomLoadOrder.MinPriority + 1,
                Maximum = CustomLoadOrder.MaxPriority,
            };
            var okButton = new Button
            {
                Text = "&OK",
                Left = inputField.Left + inputField.Width + Spacing,
                Width = 50,
                DialogResult = DialogResult.OK
            };
            okButton.Top = inputField.Top - (System.Math.Abs(okButton.Height - inputField.Height) / 2);

            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Text = caption;
            ClientSize = new Size
            {
                Width = Spacing + inputField.Width + Spacing + okButton.Width + Spacing,
                Height = Spacing + inputField.Height + Spacing
            };
            StartPosition = FormStartPosition.CenterParent;
            MinimizeBox = MaximizeBox = false;
            AcceptButton = okButton;
            Icon = Program.MainForm.Icon;
            Controls.AddRange(
                new Control[]
                {
                    inputField,
                    okButton
                });
            KeyPreview = true;
            KeyDown += OnKeyDown;

            if (value >= inputField.Minimum && value <= inputField.Maximum)
                inputField.Value = value;

            return
                ShowDialog() == DialogResult.OK
                ? (int?)System.Convert.ToInt32(inputField.Value)
                : null;
        }

        void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
    }
}
