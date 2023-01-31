using System.Transactions;
using YouYou.Business.Interfaces;
using YouYou.Business.Interfaces.BackOfficeUsers;
using YouYou.Business.Interfaces.Users;
using YouYou.Business.Models;
using YouYou.Business.Models.Pagination;

namespace YouYou.Business.Services
{
    public class BackOfficeUserService : BaseService, IBackOfficeUserService
    {
        private readonly IBackOfficeUserRepository _backOfficeUserRepository;
        private readonly IUserService _userService;

        public BackOfficeUserService(IErrorNotifier errorNotifier,
            IBackOfficeUserRepository backOfficeUserRepository,
            IUserService userService) : base(errorNotifier)
        {
            _backOfficeUserRepository = backOfficeUserRepository;
            _userService = userService;
        }
        public async Task Add(BackOfficeUser backOfficeUser, string password, Guid roleId)
        {
            if (!ExecuteValidation(backOfficeUser)) return;

            using (TransactionScope tr = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var succeeded = await _userService.Add(backOfficeUser.User, password);
                if (succeeded)
                {
                    var roleSucceeded = await _userService.AddRole(backOfficeUser.User, roleId);
                    if (roleSucceeded)
                    {
                        await _backOfficeUserRepository.Add(backOfficeUser);
                        tr.Complete();
                    }
                }
            }
        }
        public async Task<IEnumerable<BackOfficeUser>> GetAllWithIncludes(BackOfficeUsersFilter filter)
        {
            return await _backOfficeUserRepository.GetAllWithIncludes(filter);
        }
        public async Task<int> GetTotalRecords(BackOfficeUsersFilter filter)
        {
            return await _backOfficeUserRepository.GetTotalRecords(filter);
        }
        public async Task<BackOfficeUser> GetByIdWithIncludes(Guid id)
        {
            return await _backOfficeUserRepository.GetByIdWithIncludes(id);
        }
        public async Task<BackOfficeUser> GetByIdWithIncludesTracked(Guid id)
        {
            return await _backOfficeUserRepository.GetByIdWithIncludesTracked(id);
        }

        public async Task Update(BackOfficeUser backOfficeUser, string password, Guid roleId)
        {
            if (!ExecuteValidation(backOfficeUser)) return;

            using (TransactionScope tr = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                bool updateSucceeded = await _userService.UpdateRole(backOfficeUser.User, roleId);
                if (updateSucceeded)
                {
                    if (string.IsNullOrEmpty(password))
                    {
                        updateSucceeded = await _userService.Update(backOfficeUser.User);
                    }
                    else
                    {
                        updateSucceeded = await _userService.Update(backOfficeUser.User, password);
                    }

                    if (updateSucceeded)
                    {
                        await _backOfficeUserRepository.Update(backOfficeUser);
                        tr.Complete();
                    }
                }
            }
        }
        public async Task Remove(Guid id)
        {
            var backOfficeUser = await _backOfficeUserRepository.GetByIdWithIncludes(id);
            if (!ExecuteValidation(backOfficeUser)) return;
            backOfficeUser.User.NormalizedUserName = backOfficeUser.User.NormalizedUserName + "_deletado";

            using (TransactionScope tr = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await _backOfficeUserRepository.Update(backOfficeUser);
                await _backOfficeUserRepository.Remove(id);
                tr.Complete();
            }
        }
        public async Task<BackOfficeUser> GetById(Guid id)
        {
            return await _backOfficeUserRepository.GetById(id);
        }
        public async Task Disable(Guid userId)
        {
            await _userService.Disable(userId);
        }
        public async Task Enable(Guid userId)
        {
            await _userService.Enable(userId);
        }
        public void Dispose()
        {
            _backOfficeUserRepository?.Dispose();
        }
    }
}
