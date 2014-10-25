using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExemploSimplesMongodb.Models;
using ExemploSimplesMongodb.Repositorio;

namespace ExemploSimplesMongodb.Controllers
{
    public class HomeController : Controller
    {

        private readonly Repositorio<Usuario> repositorio;
        // GET: Home
        public HomeController()
        {
            repositorio = new Repositorio<Usuario>();
        }
       
        public ActionResult Index(string q="")
        {
            if (string.IsNullOrEmpty(q))
            {
                return View(repositorio.GetAll().OrderBy(x => x.Nome));
            }
            return View(repositorio.GetByFilter(x => 
                x.Nome.ToLower().Contains(q.ToLower()) ||
                x.Email.ToLower().Contains(q.ToLower())).OrderBy(x => x.Nome));
        }

        public ActionResult Cadastrar()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Cadastrar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                repositorio.Save(usuario);
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Delete(string id)
        {
            repositorio.Remove(id);
            return RedirectToAction("Index");
        }

        public ActionResult Update(string id)
        {
            var usuario = repositorio.Get(id);
            return View(usuario);
        }
        [HttpPost]
        public ActionResult Update(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                repositorio.Save(usuario);
                return RedirectToAction("Index");
            }
            return View();
        }

    }
}
