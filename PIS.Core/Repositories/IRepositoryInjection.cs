using Microsoft.EntityFrameworkCore;

namespace PIS.Framework.Repositories
{
    public interface IRepositoryInjection
    {
        IRepositoryInjection SetContext(DbContext context);
    }
}