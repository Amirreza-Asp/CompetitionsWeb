using Competitions.Domain.Entities.Places.Events;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Competitions.Persistence.Places.Handlers
{
    public class SavePlaceImageHandler : IRequestHandler<SavePlaceImageEvent>
    {
        private readonly IHostingEnvironment _webHostEnv;

        public SavePlaceImageHandler ( IHostingEnvironment webHostEnv )
        {
            _webHostEnv = webHostEnv;
        }

        public async Task<Unit> Handle ( SavePlaceImageEvent request , CancellationToken cancellationToken )
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
