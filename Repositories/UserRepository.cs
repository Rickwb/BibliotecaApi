using BibliotecaApi.Entities;
using System;
using System.Linq;

namespace BibliotecaApi.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        private const int FAILED_LOGIN_ATTEMPTS_LIMMIT = 3;

        public UserRepository()
        {
        }
        public LoginResult Login(string username, string password)
        {
            try
            {
                var allUsers = GetAll();
                var user = GetAll().Where(x => x.Username == username && x.Password == password).SingleOrDefault();

                if (user != null)
                {
                    if (user.IsLockout)
                    {
                        if (DateTime.Now <= user.LockoutDate?.AddMinutes(15))
                        {
                            return LoginResult.ErrorResult(InvalidPasswordException.ACCOUNT_LOCKED);
                        }
                        else
                        {
                            user.IsLockout = false;
                            user.FailedAttempts = 0;
                        }
                    }

                    return LoginResult.SuccessResult(user);
                }

                var userExistsForUsername = allUsers.Where(u => u.Username == username).Any();

                if (userExistsForUsername)
                {
                    user = allUsers.Where(u => u.Username == username).Single();

                    user.FailedAttempts++;

                    if (user.FailedAttempts >= FAILED_LOGIN_ATTEMPTS_LIMMIT)
                    {
                        user.IsLockout = true;
                        user.LockoutDate = DateTime.Now;

                        return LoginResult.ErrorResult(InvalidPasswordException.ACCOUNT_LOCKED);
                    }

                    return LoginResult.ErrorResult(InvalidPasswordException.INVALID_PASSWORD_EXCEPTION);
                }

                return LoginResult.ErrorResult(InvalidUsernameException.INVALID_USERNAME_EXCEPTION);
            }
            catch (Exception e)
            {
                return LoginResult.ErrorResult(new AuthenticationException(e));
            }
        }

        public bool ResetPassword(string username, string oldPassword, string newPassword)
        {
            var user = GetAll().Where(x => x.Username == username && x.Password == oldPassword).SingleOrDefault();
            if (user == null)
                return false;
            user.Password = newPassword;
            Update(user.Id, user);
            return true;

        }
    }
}
