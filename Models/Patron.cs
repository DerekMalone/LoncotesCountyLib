using System.ComponentModel.DataAnnotations;

namespace LoncotesCountyLib.Models;

public class Patron
{
    public int Id {get; set;}
    [Required]
    public string FirstName { get; set;}
    [Required]
    public string LastName {get; set;}
    [Required]
    public string Address {get; set;}
    [Required]
    public string Email {get; set;}
    public bool IsActive {get; set;} 
}