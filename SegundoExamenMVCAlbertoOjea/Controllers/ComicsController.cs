using Microsoft.AspNetCore.Mvc;
using SegundoExamenMVCAlbertoOjea.Models;
using SegundoExamenMVCAlbertoOjea.Repositories;

namespace SegundoExamenMVCAlbertoOjea.Controllers
{
    public class ComicsController : Controller
    {

        private IRepositoryComics repo;

        public ComicsController(IRepositoryComics repo)
        {
            this.repo = repo;
        }
        public IActionResult Index()
        {
            List<Comic> comics = this.repo.GetComics();
            return View(comics);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Comic comic)
        {
            this.repo.Insert(comic);
            return RedirectToAction("Index");
        }

        public IActionResult BuscarComic()
        {
            List<Comic> comics = this.repo.GetComics();
            ViewData["Comics"] = comics;

            return View();
        }

        [HttpPost]
        public IActionResult BuscarComic(int idcomic)
        {
            List<Comic> comics = this.repo.GetComics();
            ViewData["Comics"] = comics;

            Comic comic = this.repo.GetComicId(idcomic);
            return View(comic);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Comic comic = this.repo.GetComicId(id);
                if(comic == null)
                {
                    ViewData["MENSAJE"] = "No hay comic";
                }
                return View(comic);
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                this.repo.Delete(id.Value);
            }
            return RedirectToAction("Index");
        }

        public IActionResult CreateProcedure()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateProcedure(Comic comic)
        {
            this.repo.CreateComicProcedure(comic);
            return RedirectToAction("Index");
        }
    }
}
