using ExportManager.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic
{
    public class InvoiceCalculator: DatabaseClass
    {
        #region Constructor
        public InvoiceCalculator(PotplantsEntities potplantsEntities)
            :base(potplantsEntities)
        {
        }
        #endregion
        #region Functions
        private IQueryable<Invoices> InvoicesQuery(int invoiceId, DateTime date)
        {
            DateTime startDate = date.Date;
            DateTime endDate = startDate.AddDays(1);
            return potplantsEntities.Invoices.Where(
                t => t.IsActive
                && t.InvoiceId == invoiceId
                && t.InvoiceDate >= startDate
                && t.InvoiceDate <= endDate);
        }
        public IQueryable<InvoiceItemsListView> InvoiceItemsQuery(int invoiceId, DateTime date)
        {
            return InvoicesQuery(invoiceId, date).SelectMany(invoice => invoice.InvoiceItems)
                .Where(invoiceitem => invoiceitem.IsActive)
                .Select(t => new InvoiceItemsListView
                {
                    ItemNo = t.ItemNo,
                    Name = t.NameSnapshot,
                    Potsize = t.PotSizeSnapshot,
                    Height = t.HeightSnapshot,
                    Quantity = t.Quantity,
                    NetAmount = t.NetAmount,
                    TaxAmount = t.TaxAmount,
                    GrossAmount = t.GrossAmount
                });
        }
        public decimal? CalculateNetTotal(int invoiceId, DateTime date)
        {
            var result = (decimal)InvoiceItemsQuery(invoiceId, date).Sum(t => t.NetAmount);
            return Math.Round(result, 2, MidpointRounding.ToEven);
        }
        public decimal? CalculateTaxTotal(int invoiceId, DateTime date)
        {
            var result = (decimal)InvoiceItemsQuery(invoiceId, date).Sum(t => t.TaxAmount);
            return Math.Round(result, 2, MidpointRounding.ToEven);
        }
        public decimal? CalculateGrossTotal(int invoiceId, DateTime date)
        {
            var result = (decimal)InvoiceItemsQuery(invoiceId, date).Sum(t => t.GrossAmount);
            return Math.Round(result, 2, MidpointRounding.ToEven);
        }
        #endregion
    }
}
