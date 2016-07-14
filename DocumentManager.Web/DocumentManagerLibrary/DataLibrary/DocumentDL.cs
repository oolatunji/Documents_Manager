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
                    var existingSearchVal = context.SearchLists.Where(search => search.Subject == searchValue).ToList();
                    if(!existingSearchVal.Any())
                    {
                        context.SearchLists.Add(searchList);
                        context.SaveChanges();
                    }
                    
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

        public static List<SearchList> RetrieveSearchLists()
        {
            try
            {
                using (var context = new DocumentManagerDBEntities())
                {
                    var seachLists = context.SearchLists
                                        .ToList();

                    return seachLists;
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
                                        .Include(doc => doc.CatalogueCriteria)
                                        .Include(doc => doc.CatalogueCriteria1)
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
                                        .Include(doc => doc.CatalogueCriteria)
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

        public static List<DocumentDetail> SearchDocuments(List<string> searchValues)
        {
            try
            {
                using (var context = new DocumentManagerDBEntities())
                {
                    var locations = PhysicalLocationDL.RetrieveLocationsByName(searchValues);
                    var criteriaIDs = CatalogueCriteriaDL.RetrieveCriteriasByName(searchValues);
                    var userIDs = UserDL.RetrieveUsersByName(searchValues);

                    var documents = new List<DocumentDetail>();
                    var docIDs = new List<Int64?>();

                    searchValues.ForEach(searchValue =>
                    {
                        var docs = context.DocumentDetails
                                        .Where(doc => doc.DocumentContent.Contains(searchValue) || doc.Name.Contains(searchValue))
                                        .Include(doc => doc.CatalogueCriteria)
                                        .Include(doc => doc.PhysicalLocation)
                                        .Include(doc => doc.User)
                                        .Include(doc => doc.User1)
                                        .ToList();

                        documents.AddRange(docs);
                    });

                    if(documents.Any())
                    {
                        documents.ForEach(doc =>
                        {
                            docIDs.Add(doc.ID);
                        });
                    }

                    var returendDocuments = context.DocumentDetails
                                                    .Where(doc => docIDs.Contains(doc.ID) || locations.Contains(doc.Location) || criteriaIDs.Contains(doc.Catalogue) || userIDs.Contains(doc.CurrentUser))
                                                    .Include(doc => doc.CatalogueCriteria)
                                                    .Include(doc => doc.PhysicalLocation)
                                                    .Include(doc => doc.User)
                                                    .Include(doc => doc.User1)
                                                    .ToList();

                    return returendDocuments;
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
                            var user = UserDL.RetrieveUserByID(Convert.ToInt64(doc.ToUser));
                            var username = user.Username.ToUpper();

                            var existingWarehouse = context.CatalogueCriterias.Where(x => x.Name == username).ToList();
                            if (!existingWarehouse.Any())
                            {
                                var wayhouse = new CatalogueCriteria
                                    {
                                        Name = username,
                                        Description = string.Format("{0} {1}", user.Lastname, user.Othernames)
                                    };
                                context.CatalogueCriterias.Add(wayhouse);
                                context.SaveChanges();

                                existingWarehouse.Add(wayhouse);
                            }

                            var existingLocation = context.PhysicalLocations.Where(x => x.Name == username).ToList();
                            if (!existingLocation.Any())
                            {
                                var location = new PhysicalLocation
                                {
                                    Name = user.Username.ToUpper(),
                                    Location = string.Format("{0} {1}", user.Lastname, user.Othernames),
                                    Description = string.Format("{0} {1}", user.Lastname, user.Othernames)
                                };
                                context.PhysicalLocations.Add(location);
                                context.SaveChanges();

                                existingLocation.Add(location);
                            }

                            var existingDoc = new DocumentDetail();
                            existingDoc = context.DocumentDetails
                                            .Where(t => t.ID == doc.DocumentDetailID)
                                            .FirstOrDefault();

                            existingDoc.CurrentUser = doc.ToUser;
                            existingDoc.Catalogue = existingWarehouse.FirstOrDefault().ID;
                            existingDoc.Location = existingLocation.FirstOrDefault().ID;
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
