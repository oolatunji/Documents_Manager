using DocumentManagerLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagerLibrary
{
    public class DocumentDL
    {
        public DocumentDL()
        {

        }

        public static bool Save(DocumentDetail docdetail)
        {
            try
            {
                using (var context = new DocumentManagerDBEntities())
                {
                    context.DocumentDetails.Add(docdetail);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool SaveSearchSubject(string searchValue)
        {
            try
            {
                var searchList = new SearchList
                {
                    Subject = searchValue
                };

                using (var context = new DocumentManagerDBEntities())
                {
                    context.SearchLists.Add(searchList);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool RequestDocument(DocumentTransaction transaction)
        {
            try
            {
                using (var context = new DocumentManagerDBEntities())
                {
                    context.DocumentTransactions.Add(transaction);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool DocumentExists(string name)
        {
            try
            {
                var existingDoc = new DocumentDetail();
                using (var context = new DocumentManagerDBEntities())
                {
                    existingDoc = context.DocumentDetails
                                    .Where(t => t.Name.Equals(name))
                                    .FirstOrDefault();
                }

                if (existingDoc == null)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<DocumentDetail> RetrieveDocuments()
        {
            try
            {
                using (var context = new DocumentManagerDBEntities())
                {
                    var docs = context.DocumentDetails
                                        .Include(doc => doc.CatalogueCriteria)
                                        .Include(doc => doc.PhysicalLocation)
                                        .Include(doc => doc.User)
                                        .Include(doc => doc.User1)
                                        .ToList();

                    return docs;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<DocumentTransaction> RetrieveDocumentTransactions()
        {
            try
            {
                using (var context = new DocumentManagerDBEntities())
                {
                    var docs = context.DocumentTransactions
                                        .Include(doc => doc.DocumentDetail)
                                        .Include("DocumentDetail.CatalogueCriteria")
                                        .Include("DocumentDetail.PhysicalLocation")
                                        .Include(doc => doc.User)
                                        .Include(doc => doc.User1)
                                        .ToList();

                    return docs;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<DocumentTransaction> RetrieveDocumentTransactionsforApproval()
        {
            try
            {
                var status = StatusUtil.RequestStatus.Pending.ToString();

                using (var context = new DocumentManagerDBEntities())
                {
                    var docs = context.DocumentTransactions
                                        .Include(doc => doc.DocumentDetail)
                                        .Include(doc => doc.User)
                                        .Include(doc => doc.User1)
                                        .Where(doc => doc.Status == status)
                                        .ToList();

                    return docs;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<DocumentDetail> SearchDocuments(string searchValue)
        {
            try
            {
                using (var context = new DocumentManagerDBEntities())
                {
                    var docs = context.DocumentDetails
                                        .Where(doc => doc.DocumentContent.Contains(searchValue))
                                        .Include(doc => doc.CatalogueCriteria)
                                        .Include(doc => doc.PhysicalLocation)
                                        .Include(doc => doc.User)
                                        .Include(doc => doc.User1)
                                        .ToList();

                    return docs;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Document RetrieveDocumentByID(long documentID)
        {
            try
            {
                using (var context = new DocumentManagerDBEntities())
                {
                    var docs = context.Documents
                                .Where(doc => doc.ID == documentID)
                                .ToList();

                    return docs.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Update(DocumentDetail doc)
        {
            try
            {
                var existingDoc  = new DocumentDetail();
                using (var context = new DocumentManagerDBEntities())
                {
                    existingDoc = context.DocumentDetails
                                    .Where(t => t.ID == doc.ID)
                                    .FirstOrDefault();
                }

                if (existingDoc != null)
                {
                    existingDoc.Name = doc.Name;
                    existingDoc.Catalogue = doc.Catalogue;
                    existingDoc.Location = doc.Location;
                    existingDoc.CurrentUser = doc.CurrentUser;
                    
                    using (var context = new DocumentManagerDBEntities())
                    {
                        context.Entry(existingDoc).State = EntityState.Modified;

                        context.SaveChanges();
                    }

                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool ApproveDocument(DocumentTransaction doc)
        {
            try
            {
                using (var context = new DocumentManagerDBEntities())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var existingDoc = new DocumentDetail();
                            existingDoc = context.DocumentDetails
                                            .Where(t => t.ID == doc.DocumentDetailID)
                                            .FirstOrDefault();

                            existingDoc.CurrentUser = doc.ToUser;
                            context.Entry(existingDoc).State = EntityState.Modified;
                            context.SaveChanges();

                            var existingDocTransaction = new DocumentTransaction();
                            existingDocTransaction = context.DocumentTransactions
                                                        .Where(t => t.ID == doc.ID)
                                                        .FirstOrDefault();

                            existingDocTransaction.Status = doc.Status;
                            context.Entry(existingDocTransaction).State = EntityState.Modified;
                            context.SaveChanges();

                            transaction.Commit();

                            return true;
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            throw e;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool DeclineDocument(DocumentTransaction doc)
        {
            try
            {
                using (var context = new DocumentManagerDBEntities())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var existingDocTransaction = new DocumentTransaction();
                            existingDocTransaction = context.DocumentTransactions
                                                        .Where(t => t.ID == doc.ID)
                                                        .FirstOrDefault();

                            existingDocTransaction.Status = doc.Status;
                            context.Entry(existingDocTransaction).State = EntityState.Modified;
                            context.SaveChanges();

                            transaction.Commit();

                            return true;
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            throw e;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
