namespace Public.DTO.v1.Identity
{
    public class JwtResponse
    {
        public string Token { get; set; } = default!;
        
        public string Firstname { get; set; } = default!;
        
        public string? Lastname { get; set; }

        public ICollection<string> Roles { get; set; } = default!;
        
        public Guid? PersonId { get; set; }
        
        public Guid? CompanyId { get; set; }

        public ICollection<string> CompanyRoles { get; set; } = default!;
    }
}