using System;
using System.Threading.Tasks;

namespace ChatApp.Service
{
    public interface ICRUDService<A, U, G, Y>
        where Y : struct
        where U : EntityUpdateDto<Y>
        where G : EntityBaseGetDto<Y>
    {
        Task<APIResult<Guid>> Add(A model, Y? userId, bool isCommit = true);
        Task<APIResult<Guid>> Update(U model, Y? userId = default(Y?), bool isCommit = true, bool checkAuthorize = false, bool isDeleted = false);
        Task<APIResult<Guid>> Delete(Y id, Y? userId = default(Y?), bool isCommit = true, bool checkAuthorize = false);
        Task<G> GetByID(Y id, Y? userId = default(Y?), bool isDeleted = false);
    }
}
