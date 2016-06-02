namespace WitcherScriptMerger.FileIndex
{
    public class ModFileCategory
    {
        public ModFileCategory(int orderIndex, string displayName, string toolTipText, bool isSupported)
        {
            OrderIndex = orderIndex;
            DisplayName = displayName;
            ToolTipText = toolTipText;
            IsSupported = isSupported;
        }

        public int OrderIndex { get; private set; }
        public string DisplayName { get; private set; }
        public string ToolTipText { get; private set; }
        public bool IsSupported { get; private set; }

        public bool IsBundled => (this == Categories.BundleText || this == Categories.BundleUnsupported);
    }

    public static class Categories
    {
        public static ModFileCategory Script = new ModFileCategory(
            1, "Scripts", "These plaintext .ws files can be merged", true);

        public static ModFileCategory Xml = new ModFileCategory(
            2, "XML", "These .xml text files can be merged", true);

        public static ModFileCategory BundleText = new ModFileCategory(
            3, "Bundle Content - Text", "These bundled text files can be merged", true);

        public static ModFileCategory BundleUnsupported = new ModFileCategory(
            4, "Bundle Content - Unsupported", "These bundled files can't be merged (usually because they're textures or models)", false);

        public static ModFileCategory OtherUnsupported = new ModFileCategory(
            5, "Other Unsupported", "These files can't be merged", false);
    }
}
