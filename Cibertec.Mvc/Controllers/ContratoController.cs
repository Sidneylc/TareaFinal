using Cibertec.Models;
using Cibertec.UnitOfWork;
using log4net;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Cibertec.Mvc.Controllers
{
    [RoutePrefix("Contrato")]
    public class ContratoController : BaseController
    {
        public ContratoController(ILog log, IUnitOfWork unit) : base(log, unit)
        {

        }

        public ActionResult Error()
        {
            throw new System.Exception("Prueba de Validación de Error - Action Filter");
        }

        public ActionResult Index()
        {
            _log.Info("Ejecución de Contrato Controller Ok");
            return View(_unit.Contratos.GetList());
        }

        public PartialViewResult Create()
        {
            return PartialView("_Create", new Contrato());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato contrato)
        {
            if (ModelState.IsValid)
            {
                _unit.Contratos.Insert(contrato);
                return RedirectToAction("Index");
            }
            //return View(contrato);
            return PartialView("_Create", contrato);
        }

        public PartialViewResult Update(int id)
        {
            return PartialView("_Update", _unit.Contratos.GetById(id));
        }

        [HttpPost]
        public ActionResult Update(Contrato contrato)
        {
            var val = _unit.Contratos.Update(contrato);

            if (val)
            {
                return RedirectToAction("Index");
            }
            return PartialView("_Update", contrato);
        }

        public PartialViewResult Delete(int id)
        {
            return PartialView("_Delete", _unit.Contratos.GetById(id));
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeletePost(int id)
        {
            var val = _unit.Contratos.Delete(id);

            if (val) return RedirectToAction("Index");
            return PartialView("_Delete", _unit.Contratos.GetById(id));
        }

        [Route("List/{page:int}/{rows:int}")]
        public PartialViewResult List(int page, int rows)
        {
            if (page <= 0 || rows <= 0) return PartialView(new List<Contrato>());
            var startRecord = ((page - 1) * rows) + 1;
            var endRecord = page * rows;

            /*
             * Llamando a un WEB API
             */
            return PartialView("_List", _unit.Contratos.PagedList(startRecord, endRecord));
        }
        [Route("Count/{rows:int}")]
        public int Count(int rows)
        {
            var totalRecords = _unit.Contratos.Count();
            return totalRecords % rows != 0 ? (totalRecords / rows) + 1 : totalRecords / rows;
        }
    }
}