using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace BestBy
{
    public class Recipe
    {
        [JsonProperty("title")]
        public string Name { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("summary")]
        public string Summary { get; set; }
        [JsonProperty("servings")]
        public int Servings { get; set; }
        [JsonProperty("readyInMinutes")]
        public int Time { get; set; }
        [JsonProperty("image")]
        public string ImageURL { get; set; }
        [JsonProperty("sourceUrl")]
        public string Source { get; set; }
        [JsonProperty("sourceName")]
        public string SourceName { get; set; }
        [JsonProperty("extendedIngredients")]
        public List<JObject> Ingredients { get; set; }
        [JsonProperty("analyzedInstructions")]
        public List<JObject> Instructions { get; set; }
        public override string ToString()
        {
            return string.Format("{0}", Name);
        }
    }
}
