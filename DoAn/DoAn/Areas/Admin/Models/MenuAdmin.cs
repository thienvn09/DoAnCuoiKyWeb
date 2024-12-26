namespace DoAn.Areas.Admin.Models
{
    public class MenuAdmin
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string MenuUrl { get; set; }
        public int? ParentId { get; set; }
        public int MenuIndex { get; set; }
        public bool isVisible { get; set; }
    }
}
