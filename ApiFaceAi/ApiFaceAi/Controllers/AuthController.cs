using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using ApiFaceAi.Data;
using ApiFaceAi.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiFaceAi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IAmazonRekognition _rekognition;

        public AuthController(AppDbContext db, IAmazonRekognition rekognition)
        {
            _db = db;
            _rekognition = rekognition;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginDto dto)
        {
            const string collectionId = "event-collection";

            if (dto.Photo == null || dto.Photo.Length == 0)
                return BadRequest("Photo required.");

            // Read the photo into a byte array
            byte[] photoBytes;
            using (var ms = new MemoryStream())
            {
                await dto.Photo.CopyToAsync(ms);
                photoBytes = ms.ToArray();
            }

            // Search for the face in the Rekognition collection
            var searchRequest = new SearchFacesByImageRequest
            {
                CollectionId = collectionId,
                Image = new Image { Bytes = new MemoryStream(photoBytes) },
                FaceMatchThreshold = 90, // Adjust threshold as needed
                MaxFaces = 1
            };

            try
            {
                var searchResult = await _rekognition.SearchFacesByImageAsync(searchRequest);
                var match = searchResult.FaceMatches.FirstOrDefault();

                if (match == null)
                    return Unauthorized("No matching face found.");

                // Find user in DB
                var user = await _db.Users.FirstOrDefaultAsync(u => u.RekognitionFaceId == match.Face.FaceId);

                if (user == null)
                    return Unauthorized("User not found in database.");

                // Return user data, including photo as base64
                return Ok(new
                {
                    user.Id,
                    user.Name,
                    PhotoBase64 = Convert.ToBase64String(user.PhotoBlob)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Authentication failed: " + ex.Message);
            }
        }
    }
}
