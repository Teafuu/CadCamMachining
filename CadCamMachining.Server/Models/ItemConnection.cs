namespace CadCamMachining.Server.Models
{
    public class ItemConnection
    {
        public string ItemTypeConnectionId { get; set; } = string.Empty;

        public string ParentItemId { get; set; } = string.Empty;

        public string ChildItemId { get; set; } = string.Empty;
    }
}
