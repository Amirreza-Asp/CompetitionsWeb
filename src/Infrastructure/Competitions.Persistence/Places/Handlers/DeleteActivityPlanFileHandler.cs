using Competitions.Domain.Entities.Places.Events;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Competitions.Persistence.Places.Handlers
{
    public class DeleteActivityPlanFileHandler : IRequestHandler<DeleteActivityPlanFileEvent>
    {
        private readonly IHostingEnvironment _webHostEnv;

        public DeleteActivityPlanFileHandler(IHostingEnvironment webHostEnv)
        {
            _webHostEnv = webHostEnv;
        }


        public Task<Unit> Handle(DeleteActivityPlanFileEvent request, CancellationToken cancellationToken)
        {
            string path = _webHostEnv.WebRootPath + request.Path + request.Name;
            if (File.Exists(path))
                File.Delete(path);
            return Unit.Task;
        }
    }
}
