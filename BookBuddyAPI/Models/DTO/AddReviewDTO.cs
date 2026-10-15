namespace BookBuddyAPI.Models.DTO
{
    public class AddReviewDTO
    {
        public string UserId { get; set; }
        public string BookId { get; set; }
        public string ApiID { get; set; }
        public string Review { get; set; }
    }
}