using TA.Utils.Core.Diagnostics;
using Timtek.Patterns.DataAccess;
using Timtek.Patterns.DataAccess.EFCore;
using Timtek.SAR.Domain.Entities;

namespace Timtek.SAR.Infrastructure.Persistence;

public sealed class SarUnitOfWork : EntityFrameworkCoreUnitOfWork
{
    private readonly SarDbContext _context;

    public SarUnitOfWork(SarDbContext context, ILog log) : base(context, log)
    {
        _context = context;
    }

    public IRepository<Organisation, Guid> Organisations => new Repository<Organisation, Guid>(_context);
    public IRepository<Case, Guid> Cases => new Repository<Case, Guid>(_context);
    public IRepository<CaseActivityLog, Guid> CaseActivityLogs => new Repository<CaseActivityLog, Guid>(_context);
    public IRepository<SearchArea, Guid> SearchAreas => new Repository<SearchArea, Guid>(_context);
    public IRepository<Sector, Guid> Sectors => new Repository<Sector, Guid>(_context);
}
