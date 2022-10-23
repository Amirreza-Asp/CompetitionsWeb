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
    public class SaveSportImageHandler : IRequestHandler<SaveSportImageEvent>
    {
        private readonly IHostingEnvironment _webHostEnv;

        public SaveSportImageHandler ( IHostingEnvironment webHostEnv )
        {
            _webHostEnv = webHostEnv;
        }

        public async Task<Unit> Handle ( SaveSportImageEvent request , CancellationToken cancellationToken )
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