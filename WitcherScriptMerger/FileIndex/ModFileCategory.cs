namespace WitcherScriptMerger.FileIndex
{
    public class ModFileCategory
    {
        public ModFileCategory(int orderIndex, string displayName, string toolTipText, bool isSupported, bool isBundled)
        {
            OrderIndex = orderIndex;
            DisplayName = displayName;
            ToolTipText = toolTipText;
            IsSupported = isSupported;
            IsBundled = isBundled;
        }

        public int OrderIndex { get; private set; }
        public string DisplayName { get; private set; }
        public string ToolTipText { get; private set; }
        public bool IsSupported { get; private set; }
        public bool IsBundled { get; private set; }

        public override string ToString()
        {
            return DisplayName;
        }
    }

    public static class Categories
    {
        public static ModFileCategory Script = new ModFileCategory(
            1, "Scripts", "These plaintext .ws files can be merged", true, false);

        public static ModFileCategory Xml = new ModFileCategory(
            2, "Non-Bundled XML", "These .xml text files can be merged", true, false);

        public static ModFileCategory BundleText = new ModFileCategory(
            3, "Bundled Text", "These bundled text files can be merged", true, true);

        public static ModFileCategory BundleNotMergeable = new ModFileCategory(
            4, "Bundled Non-text - Not Mergeable", "These bundled files aren't text and can't be merged", false, true);

        public static ModFileCategory FlatNotMergeable = new ModFileCategory(
            5, "Not Mergeable", "Script Merger doesn't know what these files are", false, false);
    }
}
