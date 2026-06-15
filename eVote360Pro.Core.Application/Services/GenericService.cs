using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Domain.Interfaces;
using AutoMapper;

namespace eVote360Pro.Core.Application.Services
{
    public class GenericService<TEntity, TDto> : IGenericService<TDto>
        where TEntity : class
        where TDto : class
    {
        private readonly IGenericRepository<TEntity> _repository;
        private readonly IMapper _mapper;

        public GenericService(IGenericRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<TDto?> AddAsync(TDto dto)
        {
            try
            {
                TEntity entity = _mapper.Map<TEntity>(dto);
                TEntity returnEntity = await _repository.AddAsync(entity);
                return _mapper.Map<TDto>(returnEntity);
            }
            catch (Exception) { return null; }
        }

        public virtual async Task<TDto?> UpdateAsync(TDto dto, int id)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null) return null;
                _mapper.Map(dto, existing);
                await _repository.UpdateAsync(existing);
                return _mapper.Map<TDto>(existing);
            }
            catch (Exception) { return null; }
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null) return false;
                await _repository.DeleteAsync(entity);
                return true;
            }
            catch (Exception) { return false; }
        }

        public virtual async Task<TDto?> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null) return null;
                return _mapper.Map<TDto>(entity);
            }
            catch (Exception) { return null; }
        }

        public virtual async Task<List<TDto>> GetAllAsync()
        {
            try
            {
                var entities = await _repository.GetAllAsync();
                return _mapper.Map<List<TDto>>(entities);
            }
            catch (Exception) { return []; }
        }
    }
}

