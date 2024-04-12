using DriveSec.Models;
using Microsoft.AspNetCore.Mvc;

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
	}
}
