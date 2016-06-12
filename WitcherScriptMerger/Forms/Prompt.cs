// From http://stackoverflow.com/a/5427121/1641069

using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WitcherScriptMerger.Forms
{
    public static class Prompt
    {
        const int Padding = 5;
        const int ButtonSpacing = 2;

        public static string ShowDialog(string promptText, string caption)
        {
            var form = new Form()
            {
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            var prompt = new Label
            {
                Left = Padding,
                Top = Padding,
                Width = 200,
                Height = 20,
                Text = promptText
            };
            var textbox = new TextBox
            {
                Left = prompt.Left,
                Top = prompt.Top + prompt.Height,
                Width = prompt.Width
            };
            var okButton = new Button
            {
                Text = "&OK",
                Left = textbox.Left,
                Width = (textbox.Width - ButtonSpacing) / 2,
                Top = textbox.Top + textbox.Height + Padding,
                DialogResult = DialogResult.OK
            };
            var cancelButton = new Button
            {
                Text = "&Cancel",
                Left = okButton.Left + okButton.Width + ButtonSpacing,
                Width = okButton.Width,
                Top = okButton.Top,
                DialogResult = DialogResult.Cancel
            };
            form.Controls.AddRange(
                new Control[]
                {
                    prompt,
                    textbox,
                    okButton
                });
            form.ClientSize =
                new Size
                {
                    Width = form.Controls.Cast<Control>().Max(c => c.Width) + (Padding * 2),
                    Height = form.Controls.Cast<Control>().Sum(c => c.Height) + (Padding * 3)
                };
            form.Controls.Add(cancelButton);

            form.AcceptButton = okButton;
            form.CancelButton = cancelButton;
            form.MinimizeBox = form.MaximizeBox = false;
            
            return
                form.ShowDialog() == DialogResult.OK
                ? textbox.Text
                : null;
        }
    }
}
