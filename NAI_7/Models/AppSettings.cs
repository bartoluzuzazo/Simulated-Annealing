namespace NAI_7.Models;

public class AppSettings
{
    public int KnapsackCapacity { get; set; }
    public int CountOfItems { get; set; }
    public decimal Temperature { get; set; }
    public decimal? TempDecrease { get; set; }
    public List<(decimal, decimal)> Items { get; set; } = null!;
}