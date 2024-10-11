using TgLab.Application.User.DTOs;
using TgLab.Infrastructure.Context;
using TgLab.Application.User.Exceptions;
using TgLab.Application.User.Interfaces;
using UserDb = TgLab.Domain.Models.User;

namespace TgLab.Application.User.Services
{
    public class UserService : IUserService
    {
        private readonly TgLabContext _context;

        public UserService(TgLabContext context)
        {
            _context = context;
        }

        public Task Create(CreateUserDTO dto)
        {
            if(dto.IsUnder18())
            {
                throw new Under18Exception();
            }

            var user = new UserDb()
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password,
                BirthDate = dto.BirthDate,
            };

            _context.Users.Add(user);
            var result = _context.SaveChanges();

            return Task.FromResult(result);
        }
    }
}
