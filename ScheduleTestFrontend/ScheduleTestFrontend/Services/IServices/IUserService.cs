﻿using ScheduleTestFrontend.Models;

namespace ScheduleTestFrontend.Services.IServices
{
    public interface IUserService
    {
        //Task<User> GetUserByEmailPassword(string email, string password);
        Task<User> GetUserData(string token);
        Task<string> Login(string email, string password);
        Task<User> InsertUser(User user);
        Task<bool> DeleteUser(string token);
        Task<User> UpdateUser(User user, string token);
    }

}
