<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128600453/14.2.3%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T356256)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [MainWindow.xaml.cs](./CS/RuntimeReportsApplication/MainWindow.xaml.cs) (VB: [MainWindow.xaml.vb](./VB/RuntimeReportsApplication/MainWindow.xaml.vb))
<!-- default file list end -->
# How to dynamically generate a report in a WPF application


This example illustrates the process of creating a report in code in a WPF application. <br><br>
<p>The following steps are essential to create a report layout:</p>
<p>1. Create a report instance and <a href="https://documentation.devexpress.com/#XtraReports/CustomDocument15034">bind it to data</a>.</p>
<p>2. Add required <a href="https://documentation.devexpress.com/#XtraReports/CustomDocument2590">bands</a> to the report.</p>
<p>3. Add required <a href="https://documentation.devexpress.com/#XtraReports/CustomDocument2605">controls</a> to the created bands and provide data to them.</p>
<p>After the report layout is complete, you can generate the report document and display it in a <a href="https://documentation.devexpress.com/#XtraReports/CustomDocument10709">Print Preview</a>.<br><br>Starting with v.17.2, the report uses <a href="https://documentation.devexpress.com/XtraReports/119236/Creating-Reports-in-Visual-Studio/Detailed-Guide-to-DevExpress-Reporting/Providing-Data-to-Reports/Data-Binding-Overview/Data-Binding-Modes">expression bindings</a> to provide data to controls. You can switch to the legacy binding mode by setting the <a href="https://documentation.devexpress.com/XtraReports/DevExpress.XtraReports.Configuration.UserDesignerOptions.DataBindingMode.property">UserDesignerOptions.DataBindingMode</a> property to <strong>Bindings </strong>at the application startup.</p>

<br/>


<!-- feedback -->
## Does this example address your development requirements/objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=reporting-wpf-create-report-in-code&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=reporting-wpf-create-report-in-code&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->
