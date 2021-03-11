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

namespace Gtk_Theme_Manager
{
    class Program : Window
    {
        public Program(string title) : base(title)
        {
            
            SetDefaultSize(500,500);
            BashHandler bashHandler=BashHandler.Instance;


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
            HeaderBar headerBar = new HeaderBar();
            headerBar.Title = title;
            headerBar.ShowCloseButton = true;
            headerBar.Visible = true;
            Button menuButton = new Button();
            menuButton.Image = Gtk.Image.NewFromIconName("view-refresh-symbolic", IconSize.LargeToolbar);
            menuButton.Visible = true;
            Image image=new Image();
            image.File = "/usr/share/icons/Gtk-Theme-Manager-icon.png";
            image.Visible = true;    
            
            
            

            headerBar.PackStart(image);
            headerBar.PackEnd(menuButton);
            Titlebar = headerBar;


            
    
            StackSidebar stackSidebar=new StackSidebar();
            HBox hBox=new HBox();
            hBox.PackStart(stackSidebar,false,true,0);
            stackSidebar.WidthRequest = 120;
            Stack stack=new Stack();
            stackSidebar.Stack = stack;
            stack.TransitionType = StackTransitionType.SlideUpDown;
            stack.TransitionDuration=(1000);
            ThemeUi GtkTheme=new ThemeUi(ThemeMode.GtkTheme);
            ThemeUi IconTheme=new ThemeUi(ThemeMode.IconTheme);
            ThemeUi ShellTheme=new ThemeUi(ThemeMode.ShellTheme);
            ThemeUi CursorTheme=new ThemeUi(ThemeMode.CursorTheme);
            stack.RedrawOnAllocate = true;

            menuButton.Clicked += (sender, args) =>
            {
                bashHandler.Reload();
                GtkTheme.Reload();
                IconTheme.Reload();
                CursorTheme.Reload();
                ShellTheme.Reload();


            }; 
            
            stack.AddTitled(GtkTheme,"Themes","Themes" );
            stack.AddTitled(IconTheme,"IconTheme","Icons" );
            stack.AddTitled(ShellTheme,"ShellTheme","Shells" );
            stack.AddTitled(CursorTheme,"CursorTheme","Cursors" );

            stack.ShowAll();
            hBox.PackStart(new Separator(Orientation.Vertical),false,false,0 );
            
            hBox.PackStart(stack,true,true,0);
            
            // Add(box);
            // ListBox listBox=new ListBox();
            // ListBoxRow themeRow=new ListBoxRow();
            // themeRow.Child= (new Label
            // {
            //     Text = "Theme",
            //     
            //     
            // });
            // ListBoxRow iconRow=new ListBoxRow();
            // iconRow.Child= (new Label("Icons"));
            // ListBoxRow shellRow=new ListBoxRow();
            // shellRow.Child =new Label("Shell");
            // listBox.Add(themeRow);
            // listBox.Add(iconRow);
            // listBox.Add(shellRow);
            // listBox.ShowAll();
            // Add(listBox);
            
            // HBox hBox=new HBox();
            // hBox.Add(new ThemeUi(ThemeMode.GtkTheme));
            // hBox.Add(new ThemeUi(ThemeMode.IconTheme));
            // hBox.Add(new ThemeUi(ThemeMode.ShellTheme));
            hBox.ShowAll();
            //
            // Add(hBox);
            Add(hBox);
            Show();
            
           

            // ListBox  listBox= new ListBox();
            // listBox.Hexpand = true;
            // ListBoxRow listBoxRow = new ListBoxRow();
            // listBox.Add(listBoxRow);
            // HBox hBox = new HBox();
            // hBox.PackStart(new Label("hi"),true,true,0);
            // listBoxRow.Add(hBox);

            // box.Add(listBox);
            // Gtk.ListStore themeListStore = new Gtk.ListStore (typeof (string));
            // //
            // // // CellAreaBox area = new CellAreaBox();
            //
            // EntryCompletion themeBoxcompletion = new EntryCompletion();
            // themeBoxcompletion.TextColumn = 0;
            // themeBoxcompletion.Model = themeListStore;
            //
            //
            // Label themeBoxLabel = new Label("Application Theme");
            // themeBoxLabel.Halign = Align.Start;
            //
            // ComboBoxText themeBox = new Gtk.ComboBoxText();
            // // themeBox.Entry.Completion = themeBoxcompletion;
            // // // CellRendererText renderer = new CellRendererText();
            // // // area.PackStart(renderer,true,true,true);
            //
            // themeBox.EntryTextColumn = 0;
            // themeBox.WrapWidth = 5;
            // // themeBox.Entry.Completion = themeBoxcompletion;
            //
            // for (int i = 0; i < bashHandler.ThemeList.Count; i++)
            // {
            //     themeListStore.AppendValues(bashHandler.ThemeList[i]);
            //     // listBox.Add(new Label(spltOutput[i]));
            //     themeBox.AppendText(bashHandler.ThemeList[i]);
            // }
            //
            // for (int i = 0; i < bashHandler.ThemeList.Count; i++)
            // {
            //     
            //     if (bashHandler.ThemeList[i].Equals(bashHandler.GetTheme()))
            //     {
            //         themeBox.Active = i;
            //         break;
            //     }
            // }
            //
            // themeBox.Changed += (e,s) =>
            // {
            //     
            //     ComboBoxText cb = (ComboBoxText) e;
            //     bashHandler.ChangeTheme(cb.ActiveText);
            // };
            //
            // // themeBox.Changed += (e, s) =>
            // // {
            // //     
            // //     TreeIter iter;
            // //     GLib.Value row = new GLib.Value();
            // //     var cb = (ComboBox) e;
            // //     cb.Model.GetIterFirst(out iter);
            // //     cb.Model.GetValue(iter,5 , ref row);
            // //     Console.WriteLine(e);
            // //
            // //
            // // };
            // //     
            //
            //
            //
            // Gtk.ListStore iconListStore = new Gtk.ListStore (typeof (string));
            //
            // EntryCompletion iconCompletion = new EntryCompletion();
            // iconCompletion.TextColumn = 0;
            // iconCompletion.Model = iconListStore;
            //
            // Label iconBoxLabel = new Label("Icon Theme");
            // iconBoxLabel.Halign = Align.Start;
            // ComboBoxText iconThemeBox = new ComboBoxText();
            // // // CellRendererText renderer = new CellRendererText();
            // // // area.PackStart(renderer,true,true,true);
            //
            // iconThemeBox.EntryTextColumn = 0;
            // iconThemeBox.WrapWidth = 5;
            //
            // for (int i = 0; i < bashHandler.IconList.Count; i++)
            // {
            //     iconListStore.AppendValues(bashHandler.IconList[i]);
            //     // listBox.Add(new Label(spltOutput[i]));
            //     iconThemeBox.AppendText(bashHandler.IconList[i]);
            // }
            //
            // for (int i = 0; i < bashHandler.IconList.Count; i++)
            // {
            //     if (bashHandler.IconList[i].Equals(bashHandler.GetIconTheme()))
            //     {
            //         iconThemeBox.Active = i;
            //         break;
            //     }
            // }
            //
            // iconThemeBox.Changed += (e, s) =>
            // {
            //     var cb = (ComboBoxText) e;
            //     bashHandler.ChangeIcon(cb.ActiveText);
            // };
            //
            // Gtk.ListStore shellListStore = new Gtk.ListStore (typeof (string));
            //
            // Label shellBoxLabel = new Label("Shell Theme");
            // shellBoxLabel.Halign = Align.Start;
            //
            // ComboBoxText shellThemeBox = new ComboBoxText();
            // // // CellRendererText renderer = new CellRendererText();
            // // // area.PackStart(renderer,true,true,true);
            //
            // shellThemeBox.EntryTextColumn = 0;
            // shellThemeBox.WrapWidth = 5;
            //
            // for (int i = 0; i < bashHandler.ShellList.Count; i++)
            // {
            //     shellListStore.AppendValues(bashHandler.ShellList[i]);
            //     // listBox.Add(new Label(spltOutput[i]));
            //     shellThemeBox.AppendText(bashHandler.ShellList[i]);
            // }
            //
            // for (int i = 0; i < bashHandler.ShellList.Count; i++)
            // {
            //     if (bashHandler.ShellList[i].Equals(bashHandler.GetShellTheme()))
            //     {
            //         shellThemeBox.Active = i;
            //         break;
            //     }
            // }
            //
            // shellThemeBox.Changed += (e, s) =>
            // {
            //     var cb = (ComboBoxText) e;
            //     bashHandler.ChangeShell(cb.ActiveText);
            // };
            //
            //
            // // iconThemeBox.Entry.Completion = iconCompletion;
            //
            // //
            // // ListBox listBox = new ListBox();
            // // VBox vBox = new VBox();
            // // vBox.Add(listBox);
            // // var swin =new Gtk.ScrolledWindow();
            // // vBox.ShowAll();
            // // swin.Add(vBox);
            // // swin.Vexpand = true;
            // //
            // //
            // //
            // //
            // // TreeView view = new TreeView(listStore);
            // //
            // // CellRendererText rendererText = new CellRendererText();
            // // TreeViewColumn viewColumn = new TreeViewColumn("Text", rendererText, 0);
            // // view.AppendColumn(viewColumn);
            // //
            //
            // // themeBox.WrapWidth = 5;
            // // themeBox.
            //
            //     // themeBox.
            //
            //
            //
            //
            // box.Add(themeBoxLabel);
            // box.Add(themeBox);
            // box.Add(iconBoxLabel);
            // box.Add(iconThemeBox);
            // box.Add(shellBoxLabel);
            // box.Add(shellThemeBox);
            // // box.Add(view);
            // // box.Add(swin);
            // // box.Add(listBox);
            // box.ShowAll();
            // Add(box);
            // ShowAll();
        }

       

        static void Main(string[] args)
        {
            
            Gtk.Application.Init();
            new Program("Theme Manager");
            Gtk.Application.Run();
        }
    }
}