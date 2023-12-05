using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Topmovies.Models;

namespace Topmovies.Controllers
{
    public class HomeController : Controller
    {
        MovieContext _context;
        IWebHostEnvironment _appEnvironment;
        public HomeController(MovieContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            return _context.Movies != null ?
                          View(await _context.Movies.ToListAsync()) :
                          Problem("Entity set 'StudentContext.Students'  is null.");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Movie movie, IFormFile? uploadedFile)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (uploadedFile != null)
                    {
                        // Путь к папке Files
                        string path = "/Files/Pictures/" + uploadedFile.FileName; // имя файла

                        //видалення старого файлу
                        string oldPath = _appEnvironment.WebRootPath + movie.PicturePath;
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                        // Сохраняем файл в папку Files в каталоге wwwroot
                        // Для получения полного пути к каталогу wwwroot
                        // применяется свойство WebRootPath объекта IWebHostEnvironment
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await uploadedFile.CopyToAsync(fileStream); // копируем файл в поток
                        }
                        movie.PicturePath = path;
                    }

                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }
        private bool MovieExists(int id)
        {
            return (_context.Movies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> Add()
        {
            if (_context.Movies.Count() == 10)
            {
                return View("Overflow");
            }
            var movie = new Movie() { Title="Title", Genre="Genre", Description="Description", ReleaseDate= Convert.ToDateTime("10/10/2010 12:00"), PicturePath= "/Files/Pictures/noimg.png" };
            return View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int id, Movie movie, IFormFile? uploadedFile)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (uploadedFile != null)
                    {
                        // Путь к папке Files
                        string path = "/Files/Pictures/" + uploadedFile.FileName; // имя файла

                        string oldPath = _appEnvironment.WebRootPath + movie.PicturePath;

                        // Сохраняем файл в папку Files в каталоге wwwroot
                        // Для получения полного пути к каталогу wwwroot
                        // применяется свойство WebRootPath объекта IWebHostEnvironment
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await uploadedFile.CopyToAsync(fileStream); // копируем файл в поток
                        }
                        movie.PicturePath = path;
                    }

                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }
        public async Task<IActionResult> Remove(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            if (_context.Movies == null)
            {
                return Problem("Entity set 'MovieContext.Movies'  is null.");
            }
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                string oldPath = _appEnvironment.WebRootPath + movie.PicturePath;
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                _context.Movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
