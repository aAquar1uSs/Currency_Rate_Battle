using CRBClient.Models;

namespace CRBClient.Services.Interfaces;
public interface IRatingService
{
    Task<List<RatingViewModel>> GetUserRatings(CancellationToken cancellationToken);

    void RatingListSorting(ref List<RatingViewModel> ratingInfo, string sortOrder);
}
