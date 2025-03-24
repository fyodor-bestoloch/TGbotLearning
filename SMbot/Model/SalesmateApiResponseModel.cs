using System.Text.Json.Serialization;

namespace SMbot.Model;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class SalesmateApiResponseModel
{
    public string Status { get; set; }
    public DataWrapper Data { get; set; }
}

public class DataWrapper
{
    public List<Contact> Data { get; set; }
    public int SelectedView { get; set; }
    public int TotalRows { get; set; }
    public int TotalPages { get; set; }
}

public class Contact
{
    public int Id { get; set; }
    public string Name { get; set; }
}



