using Competitions.Domain.Entities.Sports.Events;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Competitions.Persistence.Sports.Handlers
{
    public class DeleteSportImageHandler : IRequestHandler<DeleteSportImageEvent>
    {
        private readonly IHostingEnvironment _webHostEnv;

        public DeleteSportImageHandler(IHostingEnvironment webHostEnv)
        {
            _webHostEnv = webHostEnv;
        }


        public Task<Unit> Handle(DeleteSportImageEvent request, CancellationToken cancellationToken)
        {
            string path = _webHostEnv.WebRootPath + request.Path + request.Name;
            if (File.Exists(path))
                File.Delete(path);
            return Unit.Task;
        }
    }
}
