using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DAL_Celebrity_MSSQL;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using ASPA008_1.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using ANC25_WEBAPI_DLL.Services;
using ASPA008_1.Filters;
using ANC25_WEBAPI_DLL;

namespace ASPA008_1.Controllers
{ // Статический класс для хранения временного пути загруженного изображения
    public static class ImgPath {
        public static string path = "";
    }
    public class CelebritiesController : Controller
    {
        private readonly IRepository _repository;
        private readonly IWebHostEnvironment _env; // Окружение приложения (нужен для получения корневого пути)
        private readonly CelebritiesConfig _config;// Конфигурация, получаемая через IOptions
        private readonly string _customPhotosPath = @"E:\\TexProgVInternete\\ЛАБ8\\ASPA008_1\\Photos";

        
        public CelebritiesController(
            IRepository repository,
            IWebHostEnvironment env,
            IOptions<CelebritiesConfig> config)
        {
            _repository = repository;
            _env = env;// Сохраняем ссылку на окружение.
            _config = config.Value;
        }

        public class IndexModel
        {
            public string photosrequestpath { get; set; }
            public IEnumerable<Celebrity> celebrities { get; set; }
        }
        // Метод обрабатывает GET-запрос на главную страницу /Celebrities/Index.
        public IActionResult Index()
        {/**/
            var model = new IndexModel
            {
                photosrequestpath = _config.PhotosFolderRequestPath,  
                celebrities = _repository.GetAllCelebrities()
            };
            return View(model);
            
        }

        public IActionResult NewHumanForm(bool isCancel = false)
        {  // Если пользователь нажал "Отмена" на предыдущем шаге
            if (isCancel)
            { 
                if (!string.IsNullOrEmpty(ImgPath.path))
                {   // Формируем абсолютный путь к временному изображению
                    var tempFilePath = Path.Combine(_customPhotosPath, ImgPath.path);
                    if (System.IO.File.Exists(tempFilePath))   
                    {
                        System.IO.File.Delete(tempFilePath);
                    }
                    ImgPath.path = "";  
                }
                TempData.Clear();   
            }

            var countriesPath = Path.Combine(_env.ContentRootPath, _config.ISO3166alpha2Path); 
            var countryCodes = CountryCodes.LoadFromFile(countriesPath); 
            ViewData["Nationality"] = new SelectList(countryCodes, "Code", "Name"); // Передаём список стран во ViewData для использования в выпадающем списке

            return View(new CelebrityViewModel { IsCorrect = true });  
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessForm(CelebrityViewModel model, string handler)//Обрабатывает отправку формы создания/редактирования знаменитости.
        {
            if (handler == "render")// Проверяет валидность полей формы.
            {
                return HandleRender(model);
            }
            else if (handler == "save")
            {
                return await HandleSave(model);
            }
            else if (handler == "cancel")  
            {
                return HandleCancel();
            }

            
            return RedirectToAction("NewHumanForm");
        }

        private IActionResult HandleRender(CelebrityViewModel model)// Обрабатывает ввод пользователя и временно сохраняет загруженное изображение.


        {
            if (!ModelState.IsValid)
            {
                return View("NewHumanForm", model);
            }

            if (string.IsNullOrWhiteSpace(model.FullName) || string.IsNullOrWhiteSpace(model.Nationality))
            {
                ModelState.AddModelError("", "Пожалуйста, заполните все обязательные поля");
                return View("NewHumanForm", model);
            }

            if (model.Upload == null || model.Upload.Length == 0)
            {
                ModelState.AddModelError("Upload", "Пожалуйста, загрузите фотографию");
                return View("NewHumanForm", model);
            }

            if (model.Upload != null && model.Upload.Length > 0)// Если файл действительно загружен и имеет содержимое — переходим к сохранению.
            {
                ImgPath.path = model.Upload.FileName;//Сохраняем имя файла во временную переменную 
                var tempFileName = model.Upload.FileName;
                var tempFilePath = Path.Combine(_customPhotosPath, tempFileName);

                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    model.Upload.CopyTo(stream);
                }
                ViewData["TempPhotoPath"] = tempFileName;
                TempData["TempPhotoPath"] = tempFileName;
                TempData.Keep("TempPhotoPath");
            }
            ViewData["FullName"] = model.FullName;
            ViewData["Nationality"] = model.Nationality;
            TempData["FullName"] = model.FullName;
            TempData["Nationality"] = model.Nationality;
            TempData.Keep("FullName");
            TempData.Keep("Nationality");

            model.IsCorrect = false;
            return View("NewHumanForm", model); 
        }
        
