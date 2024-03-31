using Microsoft.AspNetCore.Identity;
using MusicApp.Repositories;
using MusicApp.Repositories.Interfaces;
using MusicApp.Services.Interfaces;

namespace MusicApp.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<IdentityRole> GetAllRoles()
        {
            List<IdentityRole> rolesList = _unitOfWork.Role.GetAll().ToList();

            return rolesList;
        }
    }
}
