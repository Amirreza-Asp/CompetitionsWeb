using Competitions.Application.Places.Interfaces;
using Competitions.Common.Helpers;
using Competitions.Domain.Dtos.Places.Places;
using Competitions.Domain.Entities.Places;
using Competitions.Persistence.Data;
using Competitions.SharedKernel.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Competitions.Persistence.Places.Services
{
    public class PlaceService : IPlaceService
    {
        private readonly ApplicationDbContext _context;

        public PlaceService ( ApplicationDbContext context )
        {
            _context = context;
        }

        public async Task CreateAsync ( CreatePlaceDto command , IFormFileCollection files )
        {
            var place = new Place(command.Title , command.PlaceTypeId , command.Address , command.Meterage , command.ParentPlaceId);
            place.WithSupervisor(new Supervisor(command.Supervisor.Name , command.Supervisor.PhoneNumber));

            var sportsIds = command.NewSports.Split(',').ToList();
            sportsIds.ForEach(sportId => place.AddSport(new PlaceSports(long.Parse(sportId) , place.Id)));


            files.ToList().ForEach(image =>
            {
                var img = new PlaceImages(place.Id , new Document(image.FileName , image.ReadBytes()));
                img.SaveImage();
                place.AddImage(img);
            });


            _context.Place.Add(place);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync ( UpdatePlaceDto command , IFormFileCollection files )
        {
            // get entity
            var place = _context.Place
                .Include(u => u.Supervisor)
                .Include(u => u.Sports)
                .FirstOrDefault(u => u.Id == command.Id);

            if ( place == null )
                return;

            // update properies
            place.WithTitle(command.Title)
                .WithMeterage(command.Meterage)
                .WithAddress(command.Address)
                .WithPlaceTypeId(command.PlaceTypeId)
                .WithParentId(command.ParentPlaceId)
                .Supervisor.WithName(command.Supervisor.Name)
                    .WithPhoneNumber(command.Supervisor.PhoneNumber);

            // get sport Ids
            var sportsIds = command.NewSports.Split(',').ToList();

            // add new sports
            sportsIds.Where(id => !place.Sports.Select(u => u.SportId).Contains(long.Parse(id)))
                .ToList().ForEach(sportId => place.AddSport(new PlaceSports(long.Parse(sportId) , place.Id)));

            // remove sports
            place.Sports.Where(sport => !sportsIds.Contains(sport.SportId.ToString()))
                .ToList().ForEach(sport => place.RemoveSport(sport));

            // add new images
            files.ToList().ForEach(image =>
            {
                var img = new PlaceImages(place.Id , new Document(image.FileName , image.ReadBytes()));
                img.SaveImage();
                place.AddImage(img);
            });

            // save to db
            _context.Place.Update(place);
            await _context.SaveChangesAsync();
        }
    }
}
