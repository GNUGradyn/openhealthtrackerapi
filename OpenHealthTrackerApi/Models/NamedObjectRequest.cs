namespace OpenHealthTrackerApi.Models;

public class NamedObjectRequest
{
    public NamedObjectRequest(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}