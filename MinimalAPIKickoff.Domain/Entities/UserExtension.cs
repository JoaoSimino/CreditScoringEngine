using MinimalAPIKickoff.Domain.DTOs;

namespace MinimalAPIKickoff.Domain.Entities;

public static class UserExtension
{
    public static User DtoToEntity(this User user, UserDto userDto)
    {
        user.Id = Guid.NewGuid();
        user.Email = userDto.Email;
        user.Name = userDto.Name;

        return user;
    }
}
