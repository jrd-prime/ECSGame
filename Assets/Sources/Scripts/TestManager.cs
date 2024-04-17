using Sources.Scripts.Annotation;
using Sources.Scripts.TestConfig;
using AppContext = Sources.Scripts.DI.AppContext;

namespace Sources.Scripts
{
    public class TestManager
    {
        [JInject] private AppContext context;

        [JInject] private IServiceConfig _serviceConfig;

        private void Start()
        {
            // context.Desc();

            // a.show();
        }

        private void FixedUpdate()
        {
            context.Desc();
        }
    }
}