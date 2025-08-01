namespace Models;
public class AccountUsage
{
    public required string CustomerId { get; set; }
    public double DataUsedGB { get; set; }
    public int MinutesUsed { get; set; }
    public int SMSUsed { get; set; }
}
