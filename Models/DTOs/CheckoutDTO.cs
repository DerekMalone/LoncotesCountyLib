namespace LoncotesCountyLib.Models.DTO;

public class CheckoutDTO
{
    public int Id {get; set;}
    public int MaterialId {get; set;}
    public MaterialDTO Material {get; set;}
    public int PatronId {get; set;}
    public PatronDTO Patron {get; set;}
    public DateTime CheckoutDate {get; set;}
    public DateTime? ReturnDate {get; set;}
    private static decimal _lateFeePerDay = .50M;
    public decimal? LateFee {get; set;}
    // below is begining of logic to calculate latefee.
    // {
    //     get
    //     {
    //         if (daysLate > 0)
    //         {
    //             return 0;
    //             //do logic to return fee...
    //         }
    //     return null;
    //     } 
    //     set;    
    // }
}