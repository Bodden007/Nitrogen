using Avalonia.Controls;
using Avalonia.Input;
using System.Collections.Generic;

namespace Nitrogen.Views.Common
{
    internal class HotKeyFocusMap
    {
        private readonly Dictionary<Key, TextBox> _map = new();

        public void Add(Key key, TextBox textBox)
        {
            _map[key] = textBox;

            textBox.GotFocus += (_, _) => textBox.SelectAll();

            textBox.PointerPressed += (_, e) =>
            {
                textBox.Focus();
                textBox.SelectAll();
                e.Handled = true;
            };
        }

        public bool Focus(Key key)
        {
            if (!_map.TryGetValue(key, out var textBox))
                return false;

            textBox.Focus();
            textBox.SelectAll();
            return true;
        }
    }
}
