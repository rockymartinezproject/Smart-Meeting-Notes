namespace MeetMind.Application.DTOs;

public record RegisterRequest(string Email, string Password, string? FullName = null);

public record LoginRequest(string Email, string Password);

public record RefreshRequest(string RefreshToken);

public record AuthResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt,
    UserDto User);

public record UserDto(
    Guid Id,
    string Email,
    string? FullName,
    string SubscriptionTier);
