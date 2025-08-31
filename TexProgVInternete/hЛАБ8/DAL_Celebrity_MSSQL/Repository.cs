using Microsoft.EntityFrameworkCore;
using Exceptions;
namespace DAL_Celebrity_MSSQL
{
    public interface IRepository : DAL_Celebrity.IRepository<Celebrity, LifeEvent> { }
    /*Этот интерфейс наследует интерфейс IRepository<T1, T2> из пространства имён DAL_Celebrity, подставляя в него конкретные типы:*/
    public class Repository : IRepository
    {
        Context context;
        /*С большой вероятностью это класс, унаследованный от DbContext из библиотеки Entity Framework Core, 
         context — это основное средство доступа к базе данных внутри класса Repository*/

        public Repository() { this.context = new Context(); }//конструктор по умолчанию для класса Repository.

        public Repository(string connectionString) { this.context = new Context(connectionString); }
        /*перегруженный конструктор класса Repository, который принимает строку подключения к базе данных в качестве параметра.*/

        public static IRepository Create() { return new Repository(); }
        /* статический метод-фабрика, который:

не требует создания экземпляра класса Repository заранее;

создает и возвращает новый объект Repository, используя конструктор по умолчанию;

возвращает его как интерфейс IRepository.

*/

        public static IRepository Create(string connectionString) { return new Repository(connectionString); }
        /*Это перегруженный статический метод Create, который:

Принимает строку подключения к базе данных в параметре connectionString;

Создаёт экземпляр Repository, передавая эту строку в конструктор;

Возвращает его как интерфейс IRepository.

*/

        public List<Celebrity> GetAllCelebrities() { return this.context.Celebrities.ToList<Celebrity>(); }
        public Celebrity? GetCelebrityById(int id) { return this.context.Celebrities.FirstOrDefault(c => c.Id == id); }

        public bool AddCelebrity(Celebrity celebrity)// Метод добавляет нового знаменитость в базу данных. Принимает объект celebrity. Возвращает true при успехе.
        {
            if (this.context.Celebrities.Add(celebrity) is not null)
            {
                context.SaveChanges(); // Сохраняем изменения в базе данных. Без этого INSERT не произойдёт.
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public bool DeleteCelebrity(int id)
        {
            Celebrity? celebrity = this.context.Celebrities.FirstOrDefault(c => c.Id == id); // Ищем первую знаменитость с заданным ID в базе. Если не найдена — вернётся null.
            if (celebrity is not null)// Если такая знаменитость найдена...
            {
                this.context.Celebrities.Remove(celebrity);
                context.SaveChanges();
                return true;

            }
            else
            {
                return false;
            }
        }


        public bool UpdateCelebrity(int id, Celebrity celebrity)
        {
            Celebrity? celeb = this.context.Celebrities.FirstOrDefault(c => c.Id == id);// Ищем существующую знаменитость по ID
            if (celeb != null)// Если такая знаменитость найдена...
            {
                // Обновляем поля существующего объекта значениями из переданного объекта
                celeb.FullName = celebrity.FullName;
                celeb.Nationality = celebrity.Nationality;
                celeb.ReqPhotoPath = celebrity.ReqPhotoPath;

                // Обновляем запись в контексте
                this.context.Celebrities.Update(celeb);

                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }

        public List<LifeEvent> GetAllLifeEvents() { return this.context.LifeEvents.ToList<LifeEvent>(); }

        public LifeEvent? GetLifeEventById(int lifeEventId) { return this.context.LifeEvents.FirstOrDefault(l => l.Id == lifeEventId); }

        public bool AddLifeEvent(LifeEvent lifeEvent)// Метод добавляет новое событие в базу данных.
        {

            // Пытаемся добавить событие в контекст базы данных.
            // Метод Add() возвращает объект EntityEntry<LifeEvent>, он никогда не равен null.
            if (this.context.LifeEvents.Add(lifeEvent) is not null)
            {
                context.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteLifeEvent(int lifeEventId)
        {

            // Ищем событие по заданному ID в базе данных.
            // Если событие найдено — возвращает объект LifeEvent, иначе null.
            LifeEvent? lifeEvent = this.context.LifeEvents.FirstOrDefault(l => l.Id == lifeEventId);

            // Если событие найдено...
            if (lifeEvent != null)
            {
                // Удаляем событие из контекста (подготавливаем к удалению из базы)
                this.context.LifeEvents.Remove(lifeEvent);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateLifeEvent(int id, LifeEvent lifeEvent)
        {
            // Ищем существующее событие по ID
            LifeEvent? lifeEv = this.context.LifeEvents.FirstOrDefault(l => l.Id == id);
            if(lifeEv != null)
            { // Обновляем только нужные поля
                lifeEv = lifeEvent;

                this.context.LifeEvents.Update(lifeEv);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;   
            }
        }


        public List<LifeEvent> GetLifeEventsByCelebrityId(int celebrityId)
        {
            return this.context.LifeEvents.Where(l=>l.CelebrityId==celebrityId).ToList<LifeEvent>();
        }


        // Метод получает знаменитость по ID события из жизни.
        // Возвращает объект Celebrity, связанный с переданным LifeEvent, или null, если ничего не найдено.
        public Celebrity? GetCelebrityByLifeEventId(int lifeEventId)
        {
            // Ищем событие в таблице LifeEvents по переданному ID.
            // Если событие не найдено, будет возвращено null.
            LifeEvent? lifeEvent = this.context.LifeEvents.FirstOrDefault(l => l.Id == lifeEventId);
            if(lifeEvent != null)
            {
                // ...ищем знаменитость, у которой Id совпадает с CelebrityId, указанным в событии.
                // Возвращаем объект Celebrity или null, если не найдено.
                return this.context.Celebrities.FirstOrDefault(c => c.Id == lifeEvent.CelebrityId);
            }
            else
            {
                return null;
            }
        }
        // Метод ищет первую знаменитость, чьё полное имя содержит указанную подстроку.
        // Возвращает её Id, либо 0, если ничего не найдено.
        public int GetCelebrityByName(string name)
        {  // Выполняем поиск первой знаменитости, в имени которой содержится переданная строка `name`.
           // Используется метод Contains — регистрозависимый поиск (если не настроено иное в БД)
            Celebrity? celebrity = this.context.Celebrities.FirstOrDefault(c => c.FullName.Contains(name));
            if (celebrity != null) // Если такая знаменитость найдена — возвращаем её Id.
            {
                return celebrity.Id;
            }
            else
            {
                return 0;
            }

        }


        public void Dispose() { }

    }


}
