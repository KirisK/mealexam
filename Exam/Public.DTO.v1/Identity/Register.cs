namespace Public.DTO.v1.Identity
{
    public class Register
    {
        public string Email { get; set; } = default!;
        
        public string Password { get; set; } = default!;

        
        
        public string Firstname { get; set; }= default!;
        
        public string? Lastname { get; set; }= default!;
        
        public string? Phone { get; set; }
        
    }
}