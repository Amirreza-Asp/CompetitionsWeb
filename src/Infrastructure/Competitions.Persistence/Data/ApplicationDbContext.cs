using Competitions.Domain.Entities.Authentication;
using Competitions.Domain.Entities.Extracurriculars;
using Competitions.Domain.Entities.Managment;
using Competitions.Domain.Entities.Notifications;
using Competitions.Domain.Entities.Places;
using Competitions.Domain.Entities.Sports;
using Competitions.Domain.Entities.Static;
using Competitions.SharedKernel.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Competitions.Persistence.Data
{
    public class ApplicationDbContext : DbContext
    {

        private readonly IMediator _mediator;

        public ApplicationDbContext ( DbContextOptions options , IMediator mediator ) : base(options)
        {
            _mediator = mediator;
        }

        #region Authentication
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        #endregion

        #region Notifications
        public DbSet<Notification> Notification { get; set; }
        public DbSet<NotificationImage> NotificationImage { get; set; }
        #endregion

        #region Extracurriculars
        public DbSet<Extracurricular> Extracurricular { get; set; }
        public DbSet<ExtracurricularTime> ExtracurricularTime { get; set; }
        public DbSet<ExtracurricularUser> ExtracurricularUser { get; set; }
        #endregion

        #region Matches
        public DbSet<Festival> Festival { get; set; }

        public DbSet<Match> Match { get; set; }
        public DbSet<MatchDocument> MatchDocument { get; set; }
        public DbSet<MatchAudienceType> MatchAudienceType { get; set; }
        public DbSet<MatchAward> MatchAward { get; set; }
        public DbSet<MatchConditions> MatchConditions { get; set; }
        #endregion

        #region Static
        public DbSet<Evidence> Evidence { get; set; }
        public DbSet<AudienceType> AudienceType { get; set; }
        #endregion

        #region Sports
        // Sport
        public DbSet<SportType> SportType { get; set; }
        public DbSet<Sport> Sport { get; set; }

        // Coach
        public DbSet<CoachEvidenceType> CoachEvidenceType { get; set; }
        public DbSet<Coach> Coach { get; set; }
        #endregion

        #region Place
        public DbSet<PlaceType> PlaceType { get; set; }
        public DbSet<Place> Place { get; set; }
        public DbSet<PlaceImages> PlaceImages { get; set; }
        public DbSet<PlaceSports> PlaceSports { get; set; }
        #endregion


        protected override void OnModelCreating ( ModelBuilder modelBuilder )
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override async Task<int> SaveChangesAsync ( CancellationToken cancellationToken = new CancellationToken() )
        {
            // ignore events if no dispatcher provided
            if ( _mediator != null ) await SendRequests();

            int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return result;
        }

        public override int SaveChanges ()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }


        private async Task SendRequests ()
        {
            var longEntitiesWithEvents = ChangeTracker
              .Entries()
              .Select(e => e.Entity as BaseEntity<long>)
              .Where(e => e?.Events != null && e.Events.Any())
              .ToArray();


            foreach ( var entity in longEntitiesWithEvents )
            {
                var events = entity.Events.ToArray();
                entity.Events.Clear();
                foreach ( var domainEvent in events )
                {
                    await _mediator.Send(domainEvent).ConfigureAwait(false);
                }
            }

            var guidEntitiesWithEvents = ChangeTracker
              .Entries()
              .Select(e => e.Entity as BaseEntity<Guid>)
              .Where(e => e?.Events != null && e.Events.Any())
              .ToArray();

            foreach ( var entity in guidEntitiesWithEvents )
            {
                var events = entity.Events.ToArray();
                entity.Events.Clear();
                foreach ( var domainEvent in events )
                {
                    await _mediator.Send(domainEvent).ConfigureAwait(false);
                }
            }

        }
    }
}
