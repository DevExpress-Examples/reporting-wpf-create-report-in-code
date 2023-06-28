Imports System.Windows

Namespace RuntimeReportsApplication

    ''' <summary>
    ''' Interaction logic for App.xaml
    ''' </summary>
    Public Partial Class App
        Inherits Application

        Protected Overrides Sub OnStartup(ByVal e As StartupEventArgs)
            'Uncomment this line to switch to the legacy binding mode. 
            'DevExpress.XtraReports.Configuration.Settings.Default.UserDesignerOptions.DataBindingMode =
            '    DevExpress.XtraReports.UI.DataBindingMode.Bindings;
            MyBase.OnStartup(e)
        End Sub
    End Class
End Namespace
