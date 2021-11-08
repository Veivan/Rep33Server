using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Rep33.Data
{
    public class WritableJsonConfigurationProvider : JsonConfigurationProvider
    {
        public WritableJsonConfigurationProvider(JsonConfigurationSource source) : base(source)
        {
        }

        public override void Set(string key, string value)
        {
            base.Set(key, value);

            //Get Whole json file and change only passed key with passed value. It requires modification if you need to support change multi level json structure
            var fileFullPath = base.Source.FileProvider.GetFileInfo(base.Source.Path).PhysicalPath;
            string json = File.ReadAllText(fileFullPath);
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            jsonObj[key] = value;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(fileFullPath, output);
        }
    }

    public class WritableJsonConfigurationSource : JsonConfigurationSource
    {
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            this.EnsureDefaults(builder);
            return (IConfigurationProvider)new WritableJsonConfigurationProvider(this);
        }
    }

    public class AppSettings
    {
        private string appSettingValue { get; set; }

        public static string GetAppSetting(string Key)
        {
#if DEBUG
            var appset = "appsettings.Development.json";
#else
            var appset = "appsettings.json"; 
#endif
            var builder = new ConfigurationBuilder()
                            .SetBasePath(AppContext.BaseDirectory)
                            .AddJsonFile(appset, optional: false, reloadOnChange: false)
                            .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();
            var settings = new AppSettings(configuration, Key);
            return settings.appSettingValue;
        }

        public static void SetAppSetting(string Key, string value)
        {

#if DEBUG
            var appset = "appsettings.Development.json";
#else
            var appset = "appsettings.json"; 
#endif
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            IConfigurationRoot configuration = configurationBuilder.Add(
                (Action<WritableJsonConfigurationSource>)(s =>
                {
                    s.FileProvider = null;
                    s.Path = appset;
                    s.Optional = false;
                    s.ReloadOnChange = false;
                    s.ResolveFileProvider();
                })).Build();
            configuration.GetSection(Key).Value = value;
        }

        private AppSettings(IConfiguration config, string Key)
        {
            appSettingValue = config.GetValue<string>(Key);
        }


    }
}
