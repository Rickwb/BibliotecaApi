using System;
using System.Collections.Generic;
using System.Linq;
using DapperContext.Repositories;
using Domain;
using Domain.Enities;
using EFContext;

namespace BibliotecaApi.Repositories
{
    public abstract class BaseRepository<T> :IRepositoryBase<Guid, BaseEntity<Guid>>,IRepositoryDPBase<Guid,BaseEntity<Guid>> where T : BaseEntity<Guid>
    {
        protected readonly List<T> _repository;

        public BaseRepository()
        {
            _repository ??= new List<T>();
        }

        public T Add(T variavel)
        {
            _repository.Add(variavel);
            return variavel;
        }

        public void Delete(BaseEntity<Guid> entity)
        {
            
        }

        public BaseEntity<Guid> Find(BaseEntity<Guid> entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BaseEntity<Guid>> FindAll(BaseEntity<Guid> entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            return _repository;
        }

        public T GetById(Guid id)
        {
            return _repository.Find(x => x.Id == id);
        }

        public bool Insert(BaseEntity<Guid> entity)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            return _repository.Remove(item);
        }

        public bool RemoveById(Guid id)
        {
            var t = _repository.Where(x => x.Id == id).SingleOrDefault();
            return _repository.Remove(t);
        }

        public T Update(Guid id, T newT)
        {
            var index = _repository.FindIndex(x => x.Id == id);

            if (index != -1)
                _repository[index] = newT;
           
            return newT;
        }

        public bool Update(BaseEntity<Guid> entity)
        {
            throw new NotImplementedException();
        }

        IEnumerable<BaseEntity<Guid>> IRepositoryDPBase<Guid, BaseEntity<Guid>>.GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
