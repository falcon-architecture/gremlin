namespace Gremlin.BinaryTree;

public class User : IVertex
{
    public required object Id { get; init; }
    public required object PartitionKey { get; init; }
    public long Level { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? IntroducerId { get; set; }
    public string? ParentId { get; set; }
    // public decimal PersonalSales { get; set; }
    // public decimal TotalSalesVolume { get; set; }
    // public decimal Commission { get; set; }
    // public string Position { get; set; } = "left";
    // public DateTime CreatedAt { get; set; }
}
