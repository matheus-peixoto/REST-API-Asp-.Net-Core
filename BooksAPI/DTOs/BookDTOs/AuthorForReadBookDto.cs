﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksAPI.DTOs.BookDTOs
{
    public class AuthorForReadBookDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string ShortBio { get; set; }

        public AuthorForReadBookDto() { }

        public AuthorForReadBookDto(int id, string name, DateTime birthDate, string shortBio)
        {
            Id = id;
            Name = name;
            BirthDate = birthDate;
            ShortBio = shortBio;
        }
    }
}
