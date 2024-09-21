using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureUserConsole.service
{
    public interface IPasswordResetService
    {
        bool HandleFailedLogin(string username); // Track failed login attempts
       void ResetFailedLoginAttempts(string username);
        bool ResetPassword(string username, string email, string lastName, string newPassword);
        bool VerifyUserIdentity(string username, string email, string lastName);

    }
}
