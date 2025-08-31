using System.ComponentModel.DataAnnotations;
namespace ASPA008_1.Models
{// Класс модели представления (ViewModel) — используется для формы создания/редактирования знаменитости.
    public class CelebrityViewModel
    {// Атрибут [Required] означает, что поле обязательно для заполнения.
        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Nationality is required")]
        public string Nationality { get; set; }

        [Required(ErrorMessage = "Photo is required")]
        public IFormFile Upload { get; set; }

        public bool IsCorrect { get; set; } = false;
    }
}
