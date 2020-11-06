using System;
using System.Collections.Generic;
using System.Text;

namespace tnats_mobile.Services
{
    public interface ICredentialsService
    {
        string UserName { get; }
        string Password { get; }
        void SaveCredentials(string UserName, string Password);
        void DeleteCredentials();
        bool DoCredentialsExist();
    }
}
