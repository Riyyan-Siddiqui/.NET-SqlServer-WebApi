using WebAPI_tutorial.Data;
using WebAPI_tutorial.Dto;
using WebAPI_tutorial.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace WebAPI_tutorial.Services
{
    public class AuthService(IConfiguration config, AppDbContext _context) : IAuthService
    {

        public async Task<Tuple<int, TokenDto>> LoginUser(UserDto dto)
        {
            TokenDto tokenDto = new TokenDto();
            try
            {
                if (dto == null || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
                {
                    tokenDto.Message = "Email and Password are required";
                    return new Tuple<int, TokenDto>(1, tokenDto);
                }

                var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);

                if (existingUser == null)
                {
                    tokenDto.Message = "This user does not exist";
                    return new Tuple<int, TokenDto>(0, tokenDto);
                }

                var passwordHasher = new PasswordHasher<string>();
                var verifyPassword = passwordHasher.VerifyHashedPassword(dto.Email, existingUser.Password!, dto.Password);

                if (verifyPassword == PasswordVerificationResult.Success)
                {
                    UserDto payload = new()
                    {
                        Name = existingUser.Name,
                        Email = existingUser.Email,
                        Id = existingUser.Id
                    };

                    var token = GetJwtToken(payload);
                    tokenDto.Token = token;
                    tokenDto.Message = "Login successful";
                    return new Tuple<int, TokenDto>(2, tokenDto);
                }

                else if (verifyPassword == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    UserDto payload = new()
                    {
                        Name = existingUser.Name,
                        Email = existingUser.Email,
                        Id = existingUser.Id
                    };
                    
                    var token = GetJwtToken(payload);
                    existingUser.Password = GetPasswordHashed(dto);
                    _context.Users.Update(existingUser);
                    await _context.SaveChangesAsync();
                    tokenDto.Token = token;
                    tokenDto.Message = "Login successful, New hashed generated";
                    return new Tuple<int, TokenDto>(2, tokenDto);
                }

                else
                {
                    tokenDto.Message = "Incorrect password";
                    return new Tuple<int, TokenDto>(1, tokenDto);
                }


            }
            catch (Exception)
            {
                tokenDto.Message = "An error occurred during login";
                return new Tuple<int, TokenDto>(3, tokenDto);
                //throw new Exception(e.Message);
            }
        }

        private string GetJwtToken(UserDto dto)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, dto.Name!),
            new Claim(ClaimTypes.Email, dto.Email!),
            new Claim(ClaimTypes.NameIdentifier, dto.Id.ToString()),
        };

            var secretKey = config["JWT:Key"]
                ?? throw new InvalidOperationException("JWT:Key is not configured.");
            var issuer = config["JWT:Issuer"]
                ?? throw new InvalidOperationException("JWT:Issuer is not configured.");
            var audience = config["JWT:Audience"]
                ?? throw new InvalidOperationException("JWT:Audience is not configured.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Tuple<int, string>> RegisterUser(UserDto dto)
        {
            try
            {
                var existingUser = await _context.Users.AnyAsync(x => x.Email == dto.Email);

                if (existingUser)
                {
                    return new Tuple<int, string>(0, "This user already exists, Kindly Sign In");
                }

                _context.Users.Add(new Entities.User
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    Email = dto.Email,
                    Password = GetPasswordHashed(dto)
                });

                await _context.SaveChangesAsync();

                return new Tuple<int, string>(1, "User registered successfully");

            }
            catch (Exception)
            {
                return new Tuple<int, string>(3, "An error occurred during registration");

            }
        }

        private string GetPasswordHashed(UserDto dto)
        {
            var passwordHasher = new PasswordHasher<string>();
            return passwordHasher.HashPassword(dto.Email, dto.Password);
        }
    }
}
