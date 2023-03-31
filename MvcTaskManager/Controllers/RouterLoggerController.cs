using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;

namespace MvcTaskManager.Controllers
{
    public class RouterLoggerController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public RouterLoggerController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [Route("api/routerlogger")]
        public IActionResult Index()
        {
            string logMessage = null;
            using(StreamReader streamReader = new StreamReader(Request.Body, Encoding.ASCII))
            {
                logMessage = streamReader.ReadToEnd() + "\n";
            }
            string filePath = _hostingEnvironment.ContentRootPath + "\\RouterLogger.txt";
            System.IO.File.AppendAllText(filePath, logMessage);
            return Ok();
        }
    }
}
