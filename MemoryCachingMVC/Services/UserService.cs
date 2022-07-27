using MemoryCachingMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace MemoryCachingMVC.Services
{
    public class UserService
    {
        private ApplicationContext db;
        private IMemoryCache cache;

        public UserService(ApplicationContext context, IMemoryCache memoryCache)
        {
            db = context;
            cache = memoryCache;
            Initialize();
        }

        public void Initialize()
        {
            if (!db.Users.Any())
            {
                db.Users.AddRange(
                    new User { Name = "Tom", Email = "tom@gmail.com", Age = 35 },
                    new User { Name = "Alice", Email = "alice@gmail.com", Age = 29 },
                    new User { Name = "Sam", Email = "sam@gmail.com", Age = 37 }
                );
                db.SaveChanges();
            }
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await db.Users.ToListAsync();
        }

        public async Task AddUser(User user)
        {
            db.Users.Add(user);
            int n = await db.SaveChangesAsync();

            if(n > 0)
            {
                cache.Set(user.Id, user, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            }
        }

        public async Task<User> GetUser(int id)
        {
            User user = null;
            if(!cache.TryGetValue(id, out user))
            {
                user = await db.Users.FirstOrDefaultAsync(p=>p.Id == id);

                if(user != null)
                {
                    cache.Set(user.Id, user, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(5)));
                }
            }
            return user;
        }
    }
}
