﻿using CRBClient.Helpers;
using CRBClient.Models;
using Microsoft.Extensions.Options;
using CRBClient.Services.Interfaces;
using System.Net;

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

    public async Task<List<RatingViewModel>> GetUserRatings(CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(_options.GetUsersRatingURL ?? "", cancellationToken);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            _logger.LogInformation("User rating are loaded successfully");
            return await response.Content.ReadAsAsync<List<RatingViewModel>>();
        }
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogInformation("User rating not loaded, user is unauthorized");
            throw new GeneralException();
        }
        return new List<RatingViewModel>();
    }

    public void RatingListSorting(ref List<RatingViewModel> ratingInfo, string sortOrder)
    {
        switch (sortOrder)
        {
            case "bets_no":
                ratingInfo = ratingInfo.OrderByDescending(s => s.BetsNo).ToList();
                _logger.LogInformation("Sorted user rating by bets number descending");
                break;

            case "bets_no_asc":
                _logger.LogInformation("Sorted user rating by bets number");
                ratingInfo = ratingInfo.OrderBy(s => s.BetsNo).ToList();
                break;

            case "won_bets_no":
                _logger.LogInformation("Sorted user rating by won bets number descending");
                ratingInfo = ratingInfo.OrderByDescending(s => s.WonBetsNo).ToList();
                break;

            case "won_bets_no_asc":
                _logger.LogInformation("Sorted user rating by won bets number");
                ratingInfo = ratingInfo.OrderBy(s => s.WonBetsNo).ToList();
                break;

            case "profitperc":
                _logger.LogInformation("Sorted user rating by prifit percent descending");
                ratingInfo = ratingInfo.OrderByDescending(s => s.ProfitPercentage).ToList();
                break;

            case "profitperc_asc":
                _logger.LogInformation("Sorted user rating by prifit percent");
                ratingInfo = ratingInfo.OrderBy(s => s.ProfitPercentage).ToList();
                break;

            case "wonbetsperc":
                _logger.LogInformation("Sorted user rating by won bets percent descending");
                ratingInfo = ratingInfo.OrderByDescending(s => s.WonBetsPercentage).ToList();
                break;

            case "wonbetsperc_asc":
                _logger.LogInformation("Sorted user rating by won bets percent");
                ratingInfo = ratingInfo.OrderBy(s => s.WonBetsPercentage).ToList();
                break;

            default:
                ratingInfo = ratingInfo.OrderByDescending(s => s.BetsNo).ToList();
                break;
        }
    }
}
