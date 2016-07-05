using DocumentManagerLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagerLibrary
{
    public class PhysicalLocationDL
    {
        public PhysicalLocationDL()
        {

        }

        public static bool Save(PhysicalLocation location)
        {
            try
            {
                using (var context = new DocumentManagerDBEntities())
                {
                    context.PhysicalLocations.Add(location);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool LocationExists(PhysicalLocation location)
        {
            try
            {
                var existingLocation = new PhysicalLocation();
                using (var context = new DocumentManagerDBEntities())
                {
                    existingLocation = context.PhysicalLocations
                                    .Where(t => t.Name.Equals(location.Name) || t.Location.Equals(location.Location))
                                    .FirstOrDefault();
                }

                if (existingLocation == null)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<PhysicalLocation> RetrieveLocations()
        {
            try
            {
                using (var context = new DocumentManagerDBEntities())
                {
                    var locations = context.PhysicalLocations.ToList();

                    return locations;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Update(PhysicalLocation location)
        {
            try
            {
                var existinglocation  = new PhysicalLocation();
                using (var context = new DocumentManagerDBEntities())
                {
                    existinglocation = context.PhysicalLocations
                                    .Where(t => t.ID == location.ID)
                                    .FirstOrDefault();
                }

                if (existinglocation != null)
                {
                    existinglocation.Name = location.Name;
                    existinglocation.Description = location.Description;
                    existinglocation.Location = location.Location;

                    using (var context = new DocumentManagerDBEntities())
                    {
                        context.Entry(existinglocation).State = EntityState.Modified;

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
