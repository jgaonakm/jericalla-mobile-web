namespace Models;


public class Ticket
{
    public string? TicketId { get; set; }
    public string? CustomerId { get; set; }
    public required string Subject { get; set; }
    public required string Description { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? Status { get; set; }
}
