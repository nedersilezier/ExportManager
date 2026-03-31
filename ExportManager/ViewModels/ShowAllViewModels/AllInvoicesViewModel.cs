using ExportManager.Models;
using ExportManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using ExportManager.ViewModels;
using ExportManager.ViewModels.Abstract;
using ExportManager.Models.DTO;
using ExportManager.Models.Extensions;
using ExportManager.ViewModels.AddViewModels;
using ExportManager.ViewModels.Windows;
using ExportManager.Models.Parameters;

namespace ExportManager.ViewModels.ShowAllViewModels
{
    public class AllInvoicesViewModel: AllViewModel<InvoicesListView>
    {
        #region List
        public override void Load()
        {
            List = new ObservableCollection<InvoicesListView>(potplantsEntities.Invoices.Where(i => i.IsActive == true).Select(i => new InvoicesListView
            {
                InvoiceId = i.InvoiceId,
                InvoiceNo = i.InvoiceNo,
                InvoiceDate = i.InvoiceDate,
                PaymentDate = i.PaymentDate,
                Status = i.Status,
                IsApproved = i.IsApproved,
                TotalAmount = i.TotalAmount,
                TotalGross = i.TotalGross,
                TotalTax = i.TotalTax,
                TotalNet = i.TotalNet,
                TotalStorageCost = i.TotalStorageCost,
                TotalTransportCost = i.TotalTransportCost,
                PaymentMethod = i.PaymentMethods.Name,
                Buyer = i.InvoiceParties.Where(ip => ip.IsActive == true && ip.Role == InvoicePartyRoles.Buyer).Select(ip => new InvoicePartyListView
                {
                    InvoicePartyId = ip.InvoicePartyId,
                    Name = ip.Name,
                    TaxId = ip.TaxId,
                    City = ip.City,
                    Street = ip.Street,
                    FullHouseNumber = ip.FullHouseNo,
                    PostalCode = ip.PostalCode,
                    Country = ip.Country,
                    CountryCode = ip.CountryCode,
                    Role = ip.Role
                }).FirstOrDefault(),
                Seller = i.InvoiceParties.Where(ip => ip.IsActive == true && ip.Role == InvoicePartyRoles.Seller).Select(ip => new InvoicePartyListView
                {
                    InvoicePartyId = ip.InvoicePartyId,
                    Name = ip.Name,
                    TaxId = ip.TaxId,
                    City = ip.City,
                    Street = ip.Street,
                    FullHouseNumber = ip.FullHouseNo,
                    PostalCode = ip.PostalCode,
                    Country = ip.Country,
                    CountryCode = ip.CountryCode,
                    Role = ip.Role
                }).FirstOrDefault(),
                TotalQuantity = i.InvoiceItems.Where(ii => ii.IsActive == true).Sum(ii => ii.Quantity),
                Remarks = i.Remarks
            }).ToList());
        }
        #endregion
        #region Constructor
        public AllInvoicesViewModel()
            : base()
        {
            base.DisplayName = "Invoices";
        }
        #endregion

        #region Functions
        public override void OnAdd()
        {
            OpenNewTab(() => new NewInvoiceViewModel(), Load);
        }
        public override void OnEdit()
        {
            OnRequestWindow<EditInvoiceViewModel>(new InvoiceParameter(SelectedItem.InvoiceId, Load));
        }
        public override void OnRemove()
        {
            return;
        }
        #endregion
        #region Sorting and searching
        public override List<string> getComboBoxSortList()
        {
            return new List<string> { "Not implemented yet" };
        }
        public override void Sort()
        {
            return;
        }
        public override List<string> getComboBoxFindList()
        {
            return new List<string> { "Not implemented yet" };
        }
        public override void Find()
        {
            return;
        }
        #endregion
    }
}
