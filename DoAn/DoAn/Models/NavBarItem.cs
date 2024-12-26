namespace DoAn.Models
{
    public class NavBarItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? ParentId { get; set; }
        public string? MenuUrl { get; set; }
        public int MenuIndex { get; set; }
        public bool isVisible { get; set; }
        public List<NavBarItem>? subItems { get; set; }
    }
}
