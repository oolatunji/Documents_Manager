using DocumentManagerLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagerLibrary
{
    public class CatalogueCriteriaPL
    {
        public CatalogueCriteriaPL()
        {

        }

        public static bool Save(CatalogueCriteria criteria, out string message)
        {
            try
            {
                if (CatalogueCriteriaDL.CriteriaExists(criteria))
                {
                    message = string.Format("Criteria with name: {0} exists already", criteria.Name);
                    return false;
                }
                else
                {
                    message = string.Empty;
                    return CatalogueCriteriaDL.Save(criteria);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Update(CatalogueCriteria criteria)
        {
            try
            {
                return CatalogueCriteriaDL.Update(criteria);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Object> RetrieveCriterias()
        {
            try
            {
                var returnedCriterias = new List<object>();

                var criterias = CatalogueCriteriaDL.RetrieveCriterias();

                foreach (CatalogueCriteria criteria in criterias)
                {
                    Object criteriObj = new
                    {
                        ID = criteria.ID,
                        Name = criteria.Name,
                        Description = criteria.Description,
                        Date = String.Format("{0:g}", Convert.ToDateTime(criteria.Date)),
                    };

                    returnedCriterias.Add(criteriObj);
                }

                return returnedCriterias;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
