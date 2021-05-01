using Stack = Gtk.Stack;
using Window = Gtk.Window;
using Settings = Gtk.Settings;

using Gtk;
using Gdk;
using System;

using ThemeManager.Layout;

namespace ThemeManager
{
    public class Program : Window
    {
        [System.Runtime.Versioning.SupportedOSPlatform("linux")]
        private static void Main(string[] args)
        {
            Application.Init();
            new Program("Theme Manager");
            Application.Run();
        }
        
        // You should probably refactor this into something smaller
        private Program(string title) : base(title)
        {
            var settings= new AppSettings();
            var settingsData = settings.GetSettings();
            Settings.Default.ApplicationPreferDarkTheme = settingsData.DarkModeEnabled;
            
            SetDefaultSize(500, 500);
            
            var bashHandler = BashHandler.Instance;

            try { SetIconFromFile("/usr/share/icons/Gtk-Theme-Manager-icon.png"); }
            catch (Exception e) { Console.WriteLine(e); }

            DeleteEvent += delegate { Gtk.Application.Quit(); };

            var headerBar = new HeaderBar
            {
                Title = title,
                ShowCloseButton = true,
                Visible = true
            };

            Button menuButton = new Button
            {
                Image = Image.NewFromIconName("view-refresh-symbolic", IconSize.LargeToolbar),
                Visible = true  
            };

            var settingPopOverMenu= new SettingPopOverMenu();

            var themeButton = new MenuButton
            {
                Image = Image.NewFromIconName("open-menu-symbolic", IconSize.LargeToolbar),
                Visible = true,
                Popover = settingPopOverMenu

            };

            Pixbuf pixbuf=null;
            try
            {
                pixbuf=new Pixbuf( "/usr/share/icons/Gtk-Theme-Manager-icon.png");
                pixbuf=pixbuf.ScaleSimple(32, 32 ,InterpType.Bilinear);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            var image = new Image
            {
                Pixbuf = pixbuf,
                Visible = true
            };

            headerBar.PackStart(image);
            headerBar.PackEnd(menuButton);
            headerBar.PackEnd(themeButton);
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
            var layoutUi = new LayoutUI();
            stack.RedrawOnAllocate = true;

            menuButton.Clicked += (sender, args) =>
            {
                bashHandler.Reload();
                GtkTheme.Reload();
                IconTheme.Reload();
                CursorTheme.Reload();
                ShellTheme.Reload();
                layoutUi.Reload();
            };

            stack.AddTitled(GtkTheme, "Themes", "Themes");
            stack.AddTitled(IconTheme, "IconTheme", "Icons");
            stack.AddTitled(ShellTheme, "ShellTheme", "Shells");
            stack.AddTitled(CursorTheme, "CursorTheme", "Cursors");
            stack.AddTitled(layoutUi,"LayoutUI","Layouts");

            stack.ShowAll();
            hBox.PackStart(new Separator(Orientation.Vertical), false, false, 0);
            hBox.PackStart(stack, true, true, 0);

            hBox.ShowAll();
            Add(hBox);
            Show();
        }
    }
}
