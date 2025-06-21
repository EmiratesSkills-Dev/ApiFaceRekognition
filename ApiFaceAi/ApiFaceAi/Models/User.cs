namespace ApiFaceAi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }   
        public byte[] PhotoBlob { get; set; }
        public string RekognitionFaceId { get; set; }   
    }
}
