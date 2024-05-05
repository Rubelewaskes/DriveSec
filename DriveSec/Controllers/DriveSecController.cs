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

        private readonly string _key = "y0_AgAAAABx6uBRAAudwwAAAAEB1aPlAABsDgxMSDBMOrHUa6QLba4nZneYag";
        private static readonly string _pathstandart = "disk:/DriveSec";
        private static readonly int _userid = 1; // пока тут 1, позже будет обычный userid
        private static readonly string _path;
        static DriveSecController()
        {
            _path = _pathstandart + "/" + _userid;
        }

        public IActionResult Index()
        {
            try
            {
                // Получение данных о пользователях
                var usersData = _context.Users.Select(c => new ChosenViewModel
                {
                    login = c.Login,
                    password = c.Password
                }).ToList();


                if (usersData != null)
                {
                    /*ViewData["SuccessMessage"] = "Успешно получены данные о пользователях из базы данных!";*/
                    ViewData["SuccessMessage"] = "Все гуд!";
                    ViewData["UsersData"] = usersData;
                }
                else
                {
                    /*ViewData["ErrorMessage"] = "Ошибка при получении данных о пользователях из базы данных!";*/
                    ViewData["ErrorMessage"] = "Все не гуд!";
                }

                // Получение списка файлов в папке с ID = 1
                var filesInFolder = _context.Files
                    .Where(file => file.FolderId == 1)//_userid)
                    .ToList();

                // Получение списка папок в папке с ID = 1
                var foldersInFolder = _context.Folders.Where(d => d.FolderWay == _path)
                    .ToList();

                return View((filesInFolder, foldersInFolder));
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Ошибка: {ex.Message}";
                return View();
            }
        }


        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFile(IFormFile file, string description, string selectedUsers)
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
                UploadFileDB(file.FileName, false, description, 1);

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
                UploadFolderDB(folderName, "", _path);

                return RedirectToAction("Index", new { successMessage = $"Папка '{folderName}' успешно создана на Яндекс Диске" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Возникла ошибка при создании папки: {ex.Message}");
            }
        }

        protected void UploadFolderDB(string folderName, string folderDescription, string folderWay)
        {
            var newFolder = new Models.Folder
            {
                FolderName = folderName,
                FolderWay = folderWay,
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
            Models.File file = _context.Files.Find(fileId);
            if (file != null)
            {
                _context.Files.Remove(file);
                _context.SaveChanges();
            }
            return;
            
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFolder(int folderId)
        {
            try
            {
                var api = new DiskHttpApi(_key);

                string folderName = _context.Folders
                    .Where(d => d.FolderId == folderId)
                    .Select(s => s.FolderName)
                    .FirstOrDefault();

                string resourcePath = _path + "/" + folderName + "/";

                Console.WriteLine("ТЕСТОВЫЕ ПАРАМЕТРЫ");
                Console.WriteLine(resourcePath);
                Console.WriteLine(folderName);
                Console.WriteLine(folderId);

                var encodedPath = Uri.EscapeDataString(resourcePath);
                var url = $"https://cloud-api.yandex.net/v1/disk/resources?path={encodedPath}";

                using (var httpClient = new HttpClient())
                { 
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"OAuth {_key}");

                    var response = await httpClient.DeleteAsync(url);

                    response.EnsureSuccessStatusCode();
                }

                DeleteFolderDB(folderId, folderName);

                return RedirectToAction("Index", new { successMessage = "Папка успешно удалена с Яндекс Диска" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Возникла ошибка при удалении файла: {ex.Message}");
            }
        }
        protected void DeleteFolderDB(int folderId, string folderName)
        {
            Models.Folder folder = _context.Folders.Find(folderId);
            string wayToFolder = _path + "/" + folderName;
            if (folder != null)
            {
                Models.Folder daughterFolder = _context.Folders.Where(d => d.FolderWay == wayToFolder).FirstOrDefault();
                while (daughterFolder != null)
                {
                    DeleteFolderDB(daughterFolder.FolderId, daughterFolder.FolderName);
                    daughterFolder = _context.Folders.Where(d => d.FolderWay == wayToFolder).FirstOrDefault();
                } 

                _context.Folders.Remove(folder);
                _context.SaveChanges();

            }
        }
            
    }
}
