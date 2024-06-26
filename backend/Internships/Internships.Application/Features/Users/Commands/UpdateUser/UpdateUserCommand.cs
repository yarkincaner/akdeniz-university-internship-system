using Internships.Core.Exceptions;
using Internships.Core.Interfaces;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Internships.Core.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<Response<string>>
    {
        public long TcKimlikNo { get; set; }
        public int BirthYear { get; set; }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Response<string>>
    {
        private readonly IUserRepositoryAsync _userRepositoryAsync;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public UpdateUserCommandHandler(IUserRepositoryAsync userRepositoryAsync, IAuthenticatedUserService authenticatedUserService)
        {
            _userRepositoryAsync = userRepositoryAsync;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<Response<string>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var userId = _authenticatedUserService.UserId;
            var user = await _userRepositoryAsync.GetByUserId(userId);
            if (user == null)
            {
                throw new EntityNotFoundException("User", userId);
            }

            var isValid = await ConfirmCitizienShip(command.TcKimlikNo.ToString(), command.BirthYear, user.FirstName, user.LastName);

            if (!isValid)
            {
                throw new ApiException("Kullanıcı Doğrulanamadı!");
            }

            user.BirthYear = command.BirthYear;
            user.TcKimlikNo = command.TcKimlikNo;

            await _userRepositoryAsync.UpdateAsync(user);
            return new Response<string>
            {
                Data = user.UserId,
                Succeeded = true,
            };
        }

        private async Task<bool> ConfirmCitizienShip(string tcKimlikNo, int birthyear, string name, string surname)
        {
            string tc = tcKimlikNo;
            string patternTC = @"^[1-9]{1}[0-9]{9}[02468]{1}$";

            if (!Regex.IsMatch(tc, patternTC))
            {
                throw new ApiException("Geçersiz TC Kimlik no!");
            }

            long tcAsLong = long.Parse(tc);

            bool isCitizien = await checkCitizienShip(tcAsLong, name, surname, birthyear);

            return isCitizien;
        }

        private async Task<bool> checkCitizienShip(long tcNo, string name, string surname, int birthyear)
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
