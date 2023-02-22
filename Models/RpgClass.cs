using System.Text.Json.Serialization;

namespace dotnet_rpg.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))] //get the name instead of index
    public enum RpgClass
    {
        Fighter = 1,
        Mage = 2,
        Marksman=3,
        Tank=4,
        Support=5
    }
}