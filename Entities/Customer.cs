using System;
using System.Collections.Generic;

namespace BibliotecaApi.Entities
{
    public class Customer : Person
    {
        private List<Reservation> _reservations;
        private List<Withdraw> _withdraws;

        //Create Constructor
        public Customer(Guid id,string name,int age, string document, string cep, Guid userId,DateTime birthdate)
        {
            Id = id;
            UserId = userId;
            Age = age;
            _reservations ??= new List<Reservation>();
            _withdraws ??= new List<Withdraw>();
            Name = name;
            Document = document;
            Cep = cep;
            BirthDate = birthdate;
        }
        public Customer(Guid id, string name,int age, string document, Guid userId, DateTime birthdate,Adress adress)
        {
            Id = id;
            UserId = userId;
            Age= age;
            _reservations ??= new List<Reservation>();
            _withdraws ??= new List<Withdraw>();
            Name = name;
            Document = document;
            Adress = adress;
            BirthDate = birthdate;
        }
        //Update Constructor
        public Customer(Guid id,string name,int age, string document, string cep)
        {
            Id=id;
            Name = name;
            Age = age;
            Document = document;
            Cep = cep;
            _reservations ??= new List<Reservation>();
            _withdraws ??= new List<Withdraw>();
        }
        public Customer(Guid id,string name,string document,int age,string cep,DateTime birthdate,List<Reservation> reservations,List<Withdraw> withdraws)
        {
            Id = id;
            _reservations ??= reservations;
            _withdraws ??= withdraws;
            Age = age;
            Name = name;
            Document = document;
            Cep = cep;
            BirthDate = birthdate;
            _reservations ??= new List<Reservation>();
            _withdraws ??= new List<Withdraw>();
        }

        public IReadOnlyList<Reservation> Reservations => _reservations;
        public IReadOnlyList<Withdraw> Withdraws => _withdraws;

        private decimal Multa { get; set; }
        public Adress Adress { get; set; }

        public void PropretiesUpdate(string name, string document, string cep,Adress adress)
        {
            Name = name;
            Document=document;
            Cep=cep;
            Adress = adress;
        }

        public List<Reservation> GetReservationsCustomer()
        {
            return _reservations;
        }
        public List<Withdraw> GetWithdrawsCustomer()
        {
            return _withdraws;
        }

    }
}
