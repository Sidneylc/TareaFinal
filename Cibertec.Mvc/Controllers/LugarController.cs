using Cibertec.Models;
using Cibertec.UnitOfWork;
using log4net;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Cibertec.Mvc.Controllers
{
    [RoutePrefix("Lugar")]
    public class LugarController : BaseController
    {
        public LugarController(ILog log, IUnitOfWork unit) : base(log, unit)
        {

        }

        public ActionResult Error()
        {
            throw new System.Exception("Prueba de Validación de Error - Action Filter");
        }

        public ActionResult Index()
        {
            _log.Info("Ejecución de Lugar Controller Ok");
            return View(_unit.Lugares.GetList());
        }

        public PartialViewResult Create()
        {
            return PartialView("_Create", new Lugar());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Lugar lugar)
        {
            if (ModelState.IsValid)
            {
                _unit.Lugares.Insert(lugar);
                return RedirectToAction("Index");
            }
            //return View(lugar);
            return PartialView("_Create", lugar);
        }

        public PartialViewResult Update(int id)
        {
            return PartialView("_Update", _unit.Lugares.GetById(id));
        }

        [HttpPost]
        public ActionResult Update(Lugar lugar)
        {
            var val = _unit.Lugares.Update(lugar);

            if (val)
            {
                return RedirectToAction("Index");
            }
            return PartialView("_Update", lugar);
        }

        public PartialViewResult Delete(int id)
        {
            return PartialView("_Delete", _unit.Lugares.GetById(id));
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeletePost(int id)
        {
            var val = _unit.Lugares.Delete(id);

            if (val) return RedirectToAction("Index");
            return PartialView("_Delete", _unit.Lugares.GetById(id));
        }

        [Route("List/{page:int}/{rows:int}")]
        public PartialViewResult List(int page, int rows)
        {
            if (page <= 0 || rows <= 0) return PartialView(new List<Lugar>());
            var startRecord = ((page - 1) * rows) + 1;
            var endRecord = page * rows;

            /*
             * Llamando a un WEB API
             */
            return PartialView("_List", _unit.Lugares.PagedList(startRecord, endRecord));
        }
        [Route("Count/{rows:int}")]
        public int Count(int rows)
        {
            var totalRecords = _unit.Lugares.Count();
            return totalRecords % rows != 0 ? (totalRecords / rows) + 1 : totalRecords / rows;
        }
    }
}