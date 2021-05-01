using System.Threading;
using Gtk;
using ThemeManager;

namespace Gtk_Theme_Manager
{
    public class LayoutUI : ScrolledWindow
    {
        public LayoutUI()
        {
            Initalize();

        }

        public void Reload() => Initalize();
        
        

        private void  Initalize()
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
            LayoutHandler layoutHandler = new LayoutHandler();
            
            ListBox box = new ListBox();
            box.SelectionMode = SelectionMode.None;

            
            Button button = new Button();
            button.Label = "Add Layout";
            
            VBox vBox = new VBox();
            vBox.PackStart(button,false,false,5);

            foreach (var layoutItem in layoutHandler.LayoutItems)
            {
                ListBoxRow row = new ListBoxRow();
                EventBox eventBox = new EventBox();
                row.Child = layoutItem;
                eventBox.Add(row);
                eventBox.ShowAll();
                layoutItem.ShowAll();
                box.Add(eventBox);
                
                layoutItem.DeleteButton.Clicked += (o, eventArgs) =>
                {
                    eventBox.Remove(row);
                    box.Remove(eventBox);
                    layoutHandler.DeleteLayout(layoutItem);
                        
                }; 


                eventBox.ButtonPressEvent += (o, eventArgs) =>
                {
                    layoutHandler.ApplyLayout(layoutItem);
                    box.UnselectAll();
                };
                
                
            }

            button.Clicked += (sender, args) =>
            {
                var layoutItem = layoutHandler.AddLayoutItem();
                if (layoutItem != null)
                {
                    ListBoxRow row = new ListBoxRow();
                    EventBox eventBox = new EventBox();
                    row.Child = layoutItem;
                    eventBox.Add(row);
                    eventBox.ShowAll();
                    layoutItem.ShowAll();
                    box.Add(eventBox);

                    layoutItem.DeleteButton.Clicked += (o, eventArgs) =>
                    {
                        eventBox.Remove(row);
                        box.Remove(eventBox);
                        layoutHandler.DeleteLayout(layoutItem);
                        
                    }; 

                    eventBox.ButtonPressEvent += (o, eventArgs) =>
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