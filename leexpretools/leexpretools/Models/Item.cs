using System;
using Xamarin.Forms;

namespace leexpretools.Models {
    public class Item {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Expires { get; set; }
        public string Ean { get; set; }
        public string Description { get; set; }
        public Area Area { get; set; }
        public ItemGroup ItemGroup { get; set; }
        public bool Expired { get; set; }
        public float Price { get; set; }
        public string Zone { get; set; }
        public Color FlagColor { get; set; }
    }
}