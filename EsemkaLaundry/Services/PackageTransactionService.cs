using EsemkaLaundry.Helper;
using EsemkaLaundry.ViewModel;

namespace EsemkaLaundry.Services
{
    public class PackageTransactionService : BaseService
    {
        private EsemkaLaundryContext db = new EsemkaLaundryContext();

        public PackageTransactionService(EsemkaLaundryContext db, string email) : base(db, email)
        {
            this.db = db;
        }

        public async Task<Response> GetPackageTransaction(int page = 0, string? keyword = null)
        {
            try
            {
                if (page != 0 && keyword != null)
                {
                    var packageTransaction = await db.PackageTransactions
                        .Where(s => s.User.Email.Contains(keyword) ||
                        s.User.Name.Contains(keyword) ||
                        s.User.PhoneNumber.Contains(keyword) ||
                        s.Package.Service.Name.Contains(keyword))
                        .Include(s => s.User)
                        .Include(s => s.Package)
                        .Include(s => s.Package.Service)
                        .Skip((page - 1) * 3)
                        .Take(3)
                        .OrderByDescending(s => s.CreatedAt)
                        .ToListAsync();
                    return response(200, data: packageTransaction);
                }
                else if (keyword != null)
                {
                    var packageTransactions = await db.PackageTransactions
                             .Where(s => keyword == null || s.User.Email.Contains(keyword) ||
                             s.User.Name.Contains(keyword) ||
                             s.User.PhoneNumber.Contains(keyword) ||
                             s.Package.Service.Name.Contains(keyword))
                             .Include(s => s.User)
                            .Include(s => s.Package)
                            .Include(s => s.Package.Service)
                            .OrderByDescending(s => s.CreatedAt)
                            .ToListAsync();
                    return response(200, data: packageTransactions);
                }
                else if (page != 0)
                {
                    var packageTransaction = await db.PackageTransactions
                        .Include(s => s.User)
                        .Include(s => s.Package)
                        .Include(s => s.Package.Service)
                        .Skip((page - 1) * 3)
                        .Take(3)
                        .OrderByDescending(s => s.CreatedAt)
                        .ToListAsync();
                    return response(200, data: packageTransaction);
                }
                var packageTransactionss = await db.PackageTransactions
                        .Include(s => s.User)
                        .Include(s => s.Package)
                        .Include(s => s.Package.Service)
                        .OrderByDescending(s => s.CreatedAt)
                        .ToListAsync();
                return response(200, data: packageTransactionss);
            }
            catch (Exception x)
            {
                return response(500, x.Message);
            }
        }

        public async Task<Response> PostPackageTransaction(PackageTransactionViewModel request)
        {
            try
            {
                var checkPackage = await db.Packages.Where(s => s.Id == request.PackageId).FirstOrDefaultAsync();
                if (checkPackage == null) return response(404, "Package not found");
                var checkEmail = await db.Users.Where(s => s.Email == request.UserEmail).FirstOrDefaultAsync();
                if (checkEmail == null) return response(404, "User not found");
                var packageTransaction = new PackageTransaction
                {
                    Id = Guid.NewGuid(),
                    UserEmail = request.UserEmail,
                    PackageId = request.PackageId,
                    Price = checkPackage.Price,
                    AvailableUnit = 0,
                    CreatedAt = DateTime.Now,
                    CompletedAt = null
                };
                db.PackageTransactions.Add(packageTransaction);
                await db.SaveChangesAsync();
                return response(201, data: db.PackageTransactions.Where(s => s.Id == packageTransaction.Id).Include(s => s.Package.Service).FirstOrDefault());
            }
            catch (Exception x)
            {
                return response(500, x.Message);
            }
        }

        public async Task<Response> GetById(Guid id)
        {
            try
            {
                var checkPackageId = await db.PackageTransactions
                        .Where(s => s.Id == id)
                        .Include(s => s.User)
                        .Include(s => s.Package)
                        .Include(s => s.Package.Service)
                        .OrderByDescending(s => s.CreatedAt)
                        .FirstOrDefaultAsync();
                if (checkPackageId == null) return response(404, "Package Transaction not found");
                return response(200, data: checkPackageId);
            }
            catch (Exception X)
            {
                return response(500, X.Message);
            }
        }
    }
}