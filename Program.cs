using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Gdk;
using GLib;
using Gtk;
using Gtk_Theme_Manager;
using Process = System.Diagnostics.Process;
using Settings = Gtk.Settings;
using Stack = Gtk.Stack;
using Switch = Gtk.Switch;
using Window = Gtk.Window;

namespace ThemeManager
{
    public class Program : Window
    {
        Program(string title) : base(title)
        {
            AppSettings settings= new AppSettings();
            SettingsData settingsData = settings.GetSettings();
            Settings.Default.ApplicationPreferDarkTheme = settingsData.DarkModeEnabled;
            
            SetDefaultSize(500, 500);
            
            BashHandler bashHandler = BashHandler.Instance;

            try { SetIconFromFile("/usr/share/icons/Gtk-Theme-Manager-icon.png"); }
            catch (Exception e) { Console.WriteLine(e); }

            DeleteEvent += delegate { Gtk.Application.Quit(); };

            HeaderBar headerBar = new HeaderBar();
            headerBar.Title = title;
            headerBar.ShowCloseButton = true;
            headerBar.Visible = true;

            Button menuButton = new Button();
            menuButton.Image = Image.NewFromIconName("view-refresh-symbolic", IconSize.LargeToolbar);
            menuButton.Visible = true;

            SettingPopOverMenu settingPopOverMenu= new SettingPopOverMenu();

            MenuButton themeButton = new MenuButton();
            themeButton.Image = Image.NewFromIconName("open-menu-symbolic", IconSize.LargeToolbar);
            themeButton.Visible = true;
            themeButton.Popover = settingPopOverMenu;

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
            
            Image image=new Image();
            image.Pixbuf = pixbuf;
            image.Visible = true;
    
            headerBar.PackStart(image);
            headerBar.PackEnd(menuButton);
            headerBar.PackEnd(themeButton);
            Titlebar = headerBar;

            StackSidebar stackSidebar = new StackSidebar();
            HBox hBox = new HBox();
            hBox.PackStart(stackSidebar, false, true, 0);
            stackSidebar.WidthRequest = 120;

            Stack stack = new Stack();
            stackSidebar.Stack = stack;
            stack.TransitionType = StackTransitionType.SlideUpDown;
            stack.TransitionDuration = (1000);

            ThemeUI GtkTheme = new ThemeUI(ThemeMode.GtkTheme);
            ThemeUI IconTheme = new ThemeUI(ThemeMode.IconTheme);
            ThemeUI ShellTheme = new ThemeUI(ThemeMode.ShellTheme);
            ThemeUI CursorTheme = new ThemeUI(ThemeMode.CursorTheme);
            LayoutUI layoutUi = new LayoutUI();
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

        [System.Runtime.Versioning.SupportedOSPlatform("linux")]
        static void Main(string[] args)
        {
            Gtk.Application.Init();
            new Program("Theme Manager");
            Gtk.Application.Run();
        }
    }
}
