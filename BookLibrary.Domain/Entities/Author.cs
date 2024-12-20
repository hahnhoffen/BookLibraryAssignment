﻿namespace BookLibrary.Domain.Entities
{
    public class Author
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Author() { }
        public ICollection<Book> Books { get; set; } = new List<Book>();
        public Author(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
    }
}

