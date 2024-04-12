namespace Sources.Scripts.ServiceConfig
{
    public class TestServiceConfigurator : IServiceConfigurator
    {
        public T GetImpl<T>() where T : class
        {
            throw new System.NotImplementedException();
        }
    }
}