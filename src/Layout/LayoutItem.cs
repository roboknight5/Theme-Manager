using Gtk;

namespace ThemeManager.Layout
{
    public class LayoutItem :VBox
    {
        public LayoutItem(string theme,string iconTheme,string _cursorTheme,string _shellTheme)
        {
            Theme = theme;
            IconTheme = iconTheme;
            CursorTheme = _cursorTheme;
            ShellTheme = _shellTheme;

            var vBox = new HBox();
            vBox.PackStart(new Label("Theme: "+theme.Replace("_","__")) ,false,false,5);
            vBox.PackEnd(new Label("Icon: "+iconTheme.Replace("_", "__")), false, false, 5);
            
            var vBox2 = new HBox();
            vBox2.PackStart(new Label("Cursor: "+_cursorTheme.Replace("_","__")) ,false,false,5);
            vBox2.PackEnd(new Label("Shell: "+_shellTheme.Replace("_", "__")), false, false, 5);
            
            DeleteButton = new Button("Delete");
            
            PackStart(vBox,false,false,5);
            PackStart(vBox2,false,false,5);
            PackEnd(DeleteButton,false,false,0);
        }

        public string Theme { get; }
        public string IconTheme { get; }
        public string CursorTheme { get; }
        public string ShellTheme { get; }
        
        public Button DeleteButton { get; }
        
        public override string ToString()
        {
            return $"Theme: {Theme} Icon: {IconTheme} Cursor: {CursorTheme} Shell {ShellTheme}";
        }
    }
}