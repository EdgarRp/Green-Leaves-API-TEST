using GreenLeaves.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GreenLeaves.Data.ServiceManager {
    public class AccountManager {

        #region Props
        readonly GreenLeaveContext _ctx;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly UserManager<UserModel> _manager;
        #endregion

        #region Constructor and Dependency Injection
        public AccountManager( GreenLeaveContext ctx, SignInManager<UserModel> singInManager, UserManager<UserModel> manager ) {
            _ctx = ctx;
            _signInManager = singInManager;
            _manager = manager;
        }
        #endregion

        #region READ

        /// <summary>
        /// Get cities
        /// </summary>
        /// <returns>Lis of City</returns>
        public async Task<List<City>> GetCities() {
            return await
            ( from c in _ctx.City
              select c ).ToListAsync();
        }


        /// <summary>
        /// Return SignInResults
        /// </summary>
        /// <param name="user"> user model</param>
        /// <param name="password">user password</param>
        /// <param name="locoOnFailure">lock on failure</param>
        /// <returns>Return success if creeds are corrects</returns>
        public async Task<SignInResult> CheckPasswordAsync( UserModel user, string password, bool locoOnFailure ) {
            return await _signInManager.CheckPasswordSignInAsync( user, password, locoOnFailure );
        }

        /// <summary>
        /// Find user model by username
        /// </summary>
        /// <param name="username">user name</param>
        /// <returns>UserModel</returns>
        public async Task<UserModel> FindByUserName( string username ) {
            return await
            ( from u in _ctx.Users.Where( x => x.UserName.Equals( username ) )
              select u ).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Find user by e-mail
        /// </summary>
        /// <param name="email">user e-mail</param>
        /// <returns>UserModel</returns>
        public async Task<UserModel> FindByEmail( string email ) {
            return await
            ( from u in _ctx.Users.Where( x => x.Email.Equals( email ) )
              select u ).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get roles by user
        /// </summary>
        /// <param name="user">user model</param>
        /// <returns>return role</returns>
        public async Task<string> GetRol( UserModel user ) {
            var roles = await _manager.GetRolesAsync( user );
            return roles.Select( x => x ).FirstOrDefault();
        }
        #endregion



    }
}
