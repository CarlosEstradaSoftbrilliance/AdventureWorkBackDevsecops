using Microsoft.EntityFrameworkCore;

namespace AdventureWorks.Data;

public sealed class AdventureWorksContext(DbContextOptions<AdventureWorksContext> options) : DbContext(options)
{
}
