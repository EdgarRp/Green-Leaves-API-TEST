using GreenLeaves.Common;
using GreenLeaves.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLeaves.Data {
    public class GreenLeaveSeeder {
        private readonly GreenLeaveContext _ctx;
        private readonly UserManager<UserModel> _manager;
        private readonly RoleManager<IdentityRole> _roleManager;

        #region Constructor and Dependency Injection
        public GreenLeaveSeeder(GreenLeaveContext ctx, UserManager<UserModel> manager, RoleManager<IdentityRole> roleManager) {
            _ctx = ctx;
            _manager = manager;
            _roleManager = roleManager;
        }
        #endregion

        public async Task<bool> SeedAsync() {
            _ctx.Database.EnsureCreated();

            #region Vars
            const string userEmail = V_Constants.USER_EMAIL_DEFAULT;
            const string userName = V_Constants.USER_NAME_DEFAULT;
            UserModel root;
            IdentityResult result;
            #endregion


            await AddRoles();

            #region root
            root = await _manager.FindByEmailAsync( userEmail );

            if (null == root) {
                root = new UserModel() {
                    FirstName = V_Constants.USER_FIRSTNAME_DEFAULT,
                    LastName = V_Constants.USER_LASTNAME_DEFAULT,
                    Email = userEmail,
                    UserName = userName
                };
                result = await _manager.CreateAsync( root, V_Constants.DEFAULT_PASS );

                if (IdentityResult.Success == result) {
                    await _manager.AddToRoleAsync( root, V_Constants.USER_ROL_ROOT );
                }
                
            }
            #endregion

            #region Cities
            if (_ctx.City.Count() == 0) {
                List<City> cities = new List<City>() {
                    new City(){
                        Name = "La Paz, Baja California Sur, México"
                    },
                    new City(){
                        Name = "Los Cabos, Baja California Sur, México"
                    },
                    new City(){
                        Name = "Campeche, Campeche, México"
                    },
                    new City(){
                        Name = "Ciudad del Carmen, Campeche, México"
                    },
                    new City(){
                        Name = "Saltillo, Coahuila de Zaragoza, México"
                    },
                    new City(){
                        Name = "Monclova-Frontera, Coahuila de Zaragoza, México"
                    },
                    new City(){
                        Name = "La Laguna, Coahuila de Zaragoza, México"
                    },
                    new City(){
                        Name = "Piedras Negras, Coahuila de Zaragoza, México"
                    },
                    new City(){
                        Name = "Tecomán, Colima, México"
                    },
                    new City(){
                        Name = "Colima-Villa de Álvarez, Colima, México"
                    },
                    new City(){
                        Name = "Manzanillo, Colima, México"
                    },
                    new City(){
                        Name = "Tepic, Nayarit, México"
                    },
                    new City(){
                        Name = "Tula, Hidalgo, México"
                    },
                    new City(){
                        Name = "Acapulco, Guerrero, México"
                    },
                    new City(){
                        Name = "Cuernavaca	Morelos, México"
                    },
                    new City(){
                        Name = "Tehuantepec-Salina Cruz, Oaxaca, México"
                    }
                };
                await _ctx.City.AddRangeAsync( cities );
                await _ctx.SaveChangesAsync();
            }
            #endregion
            return true;
        }


        public async Task AddRoles() {
            #region Vars
            bool exists;
            #endregion

            #region Logic
            exists = await _roleManager.RoleExistsAsync( V_Constants.USER_ROL_ROOT );
            if (!exists) {
                _ = await _roleManager.CreateAsync( new IdentityRole( V_Constants.USER_ROL_ROOT ) );
            }
            exists = await _roleManager.RoleExistsAsync( V_Constants.USER_ROL_DEFAULT );
            if (!exists) {
                _ = await _roleManager.CreateAsync( new IdentityRole( V_Constants.USER_ROL_DEFAULT ) );
            }
            #endregion
        }
    }


}
