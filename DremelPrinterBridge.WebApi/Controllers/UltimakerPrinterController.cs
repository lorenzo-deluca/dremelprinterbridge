using DremelPrinterBridge.Database;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DremelPrinterBridge.Core.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class UltimakerPrinterController : Controller
    {
        private readonly InMemoryContextDb inMemoryDb;

        public UltimakerPrinterController(InMemoryContextDb _inMemoryDb)
        {
            inMemoryDb = _inMemoryDb;
        }

        [HttpGet("system/firmware")]
        public IActionResult GetFirmware()
        {
            var printer = inMemoryDb.Printers.FirstOrDefault();
            if (printer == null)
                return null;

            return Ok(printer.SoftwareVersion);
        }
    }
}
