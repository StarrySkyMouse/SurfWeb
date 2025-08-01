using System.Net;
using System.Net.Mail;

namespace Utils.Mail;

public static class MailUtil
{
    /// <summary>
    ///     发送Mai
    /// </summary>
    /// <param name="fromEmail">发送的邮箱</param>
    /// <param name="toEmail">接收的邮箱</param>
    /// <param name="password">授权码</param>
    /// <param name="title">邮箱名字</param>
    /// <param name="msg">邮箱信息</param>
    public static void SendMessage(string fromEmail, string toEmail, string password, string title, string msg)
    {
        // 邮箱配置信息
        var smtpServer = "smtp.qq.com";
        var smtpPort = 587; // QQ邮箱推荐587端口（SSL用465）

        // 创建邮件
        var mail = new MailMessage();
        mail.From = new MailAddress(fromEmail);
        mail.To.Add(toEmail);
        mail.Subject = title;
        mail.Body = msg;
        // 配置SMTP客户端
        var smtp = new SmtpClient(smtpServer, smtpPort);
        smtp.Credentials = new NetworkCredential(fromEmail, password);
        smtp.EnableSsl = true; // QQ邮箱必须启用SSL

        // 发送邮件
        try
        {
            smtp.Send(mail);
            Console.WriteLine("邮件发送成功！");
        }
        catch (Exception ex)
        {
            Console.WriteLine("邮件发送失败：" + ex.Message);
        }
    }
}