
using ASS_PRC.Data.Entity;
using ASS_PRC.Data.UnitOfWork;
using ASS_PRC.Services.Helpers;
using ASS_PRC.Services.Responses;

using FirebaseAdmin.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ASS_PRC.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppSettings _appSettings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        public AccountService(IOptions<AppSettings> appSettings, IUnitOfWork unitOfWork, IConfiguration config)
        {
            _appSettings = appSettings.Value;
            _unitOfWork = unitOfWork;
            _config = config;
        }

        
        public async Task<string> AuthenticateWebAdminAsync(string idToken)
        {
            FirebaseToken decodedToken = null;
            try
            {
                decodedToken = await FirebaseAuth.DefaultInstance
        .VerifyIdTokenAsync(idToken);
            }
            catch (Exception)
            {
                return null;
            }
            String firebaseUid = decodedToken.Uid;
            Console.WriteLine(firebaseUid);
            object tmp = null;
            String email = "";
            String phoneNumber = "";
            String firebaseProvider = "";

            decodedToken.Claims.TryGetValue("firebase", out tmp);
            if (tmp.ToString().Contains("google"))
            {
                firebaseProvider = "google.com";
            }
            else return null;

            decodedToken.Claims.TryGetValue("email", out tmp);
            if (tmp != null) email = tmp.ToString();

            decodedToken.Claims.TryGetValue("phoneNumber", out tmp);
            if (tmp != null) phoneNumber = tmp.ToString();

            decodedToken.Claims.TryGetValue("name", out tmp);
            String name = tmp.ToString();

            Account account = _unitOfWork.Repository<Account>().Find(x => x.FirebaseUid == firebaseUid && !x.IsDelete);

            if (account == null)
            {
                _unitOfWork.Repository<Account>().Insert(new Account()
                {
                    Id = Guid.NewGuid(),
                    FullName = name,
                    Email=email,
                    FirebaseProvider= "google.com",
                    FirebaseUid= firebaseUid,
                    CreateDate= GetTimeNowVN.GetTimeNowVietNam(),
                    IsDelete=false,
                    ModifyDate=null,
                    PhoneNumber=phoneNumber,
                    Role=0
                });
                _unitOfWork.Commit();
                account = _unitOfWork.Repository<Account>().Find(x => x.FirebaseUid == firebaseUid && !x.IsDelete);
            }
            else
            {
                if(account.Role != 0)
                {
                    return null;
                }               
            }           
            var jwt = await generateJwtToken(account);
            return jwt;


        }
       

        private async Task<string> generateJwtToken(Account account)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Role , account.Role.ToString()),
                new Claim(ClaimTypes.Name , account.FullName)             
            };
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["AppSettings:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["AppSettings:Issuer"],
                _config["AppSettings:Issuer"],
                claims,
                expires: GetTimeNowVN.GetTimeNowVietNam().AddDays(7),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
