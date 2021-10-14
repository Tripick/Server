using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TripickServer.Models;
using TripickServer.Utils;
using TripickServer.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Data.SqlClient;
using TripickServer.Models.Common;

namespace TripickServer.Managers
{
    public class ManagerAccount : ManagerBase
    {
        #region Properties

        private UserManager<AppUser> userManager;

        private SignInManager<AppUser> signInManager;

        #endregion

        #region Constructor

        public ManagerAccount(
            ILogger<ServerLogger> logger,
            Func<AppUser> user,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            TripickContext tripickContext) : base(logger, user, tripickContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        #endregion

        #region Private

        public async Task<UserContext> Register(string email, string username, string password, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
                throw new ArgumentNullException("Invalid credentials.");

            if (password != confirmPassword)
                throw new InvalidOperationException("Passwords don't match.");

            AppUser newUser = new AppUser
            {
                Email = email,
                UserName = username,
                FirstName = string.Empty,
                LastName = string.Empty,
                Photo = new ImageAppUser() { CreationDate = DateTime.Now, Image = "data:image/jpeg;base64," + Images.ImageToBase64("./Resources/Account.jpg") }
            };

            IdentityResult userCreationResult = null;
            try
            {
                userManager.Options.SignIn.RequireConfirmedEmail = false;
                userCreationResult = await userManager.CreateAsync(newUser, password);
            }
            catch (SqlException)
            {
                throw new SystemException("Server error, please try again.");
            }
            catch (Exception e)
            {
                throw new SystemException("Server error, please try again : " + e.Message);
            }

            if (!userCreationResult.Succeeded)
                throw new InvalidOperationException(!userCreationResult.Errors.Any() ? "" : userCreationResult.Errors.First().Description);

            // If a confirmation is required
            if (Constants.AuthenticationConfirmEmail)
            {
                //string emailConfirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
                //string callbackUrl = Url.Page("/Account/ConfirmEmail", pageHandler: null, values: new { userId = newUser.Id, code = emailConfirmationToken }, protocol: Request.Scheme);
                //await emailSender.SendEmailAsync(credentials.Email, "Tripick : Confirm your account",
                //$"Please confirm your Tripick account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                //await signInManager.SignInAsync(newUser, isPersistent: false);
                //return Ok($"Registration completed, please verify your email - {newUser.Email}");
            }

            return await Login(email, password, false);
        }

        public async Task<UserContext> Login(string email, string password, bool rememberMe)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("Invalid credentials.");

            AppUser user = await userManager.FindByEmailAsync(email);
            if (user == null)
                throw new NullReferenceException("This account doesn't exist.");

            await signInManager.SignOutAsync();

            if (Constants.AuthenticationConfirmEmail && !user.EmailConfirmed)
                throw new InvalidOperationException("Email not confirmed, please check your email for confirmation link.");

            Microsoft.AspNetCore.Identity.SignInResult passwordSignInResult = await signInManager.PasswordSignInAsync(user, password, isPersistent: true, lockoutOnFailure: false);
            if (!passwordSignInResult.Succeeded)
                throw new InvalidOperationException("Incorrect login or password.");

            // Generate new token
            await userManager.RemoveAuthenticationTokenAsync(user, Constants.TokenProviderName, Constants.AuthenticationTokenName);
            var newToken = await userManager.GenerateUserTokenAsync(user, Constants.TokenProviderName, Constants.AuthenticationTokenName);
            await userManager.SetAuthenticationTokenAsync(user, Constants.TokenProviderName, Constants.AuthenticationTokenName, newToken);

            // Send User and AuthenticationKeys
            AppUser fullUser = NotConnectedGet(user.Id);
            user.Photo = fullUser.Photo;
            return new UserContext(user, newToken, LoadConfiguration());
        }

        public async Task<bool> Reset(string email)
        {
            //throw new NotImplementedException("Resetting password needs to send emails.");
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException("Invalid email address.");
            AppUser user = await userManager.FindByEmailAsync(email);
            if (user == null)
                throw new NullReferenceException("This account doesn't exist.");
            string resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
            await userManager.ResetPasswordAsync(user, resetToken, "bounouahugo13");
            return true;
        }

        public async Task<UserContext> LoginByToken(int id, string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException("Token required.");

            AppUser user = await userManager.FindByIdAsync(id.ToString());
            if (user == null)
                throw new NullReferenceException("This account doesn't exist.");

            string existingToken = await userManager.GetAuthenticationTokenAsync(user, Constants.TokenProviderName, Constants.AuthenticationTokenName);
            if (existingToken == token)
            {
                await signInManager.SignInAsync(user, isPersistent: true);
                AppUser fullUser = NotConnectedGet(user.Id);
                user.Photo = fullUser.Photo;
                return new UserContext(user, token, LoadConfiguration());
            }
            else
                throw new InvalidOperationException("Authentication keys invalid or expired.");
        }

        public async Task<string> Logout(int id, string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException("Token required.");

            AppUser user = await userManager.FindByIdAsync(id.ToString());
            if (user == null)
                throw new NullReferenceException("This account doesn't exist.");

            // Generate new token
            await userManager.RemoveAuthenticationTokenAsync(user, Constants.TokenProviderName, Constants.AuthenticationTokenName);
            var newToken = await userManager.GenerateUserTokenAsync(user, Constants.TokenProviderName, Constants.AuthenticationTokenName);
            await userManager.SetAuthenticationTokenAsync(user, Constants.TokenProviderName, Constants.AuthenticationTokenName, newToken);

            return "Logged out.";
        }

        public async Task<string> Delete(string email, string password, bool rememberMe)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("Invalid credentials.");

            AppUser user = await userManager.FindByEmailAsync(email);
            if (user == null)
                throw new NullReferenceException("This account doesn't exist.");

            await signInManager.SignOutAsync();

            Microsoft.AspNetCore.Identity.SignInResult passwordSignInResult = await signInManager.PasswordSignInAsync(user, password, isPersistent: true, lockoutOnFailure: false);
            if (!passwordSignInResult.Succeeded)
                throw new InvalidOperationException("Incorrect credentials.");

            await userManager.DeleteAsync(user);
            return "Account deleted.";
        }

        public AppUser Get()
        {
            AppUser user = this.TripickContext.Users
                .Where(x => x.Id == this.ConnectedUser().Id)
                .Include(x => x.Photo)
                .Include(x => x.Friendships)
                .SingleOrDefault();
            if (user == null)
                throw new NullReferenceException("Unable to fetch connected user.");

            return user;
        }

        #endregion

        #region NotConnected

        public AppUser NotConnectedGet(int id)
        {
            AppUser user = this.TripickContext.Users
                .Where(x => x.Id == id)
                .Include(x => x.Photo)
                .Include(x => x.Friendships)
                .SingleOrDefault();
            if (user == null)
                throw new NullReferenceException($"Unable to fetch user[{id}].");

            // Update last connection date
            user.LastConnectionDate = user.NewConnectionDate;
            user.NewConnectionDate = DateTime.Now;
            this.TripickContext.SaveChanges();
            return user;
        }

        #endregion
    }
}
