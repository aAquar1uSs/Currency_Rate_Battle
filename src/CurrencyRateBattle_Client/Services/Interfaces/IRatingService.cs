using CRBClient.Models;

namespace CRBClient.Services.Interfaces;
public interface IRatingService
{
    Task<List<RatingViewModel>> GetUserRatings();

    void RatingListSorting(ref List<RatingViewModel>? ratingInfo, string sortOrder);
}