        private async Task<IActionResult> HandleSave(CelebrityViewModel model)
        {
            try
            {
                /*Пытаемся получить имя временного изображения сначала из ViewData, потом из TempData.
🔹 ?? означает: если первое null, используй второе.*/
                var tempFileName = ViewData["TempPhotoPath"] as string ?? TempData["TempPhotoPath"] as string;
                TempData.Keep("TempPhotoPath");//Помечаем TempData["TempPhotoPath"] как актуальное, чтобы не удалилось после текущего запроса.

                if (string.IsNullOrEmpty(tempFileName))
                {
                    ModelState.AddModelError("", "Фото не было загружено");
                    return View("NewHumanForm", model);
                }

                var tempFilePath = Path.Combine(_customPhotosPath, tempFileName);
                var uniqueFileName = $"perm_{Guid.NewGuid()}{Path.GetExtension(tempFileName)}";
                var permFilePath = Path.Combine(_customPhotosPath, uniqueFileName);


                System.IO.File.Move(tempFilePath, permFilePath);

                using (var db = new Repository(_config.ConnectionString)) // Создаём экземпляр репозитория (с новым подключением к БД).
                {
                    var celebrity = new Celebrity
                    {
                        FullName = TempData["FullName"]?.ToString(),
                        Nationality = TempData["Nationality"]?.ToString(),
                        ReqPhotoPath = uniqueFileName //ReqPhotoPath сохраняет имя файла (без полного пути).
                    };

                    if (!db.AddCelebrity(celebrity))
                    {
                        if (System.IO.File.Exists(permFilePath))// Удаляем файл, если он был перемещён, но запись не добавлена в базу (чистим за собой).
                        {
                            System.IO.File.Delete(permFilePath);
                        }

                        ModelState.AddModelError("", "Не удалось сохранить данные в базу данных");
                        return View("NewHumanForm", model);
                    }
                }
                
                TempData.Remove("TempPhotoPath");
                TempData.Remove("FullName");
                TempData.Remove("Nationality");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ошибка: {ex.Message}");
                return View("NewHumanForm", model);
            }
        }

