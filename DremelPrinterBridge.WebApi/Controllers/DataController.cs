using System.Collections.Generic;
using System.Linq;
using DremelPrinterBridge.Core.Entities;
using DremelPrinterBridge.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DremelPrinterBridge.Controllers
{
    [ApiController]
    [Route("data")]
    public class DataController : Controller
    {
        private readonly InMemoryContextDb inMemoryDb;
        private readonly ILogger<DataController> logger;

        public DataController(InMemoryContextDb _inMemoryDb,
                            ILogger<DataController> _logger)
        {
            inMemoryDb = _inMemoryDb;
            logger = _logger;
        }

        [HttpGet("printers")]
        public IEnumerable<Printer> GetPrinters()
        {
            return inMemoryDb.Printers.ToArray();
        }
    }
}
