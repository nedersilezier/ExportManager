using ExportManager.Models;
using ExportManager.Models.DTO;
using ExportManager.Models.EntitiesForView;
using ExportManager.Models.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

namespace ExportManager.ViewModels.Windows
{
    public class InvoicePreviewViewModel : BaseViewModel, IParameterReceiver<InvoiceParameter>
    {
        #region Fields
        private readonly PotplantsEntities potplantsEntities;
        private InvoicesListView _Invoice;
        private List<InvoiceItemsListView> _InvoiceItemsList;
        private FixedDocument _FixedInvoiceDocument;
        #endregion

        #region Constants
        private const double PageWidth = 793.7;
        private const double PageHeight = 1122.5;
        private const double PageMargin = 20;
        private const double ContentWidth = PageWidth - PageMargin * 2;
        private const double RowHeight = 26;
        private const double DefaultFontSize = 14;
        private const double TitleFontSize = 22;
        private const double TitleLineHeight = 40;
        private const double FooterLineHeight = 20;
        private static readonly FontFamily DocumentFont = new FontFamily("Georgia");
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
            var fixedDoc = new FixedDocument();
            string invoiceNo = Invoice?.InvoiceNo ?? string.Empty;
            string invoiceDate = Invoice == null ? string.Empty : Invoice.InvoiceDate.ToString("d");
            string paymentDate = (Invoice?.PaymentDate.HasValue == true)
                ? Invoice.PaymentDate.Value.ToString("d")
                : string.Empty;

            var items = InvoiceItemsList ?? new List<InvoiceItemsListView>();

            int itemIndex = 0;
            int pageNumber = 0;
            bool totalsPlaced = false;

            do
            {
                var fixedPage = new FixedPage
                {
                    Width = PageWidth,
                    Height = PageHeight,
                    Background = Brushes.White
                };

                double y = PageMargin;

                // Page header — full on page 1, compact on pages > 1
                if (pageNumber == 0)
                    y = AddFirstPageHeader(fixedPage, y, invoiceNo, invoiceDate, paymentDate);
                else
                    y = AddContinuationHeader(fixedPage, y, invoiceNo, invoiceDate);

                // Table column headers + separator line
                y = AddTableColumnHeaders(fixedPage, y);
                y = AddHorizontalLine(fixedPage, y, Brushes.Black);
                y += 6;

                // Item rows
                while (itemIndex < items.Count)
                {
                    double spaceNeeded = RowHeight;

                    // If this is the last item, reserve room for the separator, totals row and page number
                    if (itemIndex == items.Count - 1)
                        spaceNeeded += RowHeight + 12 + FooterLineHeight;

                    if (y + spaceNeeded > PageHeight - PageMargin - FooterLineHeight)
                    {
                        AddFooter(fixedPage, PageHeight - PageMargin - FooterLineHeight, pageNumber + 1);
                        break;
                    }
                        
                    //if (y + spaceNeeded > PageHeight - PageMargin)
                    //    break;

                    y = AddItemRow(fixedPage, y, items[itemIndex]);
                    itemIndex++;
                }

                // Place totals once all items have been laid out
                if (itemIndex >= items.Count && !totalsPlaced)
                {
                    y = AddHorizontalLine(fixedPage, y, Brushes.LightGray);
                    y += 6;
                    y = AddTotalsRow(fixedPage, y);

                    //add page number at the bottom of the last pafe
                    while (PageHeight - PageMargin - y > FooterLineHeight)
                        y++;
                    AddFooter(fixedPage, PageHeight - PageMargin - FooterLineHeight, pageNumber + 1);
                    totalsPlaced = true;
                }

                var pageContent = new PageContent();
                ((IAddChild)pageContent).AddChild(fixedPage);
                fixedDoc.Pages.Add(pageContent);
                pageNumber++;
            }
            while (!totalsPlaced);

            FixedInvoiceDocument = fixedDoc;
        }
        #endregion

