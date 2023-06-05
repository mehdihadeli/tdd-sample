namespace TDDSample.Users.Models;

public class User
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required Address Address { get; set; }
}
