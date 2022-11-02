using Competitions.Domain.Entities.Notifications.Events;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Competitions.Persistence.Managment.Handlers.Notifications
{
    public class DeleteNotificationImageHandler : IRequestHandler<DeleteNotificationImageEvent>
    {
        private readonly IHostingEnvironment _webHostEnv;

        public DeleteNotificationImageHandler ( IHostingEnvironment webHostEnv )
        {
            _webHostEnv = webHostEnv;
        }


        public Task<Unit> Handle ( DeleteNotificationImageEvent request , CancellationToken cancellationToken )
        {
            string path = _webHostEnv.WebRootPath + request.Path + request.Name;
            File.Delete(path);
            return Unit.Task;
        }
    }
}
