﻿using BibliotecaApi.Entities;
using System;
using System.Collections.Generic;

namespace BibliotecaApi.DTOs
{
    public class CreateWithdrawDTO : BaseDTO
    {
        public Guid IdCustomer { get; set; }
        public Guid IdReservation { get; set; }
        public DateTime WithdrawDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public List<Book> BooksNoReservation { get; set; }
        public override void Valid()
        {
            IsValid=true;
        }


    }
}