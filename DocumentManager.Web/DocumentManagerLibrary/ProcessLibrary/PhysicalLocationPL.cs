using DocumentManagerLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagerLibrary
{
    public class PhysicalLocationPL
    {
        public PhysicalLocationPL()
        {

        }

        public static bool Save(PhysicalLocation location, out string message)
        {
            try
            {
                if (PhysicalLocationDL.LocationExists(location))
                {
                    message = string.Format("Physical location with name: {0} or location: {1} exists already", location.Name, location.Location);
                    return false;
                }
                else
                {
                    message = string.Empty;
                    return PhysicalLocationDL.Save(location);
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
                return PhysicalLocationDL.Update(location);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Object> RetrieveLocations()
        {
            try
            {
                var returnedLocations = new List<object>();

                var locations = PhysicalLocationDL.RetrieveLocations();

                foreach (PhysicalLocation location in locations)
                {
                    Object locationObj = new
                    {
                        ID = location.ID,
                        Name = location.Name,
                        Description = location.Description,
                        Location = location.Location,
                        Date = String.Format("{0:g}", Convert.ToDateTime(location.Date)),
                    };

                    returnedLocations.Add(locationObj);
                }

                return returnedLocations;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
