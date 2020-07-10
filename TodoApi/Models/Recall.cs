using Newtonsoft.Json;

namespace TransportCanada.API3.Models
{
   
    public class Recall
    {
        
        public string recallNumber { get; set; }
        
        public string manufactureName { get; set; }
        
        public string makeName { get; set; }
      
        public string modelName { get; set; }
       
        public string recallYear { get; set; }

        [JsonProperty(PropertyName = "MANUFACTURER_RECALL_NO_TXT")] 
        public string MANUFACTURER_RECALL_NO_TXT { get; set; }

        [JsonProperty(PropertyName = "CATEGORY_ETXT")] 
        public string CATEGORY_ETXT { get; set; }

        [JsonProperty(PropertyName = "CATEGORY_FTXT")]
        public string CATEGORY_FTXT { get; set; }

        [JsonProperty(PropertyName = "SYSTEM_TYPE_ETXT")]
        public string SYSTEM_TYPE_ETXT { get; set; }

        [JsonProperty(PropertyName = "SYSTEM_TYPE_FTXT")]
        public string SYSTEM_TYPE_FTXT { get; set; }

    }
}
