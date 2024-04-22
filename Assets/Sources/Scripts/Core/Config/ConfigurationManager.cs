
using Sources.Scripts.TestConfig;

namespace Sources.Scripts.Core.Config
{
    public class ConfigurationManager
    {
        private static ConfigurationManager _configurationManager;
        public static ConfigurationManager I => _configurationManager ??= new ConfigurationManager();

        private ConfigurationManager()
        {
        }

        public IConfiguration GetConfiguration(IConfiguration configuration)
        {
            // TODO collect configurations and return dict for bind

            return new TestBinds();
        }
    }
}