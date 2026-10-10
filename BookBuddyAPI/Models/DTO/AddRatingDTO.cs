namespace BookBuddyAPI.Models.DTO
{
    public class AddRatingDTO
    {
        public string UserId { get; set; }
        public string BookId { get; set; }
        public int Rating { get; set; }
    }
}