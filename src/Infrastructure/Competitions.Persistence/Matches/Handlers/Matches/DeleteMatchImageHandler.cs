using Competitions.Domain.Entities.Managment.Events.Matches;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Competitions.Persistence.Managment.Handlers.Matches
{
    public class DeleteMatchImageHandler : IRequestHandler<DeleteMatchImageEvent>
    {
        private readonly IHostingEnvironment _webHostEnv;

        public DeleteMatchImageHandler(IHostingEnvironment webHostEnv)
        {
            _webHostEnv = webHostEnv;
        }


        public Task<Unit> Handle(DeleteMatchImageEvent request, CancellationToken cancellationToken)
        {
            string path = _webHostEnv.WebRootPath + request.Path + request.Name;
            if (File.Exists(path))
                File.Delete(path);
            return Unit.Task;
        }
    }
}
