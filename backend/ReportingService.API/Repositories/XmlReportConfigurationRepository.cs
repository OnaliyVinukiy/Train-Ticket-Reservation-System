using System.Xml.Linq;
using ReportingService.API.Models;


namespace ReportingService.API.Repositories;


public class XmlReportConfigurationRepository
{

    private readonly string path =
        "XML/ReportConfiguration.xml";


    public ReportConfiguration GetConfiguration()
    {

        XDocument document =
            XDocument.Load(path);


        return new ReportConfiguration
        {

            CompanyName =
                document
                .Root?
                .Element("CompanyName")?
                .Value
                ?? "",


            Currency =
                document
                .Root?
                .Element("Currency")?
                .Value
                ?? "",


            DefaultExportFormat =
                document
                .Root?
                .Element("DefaultExportFormat")?
                .Value
                ?? "",


            Reports =
                document
                .Root?
                .Element("Reports")?
                .Elements("Report")
                .Select(x =>
                    x.Element("Name")?.Value ?? "")
                .ToList()
                ??
                new List<string>()

        };

    }

}