using System;
using System.Configuration;
using System.Reflection;

namespace WitcherScriptMerger
{
    public class AppSettings
    {
        private string _assemblyPath;
        private Configuration _cachedConfig;

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
            _cachedConfig.Save(ConfigurationSaveMode.Minimal);
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
            try
            {
                if (_cachedConfig == null)
                    config.Save(ConfigurationSaveMode.Minimal);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(string.Format(
                    "Failed to save setting ({0}) due to error: {1}",
                    key,
                    ex.Message));
            }
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
    }
}
