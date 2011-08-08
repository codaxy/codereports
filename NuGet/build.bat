mkdir Codaxy.CodeReports\lib\
copy ..\Libraries\Codaxy.CodeReports\bin\Release\Codaxy.CodeReports.* Codaxy.CodeReports\lib\
nuget pack Codaxy.CodeReports\Codaxy.CodeReports.nuspec

mkdir Codaxy.CodeReports.Exporters.Xlio\lib\
copy ..\Libraries\Codaxy.CodeReports.Exporters.Xlio\bin\Release\Codaxy.CodeReports.Exporters.Xlio* Codaxy.CodeReports.Exporters.Xlio\lib\
nuget pack Codaxy.CodeReports.Exporters.Xlio\Codaxy.CodeReports.Exporters.Xlio.nuspec

mkdir Codaxy.CodeReports.Exporters.Html\lib\
copy ..\Libraries\Codaxy.CodeReports.Exporters.Html\bin\Release\Codaxy.CodeReports.Exporters.Html* Codaxy.CodeReports.Exporters.Html\lib\
nuget pack Codaxy.CodeReports.Exporters.Html\Codaxy.CodeReports.Exporters.Html.nuspec

mkdir Codaxy.CodeReports.Exporters.Text\lib\
copy ..\Libraries\Codaxy.CodeReports.Exporters.Text\bin\Release\Codaxy.CodeReports.Exporters.Text* Codaxy.CodeReports.Exporters.Text\lib\
nuget pack Codaxy.CodeReports.Exporters.Text\Codaxy.CodeReports.Exporters.Text.nuspec


