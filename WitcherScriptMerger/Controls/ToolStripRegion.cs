using System.Linq;
using System.Windows.Forms;

namespace WitcherScriptMerger.Controls
{
    class ToolStripRegion
    {
        public ToolStripItemCollection Items;

        public bool Available => Items.Cast<ToolStripItem>().Any(item => item.Available);

        public ToolStripRegion(ToolStrip owner, ToolStripItem[] value)
        {
            Items = new ToolStripItemCollection(owner, value);
        }
    }
}
