using ExportManager.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportManager.Models.BusinessLogic.Queries
{
    public class InvoiceItemsQuery: DatabaseClass
    {
        #region Constructor
        public InvoiceItemsQuery(PotplantsEntities potplantsEntities)
            : base(potplantsEntities)
        {
        }
        #endregion
        #region Functions
        public IQueryable<InvoiceItems> GetInvoiceItems(int invoiceId)
        {
                       return potplantsEntities.InvoiceItems.Where(ii => ii.IsActive && ii.InvoiceId == invoiceId);
        }
        public IQueryable<InvoiceItemsListView> GetInvoiceItemsDTO(int invoiceId)
        {
            return GetInvoiceItems(invoiceId).Select(ii => new InvoiceItemsListView
            {
                ItemNo = ii.ItemNo,
                Name = ii.NameSnapshot,
                Potsize = ii.PotSizeSnapshot,
                Height = ii.HeightSnapshot,
                Quantity = ii.Quantity,
                UnitPrice = ii.UnitPriceSnapshot,
                NetAmount = ii.NetAmount,
                TaxAmount = ii.TaxAmount,
                GrossAmount = ii.GrossAmount,
                SourceOrderItemId = ii.SourceOrderItemId
            });
        }
        #endregion
    }
}
