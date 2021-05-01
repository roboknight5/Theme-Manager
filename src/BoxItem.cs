using Gtk;

namespace ThemeManager
{
    public class BoxItem : HBox
    {
        public BoxItem(string itemName, RadioButton radioButtonGroup)
        {
            ItemName = itemName;
            HeightRequest = 50;

            var label = new Label(itemName.Replace("_", "__"));
            RadioButton = new RadioButton(radioButtonGroup, "");

            PackStart(label, false, false, 5);
            PackEnd(RadioButton, false, false, 5);
        }
        
        public string ItemName { get; }
        public RadioButton RadioButton { get; }
        
        public override string ToString() => ItemName;
    }
}
