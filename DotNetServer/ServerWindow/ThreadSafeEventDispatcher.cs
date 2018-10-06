using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using Cge.Server.Entities;

namespace Cge.ServerWindow
{
    public class ThreadSafeEventDispatcher
    {
        private readonly Dispatcher _dispatcher;

        public ThreadSafeEventDispatcher()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        public Action<T> Create<T>(Action<T> target)
        {
            return obj => _dispatcher.Invoke(() => target(obj));
        }
    }
}
