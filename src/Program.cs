using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Gdk;
using GLib;
using Gtk;
using Process = System.Diagnostics.Process;
using Stack = Gtk.Stack;
using Switch = Gtk.Switch;
using Window = Gtk.Window;

namespace ThemeManager
{
    internal class Program : Window
    {
        private static void Main(string[] args)
        {
            Gtk.Application.Init();
            new Program("Theme Manager");
            Gtk.Application.Run();
        }

        private Program(string title) : base(title)
        {
            Run(title);
        }

        private void Run(string title)
        {
            SetDefaultSize(500, 500);
            var bashHandler = BashHandler.Instance;

            try
            {
                SetIconFromFile("/usr/share/icons/Gtk-Theme-Manager-icon.png");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Box box = new Box(Orientation.Vertical,6);
            DeleteEvent += delegate { Gtk.Application.Quit(); };

            var headerBar = new HeaderBar()
            {
                Title = title,
                ShowCloseButton = true,
                Visible = true
            };

            var menuButton = new Button()
            {
                Image = Image.NewFromIconName("view-refresh-symbolic", IconSize.LargeToolbar),
                Visible = true
            };

            var image = new Image()
            {
                File = "/usr/share/icons/Gtk-Theme-Manager-icon.png",
                Visible = true
            };

            headerBar.PackStart(image);
            headerBar.PackEnd(menuButton);
            Titlebar = headerBar;

            var stackSidebar = new StackSidebar();
            var hBox = new HBox();
            hBox.PackStart(stackSidebar, false, true, 0);
            stackSidebar.WidthRequest = 120;

            var stack = new Stack();
            stackSidebar.Stack = stack;
            stack.TransitionType = StackTransitionType.SlideUpDown;
            stack.TransitionDuration = (1000);

            var GtkTheme = new ThemeUI(ThemeMode.GtkTheme);
            var IconTheme = new ThemeUI(ThemeMode.IconTheme);
            var ShellTheme = new ThemeUI(ThemeMode.ShellTheme);
            var CursorTheme = new ThemeUI(ThemeMode.CursorTheme);
            stack.RedrawOnAllocate = true;

            // removed sender, object because it's an unnecessary assignment
            menuButton.Clicked += (_, _) =>
            {
                bashHandler.Reload();
                GtkTheme.Reload();
                IconTheme.Reload();
                CursorTheme.Reload();
                ShellTheme.Reload();
            };

            stack.AddTitled(GtkTheme, "Themes", "Themes");
            stack.AddTitled(IconTheme, "IconTheme", "Icons");
            stack.AddTitled(ShellTheme, "ShellTheme", "Shells");
            stack.AddTitled(CursorTheme, "CursorTheme", "Cursors");

            stack.ShowAll();
            hBox.PackStart(new Separator(Orientation.Vertical), false, false, 0);
            hBox.PackStart(stack, true, true, 0);

            hBox.ShowAll();
            Add(hBox);
            Show();
        }
    }
}
