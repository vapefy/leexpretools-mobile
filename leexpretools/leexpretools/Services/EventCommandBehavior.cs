using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace leexpretools.Services {
    public class EventToCommandBehavior : Behavior<Picker> {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(EventToCommandBehavior));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        protected override void OnAttachedTo(Picker bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.SelectedIndexChanged += OnSelectedIndexChanged;
        }

        protected override void OnDetachingFrom(Picker bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.SelectedIndexChanged -= OnSelectedIndexChanged;
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (Command != null) {
                var picker = (Picker)sender;
                if (Command.CanExecute(picker.SelectedItem)) {
                    Command.Execute(picker.SelectedItem);
                }
            }
        }
    }

}