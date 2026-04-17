namespace MasterNet.Persistence.Models;
using Microsoft.AspNetCore.Identity;
public class AppUser: IdentityUser
{
    public string? NombreCompleto { get; set; }
    public string? Carrera { get; set; }
}