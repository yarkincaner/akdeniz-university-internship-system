using Internships.Core.Exceptions;
using Internships.Core.Wrappers;
using MediatR;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Internships.Core.Features.Users.Commands.ConfirmCitizienship
{
    public class ConfirmCitizienshipCommand : IRequest<Response<bool>>
    {
        public string CitizienshipId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Birthyear { get; set; }
    }

    public class ConfirmCitizienshipCommandHandler : IRequestHandler<ConfirmCitizienshipCommand, Response<bool>>
    {
        public async Task<Response<bool>> Handle(ConfirmCitizienshipCommand command, CancellationToken cancellationToken)
        {
            string tc = command.CitizienshipId;
            string patternTC = @"^[1-9]{1}[0-9]{9}[02468]{1}$";

            if (!Regex.IsMatch(tc, patternTC))
            {
                throw new ApiException("Geçersiz TC Kimlik no!");
            }

            long tcAsLong = long.Parse(tc);

            string name = command.Name.ToUpper();
            string surname = command.Surname.ToUpper();
            int birthyear = command.Birthyear;

            bool isCitizien = await checkCitizienShip(tcAsLong, name, surname, birthyear);

            return new Response<bool>(isCitizien);
        }

        public async Task<bool> checkCitizienShip(long tcNo, string name, string surname, int birthyear)
        {
            string soapMessage = $@"
                <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                    <soap:Body>
                        <TCKimlikNoDogrula xmlns=""http://tckimlik.nvi.gov.tr/WS"">
                            <TCKimlikNo>{tcNo}</TCKimlikNo>
                            <Ad>{name}</Ad>
                            <Soyad>{surname}</Soyad>
                            <DogumYili>{birthyear}</DogumYili>
                        </TCKimlikNoDogrula>
                    </soap:Body>
                </soap:Envelope>"
                ;

            // SOAP isteği gönderme
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://tckimlik.nvi.gov.tr/Service/KPSPublic.asmx");
            request.Headers.Add("SOAPAction", "\"http://tckimlik.nvi.gov.tr/WS/TCKimlikNoDogrula\"");
            request.ContentType = "text/xml;charset=\"utf-8\"";
            request.Accept = "text/xml";
            request.Method = "POST";

            using (Stream stream = request.GetRequestStream())
            {
                byte[] soapBytes = Encoding.UTF8.GetBytes(soapMessage);
                stream.Write(soapBytes, 0, soapBytes.Length);
            }

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd();
                    return result.Contains("<TCKimlikNoDogrulaResult>true</TCKimlikNoDogrulaResult>");
                }
            }
        }
    }
}
