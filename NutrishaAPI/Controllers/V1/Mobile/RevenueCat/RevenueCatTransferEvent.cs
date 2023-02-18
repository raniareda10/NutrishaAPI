using System.Collections.Generic;
using Newtonsoft.Json;

namespace NutrishaAPI.Controllers.V1.Mobile.RevenueCat
{
    public class RevenueCatTransferEvent : BaseRevenueCatEvent
    {
        [JsonProperty("transferred_from")] public IEnumerable<string> TransferredFrom { get; set; }
        [JsonProperty("transferred_to")] public  IEnumerable<string> TransferredTo { get; set; }
    }
}