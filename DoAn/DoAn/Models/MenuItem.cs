namespace DoAn.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? ParentId { get; set; }
        public string? MenuUrl { get; set; }
        public int MenuIndex { get; set; }
        public bool isVisible { get; set; }
    }
}
