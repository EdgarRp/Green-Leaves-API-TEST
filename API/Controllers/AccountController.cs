
using GreenLeaves.Core.Account;
using GreenLeaves.Data.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers {
    [Route( "api/[controller]/[action]" )]
    [ApiController]
    [Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme )]
    public class AccountController : ControllerBase {
        #region Props
        private ILogger<AccountController> _logger;
        private AccountCore _core;
        public IHttpContextAccessor _http { get; }
        #endregion

        #region Constructor and Dependency Injection
        public AccountController( ILogger<AccountController> logger, AccountCore core, IHttpContextAccessor http ) {
            _logger = logger;
            _core = core;
            _http = http;
        }
        #endregion

        #region POST's
        [HttpPost]
        [AllowAnonymous]
        public IActionResult CreateToken( [FromBody] LoginViewModel model ) {
            if (!ModelState.IsValid)
                return BadRequest( ModelState );
            return Ok( _core.CreateToken( model ) );
        }

        [HttpPost]
        public IActionResult Send( [FromBody]  UserMailViewModel model ) {
            if (!ModelState.IsValid)
                return BadRequest( ModelState );

            string user = _http.HttpContext?.User?.Identity.Name;
            if (string.IsNullOrEmpty( user ) || string.IsNullOrWhiteSpace( user ))
                return BadRequest( "Token is null" );

            return Ok( _core.SendMail( model ).Result );
        }
        #endregion

        #region GET's
        [HttpGet]
        public IActionResult GetCities() {
            string user = _http.HttpContext?.User?.Identity.Name;
            if (string.IsNullOrEmpty( user ) || string.IsNullOrWhiteSpace( user ))
                return BadRequest( "Token is null" );
            return Ok( _core.GetCities() );
        }
        #endregion

    }
}
