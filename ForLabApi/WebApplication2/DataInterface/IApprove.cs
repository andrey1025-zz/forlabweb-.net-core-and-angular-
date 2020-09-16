using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.DataInterface
{
    public interface IApprove<Approve, PendingApprovelist>
    {
        int approvalmasterdata(Approve approve);
        List<PendingApprovelist> getpendingapprovallist();
    }
}
