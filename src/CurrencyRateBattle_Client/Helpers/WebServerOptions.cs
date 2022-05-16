namespace CRBClient.Helpers;
using System.ComponentModel.DataAnnotations;

public class WebServerOptions
{
    public const string SectionName = "WebServer";
    [Required]
    public string BaseUrl { get; set; }
    public string RoomsURL { get; set; }
}
