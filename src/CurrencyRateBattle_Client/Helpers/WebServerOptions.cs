using System.ComponentModel.DataAnnotations;

namespace CRBClient.Helpers;
public class WebServerOptions
{
    public const string SectionName = "WebServer";
    [Required]
    [Url]
    public string? BaseUrl { get; set; }
    public string? RoomsURL { get; set; }
}