        [HttpPost, ActionName("HandleCancel")]
        private IActionResult HandleCancel()
        {
            if (!string.IsNullOrEmpty(ImgPath.path))
            {
                var tempFilePath = Path.Combine(_customPhotosPath, ImgPath.path);
                if (System.IO.File.Exists(tempFilePath))
                {
                    
                    System.IO.File.Delete(tempFilePath);
                }
            }
            ImgPath.path = "";// Очищаем временное имя файла (статическую переменную).
            TempData.Clear();

            return RedirectToAction("NewHumanForm");
        }
        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ///  Фильтр действия, который добавляет в HttpContext.Items данные из Википедии по имени знаменитости.////////////////////
        [InfoAsyncActionFilter(InfoAsyncActionFilter.Wikipedia)]
        public IActionResult Celebrities(int id)// Метод GET для отображения подробной карточки знаменитости по ID.
        {
            var celebrity = _repository.GetCelebrityById(id);
            if (celebrity == null)
            {
                return NotFound();
            }

            ViewBag.LifeEvents = _repository.GetLifeEventsByCelebrityId(id);

            if (HttpContext.Items.TryGetValue(InfoAsyncActionFilter.Wikipedia, out var wikiRefs))
            {
                ViewBag.WikipediaReferences = wikiRefs as Dictionary<string, string>;
                /* Если в HttpContext.Items найдены ссылки из Википедии (добавлены фильтром), сохраняем их в ViewBag.*/
            }
            //celebrity.ReqPhotoPath = Path.Combine(_customPhotosPath, celebrity.ReqPhotoPath);
            return View(celebrity);
        }
        public IActionResult Edit(int id)
        {
            var celebrity = _repository.GetCelebrityById(id);
            if (celebrity == null)
            {
                return NotFound();
            }

            var countriesPath = Path.Combine(_env.ContentRootPath, _config.ISO3166alpha2Path);
            var countryCodes = CountryCodes.LoadFromFile(countriesPath);
            ViewData["Nationality"] = new SelectList(countryCodes, "Code", "Name", celebrity.Nationality);
           

            return View(celebrity);//Отображаем форму редактирования, передаём в неё текущие данные знаменитости.
        }
        // Атрибут указывает, что метод обрабатывает POST-запрос (отправка формы редактирования).
        //роверяет защитный токен, чтобы предотвратить CSRF-атаки.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Celebrity celebrity, IFormFile newPhoto)
        {
            try
            {
                if (id != celebrity.Id)
                {
                    return NotFound();
                }

                var existingCelebrity = _repository.GetCelebrityById(id);
                if (existingCelebrity == null)
                {
                    return NotFound();
                }

                celebrity.ReqPhotoPath = existingCelebrity.ReqPhotoPath;//По умолчанию сохраняем старую фотографию (если пользователь не загрузит новую).

                if (newPhoto != null && newPhoto.Length > 0)// Проверяем, загрузил ли пользователь новую фотографию.
                {
                    if (!Directory.Exists(_customPhotosPath))// Убеждаемся, что директория для хранения фото существует. Если нет — создаём.
                    {
                        Directory.CreateDirectory(_customPhotosPath);
                    }

                    var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(newPhoto.FileName)}";
                    var filePath = Path.Combine(_customPhotosPath, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await newPhoto.CopyToAsync(stream);
                    }

                    if (!string.IsNullOrEmpty(existingCelebrity.ReqPhotoPath))//Если у знаменитости уже была фотография — удаляем старый файл с диска.
                    {
                        var oldFileName = Path.GetFileName(existingCelebrity.ReqPhotoPath);
                        var oldFilePath = Path.Combine(_customPhotosPath, oldFileName);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    celebrity.ReqPhotoPath = uniqueFileName;
                }

                if (!_repository.UpdateCelebrity(id, celebrity))
                {
                    throw new Exception("Failed to update celebrity");

                }

                return RedirectToAction(nameof(Celebrities), new { id });

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Произошла ошибка при обновлении данных");

                var originalCelebrity = _repository.GetCelebrityById(id);// Повторно загружаем старые данные, чтобы можно было отобразить их повторно в форме.
                return View(originalCelebrity ?? celebrity);
            }
        }

        public IActionResult Delete(int id)
        {
            var celebrity = _repository.GetCelebrityById(id);
            if (celebrity == null)
            {
                return NotFound();
            }

            return View(celebrity);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var celebrity = _repository.GetCelebrityById(id);
                if (celebrity == null)
                {
                    return NotFound();
                }
                    
                if (!string.IsNullOrEmpty(celebrity.ReqPhotoPath))
                {
                    var filePath = Path.Combine(_env.WebRootPath, celebrity.ReqPhotoPath.TrimStart('/'));//Формируем путь к файлу, начиная с wwwroot;
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);//Удаляем его с диска, если он существует.
                    }
                }

                if (!_repository.DeleteCelebrity(id))// Пытаемся удалить запись из базы через репозиторий.
                {
                    throw new Exception("Failed to delete celebrity");
                }

                return RedirectToAction(nameof(Index));
                /* После успешного удаления — перенаправляем пользователя на главную страницу со списком знаменитостей.*/
            }
            catch
            {
                return View("Delete", _repository.GetCelebrityById(id));
            }
        }

    }
}