
using System.Text.Json.Serialization;

namespace SMbot.Model;



public class SalesmateApiResponseModel
{
    [JsonPropertyName("Status")]
    public string Status { get; set; }

    [JsonPropertyName("Data")]
    public DataWrapper Data { get; set; }
}

public class DataWrapper
{
    [JsonPropertyName("data")]
    public List<Contact> Data { get; set; }

    [JsonPropertyName("selectedView")]
    public int SelectedView { get; set; }

    [JsonPropertyName("totalRows")]
    public int TotalRows { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }
}

public class Contact
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}



