using System;
using System.Threading;
using System.Windows.Threading;

namespace ExcelReporting.UI.Pages.Pko
{
    public static class AsyncTaskHelper
    {
        private static Dispatcher _dispatcher;

        public static void CallAsync<TResult>(Func<TResult> getResult, Action<TResult> uiAction)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                var result = getResult();
                UIThread(() => uiAction(result));
            });
        }
        
        public static void CallAsync(Action action)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                action();
            });
        }

        public static void UIThread(Action a)
        {
            if (_dispatcher == null)
                _dispatcher = Dispatcher.CurrentDispatcher;

            //this is to make sure that the event is raised on the correct Thread
            _dispatcher.Invoke(a);
        }
    }
}