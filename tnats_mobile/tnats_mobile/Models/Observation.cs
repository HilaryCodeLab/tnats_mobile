using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace tnats_mobile.Models
{
   public class Observation
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public Guid guid { get; set; }
        public int user_id { get; set; }
        public string location { get; set; }
        public string geo_location { get; set; }
        public string species { get; set; }
        public string notes { get; set; }
        public byte[] photo { get; set; }
        public bool approved { get; set; }
        public bool active { get; set; }
    }
}
