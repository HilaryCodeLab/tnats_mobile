using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace tnats_mobile.Models
{
    public class Species
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string species { get; set; }
    }
}
