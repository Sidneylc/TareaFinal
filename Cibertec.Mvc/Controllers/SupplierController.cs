using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Cibertec.UnitOfWork;
using Cibertec.Models;
using log4net;

namespace Cibertec.Mvc.Controllers
{
    //[ErrorActionFilter]
    [RoutePrefix("Supplier")]
    public class SupplierController : BaseController
    {
        public SupplierController(ILog log, IUnitOfWork unit) : base(log, unit)
        {

        }

        public ActionResult Error()
        {
            throw new System.Exception("Prueba de Validación de Error - Action Filter");
        }

        public ActionResult Index()
        {
            _log.Info("Ejecución de Supplier Controller Ok");
            return View(_unit.Suppliers.GetList());
        }

        public PartialViewResult Create()
        {
            return PartialView("_Create", new Suppliers());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Suppliers supplier)
        {
            if (ModelState.IsValid)
            {
                _unit.Suppliers.Insert(supplier);
                return RedirectToAction("Index");
            }
            //return View(supplier);
            return PartialView("_Create", supplier);
        }

        //public ActionResult Update(string id)
        public PartialViewResult Update(string id)
        {
            //return View(_unit.Suppliers.GetById(id));
            return PartialView("_Update", _unit.Suppliers.GetById(id));
        }

        [HttpPost]
        public ActionResult Update(Suppliers supplier)
        {
            var val = _unit.Suppliers.Update(supplier);

            if (val)
            {
                return RedirectToAction("Index");
            }
            //return View(supplier);
            return PartialView("_Update", supplier);
        }

        //public ActionResult Delete(String id)
        public PartialViewResult Delete(String id)
        {
            //return View(_unit.Suppliers.GetById(id));
            return PartialView("_Delete", _unit.Suppliers.GetById(id));
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeletePost(String id)
        {
            var val = _unit.Suppliers.Delete(id);

            if (val) return RedirectToAction("Index");
            //return View();
            return PartialView("_Delete", _unit.Suppliers.GetById(id));
        }

        [Route("List/{page:int}/{rows:int}")]
        public PartialViewResult List(int page, int rows)
        {
            if (page <= 0 || rows <= 0) return PartialView(new List<Suppliers>());
            var startRecord = ((page - 1) * rows) + 1;
            var endRecord = page * rows;

            /*
             * Llamando a un WEB API
             */
            return PartialView("_List", _unit.Suppliers.PagedList(startRecord, endRecord));
        }
        [Route("Count/{rows:int}")]
        public int Count(int rows)
        {
            var totalRecords = _unit.Suppliers.Count();
            return totalRecords % rows != 0 ? (totalRecords / rows) + 1 : totalRecords / rows;
        }
    }
}