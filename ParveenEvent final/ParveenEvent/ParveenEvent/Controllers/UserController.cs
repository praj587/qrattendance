using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ParveenEvent.Models;
using System.Collections.Specialized;
using System.Web;

namespace ParveenEvent.Controllers
{
    public class UserController : ApiController
    {
        private ParveenEntities db = new ParveenEntities();

        // GET: api/User
        //public IQueryable<tbl_Reg> Gettbl_Reg()
        //{
        //    return db.tbl_Reg;
        //}
        private ClientFilter GetFilter()
        {
            NameValueCollection filter = HttpUtility.ParseQueryString(Request.RequestUri.Query);

            return new ClientFilter
            {
                Name = filter["Cus_Name"],
                Id = (filter["Cus_Id"] !="")?Convert.ToInt16(filter["Cus_Id"]):0,
                Company = filter["Cus_Company"]
                
            };
        }
        [HttpGet]
        public IEnumerable<object> Get()
        {
            ClientFilter filter = GetFilter();

            var result = db.tbl_Reg.Where(c =>
                (String.IsNullOrEmpty(filter.Name) || c.Cus_Name.Contains(filter.Name)) ||
                ( c.Cus_Id.Equals(filter.Id)) ||
               (String.IsNullOrEmpty(filter.Company) || c.Cus_Company.Contains(filter.Company)) 
            );

            return result.ToArray();
        }
        public void Post([FromBody]tbl_Reg client)
        {
            db.tbl_Reg.Add(client);
            db.SaveChanges();
        }
        public void Put(int id, [FromBody]tbl_Reg editedClient)
        {
            tbl_Reg client = db.tbl_Reg.Find(id);

            if (client == null)
                return;

            client.Cus_Name = editedClient.Cus_Name;
            client.Cus_Mobile = editedClient.Cus_Mobile;
            client.Cus_Occupation = editedClient.Cus_Occupation;
            client.Cus_Email = editedClient.Cus_Email;
            client.Cus_Company = editedClient.Cus_Company;

            db.Entry(client).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
        public void Delete(int id)
        {
            tbl_Reg client = db.tbl_Reg.Find(id);

            if (client == null)
                return;

            db.tbl_Reg.Remove(client);
            db.SaveChanges();
        }




        //// GET: api/User/5
        //[ResponseType(typeof(tbl_Reg))]
        //public IHttpActionResult Gettbl_Reg(int id)
        //{
        //    tbl_Reg tbl_Reg = db.tbl_Reg.Find(id);
        //    if (tbl_Reg == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(tbl_Reg);
        //}

        //// PUT: api/User/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult Puttbl_Reg(int id, tbl_Reg tbl_Reg)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != tbl_Reg.Cus_key)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(tbl_Reg).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!tbl_RegExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/User
        //[ResponseType(typeof(tbl_Reg))]
        //public IHttpActionResult Posttbl_Reg(tbl_Reg tbl_Reg)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.tbl_Reg.Add(tbl_Reg);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = tbl_Reg.Cus_key }, tbl_Reg);
        //}

        //// DELETE: api/User/5
        //[ResponseType(typeof(tbl_Reg))]
        //public IHttpActionResult Deletetbl_Reg(int id)
        //{
        //    tbl_Reg tbl_Reg = db.tbl_Reg.Find(id);
        //    if (tbl_Reg == null)
        //    {
        //        return NotFound();
        //    }

        //    db.tbl_Reg.Remove(tbl_Reg);
        //    db.SaveChanges();

        //    return Ok(tbl_Reg);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool tbl_RegExists(int id)
        //{
        //    return db.tbl_Reg.Count(e => e.Cus_key == id) > 0;
        //}
    }
}