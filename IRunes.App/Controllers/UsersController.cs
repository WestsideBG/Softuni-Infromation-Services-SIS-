namespace IRunes.App.Controllers
{
    using System.Collections.Generic;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Result;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using IRunes.Data;
    using IRunes.Models;
    using SIS.MVCFramework.Controller;
    using SIS.MVCFramework.Attributes;
    using SIS.HTTP.Enums;

    public class UsersController : Controller
    {
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                return Encoding.UTF8.GetString(sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }

        public IHttpResponse Login(IHttpRequest httpRequest)
        {
            return this.View();
        }

        [HttpPost(Method = HttpRequestMethod.Post, ActionName = "Login")]
        public IHttpResponse LoginConfirm(IHttpRequest httpRequest)
        {
            using (var context = new RunesDbContext())
            {
                string username = ((ISet<string>)httpRequest.FormData["username"]).FirstOrDefault();
                string password = ((ISet<string>)httpRequest.FormData["password"]).FirstOrDefault();

                var userFromDb = context.Users.FirstOrDefault(user => (user.Username == username || user.Email == username) && user.Password == this.HashPassword(password));

                if (userFromDb == null)
                {
                    return this.Redirect("Login");
                }

                this.SignIn(httpRequest,userFromDb.Id,userFromDb.Username,userFromDb.Email);
            }

            return this.Redirect("/");
        }

        public IHttpResponse Register(IHttpRequest httpRequest)
        {
            return this.View();
        }

        [HttpPost(Method = HttpRequestMethod.Post, ActionName = "Register")]
        public IHttpResponse RegisterConfirm(IHttpRequest httpRequest)
        {
            using (var context = new RunesDbContext())
            {
                string username = ((ISet<string>)httpRequest.FormData["username"]).FirstOrDefault();
                string password = ((ISet<string>)httpRequest.FormData["password"]).FirstOrDefault();
                string confirmPassword = ((ISet<string>)httpRequest.FormData["confirmPassword"]).FirstOrDefault();
                string email = ((ISet<string>)httpRequest.FormData["email"]).FirstOrDefault();

                if (password != confirmPassword)
                {
                    return this.Redirect("Register");
                }

                User newUser = new User
                {
                    Username = username,
                    Password = this.HashPassword(password),
                    Email = email
                };

                context.Users.Add(newUser);
                context.SaveChanges();
            }


            return this.Redirect("Login");
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            this.SignOut(request);

            return Redirect("/");
        }
    }
}
