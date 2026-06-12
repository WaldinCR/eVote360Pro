using eVote360Pro.Core.Application.Dtos.Email;
using eVote360Pro.Core.Application.Dtos.User;
using eVote360Pro.Core.Application.Helpers;
using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Domain.Interfaces;
using AutoMapper;

namespace eVote360Pro.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public UserService(IUserRepository userRepository, IMapper mapper, IEmailService emailService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _emailService = emailService;
        }
        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<SaveUserDto?> GetByIdSaveDtoAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id); 
            if (user == null) return null;

            return _mapper.Map<SaveUserDto>(user);
        }

        public async Task<SaveUserDto> AddAsync(SaveUserDto dto)
        {
            dto.Password = PasswordEncryptation.ComputeSha256Hash(dto.Password);

            User entity = _mapper.Map<User>(dto);
            User savedEntity = await _userRepository.AddAsync(entity);

            await _emailService.SendAsync(new EmailRequestDto
            {
                Subject = "Bienvenido a eVote360 Pro",
                To = savedEntity.Email,
                HtmlBody = $"<p>Tu cuenta ha sido creada con éxito. Tu nombre de usuario es: <strong>{savedEntity.UserName}</strong></p>"
            });

            return _mapper.Map<SaveUserDto>(savedEntity);
        }

        public async Task UpdateAsync(SaveUserDto dto)
        {
            var entityDb = await _userRepository.GetByIdAsync(dto.Id);
            if (entityDb == null) return;

            _mapper.Map(dto, entityDb);
            entityDb.Password = PasswordEncryptation.ComputeSha256Hash(dto.Password);

            await _userRepository.UpdateAsync(entityDb);
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user != null)
            {
                await _userRepository.DeleteAsync(user); 
            }
        }

        public async Task<bool> ExistsUserNameAsync(string username)
        {
            return await _userRepository.ExistsUserNameAsync(username);
        }

        public async Task<bool> ExistsEmailAsync(string email)
        {
            return await _userRepository.ExistsEmailAsync(email);
        }

        public async Task<string?> ToggleActiveStatusAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return "El usuario no existe.";

            if (user.IsActive)
            {
                bool isTheOnlyAdmin = await _userRepository.IsTheOnlyActiveAdminAsync(id);
                if (isTheOnlyAdmin)
                {
                    return "No se puede desactivar al único administrador activo del sistema.";
                }
            }

            user.IsActive = !user.IsActive;
            await _userRepository.UpdateAsync(user);

            return null;
        }
        public async Task<UserDto?> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.LoginAsync(dto.UserName, dto.Password);
            if (user == null) return null;

            return _mapper.Map<UserDto>(user);
        }
        public async Task<bool> HasPoliticalPartyAssignedAsync(int userId)
        {
            // Depende de la entidad PoliticalLeader 
            // Por ahora retorna true para no bloquear el desarrollo
            return await Task.FromResult(true);
        }
    }
}