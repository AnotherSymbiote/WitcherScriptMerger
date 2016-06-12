// From http://stackoverflow.com/a/5427121/1641069

using System.Linq;
using System.Windows.Forms;

namespace WitcherScriptMerger.Forms
{
    public static class Prompt
    {
        public static string ShowDialog(string promptText, string caption)
        {
            const int border = 5;

            var form = new Form()
            {
                Width = 200,
                Height = 120,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            var prompt = new Label
            {
                Left = border,
                Top = border,
                Width = form.Width - (5 * border),
                Height = 20,
                Text = promptText
            };
            var textbox = new TextBox
            {
                Left = border,
                Top = prompt.Top + prompt.Height,
                Width = prompt.Width
            };
            var okButton = new Button
            {
                Text = "&Ok",
                Left = border,
                Width = (textbox.Width - 2) / 2,
                Top = textbox.Top + textbox.Height + border,
                DialogResult = DialogResult.OK
            };
            var cancelButton = new Button
            {
                Text = "&Cancel",
                Left = okButton.Left + okButton.Width + 2,
                Width = okButton.Width,
                Top = okButton.Top,
                DialogResult = DialogResult.Cancel
            };
            form.Controls.AddRange(
                new Control[]
                {
                    prompt,
                    textbox,
                    okButton,
                    cancelButton
                });
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
