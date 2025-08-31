using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace DAL004
{
    public interface IRepository:IDisposable
    {
        string BasePath { get; }   //полный дирректорий для JSON и фотографий 
        Celebrity[] GetAllCelebrities();//получить весь список знаменитостей 
        Celebrity? GetCelebrityById(int id); //получить по айди 
        Celebrity[] GetCelebritiesBySurename(string surename);
        string? GetPhotoPathById(int id); //получить путь для GET-запроса к фотографии 
        int? addCelebrity(Celebrity celeb); //добавить знаменитость =Id новой знаменитости 

        bool delCelebrity(int id);//удалить   знаменитость по Id, = true-успех
        int? updCelebrityById(int id , Celebrity celeb);//изменить    знаменитость по Id, =Id -   новый , Id-успех 
        int saveChanges(); //сохранить изменения в JSON, =колисечство изменений 
    }

    public record Celebrity(int Id, string Firstname, string Surname, string PhotoPath);
  
    public class Repository:IRepository
    {
        private int _lastId; // Поле для хранения последнего используемого Id, используется для генерации нового Id.
        public static string JSONFileName = "Celebrities.json";
        public string BasePath { get; }// Свойство, содержащее базовый путь к директории с данными
        public string filePath { get; }// Полный путь к JSON-файлу (с объединением базового пути и имени файла)
        public List<Celebrity> _celebrities;

        public Repository(string dirPath)//констурктор
        {
            this.BasePath = Path.Combine(Directory.GetCurrentDirectory(), dirPath);//Объединяет текущий рабочий каталог с переданным путем для формирования базового пути
            this.filePath = Path.Combine(BasePath, JSONFileName);// Формирует полный путь к JSON-файлу внутри базовой директори
            if (!Directory.Exists(this.BasePath))
            {
                Directory.CreateDirectory(this.BasePath);
            }
            if (!File.Exists(this.filePath))//не существует
            {
                File.WriteAllText(this.filePath, "[]");
            }
            LoadData();
        }

        private void LoadData()
        {
            var json = File.ReadAllText(filePath);
            _celebrities = JsonSerializer.Deserialize<List<Celebrity>>(json) ?? new List<Celebrity>();// Если десериализация возвращает null, создаём новый пустой список
            var tempCelebr = _celebrities.ToArray();
            _lastId = tempCelebr.Length;
            Console.WriteLine(_lastId);
        }

        public Celebrity[] GetAllCelebrities()
        {
            return _celebrities.ToArray();
        }

        public Celebrity? GetCelebrityById(int id)
        {
            return _celebrities.FirstOrDefault(c => c.Id == id);
        }

        public Celebrity[] GetCelebritiesBySurename(string surename)
        {
            return _celebrities.Where(c => c.Surname.Equals(surename, StringComparison.OrdinalIgnoreCase)).ToArray();
        }

        public string? GetPhotoPathById(int id)
        {
            return GetCelebrityById(id)?.PhotoPath;
        }

        public static Repository Create(string dir)// Статический метод для создания нового экземпляра Repository с заданной директорией.
        {
            return new Repository(dir);
        }
        public int? addCelebrity(Celebrity celeb)
        {// Проверяем, существует ли уже знаменитость с таким же Id или переданный Id равен 0
            if (this._celebrities.Find(c => c.Id == celeb.Id) != null||celeb.Id==0)
            {// Если условие истинно, генерируем новый уникальный Id (инкрементируя _lastId)
             // и создаём новую запись Celebrity с этим Id и остальными параметрами из входного объекта
                celeb = new Celebrity(++_lastId,celeb.Firstname,celeb.Surname,celeb.PhotoPath);
                this._celebrities.Add(celeb);
                return celeb.Id;
            }
            else
            {// Если знаменитость с таким Id не найдена, добавляем переданный объект как есть
                this._celebrities.Add(celeb);
                return celeb.Id;
            }
            
        }

        public bool delCelebrity(int id)
        {
            if(this._celebrities.Find(c=>c.Id == id) != null)
            { // Если найдена — получаем индекс этой знаменитости и удаляем её по индексу
                this._celebrities.RemoveAt(this._celebrities.FindIndex(c => c.Id == id));
                return true;
            }
            else
            {
                return false;
            }
        }

        public int? updCelebrityById(int id, Celebrity celeb)
        {// Ищем существующую знаменитость по заданному Id
            var existingCelebrity = _celebrities.FirstOrDefault(c => c.Id == id);
            if (existingCelebrity != null)
            {
                // Создаем новый объект Celebrity с обновленными значениями

                // Если найдено, создаем новый объект Celebrity с обновленными значениями,
                // используя конструкцию with, которая копирует все свойства, кроме указанных
                var updatedCelebrity = existingCelebrity with
                {
                    Firstname = celeb.Firstname,
                    Surname = celeb.Surname,
                    PhotoPath = celeb.PhotoPath
                };

                // Заменяем старую запись на новую
                _celebrities[_celebrities.FindIndex(c => c.Id == id)] = updatedCelebrity;
                return updatedCelebrity.Id;
            }
            return -1; // Знаменитость не найдена
        }

        public int saveChanges()
        {// Читаем содержимое файла по пути filePath и получаем его длину (количество символов до обновления)
            int beforeUpdLength = File.ReadAllText(this.filePath).Length;
            var updatedJsonString = JsonSerializer.Serialize(this._celebrities);   // Сериализуем список знаменитостей _celebrities в JSON-строку
            File.WriteAllText(this.filePath, updatedJsonString);  // Перезаписываем файл новым содержимым (обновлённым JSON)
            int afterUpdLength = File.ReadAllText(this.filePath).Length;  // Читаем файл повторно и получаем длину содержимого после обновления

            return afterUpdLength - beforeUpdLength;  // Возвращаем разницу между длинами содержимого файла до и после обновления
        }
       

        public void Dispose()
        {

        }
    }
}
