using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mob_Manual.Classes
{
    public class DataIn
    {
        [JsonProperty("data")]
        public List<EndUserViewModel> data = new List<EndUserViewModel>();
        [JsonProperty("cats")]
        public List<Category> cats = new List<Category>();
        [JsonProperty("subCats")]
        public List<SubCategory> subCats = new List<SubCategory>();
    }
}
