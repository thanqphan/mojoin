using mojoin.Models;

namespace mojoin.Extension
{
    public class UserBalanceManager
    {
        private readonly DbmojoinContext _dbContext;

        public UserBalanceManager(DbmojoinContext dbContext)
        {
            _dbContext = dbContext;
        }

        public float GetUserBalance(int userId)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            return (float)user.Balance;
        }

        public void UpdateUserBalance(int userId, float amount)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                user.Balance += amount;
                _dbContext.SaveChanges();
            }
        }

        // Các phương thức khác cần thiết cho việc quản lý số dư
    }

}
