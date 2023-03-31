using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
    public class ClientLocation
    {
        [Key] // also a key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // auto incremented
        public int ClientLocationID { get; set; }
        public string ClientLocationName { get; set; }
    }
}
