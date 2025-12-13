using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic
{
    public class DatabaseClass
    {
        #region Database
        protected PotplantsEntities potplantsEntities;
        #endregion
        #region Constructor
        public DatabaseClass(PotplantsEntities potplantsEntities){
            this.potplantsEntities = potplantsEntities;
            }
        #endregion
    }
}
