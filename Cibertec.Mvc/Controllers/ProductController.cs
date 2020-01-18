using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Cibertec.UnitOfWork;
using Cibertec.Models;
using log4net;

namespace Cibertec.Mvc.Controllers
{
    //[ErrorActionFilter]
    [RoutePrefix("Product")]
    public class ProductController : BaseController
    {
        public ProductController(ILog log, IUnitOfWork unit) : base(log, unit)
        {

        }

        public ActionResult Error()
        {
            throw new System.Exception("Prueba de Validación de Error - Action Filter");
        }

        public ActionResult Index()
        {
            _log.Info("Ejecución de Product Controller Ok");
            return View(_unit.Products.GetList());
        }

        public PartialViewResult Create()
        {
            return PartialView("_Create", new Products());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Products product)
        {
            if (ModelState.IsValid)
            {
                _unit.Products.Insert(product);
                return RedirectToAction("Index");
            }
            //return View(product);
            return PartialView("_Create", product);
        }

        //public ActionResult Update(string id)
        public PartialViewResult Update(int id)
        {
            //return View(_unit.Products.GetById(id));
            return PartialView("_Update", _unit.Products.GetById(id));
        }

        [HttpPost]
        public ActionResult Update(Products product)
        {
            var val = _unit.Products.Update(product);

            if (val)
            {
                return RedirectToAction("Index");
            }
            //return View(product);
            return PartialView("_Update", product);
        }

        //public ActionResult Delete(String id)
        public PartialViewResult Delete(int id)
        {
            //return View(_unit.Products.GetById(id));
            return PartialView("_Delete", _unit.Products.GetById(id));
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeletePost(int id)
        {
            var val = _unit.Products.Delete(id);

            if (val) return RedirectToAction("Index");
            //return View();
            return PartialView("_Delete", _unit.Products.GetById(id));
        }

        [Route("List/{page:int}/{rows:int}")]
        public PartialViewResult List(int page, int rows)
        {
            if (page <= 0 || rows <= 0) return PartialView(new List<Products>());
            var startRecord = ((page - 1) * rows) + 1;
            var endRecord = page * rows;

            /*
             * Llamando a un WEB API
             */
            return PartialView("_List", _unit.Products.PagedList(startRecord, endRecord));
        }
        [Route("Count/{rows:int}")]
        public int Count(int rows)
        {
            var totalRecords = _unit.Products.Count();
            return totalRecords % rows != 0 ? (totalRecords / rows) + 1 : totalRecords / rows;
        }
    }
}