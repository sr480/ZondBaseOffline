﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ZondBase
{
    public static class SynchronisationHelper
    {
        private static Window _Window;
        public static void RegisterParentWindow(Window win)
        {
            _Window = win;
        }
        private static MessageBoxResult lastResult;
        public static MessageBoxResult ShowMessage(Window owner, string text, string caption, MessageBoxButton buttons, MessageBoxImage icon)
        {
            Action action = new Action(() =>
            lastResult = MessageBox.Show(owner, text, caption, buttons, icon));

            Invoke(action);

            return lastResult;
        }
        public static MessageBoxResult ShowMessage(string text, string caption, MessageBoxButton buttons, MessageBoxImage icon)
        {
            return ShowMessage(_Window, text, caption, buttons, icon);
        }

        public static void Synchronise(Action action)
        {
            if (App.GlobalDispatcher == System.Windows.Threading.Dispatcher.CurrentDispatcher)
                action();
            else
                App.GlobalDispatcher.BeginInvoke(action);
        }

        private static void Invoke(Action action)
        {
            if (App.GlobalDispatcher == System.Windows.Threading.Dispatcher.CurrentDispatcher)
                action();
            else
                App.GlobalDispatcher.Invoke(action);
        }
    }
}
