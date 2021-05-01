using Gtk;

namespace ThemeManager.Layout
{
    public class LayoutUI : ScrolledWindow
    {
        public LayoutUI()
        {
            Initialize();
        }

        public void Reload() => Initialize();
        
        private void Initialize()
        {
            foreach (var widget in Children)
            {
                Remove(widget);
            }
            
            // Stack stack = new Stack();
            // ViewLayoutUI viewLayoutUi = new ViewLayoutUI();
            // AddLayoutUI addLayoutUi = new AddLayoutUI();
            //
            // stack.AddTitled(viewLayoutUi,"ViewLayouts","View Layouts");
            // stack.AddTitled(addLayoutUi,"AddLayout","Add Layouts");
            // StackSwitcher stackSwitcher = new StackSwitcher();
            // stackSwitcher.Stack = stack;
            // PackStart(stackSwitcher,false,false,0);
            // PackStart(stack,false,false,0);
            var layoutHandler = new LayoutHandler();

            var box = new ListBox
            {
                SelectionMode = SelectionMode.None,
            };

            var button = new Button
            {
                Label = "Add Layout"
            };
            
            var vBox = new VBox();
            vBox.PackStart(button,false,false,5);

            foreach (var layoutItem in layoutHandler.LayoutItems)
            {
                var row = new ListBoxRow();
                var eventBox = new EventBox();
                
                row.Child = layoutItem;
                eventBox.Add(row);
                eventBox.ShowAll();
                layoutItem.ShowAll();
                box.Add(eventBox);
                
                layoutItem.DeleteButton.Clicked += (_, _) =>
                {
                    eventBox.Remove(row);
                    box.Remove(eventBox);
                    layoutHandler.DeleteLayout(layoutItem);
                }; 


                eventBox.ButtonPressEvent += (_, _) =>
                {
                    layoutHandler.ApplyLayout(layoutItem);
                    box.UnselectAll();
                };
            }

            button.Clicked += (_, _) =>
            {
                var layoutItem = layoutHandler.AddLayoutItem();
                if (layoutItem != null)
                {
                    var row = new ListBoxRow();
                    var eventBox = new EventBox();
                    row.Child = layoutItem;
                    eventBox.Add(row);
                    eventBox.ShowAll();
                    layoutItem.ShowAll();
                    box.Add(eventBox);

                    layoutItem.DeleteButton.Clicked += (_, _) =>
                    {
                        eventBox.Remove(row);
                        box.Remove(eventBox);
                        layoutHandler.DeleteLayout(layoutItem);
                        
                    }; 

                    eventBox.ButtonPressEvent += (_, _) =>
                    {
                        layoutHandler.ApplyLayout(layoutItem);
                        box.UnselectAll();
                    };
                   
                    // vBox.PackStart(eventBox, false, false, 5);
                }
            };

            box.ShowAll();
            vBox.Add(box);
            Add(vBox);
            ShowAll();
        }
    }
}