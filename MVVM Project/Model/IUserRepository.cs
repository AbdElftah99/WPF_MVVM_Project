using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_Project.Model
{
    public interface IUserRepository
    {
        bool AuthenticateUser(NetworkCredential credential);
        void Add(UserModel userModel);
        void Edit(UserModel userModel);
        void Remove(UserModel userModel);
        UserModel GetById(int id);
        UserModel GetByUserName(string name);
        IEnumerable<UserModel> GetAll();
    }
}
