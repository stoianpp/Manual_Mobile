using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Mob_Manual.Classes
{
    public class SubCategory
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Image")]
        public byte[] Image { get; set; }
        [JsonProperty("CategoryId")]
        public int CategoryId { get; set; }
        [JsonProperty("Category")]
        public string Category { get; set; }
        public ImageSource Photo { get; set; }
    }
}
