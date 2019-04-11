using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mob_Manual.Classes
{
    public class EndUserViewModel
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("SubCategoryId")]
        public int SubCategoryId { get; set; }
        [JsonProperty("SubCategory")]
        public string SubCategory { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Image")]
        public byte[] Image { get; set; }
        [JsonProperty("LangText")]
        public string LangText { get; set; }
    }
}
