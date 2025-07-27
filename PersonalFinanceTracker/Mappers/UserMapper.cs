using PersonalFinanceTracker.Dtos.UserDtos;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToUserDto(this User user) {
            return new UserDto { 
                UserId = user.UserId,
                UserName = user.UserName,
                Balance = user.Balance,
                Transactions = user.Transactions
            };
        }

        public static User ToUserFromCreateDto(this CreateUserDto userDto) {
            return new User {
                UserName = userDto.UserName,
                Balance = userDto.Balance
            };
        }
    }
}
