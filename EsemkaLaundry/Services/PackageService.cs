using EsemkaLaundry.Helper;
using EsemkaLaundry.ViewModel;

namespace EsemkaLaundry.Services
{
    public class PackageService : BaseService
    {
        private EsemkaLaundryContext db = new EsemkaLaundryContext();

        public PackageService(EsemkaLaundryContext db, string email) : base(db, email)
        {
            this.db = db;
        }

        public async Task<Response> GetPackage(int page = 0, int total = 0, double minPrice = 0, double maxPrice = 0)
        {
            try
            {
                if (page == 0) page = 1;
                if (page != 0 || total != 0 || minPrice != 0 || maxPrice != 0)
                {
                    if (minPrice < maxPrice && minPrice > 0 && maxPrice > 0)
                    {
                        var package = await db.Packages.Where(s => total == 0 || s.Total == total)
                        .Where(s => minPrice == 0 || minPrice == 0 || s.Price >= minPrice && s.Price <= maxPrice)
                        .Skip((page - 1) * 10)
                        .Take(10)
                        .OrderBy(S => S.Service.Name)
                        .Include(s => s.Service)
                        .ToListAsync();
                        return response(200, data: package);
                    }
                    else
                    {
                        if (minPrice > maxPrice && minPrice != 0 && maxPrice != 0) return response(200, data: null);
                        if (minPrice != 0 && maxPrice == 0)
                        {
                            var package = await db.Packages.Where(s => total == 0 || s.Total == total)
                                .Where(s => s.Price >= minPrice)
                                .Skip((page - 1) * 10)
                                .Take(10)
                                .OrderBy(S => S.Service.Name)
                                .Include(s => s.Service)
                                .ToListAsync();
                            return response(200, data: package);
                        }
                        else if (maxPrice != 0 && minPrice == 0)
                        {
                            var package = await db.Packages.Where(s => total == 0 || s.Total == total)
                                .Where(s => s.Price <= maxPrice)
                                .Skip((page - 1) * 10)
                                .Take(10)
                                .OrderBy(S => S.Service.Name)
                                .Include(s => s.Service)
                                .ToListAsync();
                            return response(200, data: package);
                        }
                    }
                }
                var packages = await db.Packages.OrderBy(S => S.Service.Name)
                    .Include(s => s.Service)
                    .Skip((page - 1) * 10)
                    .Take(10).ToListAsync();
                return response(200, data: packages);
            }
            catch (Exception x)
            {
                return response(500, x.Message);
            }
        }

        public async Task<Response> GetPackageId(Guid id)
        {
            try
            {
                var checkPackage = await db.Packages.Where(s => s.Id == id).Include(s => s.Service).FirstOrDefaultAsync();
                if (checkPackage == null) return response(404, "Package not found");
                return response(200, data: checkPackage);
            }
            catch (Exception x)
            {
                return response(500, x.Message);
            }
        }

        public async Task<Response> PostPackage(PackageViewModel request)
        {
            try
            {
                var checkService = await db.Services.Where(s => s.Id == request.ServiceId).FirstOrDefaultAsync();
                if (checkService == null) return response(404, "Service not found");
                var package = new Package
                {
                    Id = Guid.NewGuid(),
                    ServiceId = request.ServiceId,
                    Total = request.Total,
                    Price = request.Price
                };
                db.Packages.Add(package);
                await db.SaveChangesAsync();
                return response(201, data: package);
            }
            catch (Exception x)
            {
                return response(500, x.Message);
            }
        }

        public async Task<Response> PutPackage(Guid id, PackageViewModel request)
        {
            try
            {
                var checkPackage = await db.Packages.Where(s => s.Id == id).FirstOrDefaultAsync();
                if (checkPackage == null) return response(404, "Package not found");

                var checkService = await db.Services.Where(s => s.Id == request.ServiceId).FirstOrDefaultAsync();
                if (checkService == null) return response(404, "Service not found");

                if (id != request.Id) return response(400, "Id Package must be same Id Paramater");
                checkPackage.Id = request.Id;
                checkPackage.ServiceId = request.ServiceId;
                checkPackage.Total = request.Total;
                checkPackage.Price = request.Price;
                await db.SaveChangesAsync();
                return response(200);
            }
            catch (Exception x)
            {
                return response(500, x.Message);
            }
        }

        public async Task<Response> DeletePackage(Guid id)
        {
            try
            {
                var checkPackage = await db.Packages.Where(s => s.Id == id).FirstOrDefaultAsync();
                if (checkPackage == null) return response(404, "Package not found");
                db.Packages.Remove(checkPackage);
                await db.SaveChangesAsync();
                return response(200);
            }
            catch (Exception x)
            {
                return response(500, x.Message);
            }
        }
    }
}