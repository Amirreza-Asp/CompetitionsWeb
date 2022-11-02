using Competitions.Domain.Entities.Managment.Events.Festivals;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Competitions.Persistence.Notifications.Handlers
{
    public class DeleteFestivalImageHandler : IRequestHandler<DeleteFestivalImageEvent>
    {
        private readonly IHostingEnvironment _webHostEnv;

        public DeleteFestivalImageHandler(IHostingEnvironment webHostEnv)
        {
            _webHostEnv = webHostEnv;
        }


        public Task<Unit> Handle(DeleteFestivalImageEvent request, CancellationToken cancellationToken)
        {
            string path = _webHostEnv.WebRootPath + request.Path + request.Name;
            File.Delete(path);
            return Unit.Task;
        }
    }
}
