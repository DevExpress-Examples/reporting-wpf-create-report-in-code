#region #Reference
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Sql;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.Configuration;
using DevExpress.XtraReports.UI;
using System.Drawing;
using System.Windows;
// ...
#endregion #Reference

namespace RuntimeReportsApplication {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        #region #CreateReport
        public XtraReport CreateReport() {
            // Create an empty report.
            XtraReport report = new XtraReport();

            // Bind the report to a data source.
            BindToData(report);

            // Create a master report.
            CreateReportHeader(report, "Products by Categories");
            CreateDetail(report);

            // Create a detail report.
            SqlDataSource ds = report.DataSource as SqlDataSource;
            CreateDetailReport(report, ds.Queries[0].Name + "." + ds.Relations[0].Name);

            return report;
        }
        #endregion #CreateReport

        #region #BindToData
        private void BindToData(XtraReport report) {
            // Create a data source.
            Access97ConnectionParameters connectionParameters = new Access97ConnectionParameters("../../nwind.mdb", "", "");
            DevExpress.DataAccess.Sql.SqlDataSource ds = new DevExpress.DataAccess.Sql.SqlDataSource(connectionParameters);

            // Create an SQL query to access the master table.
            CustomSqlQuery queryCategories = new CustomSqlQuery();
            queryCategories.Name = "queryCategories";
            queryCategories.Sql = "SELECT * FROM Categories";

            // Create an SQL query to access the detail table.
            CustomSqlQuery queryProducts = new CustomSqlQuery();
            queryProducts.Name = "queryProducts";
            queryProducts.Sql = "SELECT * FROM Products";

            // Add the queries to the data source collection.
            ds.Queries.AddRange(new SqlQuery[] { queryCategories, queryProducts });

            // Create a master-detail relation between the queries.
            ds.Relations.Add("queryCategories", "queryProducts", "CategoryID", "CategoryID");

            // Assign the data source to the report.
            report.DataSource = ds;
            report.DataMember = "queryCategories";
        }
        #endregion #BindToData

        #region #CreateMasterReport
        private void CreateReportHeader(XtraReport report, string caption) {
            // Create a report title.
            XRLabel label = new XRLabel();
            label.Font = new Font("Tahoma", 12, System.Drawing.FontStyle.Bold);
            label.Text = caption;
            label.WidthF = 300F;

            // Create a report header and add the title to it.
            ReportHeaderBand reportHeader = new ReportHeaderBand();
            report.Bands.Add(reportHeader);
            reportHeader.Controls.Add(label);
            reportHeader.HeightF = label.HeightF;
        }

        private void CreateDetail(XtraReport report) {
            // Create a new label with the required settings. bound to the CategoryName data field.
            XRLabel labelDetail = new XRLabel();
            labelDetail.Font = new Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
            labelDetail.WidthF = 300F;

            // Bind the label to the CategoryName data field depending on the report's data binding mode.
            if (Settings.Default.UserDesignerOptions.DataBindingMode == DataBindingMode.Bindings)
                labelDetail.DataBindings.Add("Text", report.DataSource, "queryCategories.CategoryName", "Category: {0}");
            else labelDetail.ExpressionBindings.Add(
                new ExpressionBinding("BeforePrint", "Text", "'Category: ' + [CategoryName]"));

            // Create a detail band and display the category name in it.
            DetailBand detailBand = new DetailBand();
            detailBand.Height = labelDetail.Height;
            detailBand.KeepTogetherWithDetailReports = true;
            report.Bands.Add(detailBand);
            labelDetail.TopF = detailBand.LocationFloat.Y + 20F;
            detailBand.Controls.Add(labelDetail);
        }
        #endregion #CreateMasterReport

