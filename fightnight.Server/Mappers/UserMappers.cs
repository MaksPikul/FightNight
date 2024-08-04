using fightnight.Server.Dtos.User;
using fightnight.Server.models;

namespace fightnight.Server.Mappers
{
    public static class UserMappers
    {
        public static UserDto ToUserDto(this User userModel ) {
            return new UserDto
            {
                id = userModel.id,
                name = userModel.name,
                email = userModel.email,
                pfp = userModel.pfp,
            };
        }

        public static User UserFromCreateDto(this CreateUserDto userDto) {
            return new User
            {
                name = userDto.name,
                email = userDto.email,
                pfp = userDto.pfp,
                password = userDto.password
            };
        }
     }
}
