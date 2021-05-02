using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers {
    [Route( "api/[controller]/[action]" )]
    [ApiController]

    #region Props
    #endregion
    public class AppInfoController : ControllerBase {

        #region Props
        public IConfiguration _config { get; }
        private ILogger<AppInfoController> _logger;
        #endregion

        #region Constructor and dependency injection
        public AppInfoController( IConfiguration config, ILogger<AppInfoController> logger) {
            _config = config;
            _logger = logger;
        }
        #endregion

        #region GET's
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetVersion() { 
            string version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
            return Ok( version );
        }
        #endregion

    }
}
