using Competitions.Domain.Entities.Sports.Events;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Competitions.Persistence.Sports.Handlers
{
    public class DeleteSportImageHandler : IRequestHandler<DeleteSportImageEvent>
    {
        private readonly IHostingEnvironment _webHostEnv;

        public DeleteSportImageHandler ( IHostingEnvironment webHostEnv )
        {
            _webHostEnv = webHostEnv;
        }


        public Task<Unit> Handle ( DeleteSportImageEvent request , CancellationToken cancellationToken )
        {
            string path = _webHostEnv.WebRootPath + request.Path + request.Name;
            File.Delete(path);
            return Unit.Task;
        }
    }
}
