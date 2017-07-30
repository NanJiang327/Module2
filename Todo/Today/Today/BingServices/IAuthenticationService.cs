using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Today.Services
{
    public interface IAuthenticationService
    {
        Task InitializeAsync();
        string GetAccessToken();
    }
}
