using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RuntimeReportsApplication {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            //Uncomment this line to switch to the legacy binding mode. 
            //DevExpress.XtraReports.Configuration.Settings.Default.UserDesignerOptions.DataBindingMode =
            //    DevExpress.XtraReports.UI.DataBindingMode.Bindings;
            base.OnStartup(e);
        }

    }
}
