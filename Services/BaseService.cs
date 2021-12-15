using BibliotecaApi.DTOs;
using BibliotecaApi.Entities;
using BibliotecaApi.Repositories;
using System;
using System.Collections.Generic;

namespace BibliotecaApi.Services
{
    public abstract class BaseService<_baseEntity> where _baseEntity : BaseEntity
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

        public void Add(BaseEntity entity)
        {
            _repository.Add((_baseEntity)entity);
        }

        public _baseEntity Update(Guid id, BaseEntity entity)
        {
            return _repository.Update(id, (_baseEntity)entity);
        }

        public bool Remove(Entities.BaseEntity baseEntity)
        {
            return _repository.Remove((_baseEntity)baseEntity);
        }
    }
}
