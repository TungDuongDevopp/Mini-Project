namespace UI.Handler;

    public interface IConsoleHandler<T>
    {
        T Input();
        void Output(T entity);
        void OutputList(IEnumerable<T> list);
        void Run();
    }

