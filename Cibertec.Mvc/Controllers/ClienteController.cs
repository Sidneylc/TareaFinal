using Cibertec.Models;
using Cibertec.UnitOfWork;
using log4net;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Cibertec.Mvc.Controllers
{
    [RoutePrefix("Cliente")]
    public class ClientController : BaseController
    {
        public ClientController(ILog log, IUnitOfWork unit) : base(log, unit)
        {

        }

        public ActionResult Error()
        {
            throw new System.Exception("Prueba de Validación de Error - Action Filter");
        }

        public ActionResult Index()
        {
            _log.Info("Ejecución de Cliente Controller Ok");
            return View(_unit.Clientes.GetList());
        }

        public PartialViewResult Create()
        {
            return PartialView("_Create", new Cliente());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _unit.Clientes.Insert(cliente);
                return RedirectToAction("Index");
            }
            //return View(cliente);
            return PartialView("_Create", cliente);
        }

        public PartialViewResult Update(int id)
        {
            return PartialView("_Update", _unit.Clientes.GetById(id));
        }

        [HttpPost]
        public ActionResult Update(Cliente cliente)
        {
            var val = _unit.Clientes.Update(cliente);

            if (val)
            {
                return RedirectToAction("Index");
            }
            return PartialView("_Update", cliente);
        }

        public PartialViewResult Delete(int id)
        {
            return PartialView("_Delete", _unit.Clientes.GetById(id));
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeletePost(int id)
        {
            var val = _unit.Clientes.Delete(id);

            if (val) return RedirectToAction("Index");
            return PartialView("_Delete", _unit.Clientes.GetById(id));
        }

        [Route("List/{page:int}/{rows:int}")]
        public PartialViewResult List(int page, int rows)
        {
            if (page <= 0 || rows <= 0) return PartialView(new List<Cliente>());
            var startRecord = ((page - 1) * rows) + 1;
            var endRecord = page * rows;

            /*
             * Llamando a un WEB API
             */
            return PartialView("_List", _unit.Clientes.PagedList(startRecord, endRecord));
        }
        [Route("Count/{rows:int}")]
        public int Count(int rows)
        {
            var totalRecords = _unit.Clientes.Count();
            return totalRecords % rows != 0 ? (totalRecords / rows) + 1 : totalRecords / rows;
        }
    }
}