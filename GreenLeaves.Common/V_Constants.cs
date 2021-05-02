using System;
using System.Collections.Generic;
using System.Text;

namespace GreenLeaves.Common {
    public class V_Constants {

        #region Default Root User
        public const string USER_EMAIL_DEFAULT = "edgarrp.gab@gmail.com";
        public const string DEFAULT_PASS = "Prueba1*";
        public const string USER_FIRSTNAME_DEFAULT = "Edgar";
        public const string USER_LASTNAME_DEFAULT = "Roque";
        public const string USER_NAME_DEFAULT = "e.roque";
        #endregion

        #region Rols
        public const string USER_ROL_ROOT = "root";
        public const string USER_ROL_DEFAULT = "default";
        #endregion

        #region Mail data
        public const string MAIL_ADDRESS = "edgarrp_gab@hotmail.com";
        public const string SUB_MAIL = "Grean Leaves Notificaciones";
        public const string REPLACE_NAME_DATA = "@name@";
        public const string REPLACE_EMAIL_DATA = "@email@";
        public const string REPLACE_DATE_DATA = "@date@";
        public const string REPLACE_CITY_DATA = "@city@";
        #endregion

        #region Root resources
        public const string SOURCE = "Resources";
        public const string MAIL_PATH = "\\html\\Mail.html";
        #endregion
    }
}
