using System;
using System.Configuration;
using System.Reflection;
using System.Windows.Forms;

namespace WitcherScriptMerger
{
    internal class AppSettings
    {
        string _assemblyPath;
        Configuration _cachedConfig;

        bool IsBatching => (_cachedConfig != null);

        public AppSettings()
        {
            _assemblyPath = Assembly.GetEntryAssembly().Location;

            if (!GetConfig().HasFile)
            {
                MessageBox.Show(
                    "Config file is missing.",
                    "Script Merger Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Environment.Exit(1);
            }
        }

        Configuration GetConfig() => ConfigurationManager.OpenExeConfiguration(_assemblyPath);

        public bool HasConfigFile => GetConfig().HasFile;

        public void StartBatch()
        {
            _cachedConfig = GetConfig();
        }

        public void EndBatch()
        {
            if (!IsBatching)
                return;
            TrySave(_cachedConfig);
            _cachedConfig = null;
        }

        public void Set(string key, object value)
        {
            var config = _cachedConfig ?? GetConfig();
            try
            {
                config.AppSettings.Settings[key].Value = value.ToString();
            }
            catch
            {
                config.AppSettings.Settings.Add(key, value.ToString());
            }
            if (!IsBatching)
                TrySave(config);
        }

        public T Get<T>(string key)
        {
            try
            {
                var config = _cachedConfig ?? GetConfig();
                if (config.HasFile)
                {
                    var valueString = config.AppSettings.Settings[key].Value;
                    var parseMethod = typeof(T).GetMethod("Parse", new Type[] { typeof(string) });
                    var valueObject = parseMethod.Invoke(null, new object[] { valueString });
                    return (T)valueObject;
                }

                Program.MainForm.ShowError($"Config file doesn't exist:\n\n{config.FilePath}");
                return default(T);
            }
            catch
            {
                return default(T);
            }
        }

        public string Get(string key)
        {
            try
            {
                var config = _cachedConfig ?? GetConfig();
                if (config.HasFile)
                    return config.AppSettings.Settings[key].Value;

                Program.MainForm.ShowError($"Config file doesn't exist:\n\n{config.FilePath}");
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        void TrySave(Configuration config)
        {
            try
            {
                config.Save(ConfigurationSaveMode.Minimal);
            }
            catch (Exception ex)
            {
                Program.MainForm.ShowError($"Failed to save config due to error:\n\n{ex.Message}");
            }
        }
    }
}
