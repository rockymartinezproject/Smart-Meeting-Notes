using System.Net.Http.Headers;
using System.Text.Json;
using MeetMind.Application.DTOs;
using MeetMind.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MeetMind.API.IntegrationTests;

public class UploadFlowTests : IClassFixture<MeetMindApiFactory>
{
    private readonly MeetMindApiFactory _factory;

    public UploadFlowTests(MeetMindApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Upload_ValidAudioFile_CreatesMeeting()
    {
        // Arrange
        var client = _factory.CreateClient();
        var email = $"test-{Guid.NewGuid():N}@example.com";
        var password = "Test123!";

        var registerResponse = await client.PostAsJsonAsync("/api/auth/register",
            new RegisterRequest(email, password, "Integration Test"));
        registerResponse.EnsureSuccessStatusCode();

        var authJson = await registerResponse.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(authJson), "Register response was empty");

        var auth = JsonSerializer.Deserialize<AuthResponse>(authJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.NotNull(auth);
        Assert.False(string.IsNullOrWhiteSpace(auth!.AccessToken), "Access token was empty");

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", auth.AccessToken);

        var audioBytes = new byte[] { 0x49, 0x44, 0x33, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(audioBytes);
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("audio/mpeg");
        content.Add(fileContent, "file", "integration-test.mp3");

        // Act
        var response = await client.PostAsync("/api/meetings/upload", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var meeting = await response.Content.ReadFromJsonAsync<MeetingDto>();
        Assert.NotNull(meeting);
        Assert.Equal("integration-test", meeting!.Title);
        Assert.Equal(MeetingStatus.Uploaded, meeting.Status);
    }

    [Fact]
    public async Task Upload_InvalidFileType_ReturnsBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();
        var email = $"test-{Guid.NewGuid():N}@example.com";
        var password = "Test123!";

        var registerResponse = await client.PostAsJsonAsync("/api/auth/register",
            new RegisterRequest(email, password, "Integration Test"));
        registerResponse.EnsureSuccessStatusCode();

        var authJson = await registerResponse.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(authJson), "Register response was empty");

        var auth = JsonSerializer.Deserialize<AuthResponse>(authJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.NotNull(auth);
        Assert.False(string.IsNullOrWhiteSpace(auth!.AccessToken), "Access token was empty");

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", auth.AccessToken);

        var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(new byte[] { 1, 2, 3 });
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/plain");
        content.Add(fileContent, "file", "notes.txt");

        // Act
        var response = await client.PostAsync("/api/meetings/upload", content);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }
}
