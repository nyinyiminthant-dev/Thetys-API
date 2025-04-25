﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using MODEL.Entities;

namespace BAL.Shared
{
    public class CommonTokenGenerator
    {

        private static readonly string Issuer = "Allianz_DEV";


        private static readonly string Audience = "Allianz_DEV";



        private static readonly SymmetricSecurityKey Key = new(Encoding.UTF8.GetBytes("MFwwDQYJKoZIhvcNAQEBBQADSwAwSAJBAL0zIKgOk+azCEuVZvrvtkgjRk3VcSq4 kDzbi51WD2xCUGNafzI8cmoY9KqFh7s1V7C6nw3/QbzvTytwYR/c5Q0CAwEAAQ=="));
        private static readonly SigningCredentials Credientials = new(Key, SecurityAlgorithms.HmacSha512Signature);

        public static string GenerateToken(User user, string role)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(

                issuer: Issuer,
                audience: Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(3),
            signingCredentials: Credientials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string GeneratePasswordResetTokenForUser(User user)
        {

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            };

            var token = new JwtSecurityToken(

                issuer: Issuer,
                audience: Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: Credientials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public static async Task<bool> CheckTokenIsValid(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = Issuer,
                ValidAudience = Audience,
                IssuerSigningKey = Key
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                if (validatedToken is JwtSecurityToken jwtSecurityToken && jwtSecurityToken.ValidTo < DateTime.UtcNow)
                {
                    return false; // Token is expired
                }

                return true; // Token is valid
            }
            catch (Exception)
            {
                return false; // Token validation failed
            }
        }




    }
}
