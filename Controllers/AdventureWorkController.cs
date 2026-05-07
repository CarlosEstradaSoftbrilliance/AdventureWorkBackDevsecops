using AdventureWorks.Data;
using AdventureWorks.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace Devsecops_back.Controllers
{
 
    [ApiController]
    [Route("api/v1")]
    public sealed partial class AdventureWorkController : ControllerBase
    {
        private readonly AdventureWorksContext _db;
        private readonly StoredProcedureOptions _sp;

        public AdventureWorkController(AdventureWorksContext db, IOptions<StoredProcedureOptions> sp)
        {
            _db = db;
            _sp = sp.Value;
        }

        [HttpGet("productos")]
        public async Task<ActionResult<IReadOnlyList<ProductoDto>>> GetProductos([FromQuery] int top = 50, CancellationToken ct = default)
        {
            top = NormalizeTop(top);
            ValidateProcedureName(_sp.Products);

            var rows = await _db.Database
                .SqlQueryRaw<ProductoDto>(
                    $"EXEC {_sp.Products} @Top",
                    new SqlParameter("@Top", top))
                .ToListAsync(ct);

            return Ok(rows);
        }

        [HttpGet("clientes")]
        public async Task<ActionResult<IReadOnlyList<ClienteDto>>> GetClientes([FromQuery] int top = 50, CancellationToken ct = default)
        {
            top = NormalizeTop(top);
            ValidateProcedureName(_sp.Customers);

            var rows = await _db.Database
                .SqlQueryRaw<ClienteDto>(
                    $"EXEC {_sp.Customers} @Top",
                    new SqlParameter("@Top", top))
                .ToListAsync(ct);

            return Ok(rows);
        }

        [HttpGet("ordenes-venta")]
        public async Task<ActionResult<IReadOnlyList<OrdenVentaDto>>> GetOrdenesVenta([FromQuery] int top = 50, CancellationToken ct = default)
        {
            top = NormalizeTop(top);
            ValidateProcedureName(_sp.SalesOrders);

            var rows = await _db.Database
                .SqlQueryRaw<OrdenVentaDto>(
                    $"EXEC {_sp.SalesOrders} @Top",
                    new SqlParameter("@Top", top))
                .ToListAsync(ct);

            return Ok(rows);
        }

        [HttpGet("empleados")]
        public async Task<ActionResult<IReadOnlyList<EmpleadoDto>>> GetEmpleados([FromQuery] int top = 50, CancellationToken ct = default)
        {
            top = NormalizeTop(top);
            ValidateProcedureName(_sp.Employees);

            var rows = await _db.Database
                .SqlQueryRaw<EmpleadoDto>(
                    $"EXEC {_sp.Employees} @Top",
                    new SqlParameter("@Top", top))
                .ToListAsync(ct);

            return Ok(rows);
        }

        private static int NormalizeTop(int top) => Math.Clamp(top <= 0 ? 50 : top, 1, 500);

        private static void ValidateProcedureName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || !ProcedureNameRegex().IsMatch(name))
                throw new InvalidOperationException("Nombre de procedimiento almacenado inválido.");
        }

        [GeneratedRegex(@"^[\[\]\w\.]+$")]
        private static partial Regex ProcedureNameRegex();
    }
}

