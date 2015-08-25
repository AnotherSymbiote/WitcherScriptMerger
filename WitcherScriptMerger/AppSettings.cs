using System;
using System.Configuration;
using System.Reflection;

namespace WitcherScriptMerger
{
    public class AppSettings
    {
        private Configuration _config;

        public AppSettings()
        {
            _config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetEntryAssembly().Location);
        }

        public void Set(string key, object value)
        {
            try
            {
                _config.AppSettings.Settings[key].Value = value.ToString();
            }
            catch
            {
                _config.AppSettings.Settings.Add(key, value.ToString());
            }
            _config.Save(ConfigurationSaveMode.Modified);
        }

        public T Get<T>(string key)
        {
            try
            {
                string valueString = _config.AppSettings.Settings[key].Value;
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
                return _config.AppSettings.Settings[key].Value;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
