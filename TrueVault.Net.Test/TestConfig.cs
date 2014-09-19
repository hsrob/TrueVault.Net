using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Text;

namespace TrueVault.Net.Test
{
    public sealed class TestConfig
    {
        static readonly TestConfig instance = new TestConfig();
        static TestConfig(){}
        TestConfig()
        {
            bool configured = false;
            var baseBinPath = AppDomain.CurrentDomain.BaseDirectory;
            var configFilePath = Path.Combine(baseBinPath, "clientconfig.json");
            if(File.Exists(configFilePath))
                using (var fs = new FileInfo(configFilePath).OpenRead())
                {
                    var configText = Encoding.UTF8.GetString(fs.ReadFully());
                    var fileConfig = configText.FromJson<TestConfig>();
                    if (!string.IsNullOrWhiteSpace(fileConfig.TrueVaultApiKey) &&
                        !string.IsNullOrWhiteSpace(fileConfig.TrueVaultTestVault))
                    {
                        this.TrueVaultApiKey = fileConfig.TrueVaultApiKey;
                        this.TrueVaultTestVault = fileConfig.TrueVaultTestVault;
                        configured = true;
                    }
                }

            if (!configured)
            {
                this.TrueVaultApiKey = ConfigurationManager.AppSettings["TrueVaultApiKey"];
                this.TrueVaultTestVault = ConfigurationManager.AppSettings["TrueVaultTestVault"];
            }

        }

        public static TestConfig Instance
        {
            get { return instance; }
        }

        public string TrueVaultApiKey { get; set; }
        public string TrueVaultTestVault { get; set; }
    }
}
