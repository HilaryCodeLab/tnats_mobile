using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace tnats_mobile.Data
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
