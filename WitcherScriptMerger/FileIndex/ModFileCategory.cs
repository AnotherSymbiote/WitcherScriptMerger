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
    }

    public static class Categories
    {
        public static ModFileCategory Script = new ModFileCategory(
            1, "Scripts", "These plaintext .ws files can be merged", true);

        public static ModFileCategory BundleXml = new ModFileCategory(
            2, "Bundle Content - XML", "These bundled .xml files can be merged", true);

        public static ModFileCategory BundleUnsupported = new ModFileCategory(
            3, "Bundle Content - Unsupported", "These bundled files can't be merged (usually because they're textures or models)", false);

        public static ModFileCategory OtherUnsupported = new ModFileCategory(
            4, "Other Unsupported", "These files can't be merged", false);
    }
}
