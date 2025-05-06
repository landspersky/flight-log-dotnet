namespace FlightLogNet.Integration
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Models;
    using RestSharp;

    public class ClubUserDatabase(IConfiguration configuration, IMapper mapper, ILogger<ClubUserDatabase> logger) : IClubUserDatabase
    {
        public bool TryGetClubUser(long memberId, out PersonModel personModel)
        {
            personModel = this.GetClubUsers().FirstOrDefault(person => person.MemberId == memberId);

            return personModel != null;
        }

        public IList<PersonModel> GetClubUsers()
        {
            IList<ClubUser> x = this.ReceiveClubUsers();
            return this.TransformToPersonModel(x);
        }

        private List<ClubUser> ReceiveClubUsers()
        {
            var clubDbUrl = configuration["ClubUsersApi"];
            if (string.IsNullOrEmpty(clubDbUrl))
            {
                logger.LogError("Club users endpoint URL is not configured.");
                return new List<ClubUser>();
            }

            var client = new RestClient(clubDbUrl);
            var request = new RestRequest("club/user");

            var response = client.Get<List<ClubUser>>(request);
            return response;
        }

        private List<PersonModel> TransformToPersonModel(IList<ClubUser> users)
        {
            return users.Select(user => new PersonModel
            {
                MemberId = user.MemberId,
                FirstName = user.FirstName,
                LastName = user.LastName
            }).ToList();
        }
    }
}
