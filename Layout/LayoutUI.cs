using Gtk;

namespace Gtk_Theme_Manager
{
    public class LayoutUI : VBox
    {
        public LayoutUI()
        {
            Stack stack = new Stack();
            ViewLayoutUI viewLayoutUi = new ViewLayoutUI();
            AddLayoutUI addLayoutUi = new AddLayoutUI();
            
            stack.AddTitled(viewLayoutUi,"ViewLayouts","View Layouts");
            stack.AddTitled(addLayoutUi,"AddLayout","Add Layouts");
            StackSwitcher stackSwitcher = new StackSwitcher();
            stackSwitcher.Stack = stack;
            PackStart(stackSwitcher,false,false,0);
            PackStart(stack,false,false,0);
            Halign = Align.Center;
            ShowAll();

        }
    }
}