using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static System.Reflection.Metadata.BlobBuilder;

partial class Program
{
    enum Genre
    {
        Fantasy = 1,
        ScienceFiction,
        Mystery,
        Romance,
        Horror,
        Biography,
        History
    }
    class Book
    {
        private static int nextId = 1;
        public int Id { get; private set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public Genre Genre { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public Book()
        {
            Id = nextId++;
        }
        public override string ToString()
        {
            return $"ID: {Id}, Название: \"{Title}\", Автор: {Author}, Жанр: {Genre}, Год: {Year}, Цена: {Price:C}";
        }

        class Library
        {
            private List<Book> books = new List<Book>();


            public void AddTestData()
            {
                books.AddRange(new[]
                {
                new Book { Title = "Война и мир", Author = "Лев Толстой", Genre = Genre.History, Year = 1869, Price = 1500 },
                new Book { Title = "Преступление и наказание", Author = "Федор Достоевский", Genre = Genre.Mystery, Year = 1866, Price = 1200 },
                new Book { Title = "1984", Author = "Джордж Оруэлл", Genre = Genre.ScienceFiction, Year = 1949, Price = 950 },
                new Book { Title = "Мастер и Маргарита", Author = "Михаил Булгаков", Genre = Genre.Fantasy, Year = 1967, Price = 1100 },
                new Book { Title = "Гарри Поттер и философский камень", Author = "Джоан Роулинг", Genre = Genre.Fantasy, Year = 1997, Price = 800 },
                new Book { Title = "Кладбище Домашних Животных", Author = "Стивен Кинг", Genre = Genre.Horror, Year = 1983, Price =5000}
            });
            }
            private bool IsValidYear(int year)
            {
                return year >= 1000 && year <= DateTime.Now.Year;
            }
            private bool IsValidPrice(decimal price)
            {
                return price >= 0;
            }
            private bool IsValidString(string str)
            {
                return !string.IsNullOrWhiteSpace(str);
            }
            public bool AddBook(string title, string author, Genre genre, int year, decimal price)
            {
                if (!IsValidString(title) || !IsValidString(author) || !IsValidYear(year) || !IsValidPrice(price))
                {
                    return false;
                }

                books.Add(new Book
                {
                    Title = title.Trim(),
                    Author = author.Trim(),
                    Genre = genre,
                    Year = year,
                    Price = price
                });
                return true;
            }


            public bool RemoveBook(int id)
            {
                var book = books.FirstOrDefault(b => b.Id == id);
                if (book != null)
                {
                    books.Remove(book);
                    return true;
                }
                return false;
            }


            public List<Book> FindByTitle(string title)
            {
                return books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
            }


            public List<Book> FindByAuthor(string author)
            {
                return books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)).ToList();
            }


            public List<Book> FindByGenre(Genre genre)
            {
                return books.Where(b => b.Genre == genre).ToList();
            }


            public List<Book> SortByTitle()
            {
                return books.OrderBy(b => b.Title).ToList();
            }


            public List<Book> SortByYear()
            {
                return books.OrderBy(b => b.Year).ToList();
            }


            public Book GetMostExpensiveBook()
            {
                return books.OrderByDescending(b => b.Price).FirstOrDefault();
            }

            public Book GetCheapestBook()
            {
                return books.OrderBy(b => b.Price).FirstOrDefault();
            }


            public Dictionary<string, int> GetBooksByAuthor()
            {
                return books.GroupBy(b => b.Author)
                           .ToDictionary(g => g.Key, g => g.Count());
            }

            public List<Book> GetAllBooks()
            {
                return new List<Book>(books);
            }


            class LibraryUI
            {
                private Library library = new Library();

                public void Run()
                {
                    library.AddTestData();

                    Console.WriteLine("Добро пожаловать в систему учета книг библиотеки!");

                    bool continueWorking = true;
                    while (continueWorking)
                    {
                        ShowMenu();
                        string choice = Console.ReadLine();

                        switch (choice)
                        {
                            case "1":
                                ShowAllBooks();
                                break;
                            case "2":
                                AddBook();
                                break;
                            case "3":
                                RemoveBook();
                                break;
                            case "4":
                                FindBooks();
                                break;
                            case "5":
                                SortBooks();
                                break;
                            case "6":
                                ShowPriceExtremes();
                                break;
                            case "7":
                                ShowAuthorStatistics();
                                break;
                            case "0":
                                continueWorking = false;
                                break;
                            default:
                                Console.WriteLine("Неверный выбор. Попробуйте снова.");
                                break;
                        }

                        if (continueWorking)
                        {
                            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                            Console.ReadKey();
                            Console.Clear();
                        }
                    }

                    Console.WriteLine("Программа завершена. Спасибо за использование!");
                }

                private void ShowMenu()
                {
                    Console.WriteLine("\n=== МЕНЮ БИБЛИОТЕКИ ===");
                    Console.WriteLine("1 - Показать все книги");
                    Console.WriteLine("2 - Добавить книгу");
                    Console.WriteLine("3 - Удалить книгу по ID");
                    Console.WriteLine("4 - Найти книги");
                    Console.WriteLine("5 - Сортировать книги");
                    Console.WriteLine("6 - Самая дорогая/дешевая книга");
                    Console.WriteLine("7 - Статистика по авторам");
                    Console.WriteLine("0 - Выйти");
                    Console.Write("Ваш выбор: ");
                }

                private void ShowAllBooks()
                {
                    var books = library.GetAllBooks();
                    if (books.Count == 0)
                    {
                        Console.WriteLine("В библиотеке нет книг.");
                        return;
                    }

                    Console.WriteLine($"\n=== ВСЕ КНИГИ ({books.Count}) ===");
                    foreach (var book in books)
                    {
                        Console.WriteLine(book);
                    }
                }

                private void AddBook()
                {
                    Console.WriteLine("\n=== ДОБАВЛЕНИЕ НОВОЙ КНИГИ ===");

                    try
                    {
                        Console.Write("Введите название книги: ");
                        string title = Console.ReadLine();

                        Console.Write("Введите автора: ");
                        string author = Console.ReadLine();

                        Console.WriteLine("\nДоступные жанры:");
                        foreach (Genre genre in Enum.GetValues(typeof(Genre)))
                        {
                            Console.WriteLine($"  {(int)genre} - {genre}");
                        }
                        Console.Write("Выберите жанр (число): ");
                        if (!Enum.TryParse(Console.ReadLine(), out Genre genreChoice) || !Enum.IsDefined(typeof(Genre), genreChoice))
                        {
                            Console.WriteLine("Ошибка: Неверный выбор жанра.");
                            return;
                        }

                        Console.Write("Введите год издания: ");
                        if (!int.TryParse(Console.ReadLine(), out int year))
                        {
                            Console.WriteLine("Ошибка: Год должен быть числом.");
                            return;
                        }

                        Console.Write("Введите цену: ");
                        if (!decimal.TryParse(Console.ReadLine(), out decimal price))
                        {
                            Console.WriteLine("Ошибка: Цена должна быть числом.");
                            return;
                        }

                        if (library.AddBook(title, author, genreChoice, year, price))
                        {
                            Console.WriteLine("Книга успешно добавлена!");
                        }
                        else
                        {
                            Console.WriteLine("Ошибка: Проверьте корректность введенных данных.");
                            Console.WriteLine("- Название и автор не должны быть пустыми");
                            Console.WriteLine("- Год должен быть от 1000 до текущего года");
                            Console.WriteLine("- Цена не должна быть отрицательной");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при вводе данных: {ex.Message}");
                    }
                }

                private void RemoveBook()
                {
                    Console.WriteLine("\n=== УДАЛЕНИЕ КНИГИ ===");
                    ShowAllBooks();

                    Console.Write("Введите ID книги для удаления: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        if (library.RemoveBook(id))
                        {
                            Console.WriteLine("Книга успешно удалена!");
                        }
                        else
                        {
                            Console.WriteLine("Книга с указанным ID не найдена.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: ID должен быть числом.");
                    }
                }

                private void FindBooks()
                {
                    Console.WriteLine("\n=== ПОИСК КНИГ ===");
                    Console.WriteLine("1 - Поиск по названию");
                    Console.WriteLine("2 - Поиск по автору");
                    Console.WriteLine("3 - Поиск по жанру");
                    Console.Write("Выберите тип поиска: ");

                    string searchType = Console.ReadLine();
                    List<Book> results = new List<Book>();

                    switch (searchType)
                    {
                        case "1":
                            Console.Write("Введите название для поиска: ");
                            string title = Console.ReadLine();
                            results = library.FindByTitle(title);
                            break;
                        case "2":
                            Console.Write("Введите автора для поиска: ");
                            string author = Console.ReadLine();
                            results = library.FindByAuthor(author);
                            break;
                        case "3":
                            Console.WriteLine("\nДоступные жанры:");
                            foreach (Genre genre in Enum.GetValues(typeof(Genre)))
                            {
                                Console.WriteLine($"  {(int)genre} - {genre}");
                            }
                            Console.Write("Выберите жанр для поиска: ");
                            if (Enum.TryParse(Console.ReadLine(), out Genre genreSearch) && Enum.IsDefined(typeof(Genre), genreSearch))
                            {
                                results = library.FindByGenre(genreSearch);
                            }
                            else
                            {
                                Console.WriteLine("Ошибка: Неверный выбор жанра.");
                                return;
                            }
                            break;
                        default:
                            Console.WriteLine("Неверный выбор.");
                            return;
                    }

                    if (results.Count > 0)
                    {
                        Console.WriteLine($"\n=== НАЙДЕНО КНИГ: {results.Count} ===");
                        foreach (var book in results)
                        {
                            Console.WriteLine(book);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Книги по заданным критериям не найдены.");
                    }
                }

                private void SortBooks()
                {
                    Console.WriteLine("\n=== СОРТИРОВКА КНИГ ===");
                    Console.WriteLine("1 - Сортировка по названию");
                    Console.WriteLine("2 - Сортировка по году издания");
                    Console.Write("Выберите тип сортировки: ");

                    string sortType = Console.ReadLine();
                    List<Book> sortedBooks = new List<Book>();

                    switch (sortType)
                    {
                        case "1":
                            sortedBooks = library.SortByTitle();
                            Console.WriteLine("\n=== КНИГИ ОТСОРТИРОВАНЫ ПО НАЗВАНИЮ ===");
                            break;
                        case "2":
                            sortedBooks = library.SortByYear();
                            Console.WriteLine("\n=== КНИГИ ОТСОРТИРОВАНЫ ПО ГОДУ ИЗДАНИЯ ===");
                            break;
                        default:
                            Console.WriteLine("Неверный выбор.");
                            return;
                    }

                    foreach (var book in sortedBooks)
                    {
                        Console.WriteLine(book);
                    }
                }

                private void ShowPriceExtremes()
                {
                    Console.WriteLine("\n=== САМАЯ ДОРОГАЯ И САМАЯ ДЕШЕВАЯ КНИГИ ===");

                    var mostExpensive = library.GetMostExpensiveBook();
                    var cheapest = library.GetCheapestBook();

                    if (mostExpensive != null)
                    {
                        Console.WriteLine($"Самая дорогая книга:");
                        Console.WriteLine(mostExpensive);
                    }

                    if (cheapest != null)
                    {
                        Console.WriteLine($"\nСамая дешевая книга:");
                        Console.WriteLine(cheapest);
                    }

                    if (mostExpensive == null && cheapest == null)
                    {
                        Console.WriteLine("В библиотеке нет книг.");
                    }
                }

                private void ShowAuthorStatistics()
                {
                    Console.WriteLine("\n=== СТАТИСТИКА ПО АВТОРАМ ===");

                    var authorStats = library.GetBooksByAuthor();

                    if (authorStats.Count > 0)
                    {
                        foreach (var stat in authorStats.OrderByDescending(s => s.Value))
                        {
                            Console.WriteLine($"Автор: {stat.Key}, Количество книг: {stat.Value}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("В библиотеке нет книг.");
                    }
                }

                static void Main(string[] args)
                {
                    LibraryUI ui = new LibraryUI();
                    ui.Run();
                }
            }
        }
    }
}

