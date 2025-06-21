using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using ApiFaceAi.Data;
using ApiFaceAi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ApiFaceAi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IAmazonRekognition _rekognition;

        public UsersController(AppDbContext db, IAmazonRekognition rekognition)
        {
            _db = db;
            _rekognition = rekognition;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterUserDto dto)
        {
            const string collectionId = "event-collection";

            // Ensure collection exists 
            var collections = await _rekognition.ListCollectionsAsync(new ListCollectionsRequest());
            if (!collections.CollectionIds.Contains(collectionId))
            {
                await _rekognition.CreateCollectionAsync(new CreateCollectionRequest { CollectionId = collectionId });
            }

            if (dto.Photo == null || dto.Photo.Length == 0)
                return BadRequest("Photo required");

            // Read photo as byte array
            byte[] photoBytes;
            using (var ms = new MemoryStream())
            {
                await dto.Photo.CopyToAsync(ms);
                photoBytes = ms.ToArray();
            }

            // Index face in Rekognition
            var indexRequest = new IndexFacesRequest
            {
                CollectionId = collectionId,
                Image = new Image { Bytes = new MemoryStream(photoBytes) },
                DetectionAttributes = new List<string> { "DEFAULT" }
            };
            var indexResponse = await _rekognition.IndexFacesAsync(indexRequest);
            var faceId = indexResponse.FaceRecords.FirstOrDefault()?.Face?.FaceId;

            if (faceId == null)
                return BadRequest("No face detected in the uploaded photo.");

            // Save user in database
            var user = new Models.User
            {
                Name = dto.Name,
                PhotoBlob = photoBytes,
                RekognitionFaceId = faceId
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Ok(new { user.Id, user.Name });
        }
    }
}
