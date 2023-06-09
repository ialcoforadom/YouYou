﻿using Microsoft.AspNetCore.Identity;
using YouYou.Business.Interfaces;
using YouYou.Business.Interfaces.Users;
using YouYou.Business.Models;
using YouYou.Business.Models.Enums;

namespace YouYou.Business.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUserRepository _userRepository;
        private readonly IUser _appUser;

        public UserService(IErrorNotifier errorNotifier,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager, 
            IUserRepository userRepository, IUser appUser) : base(errorNotifier)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userRepository = userRepository;
            _appUser = appUser;
        }

        public async Task<ApplicationUser> GetByIdWithPerson(Guid id)
        {
            return await _userRepository.GetByIdWithPerson(id);
        }

        public async Task<bool> Add(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                return true;
            }

            foreach (var erro in result.Errors)
            {
                NotifyError(erro.Description);
            }

            return false;
        }

        public async Task<bool> AddRoleEditor(ApplicationUser user)
        {
            string role = _roleManager.Roles.Where(c => c.Id == RoleWithIdEnum.Editor).FirstOrDefault().Name;

            return await AddRole(user, role);
        }

        public async Task<bool> AddRolePhotography(ApplicationUser user)
        {
            string role = _roleManager.Roles.Where(c => c.Id == RoleWithIdEnum.Photography).FirstOrDefault().Name;

            return await AddRole(user, role);
        }

        public async Task<bool> Update(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return true;
            }

            foreach (var erro in result.Errors)
            {
                NotifyError(erro.Description);
            }

            return false;
        }

        public async Task<bool> Update(ApplicationUser user, string password)
        {
            bool updatePasswordSucceeded = await UpdatePassword(user, password);
            if (updatePasswordSucceeded)
            {
                return await Update(user);
            }

            return false;
        }

        private async Task<bool> UpdatePassword(ApplicationUser user, string password)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetPasswordResult = await _userManager
                .ResetPasswordAsync(user, token, password);

            if (resetPasswordResult.Succeeded)
            {
                return true;
            }

            foreach (var error in resetPasswordResult.Errors)
            {
                NotifyError(error.Description);
            }

            return false;
        }

        public async Task<bool> UpdatePassword(Guid id, string password)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            return await UpdatePassword(user, password);
        }

        public async Task<bool> UpdateRole(ApplicationUser user, Guid newRoleId)
        {
            var oldRole = user.UserRoles.FirstOrDefault().Role.Name;
            var newRole = _roleManager.Roles.Where(c => c.Id == newRoleId).FirstOrDefault().Name;

            if (oldRole != newRole)
            {
                bool removeRoleSucceeded = await RemoveRole(user, oldRole);
                if (removeRoleSucceeded)
                {
                    bool addRoleSucceeded = await AddRole(user, newRole);

                    return addRoleSucceeded;
                }
                return false;
            }
            return true;
        }

        public async Task<bool> RemoveRole(ApplicationUser user, string role)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, role);

            if (result.Succeeded)
            {
                return true;
            }

            foreach (var erro in result.Errors)
            {
                NotifyError(erro.Description);
            }

            return false;
        }

        public async Task<bool> AddRole(ApplicationUser user, string role)
        {
            var result = await _userManager.AddToRoleAsync(user, role);

            if (result.Succeeded)
            {
                return true;
            }

            foreach (var erro in result.Errors)
            {
                NotifyError(erro.Description);
            }

            return false;
        }

        public async Task<bool> AddRole(ApplicationUser user, Guid roleId)
        {
            var roleName = _roleManager.Roles.Where(c => c.Id == roleId).FirstOrDefault().Name;

            return await AddRole(user, roleName);
        }

        public ICollection<ApplicationRole> GetRoles()
        {
            return _roleManager.Roles.ToList();
        }

        public ICollection<ApplicationRole> GetRolesBackOfficeUsers()
        {
            return _roleManager.Roles.Where(c => c.Id == RoleWithIdEnum.Admin || c.Id == RoleWithIdEnum.Operator).ToList();
        }

        public async Task<bool> Remove(Guid id)
        {
            var identityUser = await _userManager.FindByIdAsync(id.ToString());
            identityUser.NormalizedUserName = identityUser.NormalizedUserName + "_deletado";
            IdentityResult result = await _userManager.DeleteAsync(identityUser);
            if (result.Succeeded)
            {
                return true;
            }

            foreach (var error in result.Errors)
            {
                NotifyError(error.Description);
            }

            return false;
        }

        public async Task RemoveWithoutLogicaDeletion(Guid id)
        {
            var identityUser = await _userManager.FindByIdAsync(id.ToString());
            await _userRepository.RemoveWithoutLogicaDeletion(identityUser);
        }

        public async Task Disable(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            user.Disabled = true;

            await Update(user);
        }

        public async Task Enable(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            user.Disabled = false;

            await Update(user);
        }

        public async Task<bool> AddRoleCoordinator(ApplicationUser user)
        {
            string role = _roleManager.Roles.Where(c => c.Id == RoleWithIdEnum.Coordinator).FirstOrDefault().Name;

            return await AddRole(user, role);
        }

        //public async Task ConfirmTermsOfUse(TermsOfUseDto termsOfUseDto)
        //{
        //    var user = await _userManager.FindByIdAsync(_appUser.GetUserId().ToString());
        //    user.TermsOfUse = termsOfUseDto.CreateJson();
        //    await Update(user);
        //}
    }
}
