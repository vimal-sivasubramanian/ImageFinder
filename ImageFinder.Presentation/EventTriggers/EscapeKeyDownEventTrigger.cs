using Microsoft.Xaml.Behaviors;
using System;
using System.Windows.Input;

namespace ImageFinder.Presentation.EventTriggers
{
    public class EscapeKeyDownEventTrigger : EventTrigger
    {
        public EscapeKeyDownEventTrigger() : base("KeyDown")
        { }

        protected override void OnEvent(EventArgs eventArgs)
        {
            if (eventArgs is KeyEventArgs keyEventArgs && 
                keyEventArgs.Key == Key.Escape)
                InvokeActions(eventArgs);
        }
    }
}
