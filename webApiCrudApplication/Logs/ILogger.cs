namespace webApiCrudApplication.Logs
{
    public interface  ILogger
    {
        void Log(string message, string stackTrace);
    }
}
