using CRBClient.Helpers;
using CRBClient.Models;
using Microsoft.Extensions.Options;
using CRBClient.Services.Interfaces;
using System.Net;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CRBClient.Services;

public class RatingService : IRatingService
{
    private readonly ICRBServerHttpClient _httpClient;
    private readonly WebServerOptions _options;
    private readonly ILogger<RatingService> _logger;

    public RatingService(ICRBServerHttpClient httpClient,
        IOptions<WebServerOptions> options, ILogger<RatingService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<RatingViewModel[]> GetUserRatings(CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(_options.GetUsersRatingURL ?? "", cancellationToken);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            _logger.LogInformation("User rating are loaded successfully");
           var ratingsJson =  await response.Content.ReadAsStreamAsync(cancellationToken);
           var ratings = await JsonSerializer.DeserializeAsync<RatingViewModel[]>(ratingsJson, cancellationToken: cancellationToken);
           return ratings;
        }
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogInformation("User rating not loaded, user is unauthorized");
            throw new GeneralException();
        }

        return Array.Empty<RatingViewModel>();

    }

    public RatingViewModel[] RatingListSorting(RatingViewModel[] ratingInfo, string sortOrder)
    {
        var ratingList = ratingInfo.ToList();
        switch (sortOrder)
        {
            case "bets_no":
                ratingList = ratingList.OrderByDescending(s => s.BetsNo).ToList();
                _logger.LogInformation("Sorted user rating by bets number descending");
                break;

            case "bets_no_asc":
                _logger.LogInformation("Sorted user rating by bets number");
                ratingList = ratingList.OrderBy(s => s.BetsNo).ToList();
                break;

            case "won_bets_no":
                _logger.LogInformation("Sorted user rating by won bets number descending");
                ratingList = ratingList.OrderByDescending(s => s.WonBetsNo).ToList();
                break;

            case "won_bets_no_asc":
                _logger.LogInformation("Sorted user rating by won bets number");
                ratingList = ratingList.OrderBy(s => s.WonBetsNo).ToList();
                break;

            case "profitperc":
                _logger.LogInformation("Sorted user rating by prifit percent descending");
                ratingList = ratingList.OrderByDescending(s => s.ProfitPercentage).ToList();
                break;

            case "profitperc_asc":
                _logger.LogInformation("Sorted user rating by prifit percent");
                ratingList = ratingList.OrderBy(s => s.ProfitPercentage).ToList();
                break;

            case "wonbetsperc":
                _logger.LogInformation("Sorted user rating by won bets percent descending");
                ratingList = ratingList.OrderByDescending(s => s.WonBetsPercentage).ToList();
                break;

            case "wonbetsperc_asc":
                _logger.LogInformation("Sorted user rating by won bets percent");
                ratingList = ratingList.OrderBy(s => s.WonBetsPercentage).ToList();
                break;

            default:
                ratingList = ratingList.OrderByDescending(s => s.BetsNo).ToList();
                break;
        }

        return ratingList.ToArray();
    }
}
