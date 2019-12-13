using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelRecordApp.Model
{
    public class Users
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public async static Task<bool> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return false;
            }
            else
            {
                // Look in the azure table "Users" for the user with an email address that matched to the one given by the user in the emailEntry of MainPage.xaml
                var user = (await App.MobileService.GetTable<Users>().Where(u => u.Email == email).ToListAsync()).FirstOrDefault();

                if (user != null)
                {
                    if (user.Password == password)
                    {
                        App.user = user;  // Wen the user has been authenticated, asign it to the instance created in App.xaml.cs, so that it can be shared between all ContentViews
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public async static void Register(Users user)
        {
            await App.MobileService.GetTable<Users>().InsertAsync(user);
        }

    }
}
