using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using ANC25_WEBAPI_DLL; // Для CelebritiesConfig
using DAL_Celebrity_MSSQL; // Для Celebrity
using System.Text.Encodings.Web; // Для HtmlEncoder

namespace ASPA008_1.Helpers
{
    public static class CelebrityFotoHelper
    {
        public static IHtmlContent CelebrityFoto(this IHtmlHelper htmlHelper, Celebrity celebrity, int width = 150, int height = 150)
        {
            if (string.IsNullOrEmpty(celebrity.ReqPhotoPath))
            {
                // Возвращаем заглушку или пустой div, если фото нет
                return new HtmlString($"<div style='display:inline-block; border:1px solid #ccc; text-align:center; line-height:{height}px;'>No Photo</div>");
            }

            // Получаем путь к фотографиям из конфигурации
            // Получаем IOptions<CelebritiesConfig> через IHtmlHelper.ViewContext.HttpContext.RequestServices
            var config = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<IOptions<CelebritiesConfig>>().Value;
            var photoRequestPath = config.PhotosRequestPath; // Это будет "/Photos"

            // Формируем URL для изображения, используя наш Minimal API эндпоинт
            var imageUrl = $"{photoRequestPath}/{celebrity.ReqPhotoPath}";

            // Создаем тег <img>
            var imgTag = new TagBuilder("img");
            imgTag.Attributes.Add("src", imageUrl);
            imgTag.Attributes.Add("alt", $"Фото {celebrity.FullName}");
            imgTag.Attributes.Add("width", width.ToString());
            imgTag.Attributes.Add("height", height.ToString());
            imgTag.AddCssClass("celebrity-photo"); // Опционально: добавить класс для стилизации

            // Создаем тег <a> для ссылки на страницу знаменитости
            var anchorTag = new TagBuilder("a");
            // Предполагаем, что URL для знаменитости выглядит как /celebrity/ID
            // Или используем маршрут "/{id:int:min(1)}" из Program.cs, который ведет на Celebrities/Human
            anchorTag.Attributes.Add("href", $"/{celebrity.Id}");
            anchorTag.AddCssClass("celebrity-link"); // Опционально: добавить класс для стилизации

            // Вставляем imgTag внутрь anchorTag
            anchorTag.InnerHtml.AppendHtml(imgTag);

            // Возвращаем результат
            using (var writer = new StringWriter())
            {
                anchorTag.WriteTo(writer, HtmlEncoder.Default);
                return new HtmlString(writer.ToString());
            }
        }
    }
}