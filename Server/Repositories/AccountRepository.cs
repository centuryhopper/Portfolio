
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.Contexts;
using Server.Entities;
using Shared.Models;
using static Shared.Models.ServiceResponses;

namespace Server.Repositories;

public class AccountRepository(IConfiguration configuration, IWebHostEnvironment webHostEnvironment, HttpClient httpClient) : IAccountRepository
{
    public async Task<LoginResponse> LoginAccount(LoginDTO loginDTO)
    {
        if (loginDTO is null)
        {
            return new LoginResponse(false, null!, "Login container is empty");
        }

        var response = await httpClient.PostAsJsonAsync<LoginDTO>("https://dotnetusermanagementsystem-production.up.railway.app/api/UMS/get-user-credentials?appName=Portfolio", loginDTO);

        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        using var doc = await JsonDocument.ParseAsync(stream);

        // Access root properties
        var root = doc.RootElement;

        string username = root.GetProperty("username").GetString() ?? "";
        string email = root.GetProperty("email").GetString() ?? "";
        string userId = root.GetProperty("userId").GetString() ?? "";

        var roles = root.GetProperty("roles")
            .EnumerateArray()
            .Select(r => r.GetProperty("role").GetString())
            .ToList();

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(userId))
        {
            return new LoginResponse(false, null!, "We couldn't find the user in the system.");
        }

        if (!roles.Any())
        {
            return new LoginResponse(false, null!, "You do not have any roles for this application.");
        }
        
        // 0 as a placeholder since we're not storing any users in the portfolio db. I am really just having login functionality for myself in case I need to make changes such as adding new blogs
        string token = GenerateToken(0, username, email, roles.First());

        return new LoginResponse(true, token!, "Login completed");
    }

    private string GenerateToken(int userId, string userName, string email, string role)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(webHostEnvironment.IsDevelopment() ? configuration["Jwt:Key"] : Environment.GetEnvironmentVariable("Jwt_Key")));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var userClaims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            issuer: webHostEnvironment.IsDevelopment() ? configuration["Jwt:Issuer"] : Environment.GetEnvironmentVariable("Jwt_Issuer"),
            audience: webHostEnvironment.IsDevelopment() ? configuration["Jwt:Audience"] : Environment.GetEnvironmentVariable("Jwt_Audience"),
            claims: userClaims,
            expires: JwtConfig.JWT_TOKEN_EXP_DATETIME,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}

