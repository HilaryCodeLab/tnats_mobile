using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace tnats_mobile.Models
{
    public class Location
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string location { get; set; }
    }
}
