using System;
using System.Collections.Generic;
using System.Text;

namespace tnats_mobile.Services
{
    public interface ILogClass
    {
        void AddInfo(string tag, string message);
        void AddError(string tag, string message);
        void AddWarning(string tag, string message);
        void AddDebug(string tag, string message);
    }
}
