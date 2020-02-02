using Cibertec.Models;
using Cibertec.UnitOfWork;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Cibertec.WebApi.Controllers
{
    [RoutePrefix("Servicio")]
    public class ServicioController : BaseController
    {
        public ServicioController(IUnitOfWork unit, ILog log) : base(unit,log)
        {
            _log.Info($"{typeof(ServicioController)} in execution");
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("error")]
        public IHttpActionResult CreateError()
        {
            throw new System.Exception("This is an unhandled error.");
        }

        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            return Ok(_unit.Servicios.GetById(id));
        }
        [Route("")]
        [HttpPost]
        public IHttpActionResult Post(Servicio servicio)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var id = _unit.Servicios.Insert(servicio);
            return Ok(new { id = id });
        }

        [Route("")]
        [HttpPut]
        public IHttpActionResult Put(Servicio servicio)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!_unit.Servicios.Update(servicio)) return BadRequest(ModelState);
            return Ok(new { status = true });
        }
        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            //try {
            var result = _unit.Servicios.Delete(id);
            //}
            //catch
            //{
            //}
            return Ok(new { delete = true });
        }
        [HttpGet]
        [Route("list")]
        public IHttpActionResult GetList()
        {
            return Ok(_unit.Servicios.GetList());
        }
        /*Servicio a ser consumido por el proyecto MVC:*/
        [HttpGet]
        [Route("list/{startRecord}/{endRecord}")]
        public IHttpActionResult GetListPagination(int startRecord, int endRecord)
        {
            return Ok(_unit.Servicios.PagedList(startRecord, endRecord));
        }
    }
}