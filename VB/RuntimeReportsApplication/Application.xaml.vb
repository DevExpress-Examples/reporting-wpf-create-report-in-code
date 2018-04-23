Imports System
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Windows

Namespace RuntimeReportsApplication
    ''' <summary>
    ''' Interaction logic for App.xaml
    ''' </summary>
    Partial Public Class App
        Inherits Application

        Protected Overrides Sub OnStartup(ByVal e As StartupEventArgs)
            'Uncomment this line to switch to the legacy binding mode. 
            'DevExpress.XtraReports.Configuration.Settings.Default.UserDesignerOptions.DataBindingMode =
            '    DevExpress.XtraReports.UI.DataBindingMode.Bindings;
            MyBase.OnStartup(e)
        End Sub

    End Class
End Namespace
