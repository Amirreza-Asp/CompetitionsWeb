using Competitions.Domain.Entities.Managment.Events.Matches;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Competitions.Persistence.Managment.Handlers.Matches
{
    public class DeleteMatchConditionsRegulationsHandler : IRequestHandler<DeleteMatchConditionsRegulationsEvent>
    {
        private readonly IHostingEnvironment _webHostEnv;

        public DeleteMatchConditionsRegulationsHandler ( IHostingEnvironment webHostEnv )
        {
            _webHostEnv = webHostEnv;
        }


        public Task<Unit> Handle ( DeleteMatchConditionsRegulationsEvent request , CancellationToken cancellationToken )
        {
            string path = _webHostEnv.WebRootPath + request.Path + request.Name;
            File.Delete(path);
            return Unit.Task;
        }
    }
}
