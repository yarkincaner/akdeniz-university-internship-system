namespace Internships.Core.Helpers
{
    public static class MailTemplateHelper
    {
       private static string companyApprovalTemplate = "<!doctype html><html lang='en'><head><meta name='viewport' content='width=device-width, initial-scale=1.0'><meta http-equiv='Content-Type' content='text/html; charset=UTF-8'><title>Simple Transactional Email</title><style media='all' type='text/css'>/* CSS styles here */</style></head><body><table role='presentation' border='0' cellpadding='0' cellspacing='0' class='body'><tr><td>&nbsp;</td><td class='container'><div class='content'><span class='preheader'></span><table role='presentation' border='0' cellpadding='0' cellspacing='0' class='main'><tr><td class='wrapper'><p>Sayın {{name}},</p><p>Öğrencimiz {{studentName}}, şirketiniz bünyesinde zorunlu staj yapacağını bildirmiştir. Başvurunun detaylarını görmek ve onaylamak/reddetmek için lütfen aşağıdaki bağlantıyı kullanınız:</p><table role='presentation' border='0' cellpadding='0' cellspacing='0' class='btn btn-primary'><tbody><tr><td align='left'><table role='presentation' border='0' cellpadding='0' cellspacing='0'><tbody><tr><td> <a href='{{url}}' target='_blank'>Staj başvurusunu görüntüle</a> </td></tr></tbody></table></td></tr></tbody></table><p>Eğer bağlantıya erişimde sorun yaşıyorsanız, aşağıdaki URL'yi tarayıcınıza kopyalayıp yapıştırabilirsiniz:</p><p>{{url}}</p><p>Saygılarımızla,</p><p>Akdeniz Üniversitesi</p><p>Bilgisayar Mühendisliği Bölümü</p><p>Staj Komisyonu</p></td></tr></table><div class='footer'><table role='presentation' border='0' cellpadding='0' cellspacing='0'><tr><td class='content-block'><span class='apple-link'></span></td></tr></table></div></div></td><td>&nbsp;</td></tr></table></body></html>";

        public static string CreateCompanyApprovalMail(string name, string studentName, string url)
        {
            return companyApprovalTemplate.Replace("{{name}}", name)
                                    .Replace("{{studentName}}", studentName)
                                    .Replace("{{url}}", url);
        }
    }
}
