using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace MorMor.Utils;

public class MailHelper
{
    public static void SendMail(string targerAddress, string subject, string body, string[] filePath = null)
    {
        SendMail(MorMorAPI.Setting.MailHost, MorMorAPI.Setting.SenderMail, MorMorAPI.Setting.SenderPwd, targerAddress, subject, body, filePath);
    }
    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="smtp">发送服务地址smtp.163.com</param>
    /// <param name="sendAdress">发送人邮件地址</param>
    /// <param name="sendAdressPwd">发送人邮箱密码</param>
    /// <param name="toAddress">接收人邮箱</param>
    /// <param name="subject">标题</param>
    /// <param name="bodyText">发送内容</param>
    /// <param name="filePath">附件地址</param>
    /// <returns></returns>
    public static void SendMail(string smtp, string sendAdress, string sendAdressPwd, string toAddress, string subject, string bodyText, string[]? filePath = null)
    {
        //确定smtp服务器地址。实例化一个Smtp客户端
        System.Net.Mail.SmtpClient client = new(smtp);
        //生成一个发送地址
        string strFrom = string.Empty;

        //构造一个发件人地址对象
        MailAddress from = new(sendAdress, "这是标题", Encoding.UTF8);
        //构造一个收件人地址对象
        MailAddress to = new(toAddress, strFrom, Encoding.UTF8);

        //构造一个Email的Message对象
        MailMessage message = new(from, to);
        if (filePath != null)
        {
            //为 message 添加附件
            foreach (string ss in filePath)
            {
                //得到文件名
                string fileName = ss;
                //判断文件是否存在
                if (File.Exists(fileName))
                {
                    //构造一个附件对象
                    Attachment attach = new(fileName);
                    //     得到文件的信息
                    ContentDisposition disposition = attach.ContentDisposition!;
                    disposition.CreationDate = System.IO.File.GetCreationTime(fileName);
                    disposition.ModificationDate = System.IO.File.GetLastWriteTime(fileName);
                    disposition.ReadDate = System.IO.File.GetLastAccessTime(fileName);
                    //向邮件添加附件
                    message.Attachments.Add(attach);
                }
                else
                {
                    //MessageBox.Show("文件" + fileName + "未找到！");
                }
            }
        }

        //添加邮件主题和内容
        message.Subject = subject;
        message.SubjectEncoding = Encoding.UTF8;
        message.Body = bodyText;
        message.BodyEncoding = Encoding.UTF8;

        //设置邮件的信息
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        message.BodyEncoding = System.Text.Encoding.UTF8;
        message.IsBodyHtml = false;

        client.UseDefaultCredentials = false;
        string username = sendAdress;
        string passwd = sendAdressPwd;
        //用户登陆信息
        NetworkCredential myCredentials = new(username, passwd);
        client.Credentials = myCredentials;
        //发送邮件
        client.Send(message);
    }
}
