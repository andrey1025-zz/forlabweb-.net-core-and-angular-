using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.DataInterface
{
    public interface IGroup<Group>
    {
        IEnumerable<Group> GetGroup();
        string SaveGroup(Group group);
    }
}
