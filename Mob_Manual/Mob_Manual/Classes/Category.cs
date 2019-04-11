using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mob_Manual.Classes
{
    public class Category
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Image")]
        public byte[] Image { get; set; }
    }
}
