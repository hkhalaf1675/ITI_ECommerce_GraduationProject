namespace API.HubConfig
{
    public interface IMessageHubClient
    {
        Task SendOffersToUser(List<string> message);
    }
}
