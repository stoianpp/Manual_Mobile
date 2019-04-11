using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Mob_Manual.Classes
{
    public class Product
    {
        public int Id { get; set; }
        public int SubCategoryId { get; set; }
        public string SubCategory { get; set; }
        public string Name { get; set; }
        public ImageSource Photo { get; set; }
        public byte[] Image { get; set; }
        public string LangText { get; set; }
    }
}
