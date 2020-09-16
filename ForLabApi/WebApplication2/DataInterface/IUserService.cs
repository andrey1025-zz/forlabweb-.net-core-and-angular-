using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.DataInterface
{
    public interface IUserService<User, Importdata, updatepassword, GlobalRegion>
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        int updateuserpassword(int id, updatepassword password);
        string Resetpassword(string email);
        string Getglobalregion(int id);
        int verifyaccount(int id);
        string[] savedata(User b);
        string Importdefaultdata(Importdata importdata);
        IEnumerable<GlobalRegion> Getallglobalregions();

        void updatelogincount(int userid);
    }

}
