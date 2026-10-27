namespace BackEnd_cs.Models
{
    public class LostItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Type { get; set; } = "Lost";
        public DateTime DateReported { get; set; } = DateTime.Now;
    }
}