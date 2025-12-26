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
        #region Fields
        protected bool _IsEditMode = false;
        #endregion
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
            Console.WriteLine("Save called from NewItemViewModel");
            if (!_IsEditMode)
            {
                Console.WriteLine("Weszlo?????");
                item.IsActive = true;
                potplantsEntities.Set<T>().Add(item);
            }
            try
            {
                Console.WriteLine("Item name: " + item);
                potplantsEntities.SaveChanges();
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
