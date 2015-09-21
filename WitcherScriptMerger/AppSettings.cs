using System;
using System.Configuration;
using System.Reflection;

namespace WitcherScriptMerger
{
    internal class AppSettings
    {
        private string _assemblyPath;
        private Configuration _cachedConfig;

        private bool IsBatching
        {
            get { return _cachedConfig != null; }
        }

        public AppSettings()
        {
            _assemblyPath = System.Reflection.Assembly.GetEntryAssembly().Location;
        }

        private Configuration GetConfig()
        {
            return ConfigurationManager.OpenExeConfiguration(_assemblyPath);
        }

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
                string valueString = config.AppSettings.Settings[key].Value;
                MethodInfo parseMethod = typeof(T).GetMethod("Parse", new Type[] { typeof(string) });
                object valueObject = parseMethod.Invoke(null, new object[] { valueString });
                return (T)valueObject;
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
                return config.AppSettings.Settings[key].Value;
            }
            catch
            {
                return string.Empty;
            }
        }

        private void TrySave(Configuration config)
        {
            try
            {
                config.Save(ConfigurationSaveMode.Minimal);
            }
            catch (Exception ex)
            {
                Program.MainForm.ShowMessage(
                    string.Format("Failed to save config due to error: {0}", ex.Message));
            }
        }
    }
}
