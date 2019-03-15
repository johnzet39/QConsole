using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QConsole.Commands
{
    class PropertyCommand
    {
        static PropertyCommand()
        {
            // Инициализация команды
            InputGestureCollection inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.T, ModifierKeys.Control, "Ctrl + T"));
            Property = new RoutedUICommand("Property", "Property", typeof(PropertyCommand), inputs);
        }

        public static RoutedUICommand Property { get; private set; }
    }
}
