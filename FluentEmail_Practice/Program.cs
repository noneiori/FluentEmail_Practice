using FluentEmail.Core;
using FluentEmail.Smtp;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace FluentEmail_Practice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 設定郵件伺服器資訊與登入帳號密碼
            var smtp = new SmtpSender(new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential("username@gmail.com", "password")
            });

            //將smtp設定給DefaultSender
            Email.DefaultSender = smtp;
            
            //給inline attachment用的附件檔名，這邊用guid
            var cid = Guid.NewGuid().ToString();
                        
            var email = Email
                .From("username@gmail.com")
                .To("to@gmail.com")
                .Subject("FluentEmail_Practice")
                .Body($"<img src=\"cid:{cid}\" />",isHtml:true) //注意第二個參數預設為false，如果有用inline attachment要記得用html格式
                .AttachFromFilename(@"C:\Users\username\Downloads\filename.jpg", contentType: "image/png") //一般的附件方式
                .Attach(new FluentEmail.Core.Models.Attachment() //inline的情況下要將圖片的相關資訊用attach函式，並使用attachment類別的變數
                {
                    ContentId = cid,
                    ContentType = "image/png",
                    Filename = @"filename.jpg",
                    IsInline = true,
                    Data = System.IO.File.OpenRead(@"C:\Users\username\Downloads\filename.jpg")
                })
                .Send();
        }
    }
}
