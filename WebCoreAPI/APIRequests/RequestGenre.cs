namespace WebCoreAPI.APIRequests
{
    public class RequestGenre
    {
        public string Name { get; set; }    
        public string? Description { get; set; }

        public override string ToString() => $"{Name}, {Description}";
    }
}
