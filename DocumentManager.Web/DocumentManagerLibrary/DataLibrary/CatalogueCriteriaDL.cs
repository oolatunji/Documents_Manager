using DocumentManagerLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagerLibrary
{
    public class CatalogueCriteriaDL
    {
        public CatalogueCriteriaDL()
        {

        }

        public static bool Save(CatalogueCriteria criteria)
        {
            try
            {
                using (var context = new DocumentManagerDBEntities())
                {
                    context.CatalogueCriterias.Add(criteria);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool CriteriaExists(CatalogueCriteria criteria)
        {
            try
            {
                var existingCriteria = new CatalogueCriteria();
                using (var context = new DocumentManagerDBEntities())
                {
                    existingCriteria = context.CatalogueCriterias
                                    .Where(t => t.Name.Equals(criteria.Name))
                                    .FirstOrDefault();
                }

                if (existingCriteria == null)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CatalogueCriteria> RetrieveCriterias()
        {
            try
            {
                using (var context = new DocumentManagerDBEntities())
                {
                    var criterias = context.CatalogueCriterias.ToList();

                    return criterias;
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
                CatalogueCriteria existingcriteria  = new CatalogueCriteria();
                using (var context = new DocumentManagerDBEntities())
                {
                    existingcriteria = context.CatalogueCriterias
                                    .Where(t => t.ID == criteria.ID)
                                    .FirstOrDefault();
                }

                if (existingcriteria != null)
                {
                    existingcriteria.Name = criteria.Name;
                    existingcriteria.Description = criteria.Description;

                    using (var context = new DocumentManagerDBEntities())
                    {
                        context.Entry(existingcriteria).State = EntityState.Modified;

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
