namespace CadCamMachining.Shared.Models
{
    public class ItemConnectionDto
    {
        public string ItemTypeConnectionId { get; set; } = string.Empty;

        public string ParentItemId { get; set; } = string.Empty;

        public string ChildItemId { get; set; } = string.Empty;
    }
}
