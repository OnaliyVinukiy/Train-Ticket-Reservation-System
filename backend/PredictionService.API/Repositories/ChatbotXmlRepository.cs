using System.Xml.Linq;
using PredictionService.API.Models;

namespace PredictionService.API.Repositories;

public class ChatbotXmlRepository
{

    private readonly string filePath =
        "XML/ChatbotKnowledge.xml";


    public string? FindAnswer(string message)
    {

        var document =
            XDocument.Load(filePath);


        var items =
            document.Descendants("Item");


        foreach(var item in items)
        {

            var keywords =
                item.Element("Keywords")?.Value
                ?? "";


            var words =
                keywords.Split(
                    ' ',
                    StringSplitOptions.RemoveEmptyEntries);


            foreach(var word in words)
            {
                if(message.Contains(
                    word,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return item.Element("Answer")?.Value;
                }
            }
        }


        return null;
    }

}