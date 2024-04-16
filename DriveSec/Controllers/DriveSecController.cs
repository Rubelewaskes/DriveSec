using DriveSec.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.IO;
using System.Threading.Tasks;
using YandexDisk.Client.Http;
using YandexDisk.Client.Protocol;

namespace DriveSec.Controllers
{
    public class DriveSecController : Controller
    {
        private readonly DriveSecContext _context;

        public DriveSecController(DriveSecContext context)
        {
            _context = context;
        }

        public class ChosenViewModel
        {
            public string login { get; set; }
            public string password { get; set; }
        }

        public IActionResult Index()
        {
            try
            {
                var data = _context.Users.Select(c => new ChosenViewModel
                {
                    login = _context.Users.Select(a => a.Login).FirstOrDefault(),
                    password = _context.Users.Select(a => a.Password).FirstOrDefault()
                }).ToList();

                if (data != null)
                {
                    ViewData["SuccessMessage"] = "Успешно получены данные из базы данных!";
                    return View(data);
                }
                else
                {
                    ViewData["ErrorMessage"] = "Ошибка при получении данных из базы данных!";
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Ошибка: {ex.Message}";
            }

            return View();
        }



        // private readonly string _key = "y0_AgAAAABx6uBRAAudwwAAAAEB1aPlAABsDgxMSDBMOrHUa6QLba4nZneYag"; //ключ Алексея
        private readonly string _key = "y0_AgAAAAAJYhNrAAuhwgAAAAECKacJAAACVMEM5LNGk7DiV7_CyNkQH3CywQ";// ключ Григория
        private readonly string _path = "disk:/DriveSec";


        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                return BadRequest("Не выбран файл для загрузки");
            }

            try
            {
                var api = new DiskHttpApi(_key);

                // Подготовка пути на диске для сохранения файла
                //var filePath = Path.Combine(_path, file.FileName);
                var filePath = (_path + "/" + file.FileName);

                //Выгрузка инфы в БД
                UploadFileDB(file.FileName, false, "", 1);

                // Получение ссылки для загрузки файла на Яндекс Диск
                var uploadLink = await api.Files.GetUploadLinkAsync(filePath, overwrite: true);

                // Загрузка файла на Яндекс Диск
                using (var stream = file.OpenReadStream())
                {
                    await api.Files.UploadAsync(uploadLink, stream);
                }

                return RedirectToAction("Index", new { successMessage = "Файл успешно загружен на Яндекс Диск" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Возникла ошибка при загрузке файла: {ex.Message}");
            }
        }

        protected void UploadFileDB(string fileName, bool virusAvailiability, string virusDescrition, int folderId)
        {
            var newFile = new Models.File
            {
                FileName = fileName,
                CreationDate = DateTime.Now,
                VirusAvailiability = virusAvailiability,
                VirusDescrition = virusDescrition,
                UploaderId = 1, //Потом будем получать id авторизованного 
                FolderId = folderId
            };
            _context.Files.Add(newFile);
            _context.SaveChanges();
            return;
        }
    }
}
