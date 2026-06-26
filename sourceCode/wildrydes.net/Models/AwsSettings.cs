namespace wildrydes.net.Models;

public class AwsSettings
{
    public string Region { get; set; } = string.Empty;
    public string CognitoIdentityPoolId { get; set; } = string.Empty;
    public LocationServiceSettings LocationService { get; set; } = new();
}

public class LocationServiceSettings
{
    public string MapName { get; set; } = string.Empty;
    public string PlaceIndexName { get; set; } = string.Empty;
}
