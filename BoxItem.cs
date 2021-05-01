using Gtk;

namespace ThemeManager
{
    public class BoxItem : HBox
    {
        public string ItemName { get; set; }
        public ThemeMode ItemType { get; }
        public RadioButton RadioButton;

        public BoxItem(string itemName, RadioButton radioButtonGroup,ThemeMode itemType)
        {
            this.ItemName = itemName;
            ItemType = itemType;
            HeightRequest = 50;

            Label label = new Label(itemName.Replace("_", "__"));
            RadioButton = new RadioButton(radioButtonGroup, "");

            
            PackStart(label, false, false, 5);
            PackEnd(RadioButton, false, false, 5);
        }

        public override string ToString()
            => ItemName;
    }
}