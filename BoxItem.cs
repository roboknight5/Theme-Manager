using Gtk;

namespace Gtk_Theme_Manager
{
    public class BoxItem : HBox
    {
        public string itemName { get; set; }
        public RadioButton radioButton;

        public BoxItem(string itemName,RadioButton radioButtonGroup)
        {
            itemName=itemName.Replace("_", "__");
            this.itemName = itemName;
            HeightRequest = 50;
            Label label=new Label(itemName);
            radioButton=new RadioButton(radioButtonGroup,"");

            PackStart(label,false,false,5);
            PackEnd(radioButton,false,false,5);
        }

        public override string ToString()
        {
            return itemName;
        }
    }
}