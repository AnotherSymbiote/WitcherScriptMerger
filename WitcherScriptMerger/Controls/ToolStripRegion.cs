using System.Windows.Forms;

namespace WitcherScriptMerger.Controls
{
    public class ToolStripRegion
    {
        public ToolStripItemCollection Items;

        public ToolStripRegion(ToolStrip owner, ToolStripItem[] value)
        {
            Items = new ToolStripItemCollection(owner, value);
        }
    }
}
