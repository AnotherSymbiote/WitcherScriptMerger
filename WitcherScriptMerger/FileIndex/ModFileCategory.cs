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
    }

    public static class Categories
    {
        public static ModFileCategory Script = new ModFileCategory(
            1, "Scripts", "These plaintext .ws files can be merged", true, false);

        public static ModFileCategory Xml = new ModFileCategory(
            2, "XML", "These .xml text files can be merged", true, false);

        public static ModFileCategory BundleText = new ModFileCategory(
            3, "Bundle Content - Text", "These bundled text files can be merged", true, true);

        public static ModFileCategory BundleUnsupported = new ModFileCategory(
            4, "Bundle Content - Unsupported Non-Text", "These bundled files aren't text and can't be merged", false, true);

        public static ModFileCategory OtherUnsupported = new ModFileCategory(
            5, "Other Unsupported", "Script Merger doesn't know what these files are", false, false);
    }
}
