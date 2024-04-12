namespace Sources.Scripts.ServiceConfig
{
    public interface IServiceConfigurator
    {
        public T GetImpl<T>() where T : class;
    }
}