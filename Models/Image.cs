namespace Assiginment.Models
{
    public class Image
    {
        public Guid Id { get; set; }
        public string ImageBase64 { get; set; } 
        public Guid EmployeeId { get; set; }

        public virtual Employee? Employee { get; set; }
    }
}
