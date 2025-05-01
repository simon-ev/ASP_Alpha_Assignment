using Data.Models;
using Data.Entities;
using Data.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using Business.Models;


namespace Business.Services;

public interface IUserService
{
    Task<UserResult> GetUsersAsync();
    Task<UserResult> AddUserToRole(string userId, string roleName);
    Task<UserResult> CreateUserAsync(SignUpFormData formData, string roleName = "User");
}

public class UserService(IUserRepository userRepository, UserManager<UserEntity> userManager, RoleManager<IdentityRole> roleManager) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;


    /* Map metod skriven av ChatGpt */

    public async Task<UserResult> GetUsersAsync()
    {
        var result = await _userRepository.GetAllAsync();
        return new UserResult
        {
            Succeeded = result.Succeeded,
            StatusCode = result.StatusCode,
            Result = result.Succeeded ? result.Result?.Select(MapToModel).ToList() : null,
            Error = result.Succeeded ? null : "No users found."
        };
    }

    private static User MapToModel(UserEntity entity)
    {
        return new User
        {
            Id = entity.Id
        };
    }

    public async Task<UserResult> AddUserToRole(string userId, string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
            return new UserResult { Succeeded = false, StatusCode = 404, Error = "Role not found."};

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return new UserResult { Succeeded = false, StatusCode = 404, Error = "User not found." };

        var result = await _userManager.AddToRoleAsync(user, roleName);
        return result.Succeeded
            ? new UserResult { Succeeded = true, StatusCode = 200 }
            : new UserResult { Succeeded = false, StatusCode = 400, Error = "Failed to add user to role." };
    }

    public async Task<UserResult> CreateUserAsync(SignUpFormData formData, string roleName = "User")
    {
        if (formData == null)
            return new UserResult { Succeeded = false, StatusCode = 400, Error = "Form data cannot be null." };

        var existsResult = await _userRepository.ExistsAsync(x => x.Email == formData.Email);
        if (existsResult.Succeeded)
            return new UserResult { Succeeded = false, StatusCode = 409, Error = "User with same email already exist." };

        try
        {
            var userEntity = new UserEntity
            {
                Email = formData.Email,
                UserName = formData.Email, 
                FullName = formData.FullName

            };

            var result = await _userManager.CreateAsync(userEntity, formData.Password);
            if (result.Succeeded)
            {
                var addToRoleResult = await AddUserToRole(userEntity.Id, roleName);

                return addToRoleResult.Succeeded
                    ? new UserResult { Succeeded = true, StatusCode = 201 }
                    : new UserResult { Succeeded = false, StatusCode = 201, Error = "User created but not added to role" };
            }
            return new UserResult { Succeeded = false, StatusCode = 500, Error = "Unable to create user." };

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new UserResult { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }
}
