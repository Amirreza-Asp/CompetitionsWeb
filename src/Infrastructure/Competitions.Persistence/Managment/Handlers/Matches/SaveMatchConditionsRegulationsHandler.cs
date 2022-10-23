using Competitions.Domain.Entities.Managment.Events.Matches;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Competitions.Persistence.Managment.Handlers.Matches
{
    public class SaveMatchConditionsRegulationsHandler : IRequestHandler<SaveMatchConditionsRegulationsEvent>
    {
        private readonly IHostingEnvironment _webHostEnv;

        public SaveMatchConditionsRegulationsHandler ( IHostingEnvironment webHostEnv )
        {
            _webHostEnv = webHostEnv;
        }

        public async Task<Unit> Handle ( SaveMatchConditionsRegulationsEvent request , CancellationToken cancellationToken )
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
