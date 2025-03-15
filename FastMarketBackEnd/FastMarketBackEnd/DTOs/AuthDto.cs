﻿using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace FastMarketBackEnd.DTOs;

public class AuthDto
{
    [JsonRequired]
    [EmailAddress]
    [JsonProperty("Email")]
    public string Email { get; set; }

    [JsonRequired]
    [JsonProperty("Password")]
    [MinLength(4)]
    public string Password { get; set; }
}