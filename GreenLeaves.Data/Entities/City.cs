using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GreenLeaves.Data.Entities {
    public class City {
        [Key]
        public string Id { get; set; }
        [Required(ErrorMessage = "Nombre requerido")]
        public string Name { get; set; }
        

    }
}
