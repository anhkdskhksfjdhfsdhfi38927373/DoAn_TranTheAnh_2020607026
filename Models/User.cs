﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Doan_TranTheAnh_2020607026.Models
{
    public class User
    {
        [Key] 
        
        public int Userid { get; set; }
        [Required]
        public string? UserName { get;set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
    
    }
}
