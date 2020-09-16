using ForLabApi.DataInterface;
using ForLabApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace ForLabApi.Repositories
{
    public class GroupAccessRepositories:IGroup<Group>
    {

        public IEnumerable<Group> GetGroup()
        {
            Group[] grp = new Group[]
          {

                new Group("","")
                {
                    Name="Testing14",
                    Parent="Cash-in-Hand"
                },
                  new Group("","")
                {
                    Name="Testing15",
                    Parent="Cash-in-Hand"
                }
          };
            return grp;
        }
        public string SaveGroup(Group group)
        {
            string res = "";
            return res;
        }
    }
}
