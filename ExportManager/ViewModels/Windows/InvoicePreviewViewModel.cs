using ExportManager.Models;
using ExportManager.Models.BusinessLogic.Queries;
using ExportManager.Models.DTO;
using ExportManager.Models.EntitiesForView;
using ExportManager.Models.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Markup;
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
        private FixedDocument _FixedInvoiceDocument;
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
                if (_InvoiceDocument == null)
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
        public FixedDocument FixedInvoiceDocument
        {
            get
            {
                if (_FixedInvoiceDocument == null)
                    _FixedInvoiceDocument = new FixedDocument();
                return _FixedInvoiceDocument;
            }
            set
            {
                if (value != _FixedInvoiceDocument)
                {
                    _FixedInvoiceDocument = value;
                    OnPropertyChanged(() => FixedInvoiceDocument);
                }
            }
        }
        public List<InvoiceItemsListView> InvoiceItemsList
        {
            get { return _InvoiceItemsList; }
            set
            {
                if (_InvoiceItemsList != value)
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
                ColumnWidth = 793.7 - 40,
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

            // Invoice items header
            InvoiceDocument.Blocks.Add(new Paragraph(new Bold(new Run("Invoice items"))) { Margin = new Thickness(0, 18, 0, 20) });

            // Items table (columns: name, qty, unit, net, tax, gross)
            var itemsTable = new Table { CellSpacing = 6, BreakPageBefore = false };

            itemsTable.Columns.Add(new TableColumn { Width = new GridLength(3, GridUnitType.Star) }); // name
            itemsTable.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) }); // qty
            itemsTable.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) }); // unit
            itemsTable.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) }); // net
            itemsTable.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) }); // tax
            itemsTable.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) }); // gross

            var headerGroup = new TableRowGroup();
            var headerRow = new TableRow();

            headerRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Name"))) { TextAlignment = TextAlignment.Left })); // name
            headerRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Qty"))) { TextAlignment = TextAlignment.Right })); // name
            headerRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Unit"))) { TextAlignment = TextAlignment.Right })); // name
            headerRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Net"))) { TextAlignment = TextAlignment.Right })); // name
            headerRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Tax"))) { TextAlignment = TextAlignment.Right })); // name
            headerRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Gross"))) { TextAlignment = TextAlignment.Right })); // name

            headerGroup.Rows.Add(headerRow);

            var spacerRowTop = new TableRow();
            spacerRowTop.Cells.Add(new TableCell(new Paragraph(new Run())) { ColumnSpan = 6, LineHeight = 5 });
            headerGroup.Rows.Add(spacerRowTop);

            itemsTable.RowGroups.Add(headerGroup);

            var bodyGroup = new TableRowGroup();

            foreach (var it in InvoiceItemsList ?? new List<InvoiceItemsListView>())
            {
                var row = new TableRow();
                row.Cells.Add(new TableCell(new Paragraph(new Run(it.Name ?? string.Empty))));
                row.Cells.Add(new TableCell(new Paragraph(new Run((it.Quantity).ToString()))) { TextAlignment = TextAlignment.Right, LineHeight = 24 });
                row.Cells.Add(new TableCell(new Paragraph(new Run((it.UnitPrice).ToString("F2")))) { TextAlignment = TextAlignment.Right, LineHeight = 24 });
                row.Cells.Add(new TableCell(new Paragraph(new Run((it.NetAmount ?? 0m).ToString("F2")))) { TextAlignment = TextAlignment.Right, LineHeight = 24 });
                row.Cells.Add(new TableCell(new Paragraph(new Run((it.TaxAmount ?? 0m).ToString("F2")))) { TextAlignment = TextAlignment.Right, LineHeight = 24 });
                row.Cells.Add(new TableCell(new Paragraph(new Run((it.GrossAmount ?? 0m).ToString("F2")))) { TextAlignment = TextAlignment.Right, LineHeight = 24 });
                bodyGroup.Rows.Add(row);
            }

            // Totals row separated by a horizontal linw
            var spacerRowBottom = new TableRow();
            spacerRowBottom.Cells.Add(new TableCell(new Paragraph(new Run())) { ColumnSpan = 6, BorderBrush = Brushes.LightGray, BorderThickness = new Thickness(0, 1, 0, 0), Padding = new Thickness(0, 6, 0, 6) });
            bodyGroup.Rows.Add(spacerRowBottom);

            var totalsRow = new TableRow();
            totalsRow.Cells.Add(new TableCell(new Paragraph(new Run("Totals"))) { ColumnSpan = 3, TextAlignment = TextAlignment.Right });
            totalsRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run((Invoice?.TotalNet ?? 0m).ToString("F2"))))) { TextAlignment = TextAlignment.Right });
            totalsRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run((Invoice?.TotalTax ?? 0m).ToString("F2"))))) { TextAlignment = TextAlignment.Right });
            totalsRow.Cells.Add(new TableCell(new Paragraph(new Bold(new Run((Invoice?.TotalAmount ?? 0m).ToString("F2"))))) { TextAlignment = TextAlignment.Right });
            
            bodyGroup.Rows.Add(totalsRow);

            itemsTable.RowGroups.Add(bodyGroup);
            InvoiceDocument.Blocks.Add(itemsTable);

            //fixed document test
            FixedInvoiceDocument = ConvertToFixedDocument(InvoiceDocument, InvoiceDocument.PageWidth, InvoiceDocument.PageHeight);
        }
        private FixedDocument ConvertToFixedDocument(FlowDocument flowDoc, double pageWidth, double pageHeight)
        {
            var fixedDoc = new FixedDocument();
            // page settings
            double headerHeight = 172;
            var padding = flowDoc.PagePadding;
            double contentTop = headerHeight + padding.Top;
            double contentHeight = pageHeight - contentTop - padding.Bottom;
            //double contentWidth = pageWidth - padding.Left - padding.Right;

            // create paginator
            var paginator = ((IDocumentPaginatorSource)flowDoc).DocumentPaginator;

            paginator.PageSize = new Size(pageWidth, pageHeight);
            paginator.ComputePageCount();

            string invoiceNo = Invoice?.InvoiceNo ?? string.Empty;
            string invoiceDate = Invoice == null ? string.Empty : Invoice.InvoiceDate.ToString("d");


            for (int i = 0; i < paginator.PageCount; i++)
            {
                var fixedPage = new FixedPage
                {
                    Width = pageWidth,
                    Height = pageHeight,
                    Background = Brushes.White
                };

                //header for pages > 1
                if (i > 0)
                {
                    var headerPanel = GetHeader(invoiceNo, invoiceDate, pageWidth, pageHeight);
                    FixedPage.SetLeft(headerPanel, 0);
                    FixedPage.SetTop(headerPanel, 0);
                    fixedPage.Children.Add(headerPanel);
                }

                // Render paginator page visual into the content area using a VisualBrush
                var page = paginator.GetPage(i);
                var visual = page.Visual;

                var contentRect = new System.Windows.Shapes.Rectangle
                {
                    Width = pageWidth,
                    // setting height depending on page number
                    Height = i == 0 ? Math.Max(0, contentHeight + contentTop) : Math.Max(0, contentHeight),
                    Fill = new VisualBrush(visual) { Stretch = Stretch.None, AlignmentX = AlignmentX.Left, AlignmentY = AlignmentY.Top },
                };

                FixedPage.SetTop(contentRect, i == 0 ? 10 : contentTop);
                fixedPage.Children.Add(contentRect);

                var pageContent = new PageContent();
                ((IAddChild)pageContent).AddChild(fixedPage);
                fixedDoc.Pages.Add(pageContent);
            }

            return fixedDoc;
        }
        private StackPanel GetHeader(string invoiceNo, string invoiceDate, double pageWidth, double pageHeight)
        {
            var headerPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Width = pageWidth - 40,
                Margin = new Thickness(20, 20, 20, 0),
            };
            var title = new TextBlock
            {
                Text = "Invoice",
                FontSize = 22,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Right,
                FontFamily = new FontFamily("Georgia"),
                Background = Brushes.AliceBlue,
                LineHeight = 40,
                LineStackingStrategy = LineStackingStrategy.BlockLineHeight
            };
            headerPanel.Children.Add(title);
            var invoiceNoTextBlock = new TextBlock
            {
                Text = $"Invoice No: {invoiceNo}",
                FontSize = 16,
                FontFamily = new FontFamily("Georgia"),
                LineHeight = 26,
                TextAlignment = TextAlignment.Right,
                Margin = new Thickness(0, 30, 0, 0)
            };
            var invoiceDateTextBlock = new TextBlock
            {
                Text = $"Invoice Date: {invoiceDate}",
                FontSize = 16,
                FontFamily = new FontFamily("Georgia"),
                LineHeight = 26,
                TextAlignment = TextAlignment.Right,
                Margin = new Thickness(0, 0, 0, 30)
            };
            headerPanel.Children.Add(invoiceNoTextBlock);
            headerPanel.Children.Add(invoiceDateTextBlock);
            var tableHeaderGrid = new Grid
            {
                Width = pageWidth,
                //Margin = new Thickness(4, 8, 0, 5)
            };
            tableHeaderGrid.ColumnDefinitions.Clear();
            var totalWidth = pageWidth;
            tableHeaderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(totalWidth * 3 / 8) }); // Name
            tableHeaderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(totalWidth * 1 / 8) }); // Qty
            tableHeaderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(totalWidth * 1 / 8) }); // Unit
            tableHeaderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(totalWidth * 1 / 8) }); // Net
            tableHeaderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(totalWidth * 1 / 8) }); // Tax
            tableHeaderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(totalWidth * 1 / 8) }); // Gross

            void AddHeaderCell(string text, int col, Thickness margin, TextAlignment alignment = TextAlignment.Left)
            {
                var tb = new TextBlock
                {
                    Text = text,
                    FontWeight = FontWeights.Bold,
                    FontSize = 16,
                    FontFamily = new FontFamily("Georgia"),
                    TextAlignment = alignment,
                    Margin = margin,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetColumn(tb, col);
                tableHeaderGrid.Children.Add(tb);
            }

            AddHeaderCell("Name", 0, new Thickness(5, 0, 0, 0));
            AddHeaderCell("Qty", 1, new Thickness(0, 0, 30, 0), TextAlignment.Right);
            AddHeaderCell("Unit", 2, new Thickness(30, 0, 0, 0));
            AddHeaderCell("Net", 3, new Thickness(30, 0, 0, 0));
            AddHeaderCell("Tax", 4, new Thickness(30, 0, 0, 0));
            AddHeaderCell("Gross", 5, new Thickness(10, 0, 0, 0));

            headerPanel.Children.Add(tableHeaderGrid);
            return headerPanel;
        }
        #endregion
    }
}
