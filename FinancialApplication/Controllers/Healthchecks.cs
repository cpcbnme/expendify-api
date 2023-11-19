using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FinancialApplication.Controllers
{
    [ApiController]
    [Route("")]
    public class Healthchecks : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            // json response
            return Ok(new
            {
                status = "Healthy"
            });
        }
    }
}