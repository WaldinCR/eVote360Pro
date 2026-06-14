using eVote360Pro.Core.Application.ViewModels.Alliance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Application.Interfaces
{
    public interface IAllianceRequestService
    {
        Task<List<AllianceRequestViewModel>> GetAllViewModel();
        Task<List<AllianceRequestViewModel>> GetReceivedPendingAsync(int receiverPartyId);
        Task<List<AllianceRequestViewModel>> GetSentByPartyAsync(int applicantPartyId);
        Task AddAsync(CreateAllianceRequestViewModel vm);
        Task AcceptRequestAsync(int id);
        Task RejectRequestAsync(int id);
        Task DeleteAsync(int id);
        Task<bool> HasPendingRequestAsync(int applicantId, int receiverId);
    }
}
