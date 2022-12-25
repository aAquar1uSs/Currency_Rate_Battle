using CRBClient.Models;

namespace CRBClient.Services.Interfaces;
public interface IRatingService
{
    Task<RatingViewModel[]> GetUserRatings(CancellationToken cancellationToken);

    RatingViewModel[] RatingListSorting(RatingViewModel[] ratingInfo, string sortOrder);
}
