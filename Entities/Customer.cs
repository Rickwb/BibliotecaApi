﻿using System;
using System.Collections.Generic;

namespace BibliotecaApi.Entities
{
    public class Customer : Person
    {
        private List<Reservation> _reservations;
        private List<Withdraw> _withdraws;

        //Create Constructor
        public Customer(string name, string document, string cep, Guid userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            _reservations ??= new List<Reservation>();
            _withdraws ??= new List<Withdraw>();
            Name = name;
            Document = document;
            Cep = cep;
        }
        //Update Constructor
        public Customer(string name, string document, string cep)
        {
            Name = name;
            Document = document;
            Cep = cep;
        }


        public IReadOnlyList<Reservation> Reservations => _reservations;
        public IReadOnlyList<Withdraw> Withdraws => _withdraws;

        public decimal Multa { get; set; }
        public Adress Adress { get; set; }


        
    }
}
