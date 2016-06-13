// From http://stackoverflow.com/a/5427121/1641069

using System.Drawing;
using System.Windows.Forms;
using WitcherScriptMerger.LoadOrder;

namespace WitcherScriptMerger.Forms
{
    public static class NumericPrompt
    {
        const int Padding = 5;

        public static int? ShowDialog(string caption, int value = 0)
        {
            var inputField = new NumericUpDown
            {
                Left = Padding,
                Top = Padding,
                Width = 50,
                Increment = 1,
                DecimalPlaces = 0,
                Minimum = CustomLoadOrder.MinPriority + 1,
                Maximum = CustomLoadOrder.MaxPriority,
            };
            var okButton = new Button
            {
                Text = "&OK",
                Left = inputField.Left + inputField.Width + Padding,
                Width = 50,
                DialogResult = DialogResult.OK
            };
            okButton.Top = inputField.Top - (System.Math.Abs(okButton.Height - inputField.Height) / 2);

            var form = new Form()
            {
                FormBorderStyle = FormBorderStyle.FixedToolWindow,
                Text = caption,
                ClientSize = new Size
                {
                    Width = Padding + inputField.Width + Padding + okButton.Width + Padding,
                    Height = Padding + inputField.Height + Padding
                },
                StartPosition = FormStartPosition.CenterParent,
                MinimizeBox = false,
                MaximizeBox = false,
                AcceptButton = okButton,
                Icon = Program.MainForm.Icon
            };
            form.Controls.AddRange(
                new Control[]
                {
                    inputField,
                    okButton
                });

            if (value >= inputField.Minimum && value <= inputField.Maximum)
                inputField.Value = value;

            return
                form.ShowDialog() == DialogResult.OK
                ? (int?)System.Convert.ToInt32(inputField.Value)
                : null;
        }
    }
}
