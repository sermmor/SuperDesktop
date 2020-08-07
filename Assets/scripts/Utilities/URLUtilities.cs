using System;
using System.IO;
using System.Net;
using System.Text;

public static class URLUtilities
{
    public static string getTitleUrl(string urlAddress)
    {
        try {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (String.IsNullOrWhiteSpace(response.CharacterSet))
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                string data = readStream.ReadToEnd();

                response.Close();
                readStream.Close();

                return getTitleFromHTMLContent(data);
            }
        }
        catch (Exception ex)
        {
            return "";
        }

        return "";
    }

    static string getTitleFromHTMLContent(string htmlData)
    {
        // Unfurling the web (test first with Facebook Open Graph and then with Twitter Card).
        const string openGraphTitle = "\"og:title\"";
        const string twitterCardTitle = "\"twitter:title\"";

        int indexTitle = htmlData.IndexOf(openGraphTitle);
        if (indexTitle < 0) indexTitle = htmlData.IndexOf(twitterCardTitle);
        if (indexTitle >= 0)
        {
            string titleSearch = htmlData.Substring(indexTitle);
            
            indexTitle = titleSearch.IndexOf("content");
            titleSearch = titleSearch.Substring(indexTitle);

            indexTitle = titleSearch.IndexOf("\"");
            titleSearch = titleSearch.Substring(indexTitle + 1);

            int endIndexTitle = titleSearch.IndexOf("\"");
            return titleSearch.Substring(0, endIndexTitle);
        }
        return "";
    }
}