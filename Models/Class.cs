﻿using Microsoft.EntityFrameworkCore;

namespace WebApi.Models;
public class User
{
    public Guid? Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}