﻿
using System.Text.Json.Serialization;

namespace CurrencyRateBattleServer.ApplicationServices.Dto;

public class UserDto
{
    public string Email { get; set; }

    public string Password { get; set; }
}
