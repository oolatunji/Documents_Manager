using DocumentManager.Web.Models;
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
        public HttpResponseMessage RequestDocument([FromBody]DocumentTransactionModel model)
        {
            try
            {
                string errMsg = string.Empty;

                var docTransaction = new DocumentTransaction
                {
                    FromWareHouse = model.FromUser,
                    ToUser = model.ToUser,
                    Date = System.DateTime.Now,
                    DocumentID = model.DocumentID,
                    DocumentDetailID = model.DocumentDetailID,
                    Status = StatusUtil.RequestStatus.Pending.ToString()
                };

                bool result = DocumentPL.RequestDocument(docTransaction);
                if (result)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Request made successfully.");
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

        [HttpPost]
        public HttpResponseMessage ApproveDocument([FromBody]DocumentTransactionModel model)
        {
            try
            {
                string errMsg = string.Empty;

                var docTransaction = new DocumentTransaction
                {
                    ID = model.ID,
                    ToUser = model.ToUser,
                    DocumentID = model.DocumentID,
                    DocumentDetailID = model.DocumentDetailID,
                    Status = StatusUtil.RequestStatus.Approved.ToString()
                };

                bool result = DocumentPL.ApproveDocument(docTransaction);
                if (result)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Request approved successfully.");
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

        [HttpPost]
        public HttpResponseMessage DeclineDocument([FromBody]DocumentTransactionModel model)
        {
            try
            {
                string errMsg = string.Empty;

                var docTransaction = new DocumentTransaction
                {
                    ID = model.ID,
                    ToUser = model.ToUser,
                    DocumentID = model.DocumentID,
                    DocumentDetailID = model.DocumentDetailID,
                    Status = StatusUtil.RequestStatus.Declined.ToString()
                };

                bool result = DocumentPL.DeclineDocument(docTransaction);
                if (result)
                {
                    var toUser = UserPL.RetrieveUserByID(model.ToUser);

                    Mail.SendApproveDocumentMail(toUser, model.DocumentName);

                    return Request.CreateResponse(HttpStatusCode.OK, "Request declined successfully.");
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

        [HttpPost]
        public HttpResponseMessage SearchDocument([FromBody]SearchFilterModel model)
        {
            try
            {
                var searchValues = model.SearchValue != null ? model.SearchValue.Trim().Split(' ') : new string[] { };
                var searchSubject = model.SearchValue != null ? model.SearchValue : string.Empty;
                IEnumerable<Object> documents = DocumentPL.SearchDocuments(searchValues.ToList(), searchSubject.Trim());
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

        [HttpGet]
        public HttpResponseMessage RetrieveDocumentTransaction()
        {
            try
            {
                IEnumerable<Object> documents = DocumentPL.RetrieveDocumentTransactions();
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

        [HttpGet]
        public HttpResponseMessage RetrieveSearchLists()
        {
            try
            {
                IEnumerable<string> documents = DocumentPL.RetrieveSearchLists();
                return Request.CreateResponse(HttpStatusCode.OK, documents);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                var response = Request.CreateResponse(HttpStatusCode.BadRequest);
                response.ReasonPhrase = ex.Message;
                return response;
            }
        }

        [HttpGet]
        public HttpResponseMessage RetrieveDocumentTransactionsforApproval()
        {
            try
            {
                IEnumerable<Object> documents = DocumentPL.RetrieveDocumentTransactionsforApproval();
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
