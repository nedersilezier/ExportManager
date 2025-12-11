using ExportManager.Helper;
using ExportManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ExportManager.ViewModels;
using System.Data.Entity.Validation;
using System.Windows;
using System.Collections.ObjectModel;

namespace ExportManager.ViewModels.Abstract
{
    public abstract class NewItemViewModel<T>: NewItemViewModelBase where T: class, IHasIsActive, new()
    {
        #region Database
        protected T item;
        #endregion
        #region Constructor
        public NewItemViewModel()
            :base()
        {
            item = new T();
        }
        #endregion
        #region Commands
        public override void Save()
        {
            item.IsActive = true;
            potplantsEntities.Set<T>().Add(item);
            try
            {
                potplantsEntities.SaveChanges();
            }
            catch (DbEntityValidationException dbex)
            {
                string message = string.Empty;
                foreach (var efex in dbex.EntityValidationErrors)
                {
                    message += "Type " + efex.Entry.Entity.GetType().Name + " in state " + efex.Entry.State + " has errors: \n";
                    foreach (var errormsg in efex.ValidationErrors)
                        message += "\tProperty: " + errormsg.PropertyName + ", error: " + errormsg.ErrorMessage + "/n";
                }
                MessageBox.Show(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //RaiseAdded();
        }
        #endregion
        #region Functions
        #endregion
    }
}
