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
        private int _lastId;// Поле для хранения последнего используемого Id, используется для генерации нового Id.
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
        {
            if (this._celebrities.Find(c => c.Id == celeb.Id) != null||celeb.Id==0)
            {
                celeb = new Celebrity(++_lastId,celeb.Firstname,celeb.Surname,celeb.PhotoPath);
                this._celebrities.Add(celeb);
                return celeb.Id;
            }
            else
            {
                this._celebrities.Add(celeb);
                return celeb.Id;
            }
            
        }

        public bool delCelebrity(int id)
        {
            if(this._celebrities.Find(c=>c.Id == id) != null)
            { 
                this._celebrities.RemoveAt(this._celebrities.FindIndex(c => c.Id == id));
                return true;
            }
            else
            {
                return false;
            }
        }

        public int? updCelebrityById(int id, Celebrity celeb)
        {
            var existingCelebrity = _celebrities.FirstOrDefault(c => c.Id == id);
            if (existingCelebrity != null)
            {
        
                var updatedCelebrity = existingCelebrity with
                {
                    Firstname = celeb.Firstname,
                    Surname = celeb.Surname,
                    PhotoPath = celeb.PhotoPath
                };

                
                _celebrities[_celebrities.FindIndex(c => c.Id == id)] = updatedCelebrity;
                return updatedCelebrity.Id;
            }
            return -1; 
        }

        public int saveChanges()
        {
            int beforeUpdLength = File.ReadAllText(this.filePath).Length;
            var updatedJsonString = JsonSerializer.Serialize(this._celebrities);   
            File.WriteAllText(this.filePath, updatedJsonString);  
            int afterUpdLength = File.ReadAllText(this.filePath).Length; 

            return afterUpdLength - beforeUpdLength;  
        }
       

        public void Dispose()
        {

        }
    }
}
