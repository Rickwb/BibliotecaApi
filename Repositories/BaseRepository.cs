﻿using BibliotecaApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BibliotecaApi.Repositories
{
    public abstract class BaseRepository<T>  where T: BaseEntity
    {
        private readonly List<T> _repository;

        public BaseRepository()
        {
            _repository ??= new List<T>();
        }

        public T Add(T variavel)
        {
            _repository.Add(variavel);
            return variavel;
        }

        public List<T> GetAll()
        {
            return _repository;
        }

        public T GetById(Guid id)
        {
            return (T)_repository.Find(x=>x.Id == id);
        }

        public bool Remove(T item)
        {
            return _repository.Remove(item);
        }

        public bool RemoveById(Guid id)
        {
            var t=_repository.Where(x => x.Id == id).SingleOrDefault();
            return _repository.Remove(t);
        }

        public T Update(Guid id,T newT)
        {
            var Index= _repository.IndexOf( GetById(id));

            _repository.Insert(Index, newT);

            return newT;

        }
        
    }
}