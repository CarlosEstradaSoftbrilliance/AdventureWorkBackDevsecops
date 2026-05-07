namespace AdventureWorks.Options;

public sealed class StoredProcedureOptions
{
    public const string SectionName = "StoredProcedures";

    public string Products { get; set; } = "dbo.uspGetProducts";
    public string Customers { get; set; } = "dbo.uspGetCustomers";
    public string SalesOrders { get; set; } = "dbo.uspGetSalesOrders";
    public string Employees { get; set; } = "dbo.uspGetEmployees";
}
