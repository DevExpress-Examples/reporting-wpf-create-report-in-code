#Region "#Reference"
Imports DevExpress.DataAccess.ConnectionParameters
Imports DevExpress.DataAccess.Sql
Imports DevExpress.Xpf.Printing
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraReports.Configuration
Imports DevExpress.XtraReports.UI
Imports System.Drawing
Imports System.Windows
' ...
#End Region ' #Reference

Namespace RuntimeReportsApplication
    ''' <summary>
    ''' Interaction logic for MainWindow.xaml
    ''' </summary>
    Partial Public Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
            CreateReport()
        End Sub

#Region "#CreateReport"
        Public Sub CreateReport()
            ' Create an empty report.
            Dim report As New XtraReport()

            ' Bind the report to a data source.
            BindToData(report)

            ' Create a master report.
            CreateReportHeader(report, "Products by Categories")
            CreateDetail(report)

            ' Create a detail report.
            Dim ds As SqlDataSource = TryCast(report.DataSource, SqlDataSource)
            CreateDetailReport(report, ds.Queries(0).Name & "." & ds.Relations(0).Name)

            ' Publish the report.
            PublishReport(report)
        End Sub
#End Region ' #CreateReport

#Region "#BindToData"
        Private Sub BindToData(ByVal report As XtraReport)
            ' Create a data source.
            Dim connectionParameters As New Access97ConnectionParameters("../../nwind.mdb", "", "")
            Dim ds As New DevExpress.DataAccess.Sql.SqlDataSource(connectionParameters)

            ' Create an SQL query to access the master table.
            Dim queryCategories As New CustomSqlQuery With {
                .Name = "queryCategories",
                .Sql = "SELECT * FROM Categories"
            }

            ' Create an SQL query to access the detail table.
            Dim queryProducts As New CustomSqlQuery With {
                .Name = "queryProducts",
                .Sql = "SELECT * FROM Products"
            }

            ' Add the queries to the data source collection.
            ds.Queries.AddRange(New SqlQuery() {queryCategories, queryProducts})

            ' Create a master-detail relation between the queries.
            ds.Relations.Add("queryCategories", "queryProducts", "CategoryID", "CategoryID")

            ' Assign the data source to the report.
            report.DataSource = ds
            report.DataMember = "queryCategories"
        End Sub

#End Region ' #BindToData
#Region "#CreateMasterReport"
        Private Sub CreateReportHeader(ByVal report As XtraReport, ByVal caption As String)
            ' Create a report title.
            Dim label As New XRLabel()
            label.Font = New Font("Tahoma", 12, System.Drawing.FontStyle.Bold)
            label.Text = caption
            label.WidthF = 300.0F

            ' Create a report header and add the title to it.
            Dim reportHeader As New ReportHeaderBand()
            report.Bands.Add(reportHeader)
            reportHeader.Controls.Add(label)
            reportHeader.HeightF = label.HeightF
        End Sub

        Private Sub CreateDetail(ByVal report As XtraReport)
            ' Create a new label with the required settings. bound to the CategoryName data field.
            Dim labelDetail As New XRLabel()
            labelDetail.Font = New Font("Tahoma", 10, System.Drawing.FontStyle.Bold)
            labelDetail.WidthF = 300.0F

            ' Bind the label to the CategoryName data field depending on the report's data binding mode.
            If Settings.Default.UserDesignerOptions.DataBindingMode = DataBindingMode.Bindings Then
                labelDetail.DataBindings.Add("Text", report.DataSource, "queryCategories.CategoryName", "Category: {0}")
            Else
                labelDetail.ExpressionBindings.Add(New ExpressionBinding("BeforePrint", "Text", "'Category: ' + [CategoryName]"))
            End If

            ' Create a detail band and display the category name in it.
            Dim detailBand As New DetailBand()
            detailBand.Height = labelDetail.Height
            detailBand.KeepTogetherWithDetailReports = True
            report.Bands.Add(detailBand)
            labelDetail.TopF = detailBand.LocationFloat.Y + 20.0F
            detailBand.Controls.Add(labelDetail)
        End Sub
#End Region ' #CreateMasterReport

#Region "#CreateDetailReport"
        Private Sub CreateDetailReport(ByVal report As XtraReport, ByVal dataMember As String)
            ' Create a detail report band and bind it to data.
            Dim detailReportBand As New DetailReportBand()
            report.Bands.Add(detailReportBand)
            detailReportBand.DataSource = report.DataSource
            detailReportBand.DataMember = dataMember

            ' Add a header to the detail report.
            Dim detailReportHeader As New ReportHeaderBand()
            detailReportBand.Bands.Add(detailReportHeader)

            Dim tableHeader As New XRTable()
            tableHeader.BeginInit()
            tableHeader.Rows.Add(New XRTableRow())
            tableHeader.Borders = BorderSide.All
            tableHeader.BorderColor = Color.DarkGray
            tableHeader.Font = New Font("Tahoma", 10, System.Drawing.FontStyle.Bold)
            tableHeader.Padding = 10
            tableHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft

            Dim cellHeader1 As New XRTableCell()
            cellHeader1.Text = "Product Name"
            Dim cellHeader2 As New XRTableCell()
            cellHeader2.Text = "Unit Price"
            cellHeader2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight

            tableHeader.Rows(0).Cells.AddRange(New XRTableCell() {cellHeader1, cellHeader2})
            detailReportHeader.Height = tableHeader.Height
            detailReportHeader.Controls.Add(tableHeader)

            ' Adjust the table width.
            AddHandler tableHeader.BeforePrint, AddressOf tableHeader_BeforePrint
            tableHeader.EndInit()

            ' Create a detail band.
            Dim tableDetail As New XRTable()
            tableDetail.BeginInit()
            tableDetail.Rows.Add(New XRTableRow())
            tableDetail.Borders = BorderSide.Left Or BorderSide.Right Or BorderSide.Bottom
            tableDetail.BorderColor = Color.DarkGray
            tableDetail.Font = New Font("Tahoma", 10)
            tableDetail.Padding = 10
            tableDetail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft

            Dim cellDetail1 As New XRTableCell()
            Dim cellDetail2 As New XRTableCell()
            cellDetail2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
            If Settings.Default.UserDesignerOptions.DataBindingMode = DataBindingMode.Bindings Then
                cellDetail1.DataBindings.Add("Text", report.DataSource, dataMember & ".ProductName")
                cellDetail2.DataBindings.Add("Text", report.DataSource, dataMember & ".UnitPrice", "{0:$0.00}")
            Else
                cellDetail1.ExpressionBindings.Add(New ExpressionBinding("BeforePrint", "Text", "[ProductName]"))
                cellDetail2.ExpressionBindings.Add(New ExpressionBinding("BeforePrint", "Text", "FormatString('{0:$0.00}', [UnitPrice])"))
            End If

            tableDetail.Rows(0).Cells.AddRange(New XRTableCell() {cellDetail1, cellDetail2})

            Dim detailBand As New DetailBand()
            detailBand.Height = tableDetail.Height
            detailReportBand.Bands.Add(detailBand)
            detailBand.Controls.Add(tableDetail)

            ' Adjust the table width.
            AddHandler tableDetail.BeforePrint, AddressOf tableDetail_BeforePrint
            tableDetail.EndInit()

            ' Create and assign different odd and even styles.
            Dim oddStyle As New XRControlStyle()
            Dim evenStyle As New XRControlStyle()

            oddStyle.BackColor = Color.WhiteSmoke
            oddStyle.StyleUsing.UseBackColor = True
            oddStyle.Name = "OddStyle"

            evenStyle.BackColor = Color.White
            evenStyle.StyleUsing.UseBackColor = True
            evenStyle.Name = "EvenStyle"

            report.StyleSheet.AddRange(New XRControlStyle() {oddStyle, evenStyle})

            tableDetail.OddStyleName = "OddStyle"
            tableDetail.EvenStyleName = "EvenStyle"
        End Sub

        Private Sub AdjustTableWidth(ByVal table As XRTable)
            Dim report As XtraReport = table.RootReport
            table.WidthF = report.PageWidth - report.Margins.Left - report.Margins.Right
        End Sub

        Private Sub tableHeader_BeforePrint(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
            AdjustTableWidth(TryCast(sender, XRTable))
        End Sub

        Private Sub tableDetail_BeforePrint(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
            AdjustTableWidth(TryCast(sender, XRTable))
        End Sub
#End Region ' #CreateDetailReport

        Private Sub PublishReport(ByVal report As XtraReport)
            PrintHelper.ShowRibbonPrintPreview(Me, report)
        End Sub

    End Class
End Namespace
