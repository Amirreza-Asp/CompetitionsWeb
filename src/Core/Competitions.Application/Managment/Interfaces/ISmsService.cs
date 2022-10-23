namespace Competitions.Application.Managment.Interfaces
{
    public interface ISmsService
    {
        Task<bool> SendAsync ( String message , String phoneNumber );

    }
}
