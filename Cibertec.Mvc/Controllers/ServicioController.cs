using Cibertec.Models;
using Cibertec.UnitOfWork;
using log4net;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cibertec.Mvc.Controllers
{
    [RoutePrefix("Servicio")]
    public class ServicioController : BaseController
    {
        public ServicioController(ILog log, IUnitOfWork unit) : base(log, unit)
        {

        }

        public ActionResult Error()
        {
            throw new System.Exception("Prueba de Validación de Error - Action Filter");
        }

        public ActionResult Index()
        {
            _log.Info("Ejecución de Servicio Controller Ok");
            return View(_unit.Servicios.GetList());
        }

        public PartialViewResult Create()
        {
            return PartialView("_Create", new Servicio());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Servicio servicio)
        {
            if (ModelState.IsValid)
            {
                _unit.Servicios.Insert(servicio);
                return RedirectToAction("Index");
            }
            //return View(servicio);
            return PartialView("_Create", servicio);
        }

        public PartialViewResult Update(int id)
        {
            return PartialView("_Update", _unit.Servicios.GetById(id));
        }

        [HttpPost]
        public ActionResult Update(Servicio servicio)
        {
            var val = _unit.Servicios.Update(servicio);

            if (val)
            {
                return RedirectToAction("Index");
            }
            return PartialView("_Update", servicio);
        }

        public PartialViewResult Delete(int id)
        {
            return PartialView("_Delete", _unit.Servicios.GetById(id));
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeletePost(int id)
        {
            var val = _unit.Servicios.Delete(id);

            if (val) return RedirectToAction("Index");
            return PartialView("_Delete", _unit.Servicios.GetById(id));
        }

        [Route("List/{page:int}/{rows:int}")]
        public async Task<PartialViewResult> List(int page, int rows)
        {
            var httpClient = new HttpClient();
            var credential = new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "username", "sidneylc28@gmail.com" },
                { "password", "123456" }
            };

            var token = await httpClient.PostAsync("http://localhost:55724/token",
                new FormUrlEncodedContent(credential));
            var tokenContent = token.Content.ReadAsStringAsync().Result;
            var tokenDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(tokenContent);

            //Paso 2: Consumir Servicio
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                tokenDictionary["access_token"]);
            var json = await httpClient.GetStringAsync("http://localhost:55724/Servicio/list/" + page + "/" + rows);

            List<Customers> lstCustomers = JsonConvert.DeserializeObject<List<Customers>>(json);
            return PartialView("_List", lstCustomers);
        }

        [Route("Count/{rows:int}")]
        public int Count(int rows)
        {
            var totalRecords = _unit.Servicios.Count();
            return totalRecords % rows != 0 ? (totalRecords / rows) + 1 : totalRecords / rows;
        }
    }
}