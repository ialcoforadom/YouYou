﻿using System.ComponentModel.DataAnnotations;

namespace YouYou.Api.ViewModels.Users
{
    public class RefreshTokenDataViewModel
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
