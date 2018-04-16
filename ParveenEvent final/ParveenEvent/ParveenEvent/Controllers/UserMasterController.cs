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
using System.IO;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Mail;
using System.Text;
using System.Net.Http.Headers;

namespace ParveenEvent.Controllers
{
    public class UserMasterController : ApiController
    {
        private ParveenEntities db = new ParveenEntities();

       
        [HttpGet]
        [Route("api/UserMaster/GetUsercount")]
        public HttpResponseMessage GetUsercount()
        {
            var result = db.tbl_Reg.Count();
           
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpGet]
        [Route("api/UserMaster/GetGiftcount")]
        public HttpResponseMessage GetGiftcount()
        {
            var result = db.tbl_Reg.Where(x=>x.IsGift=="Y").Count();
          
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpGet]
        [Route("api/UserMaster/GetPresentcount")]
        public HttpResponseMessage GetPresentCount()
        {
            var result = db.tbl_Reg.Where(x => x.IsPresent == "Y").Count();
           
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpGet]
        [Route("api/UserMaster/GetAbsentcount")]
        public HttpResponseMessage GetAbsentcount()
        {
            var result = db.tbl_Reg.Where(x => x.IsPresent != "Y").Count();
           
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpGet]
        [Route("api/UserMaster/GetLog")]
        public HttpResponseMessage GetLog()
        {
            var result = db.Eventlogs.OrderByDescending(x => x.Eventtime).Take(10).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("api/UserMaster/GetUsers")]
        public HttpResponseMessage GetUsers()
        {
            var result =   db.tbl_Reg.ToList();
            // Filling the list with data here...

            // Then I return the list
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpGet]
        [Route("api/UserMaster/UpdateGift")]
        public HttpResponseMessage UpdateGift(int Id)
        {
            tbl_Reg result = db.tbl_Reg.Where(x => x.Cus_Id == Id).FirstOrDefault();
            if (result != null)
            {
                String message;
                tbl_Reg Checkgift = db.tbl_Reg.Where(x => x.Cus_Id == Id && x.IsGift == "Y").FirstOrDefault();
                if (Checkgift != null)
                {
                    message = "exists";
                }
                else
                {
                    result.IsGift = "Y";
                    db.Entry(result).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    message = result.Cus_Name + "(" + result.Cus_Id + ") received the gift";
                }
                return Request.CreateResponse(HttpStatusCode.OK, message);

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "error");
            }
            
          
        }
        [HttpGet]
        [Route("api/UserMaster/UpdateAttendance")]
        public HttpResponseMessage UpdateAttendance(int Id)
        {
            tbl_Reg result = db.tbl_Reg.Where(x => x.Cus_Id == Id).FirstOrDefault();
            if (result != null)
            {
                result.IsPresent = "Y";
                db.Entry(result).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                String message = result.Cus_Name + "(" + result.Cus_Id + ") checked in";
                return Request.CreateResponse(HttpStatusCode.OK, message);

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "error");
            }


        }
        [HttpGet]
        [Route("api/UserMaster/GetUsersQr")]
        public HttpResponseMessage GetUsersQr(int si,int ei)
        {
            dynamic startindex = db.tbl_Reg.Where(x => x.Cus_Id == si).FirstOrDefault();
            dynamic endindex = db.tbl_Reg.Where(x => x.Cus_Id == ei).FirstOrDefault();
            if (startindex == null || endindex == null || (si > ei))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "invalid criteria.please enter valid id");
            }
            else
            {
                var result = db.tbl_Reg.Where(x=>x.Cus_Id>=si && x.Cus_Id<=ei).ToList();
                // Filling the list with data here...

                // Then I return the list
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
        }
        [HttpGet]
        [Route("api/UserMaster/GetUser")]
        public HttpResponseMessage GetUser(int Id)
        {
            var result = db.tbl_Reg.Where(x=>x.Cus_Id==Id).ToList();
            // Filling the list with data here...

            // Then I return the list
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpPost]
        [Route("api/UserMaster/Adduser")]
        public HttpResponseMessage Adduser(tbl_Reg client)
        {
            dynamic maxObject = db.tbl_Reg.OrderByDescending(item => item.Cus_Id).First();
            client.Cus_Id = maxObject.Cus_Id+1;
            client.IsGift = "N";
            client.IsPresent = "N";
            db.tbl_Reg.Add(client);
            db.SaveChanges();
          
            return Request.CreateResponse(HttpStatusCode.OK, "Successfully registered");
        }
        //public void Post([FromBody]tbl_Reg client)
        //{
        //    db.tbl_Reg.Add(client);
        //    db.SaveChanges();
        //}
        [HttpPost]
        [Route("api/UserMaster/Edituser")]
        public HttpResponseMessage Edituser(tbl_Reg data)
        {
            tbl_Reg client = db.tbl_Reg.Where(x=>x.Cus_Id==data.Cus_Id).FirstOrDefault();

            if (client == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Data");
            else
            {
                client.Cus_Name = data.Cus_Name;
                client.Cus_Mobile = data.Cus_Mobile;
                client.Cus_Occupation = data.Cus_Occupation;
                client.Cus_Email = data.Cus_Email;
                client.Cus_Company = data.Cus_Company;

                db.Entry(client).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Successfully updated");
        }
        [HttpGet]
        [Route("api/UserMaster/GetQRcode")]
        public HttpResponseMessage GetQRcode(int Id)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            String Qrimput = Id.ToString();
            String Imagedata = "";
            using (MemoryStream ms = new MemoryStream())
            {

                string level = "Q";
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeGenerator.ECCLevel eccLevel = (QRCodeGenerator.ECCLevel)(level == "L" ? 0 : level == "M" ? 1 : level == "Q" ? 2 : 3);
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(Qrimput.ToString(), eccLevel))
                {
                    using (QRCode qrCode = new QRCode(qrCodeData))
                    {

                        using (Bitmap bitMap = qrCode.GetGraphic(20))
                        {
                            bitMap.Save(ms, ImageFormat.Png);
                            Imagedata = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                            //mailsend(ViewBag.QRCodeImage);
                        }
                    }
                }
                // QRCodeGenerator.QRCode qrCode = new QRCode(qrCodeData)

            }

            return Request.CreateResponse(HttpStatusCode.OK, Imagedata);
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