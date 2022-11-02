using Competitions.Domain.Entities.Notifications.Events;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Competitions.Persistence.Managment.Handlers.Notifications
{
    public class SaveNotificationImageHandler : IRequestHandler<SaveNotificationImageEvent>
    {
        private readonly IHostingEnvironment _webHostEnv;

        public SaveNotificationImageHandler ( IHostingEnvironment webHostEnv )
        {
            _webHostEnv = webHostEnv;
        }

        public async Task<Unit> Handle ( SaveNotificationImageEvent request , CancellationToken cancellationToken )
        {
            var upload = _webHostEnv.WebRootPath;
            if ( !Directory.Exists(upload + request.Path) )
            {
                Directory.CreateDirectory(upload + request.Path);
            }

            string path = upload + request.Path + request.Document.Name;
            await File.WriteAllBytesAsync(path , request.Document.File);
            return Unit.Value;
        }
    }
}
