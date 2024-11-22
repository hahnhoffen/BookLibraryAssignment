using System.Collections.Generic;
using BookLibrary.Domain.Entities;

namespace BookLibrary.Infrastructure.DataBase
{
    public class FakeDatabase
    {
        public List<Book> Books { get; set; } = new();
        public List<Author> Authors { get; set; } = new();
    }
}