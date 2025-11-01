using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookMaster.Model
{
    public class Recipe
    {
        public string Title { get; set; } = string.Empty;
        public string Ingredients { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;

       

        public string? Category { get; set; }
        public string? Time { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public User CreatedBy { get; set; } = null!;



    }
}
