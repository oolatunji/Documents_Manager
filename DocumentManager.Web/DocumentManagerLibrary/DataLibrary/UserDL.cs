using DocumentManagerLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagerLibrary
{
    public class UserDL
    {
         public UserDL()
        {

        }

        public static bool Save(User user)
        {
            try
            {
                using (var context = new DocumentManagerDBEntities())
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UserExists(User user)
        {
            try
            {
                var existingUser = new User();
                using (var context = new DocumentManagerDBEntities())
                {
                    existingUser = context.Users
                                    .Where(t => t.Username.Equals(user.Username))
                                    .FirstOrDefault();
                }

                if (existingUser == null)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static User RetrieveUserByUsername(string username)
        {
            try
            {
                var existingUser = new User();
                using (var context = new DocumentManagerDBEntities())
                {
                    existingUser = context.Users
                                    .Where(t => t.Username.Equals(username))
                                    .FirstOrDefault();
                }

                return existingUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool ChangePassword(string username, string newHashedPassword)
        {
            try
            {
                User existingUser = new User();
                using (var context = new DocumentManagerDBEntities())
                {
                    existingUser = context.Users
                                    .Where(t => t.Username == username)
                                    .FirstOrDefault();
                }

                if (existingUser != null)
                {
                    existingUser.HashedPassword = newHashedPassword;
                    existingUser.FirstTime = false;
                    using (var context = new DocumentManagerDBEntities())
                    {
                        context.Entry(existingUser).State = EntityState.Modified;

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

        public static List<User> RetrieveUsers()
        {
            try
            {
                using (var context = new DocumentManagerDBEntities())
                {
                    var users = context.Users
                                        .Include("Role.RoleFunctions.Function")
                                        .ToList();

                    return users;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Int64?> RetrieveUsersByName(List<string> searchValues)
        {
            try
            {
                var criteriaIDs = new List<Int64?>();

                var criteriaList = new List<User>();

                using (var context = new DocumentManagerDBEntities())
                {
                    searchValues.ForEach(searchValue =>
                    {
                        var criterias = context.Users
                                            .Where(cri => cri.Lastname.Contains(searchValue) || cri.Othernames.Contains(searchValue))
                                            .ToList();

                        criteriaList.AddRange(criterias);

                    });

                    if (criteriaList.Any())
                    {
                        criteriaList.ForEach(criteria =>
                        {
                            criteriaIDs.Add(criteria.ID);
                        });
                    }
                    return criteriaIDs;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static User AuthenticateUser(string username, string hashedPassword)
        {
            try
            {
                using (var context = new DocumentManagerDBEntities())
                {
                    var users = context.Users
                                        .Include("Role.RoleFunctions.Function")
                                        .Where(f => f.Username == username && f.HashedPassword == hashedPassword);

                    return users.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static User RetrieveUserByID(long ID)
        {
            try
            {
                using (var context = new DocumentManagerDBEntities())
                {
                    var users = context.Users
                                        .Where(f => f.ID == ID);

                    return users.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Update(User user)
        {
            try
            {
                User existingUser = new User();
                using (var context = new DocumentManagerDBEntities())
                {
                    existingUser = context.Users
                                    .Where(t => t.ID == user.ID)
                                    .FirstOrDefault();
                }

                if (existingUser != null)
                {
                    existingUser.Email = user.Email;
                    existingUser.Gender = user.Gender;
                    existingUser.PhoneNumber = user.PhoneNumber;
                    existingUser.Lastname = user.Lastname;
                    existingUser.Othernames = user.Othernames;                    
                    existingUser.UserRole = user.UserRole;

                    using (var context = new DocumentManagerDBEntities())
                    {
                        context.Entry(existingUser).State = EntityState.Modified;

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
