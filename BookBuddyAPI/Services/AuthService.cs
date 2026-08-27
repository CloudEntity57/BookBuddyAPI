using BookBuddyAPI.Data;
using BookBuddyAPI.Models.Domain;
using BookBuddyAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace BookbuddyAPI.Services
{
public class AuthService : IAuthService
{
    private readonly BookBuddyGeneralDbContext db;
    private readonly IJwtService jwtService;
    private readonly IPasswordHasher<User> passwordHasher;
    public AuthService(
        BookBuddyGeneralDbContext db,
        IJwtService jwtService,
        IPasswordHasher<User> passwordHasher)
    {
        this.db = db;
        this.jwtService = jwtService;
        this.passwordHasher = passwordHasher;
    }
    public async Task<AuthResult> RegisterEmailAsync(RegisterRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var username = request.UserName.Trim();
        // Check email
        var emailExists = await db.Users
            .AnyAsync(u => u.Email.ToLower() == email);
        if (emailExists)
        {
            throw new Exception(
                "An account already exists with this email address.");
        }
        // Check username
        var usernameExists = await db.Users
            .AnyAsync(u => u.UserName == username);
        if (usernameExists)
        {
            throw new Exception(
                "That username is already taken.");
        }
        // Create user
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            UserName = username
        };
        // Hash password
        user.PasswordHash = passwordHasher.HashPassword(user, request.Password);
        // Create Local authentication identity
        var identity = new UserAuthIdentity
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Provider = AuthProvider.Local,
            ProviderKey = email
        };
        user.AuthIdentities.Add(identity);
        db.Users.Add(user);
        await db.SaveChangesAsync();
        // Generate JWT
        var token = jwtService.GenerateToken(user);
        return new AuthResult
        {
            Token = token,
            Provider = AuthProvider.Local
        };
    }
    public async Task<string?> GenerateTokenFromPassword(User user, string email, string password)
        {
            var result = passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash!,
                password
            );
            if(result == PasswordVerificationResult.Failed)
            {
                return null;
            }
            var token = jwtService.GenerateToken(user);
            return token;
        }

}



}