using System;
using System.Configuration;
using System.Reflection;
using System.Windows.Forms;

namespace WitcherScriptMerger
{
    class AppSettings
    {
        string _assemblyPath;

        Configuration _cachedConfig;
        Configuration CachedConfig
        {
            get
            {
                if (_cachedConfig == null)
                    _cachedConfig = ConfigurationManager.OpenExeConfiguration(_assemblyPath);
                return _cachedConfig;
            }
        }

        public bool HasConfigFile => CachedConfig.HasFile;

        public AppSettings()
        {
            _assemblyPath = Assembly.GetEntryAssembly().Location;

            if (!CachedConfig.HasFile)
            {
                MessageBox.Show(
                    "Config file is missing.",
                    "Script Merger Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Environment.Exit(1);
            }
        }

        public void Set(string key, object value)
        {
            try
            {
                CachedConfig.AppSettings.Settings[key].Value = value.ToString();
            }
            catch
            {
                CachedConfig.AppSettings.Settings.Add(key, value.ToString());
            }
        }

        public T Get<T>(string key)
        {
            try
            {
                if (CachedConfig.HasFile)
                {
                    var valueString = CachedConfig.AppSettings.Settings[key].Value;
                    var parseMethod = typeof(T).GetMethod("Parse", new Type[] { typeof(string) });
                    var valueObject = parseMethod.Invoke(null, new object[] { valueString });
                    return (T)valueObject;
                }

                Program.MainForm.ShowError($"Config file doesn't exist:\n\n{CachedConfig.FilePath}");
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
                if (CachedConfig.HasFile)
                    return CachedConfig.AppSettings.Settings[key].Value;

                Program.MainForm.ShowError($"Config file doesn't exist:\n\n{CachedConfig.FilePath}");
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public void Save()
        {
            try
            {
                CachedConfig.Save(ConfigurationSaveMode.Minimal);
            }
            catch (Exception ex)
            {
                Program.MainForm.ShowError($"Failed to save config due to error:\n\n{ex.Message}");
            }
        }
    }
}
