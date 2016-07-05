using DocumentManagerLibrary;
using DocumentManagerLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DocumentManager.Web.Controllers.api
{
    public class PhysicalLocationAPIController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage SaveLocation([FromBody]PhysicalLocation location)
        {
            try
            {
                string errMsg = string.Empty;
                location.Date = System.DateTime.Now;
                bool result = PhysicalLocationPL.Save(location, out errMsg);
                if (string.IsNullOrEmpty(errMsg))
                    return result.Equals(true) ? Request.CreateResponse(HttpStatusCode.OK, "Location added successfully.") : Request.CreateResponse(HttpStatusCode.BadRequest, "Request failed");
                else
                {
                    var response = Request.CreateResponse(HttpStatusCode.BadRequest, errMsg);
                    return response;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                return response;
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateLocation([FromBody]PhysicalLocation location)
        {
            try
            {
                bool result = PhysicalLocationPL.Update(location);
                return result.Equals(true) ? Request.CreateResponse(HttpStatusCode.OK, "Location updated successfully") : Request.CreateResponse(HttpStatusCode.BadRequest, "Request failed");
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                return response;
            }
        }

        [HttpGet]
        public HttpResponseMessage RetrieveLocations()
        {
            try
            {
                IEnumerable<Object> locations = PhysicalLocationPL.RetrieveLocations();
                object returnedLocations = new { data = locations };
                return Request.CreateResponse(HttpStatusCode.OK, returnedLocations);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                var response = Request.CreateResponse(HttpStatusCode.BadRequest);
                response.ReasonPhrase = ex.Message;
                return response;
            }
        }
    }
}
