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
    public class CatalogueCriteriaAPIController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage SaveCriteria([FromBody]CatalogueCriteria criteria)
        {
            try
            {
                string errMsg = string.Empty;
                criteria.Date = System.DateTime.Now;
                bool result = CatalogueCriteriaPL.Save(criteria, out errMsg);
                if (string.IsNullOrEmpty(errMsg))
                    return result.Equals(true) ? Request.CreateResponse(HttpStatusCode.OK, "Criteria added successfully.") : Request.CreateResponse(HttpStatusCode.BadRequest, "Request failed");
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
        public HttpResponseMessage UpdateCriteria([FromBody]CatalogueCriteria criteria)
        {
            try
            {
                bool result = CatalogueCriteriaPL.Update(criteria);
                return result.Equals(true) ? Request.CreateResponse(HttpStatusCode.OK, "Criteria updated successfully") : Request.CreateResponse(HttpStatusCode.BadRequest, "Request failed");
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                return response;
            }
        }

        [HttpGet]
        public HttpResponseMessage RetrieveCriterias()
        {
            try
            {
                IEnumerable<Object> criterias = CatalogueCriteriaPL.RetrieveCriterias();
                object returnedCriterias = new { data = criterias };
                return Request.CreateResponse(HttpStatusCode.OK, returnedCriterias);
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
