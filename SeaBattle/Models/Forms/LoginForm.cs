using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SeaBattle.Models.Forms;
using System.ComponentModel.DataAnnotations;

namespace SeaBattle.Models.Forms
{
    public class LoginForm
    {
        [Required, MinLength(5, ErrorMessage = "Минимум 5 символов")]
        public string Username { get; set; }
        [Required, MinLength(5, ErrorMessage = "Минимум 5 символов")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}