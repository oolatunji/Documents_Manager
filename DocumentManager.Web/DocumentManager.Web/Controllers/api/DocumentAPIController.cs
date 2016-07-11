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
    public class DocumentAPIController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage SaveDocument([FromBody]DocumentModel model)
        {
            try
            {
                string errMsg = string.Empty;
                bool result = DocumentPL.Save(model, out errMsg);
                if (string.IsNullOrEmpty(errMsg))
                    return result.Equals(true) ? Request.CreateResponse(HttpStatusCode.OK, "Document uploaded successfully.") : Request.CreateResponse(HttpStatusCode.BadRequest, "Request failed");
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

        [HttpPost]
        public HttpResponseMessage ViewDocument([FromBody]DocumentModel model)
        {
            try
            {
                var result = DocumentPL.ViewDocument(model);
                if (!string.IsNullOrEmpty(result))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                else
                {
                    var response = Request.CreateResponse(HttpStatusCode.BadRequest, "Request failed");
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

        [HttpGet]
        public HttpResponseMessage RetrieveDocument()
        {
            try
            {
                IEnumerable<Object> documents = DocumentPL.RetrieveDocuments();
                object returnedDocuments = new { data = documents };
                return Request.CreateResponse(HttpStatusCode.OK, returnedDocuments);
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
