using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_TCG
{
    public class ManageAccount
    {
        public TCG_DataEntities _tcgEntity = null;

        public bool CreateUser(Users_Data users_Data)
        {
            bool iscreated = false;
            try
            {
                _tcgEntity = new TCG_DataEntities();
                _tcgEntity.Users_Data.Add(users_Data);
                _tcgEntity.SaveChanges();
                iscreated = !iscreated;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return iscreated;
        }

        public bool ChecUser()
        {
            return true;
        }
    }
}
