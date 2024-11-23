namespace BookLibrary.Domain.Entities
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid AuthorId { get; set; }
        public int Year { get; set; }
        public Book() { }
        public Book(string title)
        {
            Id = Guid.NewGuid();
            Title = title;
        }
    }
}

