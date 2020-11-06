using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace tnats_mobile.Models
{
    public class Token
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Access_token { get; set; }
        public string Error_description { get; set; }
        public DateTime Expire_date { get; set; }
        public int Expire_in { get; set; }
        public Token() { }


    }
}
