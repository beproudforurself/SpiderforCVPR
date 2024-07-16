using HtmlAgilityPack;
using RestSharp;
using SpiderForCVPR.resource;
using System.Net;
using System.Net.Mail;
using System.Text;
internal class Programm
{
    private static void Main(string[] args)
    {
        MutiAutospider CVspider = Spiderfactory.CreateSpider("CVPR");
        var msg = CVspider.Spider();
        CVspider.autoSendEmail("xxxx@outlook.com", "xxxx@outlook.com", msg,"CV PaperTracking");
    }

    public class MutiAutospider
    {

        public virtual void autoSendEmail(string Email_address, string CCaddress, StringBuilder msg, string subject)      
        {
        }
        public virtual StringBuilder Spider()
        {
            return null;
        }
    }
    class CVPRspider : MutiAutospider
    {
        
        public override void autoSendEmail(string Email_address,string CCaddress, StringBuilder msg, string subject)
        {
            try
            {
                MailMessage input = new MailMessage();
                input.IsBodyHtml = true;
                input.Subject = subject;
                input.From = new MailAddress("xxxx@outlook.com");
                input.To.Add(new MailAddress(Email_address));
                input.CC.Add(new MailAddress(CCaddress));
                input.Body = msg.ToString();
                
                SmtpClient client = new SmtpClient();
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("xxxx@outlook.com", "app-code");
                client.Port = 587; // You can use Port 25 if 587 is blocked (mine is!)
                client.Host = "smtp.office365.com";
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;

                client.Send(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }
        public override StringBuilder Spider()
        {
            string url = "https://paperswithcode.com/";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    
                    string htmlText = reader.ReadToEnd();
                    var doc = new HtmlDocument();
                    doc.LoadHtml(htmlText);
                    var nodes = doc.DocumentNode.SelectNodes("//div[@class='row infinite-item item paper-card']").Take(10);
                    StringBuilder msg = new StringBuilder();
                    msg.Append("<div style='font-family: Arial, sans-serif; font-size: 17px; color: #333; line-height: 1.5;'>");
                    msg.Append("<h2 style='font-weight: bold; margin-bottom: 10px;'>Hallo Meine Frau, dies sind die neuesten zehn Papierinformationen zu Papier und Code</h2>");

                    foreach (var item in nodes)
                    {
                        var PapaerNodeTitle = item.SelectNodes(".//h1").FirstOrDefault()?.InnerText ?? "未知标题";
                        var PaperNodeLink = "https://paperswithcode.com" + item.SelectSingleNode(".//a[contains(@class,'badge-light')]")?.GetAttributeValue("href", "");
                        var CodeNodeLink = "https://paperswithcode.com" + item.SelectSingleNode(".//a[contains(@class,'badge-dark')]")?.GetAttributeValue("href", "");

                        msg.Append("<div style='margin-left: 30px; margin-bottom: 10px;'>");
                        msg.Append("<strong>Paper Title:</strong> ").Append(PapaerNodeTitle).Append("<br>");
                        msg.Append("<a style='color: #007bff; text-decoration: none; margin-left: 30px;' href='").Append(PaperNodeLink).Append("' target='_blank'>PaperLink</a><br>");
                        msg.Append("<a style='color: #007bff; text-decoration: none; margin-left: 30px;' href='").Append(CodeNodeLink).Append("' target='_blank'>CodeLink</a><br>");
                        msg.Append("</div>");
                    }
                    msg.Append("<div style='margin-top: 20px;'>");
                    msg.Append("<p>Best Regards.</p>");
                    msg.Append("<p>Mail from deine Mann.</p>");
                    msg.Append("<p>Please do not reply this mail.</p>");
                    msg.Append("</div>");
                    msg.Append("</div>");
                    return msg;
                }

            }
        }
    }
    public class Spiderfactory
    {
        public static MutiAutospider CreateSpider(string spiderType)
        {
            MutiAutospider spider = null;
            switch (spiderType)
            {
                case "CVPR":
                    spider = new CVPRspider();
                    break;
            }
            return spider;
        }
        
    }

}
