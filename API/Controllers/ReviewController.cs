using Core.DTOs.Review;
using Core.IRepositories;
using Core.Models;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        #region Repository Injection
        private readonly IReviewRepository _reviewRepository;

        public ReviewController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        } 
        #endregion

        #region Map from Review to DTO
        public static ReviewToReturnDto MapReviewToReviewDto(Review review)
        {
            var reviewDto = new ReviewToReturnDto
            {
                Id = review.Id,
                Text = review.Text,
                Date = review.Date.ToString("dd-MM-yyyy HH:mm"),
                Rating = review.Rating,
                ProductID = review.ProductID,
                UserID = review.UserID
            };

            if (review.Product != null)
            {
                reviewDto.ProductName = review.Product.Name; // Assuming there is a ProductName property in the Product class.
            }

            if (review.User != null)
            {
                reviewDto.UserName = review.User.FullName; // Assuming there is a UserName property in the User class.
            }

            return reviewDto;
        }
        #endregion

        #region GetReviewsByProductId
        [AllowAnonymous]
        [HttpGet("GetReviewsByProductId/{productId:int}", Name = "GetReviewsByProductId")] //Get /api/review/GetReviewsByProductId/1
        public ActionResult<List<ReviewToReturnDto>> GetReviewsByProductId(int productId)
        {
            ICollection<Review> reviews = _reviewRepository.GetReviewsByProduct(productId);

            List<ReviewToReturnDto> reviewsToReturnDtoList = new List<ReviewToReturnDto>();

            if (reviews.Count == 0)
            {
                return Ok(reviewsToReturnDtoList);
            }

            //Mapping
            foreach (Review review in reviews)
            {
                ReviewToReturnDto reviewToReturnDto = MapReviewToReviewDto(review);

                reviewsToReturnDtoList.Add(reviewToReturnDto);
            }

            return Ok(reviewsToReturnDtoList);
        }
        #endregion

        #region Get All Reviews

        [HttpGet("GetAllReviews")] //Get /api/review/GetAllReviews
        //[Authorize(Policy = "Admin")]
        public ActionResult<ReviewToReturnDto> GetAllReviews()
        {
            ICollection<Review> reviews = _reviewRepository.GetAll();

            List<ReviewToReturnDto> reviewsToReturnDtoList = new List<ReviewToReturnDto>();

            if (reviews.Count == 0)
            {
                return Ok(reviewsToReturnDtoList);
            }
            //Mapping
            foreach (Review review in reviews)
            {
                ReviewToReturnDto reviewToReturnDto = MapReviewToReviewDto(review);

                reviewsToReturnDtoList.Add(reviewToReturnDto);
            }

            return Ok(reviewsToReturnDtoList);
        }
        #endregion

        #region AddReview
        [HttpPost] //Post /api/review/addReview/
        [Authorize]
        public IActionResult AddReview([FromBody] ReviewToAddDTO reviewDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                   var userIdFromToken = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                   Review review = new Review
                   {
                       //Map from ReviewToAddDTO -> Review
                       Text = reviewDto.Text,
                       Date = reviewDto.Date,
                       Rating = reviewDto.Rating,
                       ProductID = reviewDto.ProductID,
                       UserID = int.Parse(userIdFromToken)
                   };
                   bool check = _reviewRepository.Add(review);
                    if (check)
                    {
                        return Ok();
                    }
                    return BadRequest("Problem with Databas");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "An error occurred while adding the review.");
                }
            }
            return BadRequest(ModelState);

        }
        #endregion

        // ------------- ADMIN -------------------------
        #region Get Review by id

        [HttpGet("GetReviewById/{id:int}", Name = "GetReviewByID")] //Get /api/review/GetProductByID/1
        [Authorize(Roles = "Admin")]
        public ActionResult<ReviewToReturnDto> GetReviewById(int id)
        {
            Review review = _reviewRepository.GetById(id);
            if (review == null)
            {
                return NotFound();
            }
            //Mapping
            ReviewToReturnDto reviewToReturnDto = MapReviewToReviewDto(review);

            return Ok(reviewToReturnDto);
        }
        #endregion

        #region Get All Reviews Admin

        [HttpGet("GetAllReviewsAdmin")] //Get /api/review/GetAllReviewsAdmin
        [Authorize(Roles = "Admin")]
        public ActionResult<ReviewToReturnDto> GetAllReviews(int? pageIndex)
        {
            ICollection<Review> reviews = _reviewRepository.GetAllAdmin(pageIndex);
            if (reviews.Count == 0)
            {
                return NotFound();
            }
            //Mapping
            List<ReviewToReturnDto> reviewsToReturnDtoList = new List<ReviewToReturnDto>();
            foreach (Review review in reviews)
            {
                ReviewToReturnDto reviewToReturnDto = MapReviewToReviewDto(review);

                reviewsToReturnDtoList.Add(reviewToReturnDto);
            }

            return Ok(reviewsToReturnDtoList);
        }
        #endregion

        #region GetByCompositeIdUserAndProduct
        [HttpGet("GetByCompositeIdUserAndProduct/{ProductId:int}/{UserId:int}")] //Get /api/review/GetByCompositeIdUserAndProduct/ProductId?1&&UserId=1
        public ActionResult<ReviewToReturnDto> GetByCompositeIdUserAndProduct(int ProductId, int UserId)
        {
            Review review = _reviewRepository.GetByCompositeId(ProductId, UserId);
            if (review == null)
            {
                return NotFound();
            }
            //Mapping
            ReviewToReturnDto reviewToReturnDto = MapReviewToReviewDto(review);

            return Ok(reviewToReturnDto);
        }
        #endregion

        #region Delete For ADMIN
        [HttpDelete("{id:int}")] //Delete /api/review/DeleteReview/1
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteReview(int id)
        {
            bool check = _reviewRepository.Delete(id);
            if (check)
            {
                return Ok("deleted Succsesfully");
            }
            return BadRequest();
        }
        #endregion

    }
}
