using MinionWarsEntitiesLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionWarsEntitiesLib.EntityManagers
{
    public static class AccountManager
    {
        public static Users LoginUser(string username, string password)
        {
            string hash = CreateMD5(password);

            using (var db = new MinionWarsEntities())
            {
                List<Users> user = db.Users.Where(x => x.username.Equals(username)).ToList();
                if(user.Count == 1)
                {
                    if (user.First().pass.Equals(hash))
                    {
                        user.First().online = 1;
                        db.Users.Attach(user.First());
                        db.Entry(user.First()).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        return user.First();
                    }
                    else return null;
                }
                else
                {
                    return null;
                }
            }
        }

        public static bool RegisterUser(string username, string password)
        {
            string hash = CreateMD5(password);

            using (var db = new MinionWarsEntities())
            {
                List<Users> user = db.Users.Where(x => x.username.Equals(username)).ToList();
                if (user.Count > 0)
                {
                    return false;
                }
                else
                {
                    Users newUser = new Users();
                    newUser.username = username;
                    newUser.pass = hash;
                    newUser.experience = 0;
                    newUser.lvl = 1;
                    newUser.online = 0;
                    newUser.location = null;

                    //traits
                    newUser.trait_leadership = 0;
                    newUser.trait_logistics = 0;
                    newUser.trait_architecture = 0;
                    newUser.trait_economics = 0;

                    db.Users.Add(newUser);
                    db.SaveChanges();

                    return true;
                }
            }
        }

        public static bool LogOffUser(string username)
        {
            using (var db = new MinionWarsEntities())
            {
                List<Users> user = db.Users.Where(x => x.username.Equals(username)).ToList();
                if (user.Count == 1)
                {
                    user.First().online = 0;
                    db.Users.Attach(user.First());
                    db.Entry(user.First()).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private static string CreateMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
