
using System;
using System.Collections.Generic;
using System.Linq;
using WebApi_M2.Models;

namespace WebApi_M2.Services;

public class UserService 
{
    private List<User> users = new List<User>();
    public UserService()
    {
        users.Add(new User() {Id = 1, Email = "shashank@gmail.com", Name = "Shashank"});
    }

    public IEnumerable<User> GetAllUsers() 
    {
        return users;
    }
    
    public User GetUserById(int id) 
    {
        return users.FirstOrDefault(u => u.Id == id);
    }

    public User AddUser(User user) 
    {
        user.Id = GetNextId();
        users.Add(user);
        return user;
    }

    public User UpdateUser(User updatedUser) 
    {
        return UpdateUser(updatedUser.Id, updatedUser);
    }

    public User UpdateUser(int id, User updatedUser) 
    {
        var user = GetUserById(id);
        if (user != null) 
        {
            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
        }
        return user;
    }

    public void DeleteUser(int id) 
    {
        var user = GetUserById(id);
        if (user != null) 
        {
            users.Remove(user);
        }
    }

    private int GetNextId() 
    {
        return users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
    }
}