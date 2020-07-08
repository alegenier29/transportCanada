using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TransportCanada.API3.Models
{
    public class Recall
    {
        [Key]
        public string recallNumber { get; set; }
        public string manufacterName { get; set; }
        public string makeName { get; set; }
        public string modelName { get; set; }
        public string recallYear { get; set; }

        [JsonProperty(PropertyName = "MANUFACTURER_RECALL_NO_TXT")] //16S17
        public string MANUFACTURER_RECALL_NO_TXT { get; set; }

        [JsonProperty(PropertyName = "CATEGORY_ETXT")] //"Truck - Med. & H.D."
        public string CATEGORY_ETXT { get; set; }

        [JsonProperty(PropertyName = "CATEGORY_FTXT")]// "Camion - usage moyen et usage intensif"
        public string CATEGORY_FTXT { get; set; }

        [JsonProperty(PropertyName = "SYSTEM_TYPE_ETX")]// "Tires"
        public string SYSTEM_TYPE_ETX { get; set; }

        [JsonProperty(PropertyName = "SYSTEM_TYPE_FTXT")]//"Pneus"
        public string SYSTEM_TYPE_FTXT { get; set; }

    }
}
