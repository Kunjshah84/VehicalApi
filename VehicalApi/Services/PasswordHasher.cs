using Microsoft.AspNetCore.Identity;
using VehicalApi.Entity;

public static class PasswordHashGenerator
{
    public static void Run()
    {
        var hasher = new PasswordHasher<User>();

        var user = new User();  

        var plainPassword = "Rajesh@123";

        var hash = hasher.HashPassword(user, plainPassword);

        Console.WriteLine(hash);
    }
}