        #region Document Building Helpers
        private double AddFirstPageHeader(FixedPage page, double y, string invoiceNo, string invoiceDate, string paymentDate)
        {
            // Title
            var title = CreateTextBlock("Invoice", TitleFontSize, bold: true, alignment: TextAlignment.Right, width: ContentWidth);
            title.Background = Brushes.AliceBlue;
            title.LineHeight = TitleLineHeight;
            title.LineStackingStrategy = LineStackingStrategy.BlockLineHeight;
            AddToPage(page, title, PageMargin, y);
            y += TitleLineHeight;

            // Invoice info (right-aligned)
            y += 30;
            AddToPage(page, CreateTextBlock($"Invoice No: {invoiceNo}", alignment: TextAlignment.Right, width: ContentWidth), PageMargin, y);
            y += RowHeight;
            AddToPage(page, CreateTextBlock($"Invoice Date: {invoiceDate}", alignment: TextAlignment.Right, width: ContentWidth), PageMargin, y);
            y += RowHeight;
            AddToPage(page, CreateTextBlock($"Payment Date: {paymentDate}", alignment: TextAlignment.Right, width: ContentWidth), PageMargin, y);
            y += RowHeight + 30;

            // From / To headings
            double colWidth = ContentWidth / 2;
            AddToPage(page, CreateTextBlock("From", bold: true, width: colWidth), PageMargin, y);
            AddToPage(page, CreateTextBlock("To", bold: true, width: colWidth), PageMargin + colWidth, y);
            y += RowHeight + 6;

            // Seller address
            double sellerY = y;
            var seller = Invoice?.Seller;
            if (seller != null)
            {
                AddToPage(page, CreateTextBlock(seller.Name ?? string.Empty, width: colWidth), PageMargin, sellerY);
                sellerY += RowHeight;
                AddToPage(page, CreateTextBlock($"{seller.Street} {seller.FullHouseNumber}".Trim(), width: colWidth), PageMargin, sellerY);
                sellerY += RowHeight;
                AddToPage(page, CreateTextBlock($"{seller.PostalCode} {seller.City}".Trim(), width: colWidth), PageMargin, sellerY);
                sellerY += RowHeight;
                AddToPage(page, CreateTextBlock(seller.Country ?? string.Empty, width: colWidth), PageMargin, sellerY);
                sellerY += RowHeight;
            }

            // Buyer address
            double buyerY = y;
            var buyer = Invoice?.Buyer;
            if (buyer != null)
            {
                AddToPage(page, CreateTextBlock(buyer.Name ?? string.Empty, width: colWidth), PageMargin + colWidth, buyerY);
                buyerY += RowHeight;
                AddToPage(page, CreateTextBlock($"{buyer.Street} {buyer.FullHouseNumber}".Trim(), width: colWidth), PageMargin + colWidth, buyerY);
                buyerY += RowHeight;
                AddToPage(page, CreateTextBlock($"{buyer.PostalCode} {buyer.City}".Trim(), width: colWidth), PageMargin + colWidth, buyerY);
                buyerY += RowHeight;
                AddToPage(page, CreateTextBlock(buyer.Country ?? string.Empty, width: colWidth), PageMargin + colWidth, buyerY);
                buyerY += RowHeight;
            }

            y = Math.Max(sellerY, buyerY) + 50;

            // "Invoice items" section heading
            AddToPage(page, CreateTextBlock("Invoice items", bold: true), PageMargin, y);
            y += RowHeight + 20;

            return y;
        }

        private double AddContinuationHeader(FixedPage page, double y, string invoiceNo, string invoiceDate)
        {
            // Title
            var title = CreateTextBlock("Invoice", TitleFontSize, bold: true, alignment: TextAlignment.Right, width: ContentWidth);
            title.Background = Brushes.AliceBlue;
            title.LineHeight = TitleLineHeight;
            title.LineStackingStrategy = LineStackingStrategy.BlockLineHeight;
            AddToPage(page, title, PageMargin, y);
            y += TitleLineHeight;

            // Compact info
            y += 30;
            AddToPage(page, CreateTextBlock($"Invoice No: {invoiceNo}", alignment: TextAlignment.Right, width: ContentWidth), PageMargin, y);
            y += RowHeight;
            AddToPage(page, CreateTextBlock($"Invoice Date: {invoiceDate}", alignment: TextAlignment.Right, width: ContentWidth), PageMargin, y);
            y += RowHeight + 30;

            return y;
        }
        private double AddFooter(FixedPage page, double y, int pageNumber)
        {
            var footerText = CreateTextBlock($"Page {pageNumber}", bold: true, alignment: TextAlignment.Right, width: ContentWidth);
            AddToPage(page, footerText, PageMargin, y);
            return y + RowHeight;
        }
        private double AddTableColumnHeaders(FixedPage page, double y)
        {
            var grid = CreateTableRow(
                ("Name", TextAlignment.Left, true),
                ("Qty", TextAlignment.Right, true),
                ("Unit", TextAlignment.Right, true),
                ("Net", TextAlignment.Right, true),
                ("Tax", TextAlignment.Right, true),
                ("Gross", TextAlignment.Right, true));
            AddToPage(page, grid, PageMargin, y);
            return y + RowHeight + 5;
        }

