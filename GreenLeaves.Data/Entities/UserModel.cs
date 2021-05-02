using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace GreenLeaves.Data.Entities {
    public class UserModel : IdentityUser {
        #region Props
        public string FirstName { get; set; }
        public string LastName { get; set; }
        #endregion

    }
}
