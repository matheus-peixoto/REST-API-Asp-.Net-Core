﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BooksAPI.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public List<AuthorBook> AuthorBooks { get; set; }
        public string Sinopse { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
