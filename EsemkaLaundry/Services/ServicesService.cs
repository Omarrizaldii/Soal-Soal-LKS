using EsemkaLaundry.Helper;
using EsemkaLaundry.Models;
using EsemkaLaundry.ViewModel;
using static EsemkaLaundry.Helper.EnumCollection;

namespace EsemkaLaundry.Services
{
    public class ServicesService : BaseService
    {
        private EsemkaLaundryContext db = new EsemkaLaundryContext();

        public ServicesService(EsemkaLaundryContext db, IConfiguration configuration, string email) : base(db, email)
        {
            this.db = db;
        }

        public async Task<Response> GetService(int page = 0, string? name = null,
            ServiceCategory? category = null, ServiceUnit? unit = null, double minPrice = 0, double maxPrice = 0)
        {
            try
            {
                if (page == 0) page = 1;
                var pageResult = 10f;
                var pageCount = Math.Ceiling(db.Services.Count() / pageResult);

                if (page != 0 || name != null || category != null || unit != null || minPrice != 0 || maxPrice != 0)
                {
                    if (minPrice < maxPrice && minPrice > 0)
                    {
                        var service = await db.Services.Where(s => name == null || s.Name.Contains(name))
                            .Where(s => category == null || s.Category == category)
                            .Where(s => unit == null || s.Unit == unit)
                            .Where(s => minPrice == 0 || maxPrice == 0 || s.Price >= minPrice && s.Price <= maxPrice)
                            .Skip((page - 1) * (int)pageResult)
                            .Take((int)pageResult)
                            .OrderBy(s => s.Category).ToListAsync();
                        return response(200, data: service);
                    }
                    else
                    {
                        if (minPrice > maxPrice && minPrice != 0 && maxPrice != 0) return response(200);
                        if (minPrice != 0 && maxPrice == 0)
                        {
                            var service = await db.Services.Where(s => name == null || s.Name.Contains(name))
                                .Where(s => category == null || s.Category == category)
                                .Where(s => unit == null || s.Unit == unit)
                                .Where(s => s.Price <= maxPrice)
                                .Skip((page - 1) * (int)pageResult)
                                .Take((int)pageResult)
                                .OrderBy(s => s.Category).ToListAsync();
                            return response(200, data: service);
                        }
                        else if (maxPrice != 0 && minPrice == 0)
                        {
                            var service = await db.Services.Where(s => name == null || s.Name.Contains(name))
                                .Where(s => category == null || s.Category == category)
                                .Where(s => unit == null || s.Unit == unit)
                                .Where(s => s.Price >= minPrice)
                                .Skip((page - 1) * (int)pageResult)
                                .Take((int)pageResult)
                                .OrderBy(s => s.Category).ToListAsync();
                            return response(200, data: service);
                        }
                    }
                }

                var services = await db.Services.OrderBy(S => S.Category).ToListAsync();
                return response(200, data: services);
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
                var service = await db.Services.Where(s => s.Id == id).FirstOrDefaultAsync();
                if (service == null) return response(404, "Service not found");
                return response(200, data: service);
            }
            catch (Exception x)
            {
                return response(500, x.Message);
            }
        }

        public async Task<Response> Post(ServiceViewModel request)
        {
            try
            {
                var checkServiceName = await db.Services.Where(s => s.Name == request.Name).FirstOrDefaultAsync();
                if (checkServiceName != null) return response(409, "Service already exist");
                db.Services.Add(new Service
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Category = request.Category,
                    Unit = request.Unit,
                    Price = request.Price,
                    EstimationDuration = request.EstimationDuration
                });
                await db.SaveChangesAsync();
                return response(201, data: request);
            }
            catch (Exception x)
            {
                return response(500, x.Message);
            }
        }

        public async Task<Response> Put(Guid id, ServiceViewModel request)
        {
            try
            {
                var checkService = await db.Services.Where(s => s.Id == id).FirstOrDefaultAsync();
                if (checkService == null) return response(404, "Service not found");

                var checkServiceName = await db.Services.Where(s => s.Name == request.Name).FirstOrDefaultAsync();
                if (checkServiceName != null) return response(409, "Name Service already exist");

                if (request.Id != id) return response(400, "Id parameter must be same id request");
                checkService.Name = request.Name;
                checkService.Category = request.Category;
                checkService.Unit = request.Unit;
                checkService.Price = request.Price;
                checkService.EstimationDuration = request.EstimationDuration;
                await db.SaveChangesAsync();
                return response(200);
            }
            catch (Exception x)
            {
                return response(500, x.Message);
            }
        }

        public async Task<Response> Delete(Guid id)
        {
            try
            {
                var checkService = await db.Services.Where(s => s.Id == id).FirstOrDefaultAsync();
                if (checkService == null) return response(404, "Service not found");
                db.Services.Remove(checkService);
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