using System.Drawing;
using System.Windows.Forms;
using WitcherScriptMerger.LoadOrder;

namespace WitcherScriptMerger.Forms
{
    public class PriorityPrompt : Form
    {
        const int Spacing = 5;

        NumericUpDown _inputField;
        TextBox _innerTextBox;
        Button _okButton;

        public int? ShowDialog(int value = 0)
        {
            _inputField = new NumericUpDown
            {
                Left = Spacing,
                Top = Spacing,
                Width = 50,
                Increment = 1,
                DecimalPlaces = 0,
                Minimum = CustomLoadOrder.TopPriority + 1,
                Maximum = CustomLoadOrder.BottomPriority,
            };
            _innerTextBox = (TextBox)_inputField.Controls[1];
            _innerTextBox.KeyDown += InputField_KeyDown;
            _innerTextBox.TextChanged += InputField_TextChanged;

            _okButton = new Button
            {
                Text = "&OK",
                Left = _inputField.Left + _inputField.Width + Spacing,
                Width = 50,
                DialogResult = DialogResult.OK
            };
            _okButton.Top = _inputField.Top - (System.Math.Abs(_okButton.Height - _inputField.Height) / 2);

            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Text = "Set Priority";
            ClientSize = new Size
            {
                Width = Spacing + _inputField.Width + Spacing + _okButton.Width + Spacing,
                Height = Spacing + _inputField.Height + Spacing
            };
            StartPosition = FormStartPosition.CenterParent;
            MinimizeBox = MaximizeBox = false;
            AcceptButton = _okButton;
            Icon = Program.MainForm.Icon;
            Controls.AddRange(
                new Control[]
                {
                    _inputField,
                    _okButton
                });
            KeyPreview = true;
            KeyDown += OnKeyDown;

            if (value >= _inputField.Minimum && value <= _inputField.Maximum)
                _inputField.Value = value;

            return
                base.ShowDialog() == DialogResult.OK
                ? (int?)System.Convert.ToInt32(_inputField.Value)
                : null;
        }

        private void InputField_TextChanged(object sender, System.EventArgs e)
        {
            _okButton.Enabled = !string.IsNullOrWhiteSpace((sender as TextBox).Text);
        }

        void InputField_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress =
                (e.KeyCode == Keys.Subtract) ||
                (IsCharacterCountMaxed() && !HasSelection() && IsNumeric(e.KeyCode)) ||
                (_innerTextBox.SelectionStart == 0 && (e.KeyCode == Keys.D0 || e.KeyCode == Keys.NumPad0));
        }

        bool IsCharacterCountMaxed()
        {
            return _innerTextBox.Text.Length == _inputField.Maximum.ToString().Length;
        }

        bool HasSelection()
        {
            return _innerTextBox.SelectionLength > 0;
        }

        bool IsNumeric(Keys keyCode)
        {
            return
                (keyCode >= Keys.D0 && keyCode <= Keys.D9) ||
                (keyCode >= Keys.NumPad0 && keyCode <= Keys.NumPad9);
        }

        void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
    }
}
