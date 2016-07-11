﻿using DocumentManagerLibrary.ModelLibrary.EntityFrameworkLib;
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
    }
}
