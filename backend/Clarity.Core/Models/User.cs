using System.Text.RegularExpressions;

namespace Clarity.Core.Models;

public class User
{
    public const int MIN_USERNAME_LENGTH = 5;
    public const int MAX_USERNAME_LENGTH = 16;
    
    private User(Guid id, string userName, string passwordHash, string email)
    {
        Id = id;
        UserName = userName;
        PasswordHash = passwordHash;
        Email = email;
    }
    
    public Guid Id { get; }
    public string UserName { get; }
    public string PasswordHash { get; }
    public string Email { get; }

    public static Result<User> Create(Guid id, string userName, string email, string passwordHash)
    {
        var errors = new List<string>();
        Regex mailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        
        if (string.IsNullOrWhiteSpace(userName) || userName.Length < MIN_USERNAME_LENGTH)
            errors.Add($"Username must be at least {MIN_USERNAME_LENGTH} characters.");
        else if (userName.Length > MAX_USERNAME_LENGTH)
            errors.Add($"Username must be less than {MAX_USERNAME_LENGTH} characters.");
        else if (!userName.All(c => char.IsLetterOrDigit(c) || c == '_'))
            errors.Add("Username can only contain letters, digits, or underscores.");
        if (string.IsNullOrWhiteSpace(email))
            errors.Add($"Email cannot be empty.");
        
        if (!mailRegex.IsMatch(email))
            errors.Add($"Email address is not valid.");
        
        if (string.IsNullOrWhiteSpace(passwordHash))
            errors.Add($"Password cannot be empty.");
        
        if (errors.Any())
            return Result<User>.Failure(errors.ToArray());
        
        var user = new User(id, userName, passwordHash, email);
        
        return Result<User>.Success(user);
    }
}