        #region #CreateDetailReport
        private void CreateDetailReport(XtraReport report, string dataMember) {
            // Create a detail report band and bind it to data.
            DetailReportBand detailReportBand = new DetailReportBand();
            report.Bands.Add(detailReportBand);
            detailReportBand.DataSource = report.DataSource;
            detailReportBand.DataMember = dataMember;

            // Add a header to the detail report.
            ReportHeaderBand detailReportHeader = new ReportHeaderBand();
            detailReportBand.Bands.Add(detailReportHeader);

            XRTable tableHeader = new XRTable();
            tableHeader.BeginInit();
            tableHeader.Rows.Add(new XRTableRow());
            tableHeader.Borders = BorderSide.All;
            tableHeader.BorderColor = Color.DarkGray;
            tableHeader.Font = new Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
            tableHeader.Padding = 10;
            tableHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;

            XRTableCell cellHeader1 = new XRTableCell();
            cellHeader1.Text = "Product Name";
            XRTableCell cellHeader2 = new XRTableCell();
            cellHeader2.Text = "Unit Price";
            cellHeader2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;

            tableHeader.Rows[0].Cells.AddRange(new XRTableCell[] { cellHeader1, cellHeader2 });
            detailReportHeader.Height = tableHeader.Height;
            detailReportHeader.Controls.Add(tableHeader);

            // Adjust the table width.
            tableHeader.BeforePrint += tableHeader_BeforePrint;
            tableHeader.EndInit();

            // Create a detail band.
            XRTable tableDetail = new XRTable();
            tableDetail.BeginInit();
            tableDetail.Rows.Add(new XRTableRow());
            tableDetail.Borders = BorderSide.Left | BorderSide.Right | BorderSide.Bottom;
            tableDetail.BorderColor = Color.DarkGray;
            tableDetail.Font = new Font("Tahoma", 10);
            tableDetail.Padding = 10;
            tableDetail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;

            XRTableCell cellDetail1 = new XRTableCell();
            XRTableCell cellDetail2 = new XRTableCell();
            cellDetail2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            if (Settings.Default.UserDesignerOptions.DataBindingMode == DataBindingMode.Bindings) {
                cellDetail1.DataBindings.Add("Text", report.DataSource, dataMember + ".ProductName");
                cellDetail2.DataBindings.Add("Text", report.DataSource, dataMember + ".UnitPrice", "{0:$0.00}");
            } else {
                cellDetail1.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[ProductName]"));
                cellDetail2.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", 
                    "FormatString('{0:$0.00}', [UnitPrice])"));
            }    

            tableDetail.Rows[0].Cells.AddRange(new XRTableCell[] { cellDetail1, cellDetail2 });

            DetailBand detailBand = new DetailBand();
            detailBand.Height = tableDetail.Height;
            detailReportBand.Bands.Add(detailBand);
            detailBand.Controls.Add(tableDetail);

            // Adjust the table width.
            tableDetail.BeforePrint += tableDetail_BeforePrint;
            tableDetail.EndInit();

            // Create and assign different odd and even styles.
            XRControlStyle oddStyle = new XRControlStyle();
            XRControlStyle evenStyle = new XRControlStyle();

            oddStyle.BackColor = Color.WhiteSmoke;
            oddStyle.StyleUsing.UseBackColor = true;
            oddStyle.Name = "OddStyle";

            evenStyle.BackColor = Color.White;
            evenStyle.StyleUsing.UseBackColor = true;
            evenStyle.Name = "EvenStyle";

            report.StyleSheet.AddRange(new XRControlStyle[] { oddStyle, evenStyle });

            tableDetail.OddStyleName = "OddStyle";
            tableDetail.EvenStyleName = "EvenStyle";
        }

        private void AdjustTableWidth(XRTable table) {
            XtraReport report = table.RootReport;
            table.WidthF = report.PageWidth - report.Margins.Left - report.Margins.Right;
        }

        void tableHeader_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e) {
            AdjustTableWidth(sender as XRTable);
        }

        void tableDetail_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e) {
            AdjustTableWidth(sender as XRTable);
        }
        #endregion #CreateDetailReport

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            XtraReport report = CreateReport();
            PrintHelper.ShowRibbonPrintPreview(this, report);
        }
    }
}
