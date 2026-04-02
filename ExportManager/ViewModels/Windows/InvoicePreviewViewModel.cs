using ExportManager.Models;
using ExportManager.Models.BusinessLogic.Queries;
using ExportManager.Models.DTO;
using ExportManager.Models.EntitiesForView;
using ExportManager.Models.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml;

namespace ExportManager.ViewModels.Windows
{
    public class InvoicePreviewViewModel : BaseViewModel, IParameterReceiver<InvoiceParameter>
    {
        #region Fields
        private PotplantsEntities potplantsEntities;
        private InvoicesListView _Invoice;
        private List<InvoiceItemsListView> _InvoiceItemsList;
        private FlowDocument _InvoiceDocument;
        #endregion
        #region Constructor
        public InvoicePreviewViewModel()
        {
            potplantsEntities = new PotplantsEntities();
        }
        #endregion
        #region Properties
        public InvoicesListView Invoice
        {
            get { return _Invoice; }
            set
            {
                if (_Invoice != value)
                {
                    _Invoice = value;
                }
            }
        }
        public FlowDocument InvoiceDocument
        {
            get 
            { 
                if(_InvoiceDocument == null)
                    _InvoiceDocument = new FlowDocument();
                return _InvoiceDocument; 
            }
            set
            {
                if (_InvoiceDocument != value)
                {
                    _InvoiceDocument = value;
                    OnPropertyChanged(() => InvoiceDocument);
                }
            }
        }
        public List<InvoiceItemsListView> InvoiceItemsList
        {
            get { return _InvoiceItemsList; }
            set
            {
                if(_InvoiceItemsList != value)
                {
                    _InvoiceItemsList = value;
                }
            }
        }
        #endregion
        #region Functions
        public void SetParameter(InvoiceParameter parameter)
        {
            if (parameter == null)
            {
                return;
            }
            DisplayName = parameter.Title;
            Invoice = parameter.Invoice;
            InvoiceItemsList = new ObservableCollection<InvoiceItemsListView>(parameter.InvoiceItems).ToList();
            BuildInvoiceDocument();
            OnPropertyChanged(() => DisplayName);
        }
        public void BuildInvoiceDocument()
        {
            // ensure document exists, set the format
            InvoiceDocument = new FlowDocument
            {
                PageWidth = 793.7,
                PageHeight = 1122.5,
                Background = System.Windows.Media.Brushes.White,
                PagePadding = new Thickness(20),
                ColumnWidth = 793.7 - 40
            };

            // Header (upper-right)
            var header = new Paragraph(new Bold(new Run("Invoice")))
            {
                LineHeight = 40,
                FontSize = 22,
                TextAlignment = TextAlignment.Right,
                Margin = new Thickness(0, 0, 0, 10),
                Background = System.Windows.Media.Brushes.AliceBlue,
                LineStackingStrategy = LineStackingStrategy.BlockLineHeight
            };
            InvoiceDocument.Blocks.Add(header);

            // Invoice basic info (right aligned, stacked)
            var invoiceNo = Invoice?.InvoiceNo ?? string.Empty;
            var invoiceDate = Invoice == null ? string.Empty : Invoice.InvoiceDate.ToString("d");
            var paymentDate = (Invoice?.PaymentDate.HasValue == true) ? Invoice.PaymentDate.Value.ToString("d") : string.Empty;

            var infoPara = new Paragraph { TextAlignment = TextAlignment.Right, Margin = new Thickness(0, 30, 0, 30), LineHeight = 26 };
            infoPara.Inlines.Add(new Run($"Invoice No: {invoiceNo}"));
            infoPara.Inlines.Add(new LineBreak());
            infoPara.Inlines.Add(new Run($"Invoice Date: {invoiceDate}"));
            infoPara.Inlines.Add(new LineBreak());
            infoPara.Inlines.Add(new Run($"Payment Date: {paymentDate}"));
            InvoiceDocument.Blocks.Add(infoPara);

            // From / To section using a two-column table
            var partiesTable = new Table { CellSpacing = 12, Margin = new Thickness(0, 20, 0, 50), LineHeight = 26 };
            partiesTable.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) });
            partiesTable.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) });

            var partiesRg = new TableRowGroup();
            // headings
            var headings = new TableRow();
            headings.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("From")))));
            headings.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("To")))));
            partiesRg.Rows.Add(headings);

            // content row
            var contentRow = new TableRow();
            // seller
            var sellerPara = new Paragraph();
            sellerPara.LineHeight = 26;
            var seller = Invoice?.Seller;
            if (seller != null)
            {
                sellerPara.Inlines.Add(new Run(seller.Name ?? string.Empty)); sellerPara.Inlines.Add(new LineBreak());
                sellerPara.Inlines.Add(new Run($"{seller.Street} {seller.FullHouseNumber}".Trim())); sellerPara.Inlines.Add(new LineBreak());
                sellerPara.Inlines.Add(new Run($"{seller.PostalCode} {seller.City}".Trim())); sellerPara.Inlines.Add(new LineBreak());
                sellerPara.Inlines.Add(new Run(seller.Country ?? string.Empty));
            }
            // buyer
            var buyerPara = new Paragraph();
            buyerPara.LineHeight = 26;
            var buyer = Invoice?.Buyer;
            if (buyer != null)
            {
                buyerPara.Inlines.Add(new Run(buyer.Name ?? string.Empty)); buyerPara.Inlines.Add(new LineBreak());
                buyerPara.Inlines.Add(new Run($"{buyer.Street} {buyer.FullHouseNumber}".Trim())); buyerPara.Inlines.Add(new LineBreak());
                buyerPara.Inlines.Add(new Run($"{buyer.PostalCode} {buyer.City}".Trim())); buyerPara.Inlines.Add(new LineBreak());
                buyerPara.Inlines.Add(new Run(buyer.Country ?? string.Empty));
            }

            contentRow.Cells.Add(new TableCell(sellerPara) { Padding = new Thickness(0, 6, 6, 6) });
            contentRow.Cells.Add(new TableCell(buyerPara) { Padding = new Thickness(6, 6, 0, 6) });
            partiesRg.Rows.Add(contentRow);
            partiesTable.RowGroups.Add(partiesRg);
            InvoiceDocument.Blocks.Add(partiesTable);

            // Invoice items heading
            InvoiceDocument.Blocks.Add(new Paragraph(new Bold(new Run("Invoice items"))) { Margin = new Thickness(0, 18, 0, 20) });

            // Items table (columns: name, qty, unit, net, tax, gross)
            var itemsTable = new Table { CellSpacing = 6 };
            itemsTable.Columns.Add(new TableColumn { Width = new GridLength(3, GridUnitType.Star) }); // name
            itemsTable.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) }); // qty
            itemsTable.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) }); // unit
            itemsTable.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) }); // net
            itemsTable.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) }); // tax
            itemsTable.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) }); // gross

            var itemsRg = new TableRowGroup();
            var headerRow = new TableRow();
            headerRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Name")))));
            headerRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Qty")))) { TextAlignment = TextAlignment.Right });
            headerRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Unit")))) { TextAlignment = TextAlignment.Right });
            headerRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Net")))) { TextAlignment = TextAlignment.Right });
            headerRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Tax")))) { TextAlignment = TextAlignment.Right });
            headerRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Gross")))) { TextAlignment = TextAlignment.Right });
            itemsRg.Rows.Add(headerRow);

            foreach (var it in InvoiceItemsList ?? new List<InvoiceItemsListView>())
            {
                var row = new TableRow();
                row.Cells.Add(new TableCell(new Paragraph(new Run(it.Name ?? string.Empty))));
                row.Cells.Add(new TableCell(new Paragraph(new Run((it.Quantity).ToString()))) { TextAlignment = TextAlignment.Right, LineHeight = 24 });
                row.Cells.Add(new TableCell(new Paragraph(new Run((it.UnitPrice).ToString("F2")))) { TextAlignment = TextAlignment.Right, LineHeight = 24 });
                row.Cells.Add(new TableCell(new Paragraph(new Run((it.NetAmount ?? 0m).ToString("F2")))) { TextAlignment = TextAlignment.Right, LineHeight = 24 });
                row.Cells.Add(new TableCell(new Paragraph(new Run((it.TaxAmount ?? 0m).ToString("F2")))) { TextAlignment = TextAlignment.Right, LineHeight = 24 });
                row.Cells.Add(new TableCell(new Paragraph(new Run((it.GrossAmount ?? 0m).ToString("F2")))) { TextAlignment = TextAlignment.Right, LineHeight = 24 });
                itemsRg.Rows.Add(row);
            }

            // Totals row separated by a horizontal linw
            var spacerRow = new TableRow();
            spacerRow.Cells.Add(new TableCell(new Paragraph(new Run())) { ColumnSpan = 6, BorderBrush = Brushes.LightGray, BorderThickness = new Thickness(0, 1, 0, 0), Padding = new Thickness(0, 6, 0 , 6) });
            itemsRg.Rows.Add(spacerRow);
            var totalsRow = new TableRow();
            totalsRow.Cells.Add(new TableCell(new Paragraph(new Run("Totals"))) { ColumnSpan = 3, TextAlignment = TextAlignment.Right });
            totalsRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run((Invoice?.TotalNet ?? 0m).ToString("F2"))))) { TextAlignment = TextAlignment.Right });
            totalsRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run((Invoice?.TotalTax ?? 0m).ToString("F2"))))) { TextAlignment = TextAlignment.Right });
            totalsRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run((Invoice?.TotalAmount ?? 0m).ToString("F2"))))) { TextAlignment = TextAlignment.Right });
            itemsRg.Rows.Add(totalsRow);

            itemsTable.RowGroups.Add(itemsRg);
            InvoiceDocument.Blocks.Add(itemsTable);
        }
        #endregion
    }
}
