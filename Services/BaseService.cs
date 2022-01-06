using BibliotecaApi.DTOs;
using BibliotecaApi.Repositories;
using Domain.Enities;
using System;
using System.Collections.Generic;

namespace BibliotecaApi.Services
{
    public abstract class BaseService<_baseEntity> where _baseEntity : BaseEntity<Guid>
    {
        private readonly BaseRepository<_baseEntity> _repository;
        public BaseService(BaseRepository<_baseEntity> repository)
        {
            _repository = repository;
        }

        public IEnumerable<_baseEntity> GetAll()
        {
            return _repository.GetAll();
        }

        public _baseEntity GetbyId(Guid id)
        {
            return _repository.GetById(id);
        }

        public void Add(BaseEntity<Guid> entity)
        {
            _repository.Add((_baseEntity)entity);
        }

        public _baseEntity Update(Guid id, BaseEntity<Guid> entity)
        {
            return _repository.Update(id, (_baseEntity)entity);
        }

        public bool Remove(BaseEntity<Guid> baseEntity)
        {
            return _repository.Remove((_baseEntity)baseEntity);
        }
    }
}
