using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppBook.Base_Class;
using WebAppBook.Models;

namespace WebAppBook.Base_Class
{
    public enum CommandType { AddCommand , EditCommand }
    //Суперкласс для работы со сложными таблицами

    public abstract class TableOperationContext<T> where T : class
    {
        protected readonly CommandVoidContext contextVoid;
        protected AddWithFileCommand<T> FileController { get; set; }
        protected CommandType type { get; set; }

        public TableOperationContext(AddWithFileCommand<T> FileController, CommandType type)
        {
            this.FileController = FileController;
            this.type = type;

            contextVoid = CommandVoidContext.getContext();
        }

        abstract public void SetCommand(T book);

        protected ICommandVoid GetCommand(SearchTableFromContext<T> selectorInclude, List<ITableInclude<T>> listAdd, T obj)
        {
            if (type == CommandType.EditCommand)
            {
                return new EditManyToManyCommand<T>(
                    selectorInclude,
                    listAdd,
                    ReflectionEfId<T>.GetId(obj)
                );
            }
            return new AddManyToManyCommand<T>(selectorInclude, listAdd);
        }
    }
    //Класс для манипуляций с классом Book
    public sealed class BookOperation : TableOperationContext<Book>
    {
        public BookOperation(AddWithFileCommand<Book> bookController, CommandType type) : base(bookController, type)
        { }
        public sealed override void SetCommand(Book book)
        {
            FileController.Obj = book;
            contextVoid.SetVoidCommand(FileController);

            RelatedTableCommand<Book, Genre> relTableGenre = new RelatedTableCommand<Book, Genre>(
                bookType => bookType.Genres, 
                bookDb => bookDb.Genres, 
                ListCollection.genres
                );
            RelatedTableCommand<Book, Language> relTableLanguage = new RelatedTableCommand<Book, Language>(
                bookType => bookType.Languages, 
                bookDb => bookDb.Languages, 
                ListCollection.languages
                );
            RelatedTableCommand<Book, Translator> relTableTranslator = new RelatedTableCommand<Book, Translator>(
                bookType => bookType.Translators, 
                bookDb => bookDb.Translators, 
                ListCollection.translators
                );

            List<ITableInclude<Book>> listManyBook = new List<ITableInclude<Book>>() 
            { 
                relTableGenre, 
                relTableLanguage, 
                relTableTranslator 
            };

            SearchTableFromContext<Book> bookContext = s => s.Books
            .Include("Genres")
            .Include("Languages")
            .Include("Translators");

            contextVoid.SetVoidCommand(
                GetCommand(bookContext, listManyBook, book)
                );
        }
    }
    //Класс для манипуляций с классом Author
    public sealed class AuthorOperation : TableOperationContext<Author>
    {
        public AuthorOperation(AddWithFileCommand<Author> AuthorController, CommandType type) : base(AuthorController, type)
        { }

        public sealed override void SetCommand(Author author)
        {
            FileController.Obj = author;
            contextVoid.SetVoidCommand(FileController);

            RelatedTableCommand<Author, Genre> relTableGenre = new RelatedTableCommand<Author, Genre>(
                authorType => authorType.Genres, 
                bookDb => bookDb.Genres, ListCollection.genres
                );

            SearchTableFromContext<Author> AuthorContext = s => s.Authors.Include("Genres");
            List<ITableInclude<Author>> listManyAuthor = new List<ITableInclude<Author>> { relTableGenre };

            contextVoid.SetVoidCommand(
                GetCommand(AuthorContext,listManyAuthor,author)
                );
        }

    }
}