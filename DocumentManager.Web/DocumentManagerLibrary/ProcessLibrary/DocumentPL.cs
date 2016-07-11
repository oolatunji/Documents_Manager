using DocumentManagerLibrary.ModelLibrary.EntityFrameworkLib;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DocumentManagerLibrary
{
    public class DocumentPL
    {
        public DocumentPL()
        {

        }

        public static bool Save(DocumentModel docModels, out string message)
        {
            try
            {
                if (!DocumentDL.DocumentExists(docModels.Name))
                {
                    var ms = new MemoryStream();
                    var pdfdoc = new iTextSharp.text.Document();

                    PdfWriter writer = PdfWriter.GetInstance(pdfdoc, ms);
                    var rawDocument = System.Convert.FromBase64String(docModels.RawDocument);
                    var reader = new PdfReader(rawDocument);

                    var docContent = string.Empty;

                    int intPageNum = reader.NumberOfPages;
                    string[] words;
                    for (int i = 1; i <= intPageNum; i++)
                    {
                        var text = PdfTextExtractor.GetTextFromPage(reader, i, new LocationTextExtractionStrategy());
                        words = text.Split('\n');
                        for (int j = 0, len = words.Length; j < len; j++)
                        {
                            docContent += Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(words[j]));
                        }
                    }
                    //pdfdoc.Close();
                    ms.Flush();
                    ms.Dispose();

                    var docDetail = new DocumentDetail();
                    docDetail.Name = docModels.Name;
                    docDetail.Catalogue = docModels.Catalogue;
                    docDetail.Location = docModels.Location;
                    docDetail.Uploader = docModels.Uploader;
                    docDetail.CurrentUser = docModels.CurrentUser;
                    docDetail.Date = System.DateTime.Now;
                    docDetail.DocumentContent = docContent;
                    docDetail.Document = new Document { RawDocument = rawDocument };

                    message = string.Empty;
                    return DocumentDL.Save(docDetail);
                }
                else
                {
                    message = string.Format("Document with name: {0} exists already", docModels.Name);
                    return false;
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
                return DocumentDL.Update(doc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string ViewDocument(DocumentModel docModel)
        {
            try
            {
                var document = DocumentDL.RetrieveDocumentByID(Convert.ToInt64(docModel.DocumentID));

                var base64EncodedPDF = System.Convert.ToBase64String(document.RawDocument);

                return base64EncodedPDF;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static List<Object> RetrieveDocuments()
        {
            try
            {
                var returnedDocs = new List<object>();

                var docs = DocumentDL.RetrieveDocuments();

                foreach (DocumentDetail doc in docs)
                {
                    Object docObj = new
                    {
                        ID = doc.ID,
                        Name = doc.Name,
                        CatalogueCriteria = doc.CatalogueCriteria.Name,
                        PhysicalLocation = doc.PhysicalLocation.Name,
                        Uploader = doc.User.Username,
                        CurrentUser = string.Format("{0} {1}", doc.User1.Lastname, doc.User1.Othernames),
                        Date = String.Format("{0:g}", Convert.ToDateTime(doc.Date)),
                        DocumentID = doc.DocumentID
                    };

                    returnedDocs.Add(docObj);
                }

                return returnedDocs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
