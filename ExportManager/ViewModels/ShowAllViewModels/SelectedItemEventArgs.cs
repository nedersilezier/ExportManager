using ExportManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.ViewModels.ShowAllViewModels
{
    //public class SelectedItemEventArgs<T> : EventArgs
    public class SelectedItemEventArgs : EventArgs
    {
        public int ItemId { get; private set; }
        public string DisplayName { get; private set; }
        //public T SelectedItem { get; private set; }
        public SelectedItemEventArgs(int itemId, string displayName)
        {
            ItemId = itemId;
            DisplayName = displayName;
        }
        //public SelectedItemEventArgs(T selectedItem)
        //{
        //    SelectedItem = selectedItem;
        //}
    }
}
