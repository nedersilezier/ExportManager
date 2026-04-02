using ExportManager.Helper;
using ExportManager.Models;
using ExportManager.Models.DTO;
using ExportManager.Models.Extensions;
using ExportManager.Models.Parameters;
using ExportManager.ViewModels;
using ExportManager.ViewModels.Abstract;
using ExportManager.ViewModels.AddViewModels;
using ExportManager.ViewModels.Windows;
using ExportManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace ExportManager.ViewModels.ShowAllViewModels
{
    public class AllInvoicesViewModel : AllViewModel<InvoicesListView>
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
        #region Commands
        private BaseCommand _ApproveInvoiceCommand;
        public ICommand ApproveInvoiceCommand
        {
            get
            {
                if (_ApproveInvoiceCommand == null)
                {
                    _ApproveInvoiceCommand = new BaseCommand(() => OnApproveInvoice());
                }
                return _ApproveInvoiceCommand;
            }
        }
        #endregion
        #region Functions
        public override void OnAdd()
        {
            OpenNewTab(() => new NewInvoiceViewModel(), Load);
        }
        public override void OnEdit()
        {
            OnRequestWindow<EditInvoiceViewModel>(new InvoiceParameter(SelectedItem, Load));
        }
        public override void Remove()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("Please select an invoice to delete.");
                return;
            }
            if (SelectedItem.IsApproved)
            {
                MessageBox.Show("Approved invoices cannot be deleted.");
                return;
            }
            OnRemove();
        }
        public override void OnRemove()
        {
            var result = MessageBox.Show(
                        "Delete this draft?",
                        $"{SelectedItem.Buyer.Name}",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                using (var transaction = potplantsEntities.Database.BeginTransaction())
                {
                    try
                    {
                        InvoiceCleanup();
                        SoftDelete(SelectedItem.InvoiceId);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

            }
        }
        private void InvoiceCleanup()
        {
            var now = DateTime.Now;
            var user = Environment.UserName;
            var invoiceParties = potplantsEntities.InvoiceParties.Where(ip => ip.InvoiceId == SelectedItem.InvoiceId).ToList();
            invoiceParties.ForEach(ip =>
            {
                ip.IsActive = false;
                ip.DeletedAt = now;
                ip.DeletedBy = user;
            });
            var invoiceItems = potplantsEntities.InvoiceItems.Where(ii => ii.IsActive == true && ii.InvoiceId == SelectedItem.InvoiceId).ToList();

            invoiceItems.ForEach(ii => {
                ii.IsActive = false;
                ii.DeletedAt = now;
                ii.DeletedBy = user;
            });
            var orders = invoiceItems.Where(ii => ii.SourceOrderItemId.HasValue).Select(ii => ii.OrderItems.Orders).Distinct().ToList();
            orders.ForEach(o => {
                o.Status = OrderStatuses.Closed; 
                o.UpdatedAt = now;
                o.UpdatedBy = user;
                });
        }
        private void SoftDelete(int itemId)
        {
            var item = potplantsEntities.Invoices.Find(itemId);
            if (item == null)
            { 
                MessageBox.Show("Invoice not found.");
                return;
            }
                
            item.IsActive = false;
            item.DeletedAt = DateTime.Now;
            item.DeletedBy = Environment.UserName;
            item.Status = InvoiceStatuses.Canceled;
            potplantsEntities.SaveChanges();
            Load();
        }
        public override IList<CommandViewModel> CreateExtraCommands()
        {
            return new List<CommandViewModel>
            {
                new CommandViewModel("Approve", ApproveInvoiceCommand)
            };
        }
        private void OnApproveInvoice()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("Please select an invoice to approve.");
                return;
            }
            //TODO: Implement invoice approval logic here, such as updating the invoice status and saving changes to the database.
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
