namespace WitcherScriptMerger.LoadOrder
{
    class ModLoadSetting
    {
        public string ModName { get; set; }

        public bool IsEnabled { get; set; }

        public int Priority { get; set; }

        public ModLoadSetting()
        { }

        public ModLoadSetting(string modName)
        {
            ModName = modName;
        }

        public override string ToString()
        {
            return $"{ModName}, priority {Priority}, {(IsEnabled ? "enabled" : "disabled")}";
        }
    }
}
