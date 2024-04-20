using DriveSec.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.IO;
using System.Threading.Tasks;
using YandexDisk.Client.Clients;
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
                // Получение данных о пользователях
                var usersData = _context.Users.Select(c => new ChosenViewModel
                {
                    login = _context.Users.Select(a => a.Login).FirstOrDefault(),
                    password = _context.Users.Select(a => a.Password).FirstOrDefault()
                }).ToList();

                if (usersData != null)
                {
                    ViewData["SuccessMessage"] = "Успешно получены данные о пользователях из базы данных!";
                    ViewData["UsersData"] = usersData;
                }
                else
                {
                    ViewData["ErrorMessage"] = "Ошибка при получении данных о пользователях из базы данных!";
                }

                // Получение списка файлов в папке с ID = 1
                var filesInFolder = _context.Files
                    .Where(file => file.FolderId == 1)
                    .ToList();

                //Получение списка папок (говно, надо иерархию настроить как-то)
                var foldersInFolder = _context.Folders
                    .ToList();

                return View((filesInFolder, foldersInFolder));
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Ошибка: {ex.Message}";
                return View();
            }
        }

        //private readonly string _key = "y0_AgAAAABx6uBRAAudwwAAAAEB1aPlAABsDgxMSDBMOrHUa6QLba4nZneYag"; //ключ Алексея
        private readonly string _key = "y0_AgAAAAAJYhNrAAulOwAAAAECXGhVAAB3SSF1c3lJU61vXwDtn389M9CHLw";// ключ Григория
        private static readonly string _pathstandart = "disk:/DriveSec";
        private static readonly int _userid = 1; // пока тут 1, позже будет обычный userid
        private static readonly string _path;
        static DriveSecController() {
            _path = _pathstandart + "/" + _userid;
        }


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

                // Получение ссылки для загрузки файла на Яндекс Диск
                var uploadLink = await api.Files.GetUploadLinkAsync(filePath, overwrite: true);
                
                // Загрузка файла на Яндекс Диск
                using (var stream = file.OpenReadStream())
                {
                    await api.Files.UploadAsync(uploadLink, stream);
                }

                //Выгрузка инфы в БД
                UploadFileDB(file.FileName, false, "", 1);

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
                VirusDescription = virusDescrition,
                UploaderId = 1, //Потом будем получать id авторизованного 
                FolderId = folderId
            };
            _context.Files.Add(newFile);
            _context.SaveChanges();
            return;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFolder(string folderName)
        {
            if (string.IsNullOrEmpty(folderName))
            {
                return BadRequest("Имя папки не может быть пустым");
            }

            try
            {
                var api = new DiskHttpApi(_key);

                // Подготовка пути на диске для создания папки
                var folderPath = _path + "/" + folderName;

                // Формирование URL запроса с закодированным путем к создаваемой папке
                var encodedPath = Uri.EscapeDataString(folderPath);
                var url = $"https://cloud-api.yandex.net/v1/disk/resources?path={encodedPath}";

                // Создание объекта HttpClient
                using (var httpClient = new HttpClient())
                {
                    // Установка заголовка авторизации
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"OAuth {_key}");

                    // Отправка запроса методом PUT
                    var response = await httpClient.PutAsync(url, null);

                    // Проверка статуса ответа
                    response.EnsureSuccessStatusCode();
                }

                //Выгрузка инфы в БД
                UploadFolderDB(folderName, "");

                return RedirectToAction("Index", new { successMessage = $"Папка '{folderName}' успешно создана на Яндекс Диске" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Возникла ошибка при создании папки: {ex.Message}");
            }
        }

        protected void UploadFolderDB(string folderName, string folderDescription)
        {
            var newFolder = new Models.Folder
            {
                FolderName = folderName,
                FolderDescription = folderDescription,
                CreationDate = DateTime.Now
            };
            _context.Folders.Add(newFolder);
            _context.SaveChanges();
            return;
        }


        [HttpPost]
        public async Task<IActionResult> DownloadFile(int fileId)
        {
            try
            {
                var api = new DiskHttpApi(_key);

                // загрузка файла на компьютер

                string filename = _context.Files
                    .Where(d => d.FileId == fileId)
                    .Select(s => s.FileName)
                    .FirstOrDefault();

                // Скачиваем файл и сохраняем его по указанному пути
                await api.Files.DownloadFileAsync(_path + "/" + filename, filename);
                return RedirectToAction("Index", new { successMessage = "Файл успешно загружен на Ваш компьютер" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Возникла ошибка при загрузке файла: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFile(int fileId)
        {
            try
            {
                var api = new DiskHttpApi(_key);

                // Получение имени файла по его идентификатору
                string filename = _context.Files
                    .Where(d => d.FileId == fileId)
                    .Select(s => s.FileName)
                    .FirstOrDefault();


                // Подготовка пути к удаляемому ресурсу на Яндекс.Диске
                string resourcePath = _path + "/" + filename;

                // Формирование URL запроса с закодированным путем к удаляемому ресурсу
                var encodedPath = Uri.EscapeDataString(resourcePath);
                var url = $"https://cloud-api.yandex.net/v1/disk/resources?path={encodedPath}";


                // Создание объекта HttpClient
                using (var httpClient = new HttpClient())
                {
                    // Установка заголовка авторизации
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"OAuth {_key}");

                    // Отправка запроса методом DELETE
                    var response = await httpClient.DeleteAsync(url);

                    // Проверка статуса ответа
                    response.EnsureSuccessStatusCode();
                }

                Models.File file = _context.Files.Find(fileId);
                if (file != null)
                {
                    _context.Files.Remove(file);
                    _context.SaveChanges(); // Сохранение изменений в базе данных

                }

                DeleteFileDB(fileId);

                return RedirectToAction("Index", new { successMessage = "Файл успешно удален с Яндекс Диска" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Возникла ошибка при удалении файла: {ex.Message}");
            }
        }

        protected void DeleteFileDB(int fileId)
        {
            var fileToDelete = _context.Files.FirstOrDefault(f => f.FileId == fileId);

            if (fileToDelete != null)
            {
                _context.Files.Remove(fileToDelete);
                _context.SaveChanges();
            }
            return;
            
        }

    }
}
