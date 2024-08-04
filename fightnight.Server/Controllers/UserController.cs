using fightnight.Server.Data;
using fightnight.Server.Dtos.User;
using fightnight.Server.Interfaces;
using fightnight.Server.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fightnight.Server.Controllers
{
    [Route("/api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IUserRepo _userRepo;
        public UserController(AppDBContext context, IUserRepo userRepo)
        {
           _userRepo = userRepo;
           _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepo.GetAllAsync();
                
            var usersDto = users.Select(s => s.ToUserDto());

            return Ok(users); 
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            
            if (user == null) {
                return NotFound();
            }

            return Ok(user.ToUserDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto){

            var userModel = userDto.UserFromCreateDto();
            await _userRepo.CreateUserAsync(userModel);
            return CreatedAtAction(
                nameof(GetById), 
                new { id = userModel.id }, 
                userModel.ToUserDto()
             );
        }
    }
}
