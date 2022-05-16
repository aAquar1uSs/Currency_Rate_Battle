namespace CRBClient.Helpers;
using System.ComponentModel.DataAnnotations;

public class WebServerOptions
{
    [Required]
    public string BaseUrl { get; set; }
}
