using GreenLeaves.Common;
using GreenLeaves.Core.SendGrid;
using GreenLeaves.Data.Entities;
using GreenLeaves.Data.ServiceManager;
using GreenLeaves.Data.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GreenLeaves.Core.Account {
    public class AccountCore {
        #region Props
        public readonly AccountManager _manager;
        private readonly IConfiguration _config;
        private readonly ISendGrid _send;
        private IHostingEnvironment _enviroment;
        #endregion

        public AccountCore(AccountManager manager, IConfiguration config, ISendGrid grid, IHostingEnvironment enviroment) {
            _manager = manager;
            _config = config;
            _send = grid;
            _enviroment = enviroment;

        }

        public async Task<bool> SendMail( UserMailViewModel model ) {
            #region Vars
            string key, content_mail, web_root, path;
            #endregion

            #region Initilize vars
            //Esto no es recomendado, pero para no crear tablas y y hacer mas consultas de configuración lo dejo de momento en el .json
            key = _config [ "KeySendGrid" ];

            web_root = _enviroment.WebRootPath;
            path =  web_root +  V_Constants.MAIL_PATH ;
            content_mail = System.IO.File.ReadAllText( path );
            #endregion

            #region Prepare return
            content_mail = content_mail.Replace( V_Constants.REPLACE_EMAIL_DATA, model.Email );
            content_mail = content_mail.Replace( V_Constants.REPLACE_NAME_DATA, model.Name );
            content_mail = content_mail.Replace( V_Constants.REPLACE_DATE_DATA, model.Date.ToShortDateString() );
            content_mail = content_mail.Replace( V_Constants.REPLACE_CITY_DATA, model.City );
            return await _send.SendMailAsync( key, V_Constants.MAIL_ADDRESS, content_mail, V_Constants.SUB_MAIL, model.Email );
            #endregion
        }

        /// <summary>
        /// Get Cities
        /// </summary>
        /// <returns>List of City</returns>
        public List<City> GetCities() {
            return _manager.GetCities().Result;
        }

        /// <summary>
        /// Create token
        /// </summary>
        /// <param name="creeds">Login view model</param>
        /// <returns>UserViewModel</returns>
        public UserViewModel CreateToken(LoginViewModel creeds) {
            #region Vars
            UserViewModel response;
            UserModel user;
            SignInResult result;
            string role;
            int expireAt;
            #endregion

            #region Initialize vars
            response = new UserViewModel();

            user = ( creeds.UserName.Contains( "@" ) ) ?
                _manager.FindByEmail( creeds.UserName ).Result :
                _manager.FindByUserName( creeds.UserName ).Result;
            #endregion

            #region Logic
            if (null != user) {
                result =_manager.CheckPasswordAsync( user, creeds.Password, false ).Result;

                if (result.Succeeded) {
                    role =_manager.GetRol( user ).Result;
                    var claims = new [] {
                       new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                       new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                       new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)

                    };

                    expireAt = ( string.IsNullOrEmpty( _config [ "timeExpires" ] ) ) ? 5 : Convert.ToInt32( _config [ "timeExpires" ] );

                    var key = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( _config [ "Tokens:Key" ] ) );
                    
                    var creds = new SigningCredentials( key, SecurityAlgorithms.HmacSha256 );

                    var token = new JwtSecurityToken(
                                          _config [ "Tokens:Issuer" ],
                                          _config [ "Tokens:Audience" ],
                                          claims,
                                          expires: DateTime.Now.AddMinutes( expireAt ),
                                          signingCredentials: creds );

                    response = new UserViewModel() {
                        Expiration = token.ValidTo,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Rol = role,
                        Token = new JwtSecurityTokenHandler().WriteToken( token ),
                        UserName = user.UserName
                    };
                }
            }
            #endregion

            #region Prepare return
            return response;
            #endregion
        }
    }
}
