using SubastaMaestra.Entities.Core;
using SubastaMaestra.Models.DTOs.Reports;
using SubastaMaestra.Models.DTOs.User;
using SubastaMaestra.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<OperationResult<int>> RegisterUserAsync(UserCreateDTO user);
        Task<OperationResult<User>> ValidateUserAsync(LoginRequestDTO userdto);
        // reportes 
        Task<List<MostActiveUserDTO>> ObtenerUsuariosMasActivos();
        //Task<User?> GetUser(UserDTO userDTO);
    }
}
