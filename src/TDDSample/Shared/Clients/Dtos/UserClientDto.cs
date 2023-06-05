namespace TDDSample.Shared.Clients.Dtos;

public class UserClientDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public AddressClientDto Address { get; set; } = default!;
}
