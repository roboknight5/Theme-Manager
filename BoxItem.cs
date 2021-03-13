using Gtk;

namespace Gtk_Theme_Manager
{
    public class BoxItem : HBox
    {
        public string ItemName { get; set; }
        public RadioButton RadioButton;

        public BoxItem(string itemName, RadioButton radioButtonGroup)
        {
            this.ItemName = itemName;
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
