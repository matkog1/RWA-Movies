﻿using DAL.Models;
namespace DAL.APIResponse;

public class ResponseUser
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    public int CountryId { get; set; }

}
