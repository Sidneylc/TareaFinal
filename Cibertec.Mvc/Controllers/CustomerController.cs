using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Cibertec.UnitOfWork;
using Cibertec.Models;
using log4net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using Newtonsoft.Json;

namespace Cibertec.Mvc.Controllers
{
    //[ErrorActionFilter]
    [RoutePrefix("Customer")]
    public class CustomerController : BaseController
    {
        /*Ya no es necesario porque se maneja en el padre BaseController*/
        //private readonly IUnitOfWork _unit;

        /*Ya no es necesario porque se utiliza Inyección de Dependencia*/
        /*
        public CustomerController()
        {
            _unit = new NorthwindUnitOfWork(
                ConfigurationManager.ConnectionStrings["NorthwindConnection"].ToString());
        }
        */
        public CustomerController(ILog log, IUnitOfWork unit): base(log,unit)
        {
            //_unit = unit;
        }

        //Simulación de Error
        public ActionResult Error()
        {
            throw new System.Exception("Prueba de Validación de Error - Action Filter");
        }

        // GET: Customer
        public ActionResult Index()
        {
            _log.Info("Ejecución de Customer Controller Ok");
            return View(_unit.Customers.GetList());
        }

        //CREATE: Customer
        //public ActionResult Create()
        public PartialViewResult Create()
        {
            //return View();
            return PartialView("_Create", new Customers());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customers customer)
        {
            if (ModelState.IsValid)
            {
                _unit.Customers.Insert(customer);
                return RedirectToAction("Index");
            }
            //return View(customer);
            return PartialView("_Create", customer);
        }

        //public ActionResult Update(string id)
        public PartialViewResult Update(string id)
        {
            //return View(_unit.Customers.GetById(id));
            return PartialView("_Update",_unit.Customers.GetById(id));
        }

        [HttpPost]
        public ActionResult Update(Customers customer)
        {
            var val = _unit.Customers.Update(customer);

            if (val)
            {
                return RedirectToAction("Index");
            }
            //return View(customer);
            return PartialView("_Update", customer);
        }

        //public ActionResult Delete(String id)
        public PartialViewResult Delete(String id)
        {
            //return View(_unit.Customers.GetById(id));
            return PartialView("_Delete",_unit.Customers.GetById(id));
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeletePost(String id)
        {
            var val = _unit.Customers.Delete(id);

            if (val) return RedirectToAction("Index");
            //return View();
            return PartialView("_Delete", _unit.Customers.GetById(id));
        }

        [Route("List/{page:int}/{rows:int}")]
        public PartialViewResult List(int page, int rows)
        {
            if (page <= 0 || rows <= 0) return PartialView(new List<Customers>());
            var startRecord = ((page - 1) * rows) + 1;
            var endRecord = page * rows;
            
            return PartialView("_List", _unit.Customers.PagedList(startRecord, endRecord));
        }

        [Route("List")]
        public PartialViewResult List()
        {
            /* Metodo que consume de la Web api */

            /* Solicitar Token */
            var cliente = new HttpClient();           
            string api = "http://localhost:55724";

            string header = "username=sidmneylc28@gmail.com" + "&" +
                            "password=123456" + "&" +
                            "grant_type=password";
            StringContent payload = new StringContent(header);
            var tokenResponse = cliente.PostAsync(api + "/token", payload).Result;
            var token = tokenResponse.Content.ReadAsAsync<Token>(
                new[] { new JsonMediaTypeFormatter() }).Result;
            // el token esta en token.AccessToken


            /* Consumir el servicio con el token */
            var cliente1 = new HttpClient();
            cliente1.BaseAddress = new Uri(api);
            cliente1.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("bearer", token.AccessToken);
            HttpResponseMessage response = cliente1.GetAsync("Customer/List").Result;
            var customer = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                return PartialView("_List", customer);
            }
            
        }

        [Route("Count/{rows:int}")]
        public int Count(int rows)
        {
            var totalRecords = _unit.Customers.Count();
            return totalRecords % rows != 0 ? (totalRecords / rows) + 1 : totalRecords / rows; 
        }
    }
}