        private double AddItemRow(FixedPage page, double y, InvoiceItemsListView item)
        {
            var grid = CreateTableRow(
                (item.Name ?? string.Empty, TextAlignment.Left, false),
                (item.Quantity.ToString(), TextAlignment.Right, false),
                (item.UnitPrice.ToString("F2"), TextAlignment.Right, false),
                ((item.NetAmount ?? 0m).ToString("F2"), TextAlignment.Right, false),
                ((item.TaxAmount ?? 0m).ToString("F2"), TextAlignment.Right, false),
                ((item.GrossAmount ?? 0m).ToString("F2"), TextAlignment.Right, false));
            AddToPage(page, grid, PageMargin, y);
            return y + RowHeight;
        }

        private double AddTotalsRow(FixedPage page, double y)
        {
            var grid = new Grid { Width = ContentWidth };
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var totalsLabel = CreateTextBlock("Totals", bold: true, alignment: TextAlignment.Right);
            Grid.SetColumn(totalsLabel, 0);
            Grid.SetColumnSpan(totalsLabel, 3);
            grid.Children.Add(totalsLabel);

            var netTotal = CreateTextBlock((Invoice?.TotalNet ?? 0m).ToString("F2"), bold: true, alignment: TextAlignment.Right);
            Grid.SetColumn(netTotal, 3);
            grid.Children.Add(netTotal);

            var taxTotal = CreateTextBlock((Invoice?.TotalTax ?? 0m).ToString("F2"), bold: true, alignment: TextAlignment.Right);
            Grid.SetColumn(taxTotal, 4);
            grid.Children.Add(taxTotal);

            var grossTotal = CreateTextBlock((Invoice?.TotalAmount ?? 0m).ToString("F2"), bold: true, alignment: TextAlignment.Right);
            Grid.SetColumn(grossTotal, 5);
            grid.Children.Add(grossTotal);

            AddToPage(page, grid, PageMargin, y);
            return y + RowHeight;
        }

        private double AddHorizontalLine(FixedPage page, double y, Brush color)
        {
            var line = new Border
            {
                Width = ContentWidth,
                Height = 1,
                Background = color,
                VerticalAlignment = VerticalAlignment.Top
            };
            AddToPage(page, line, PageMargin, y);
            return y + 1;
        }

        private Grid CreateTableRow(params (string text, TextAlignment align, bool bold)[] cells)
        {
            var grid = new Grid { Width = ContentWidth };
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            for (int i = 0; i < cells.Length && i < 6; i++)
            {
                var tb = CreateTextBlock(cells[i].text, bold: cells[i].bold, alignment: cells[i].align);
                Grid.SetColumn(tb, i);
                grid.Children.Add(tb);
            }

            return grid;
        }

        private TextBlock CreateTextBlock(double fontSize = DefaultFontSize, bool bold = false,
            TextAlignment alignment = TextAlignment.Left, double width = 0, string text = "")
        {
            var tb = new TextBlock
            {
                Text = text,
                FontSize = fontSize,
                FontFamily = DocumentFont,
                FontWeight = bold ? FontWeights.Bold : FontWeights.Normal,
                TextAlignment = alignment,
                LineHeight = RowHeight
            };
            if (width > 0)
                tb.Width = width;
            return tb;
        }

        private TextBlock CreateTextBlock(string text, double fontSize = DefaultFontSize, bool bold = false,
            TextAlignment alignment = TextAlignment.Left, double width = 0)
        {
            return CreateTextBlock(fontSize, bold, alignment, width, text);
        }

        private void AddToPage(FixedPage page, UIElement element, double left, double top)
        {
            FixedPage.SetLeft(element, left);
            FixedPage.SetTop(element, top);
            page.Children.Add(element);
        }
        #endregion
    }
}
