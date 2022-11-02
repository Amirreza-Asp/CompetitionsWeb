using Competitions.Application.Managment.Interfaces;
using Competitions.Common.Helpers;
using Competitions.Domain.Dtos.Matches.Matches;
using Competitions.Domain.Entities.Managment;
using Competitions.Domain.Entities.Managment.ValueObjects;
using Competitions.Persistence.Data;
using Competitions.SharedKernel.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Competitions.Persistence.Managment.Services
{
    public class MatchService : IMatchService
    {
        private readonly ApplicationDbContext _context;

        public MatchService ( ApplicationDbContext context )
        {
            _context = context;
        }

        public async Task CreateAsync ( MatchFirstInfoDto firstInfo , MatchSecondInfoDto secondInfo , MatchConditionDto condition , MatchDocumentDto document , MatchAwardListDto award )
        {
            if ( secondInfo.ImageFile == null || condition.RGFile == null )
                return;

            var startPutOn = DateTimeConvertor.GetDateFromString(firstInfo.StartPutOn);
            var endPutOn = DateTimeConvertor.GetDateFromString(firstInfo.EndPutOn);
            var startRegister = DateTimeConvertor.GetDateFromString(firstInfo.StartRegister);
            var endRegister = DateTimeConvertor.GetDateFromString(firstInfo.EndRegister);

            var entity = new Match(firstInfo.Name , firstInfo.FestivalId , firstInfo.PlaceId , firstInfo.SportId , firstInfo.Gender , firstInfo.Level ,
                new DateTimeRange(startRegister , endRegister) , firstInfo.Capacity , firstInfo.TeamCount , new DateTimeRange(startPutOn , endPutOn) ,
                    new Document(secondInfo.Image , secondInfo.ImageFile) , secondInfo.Description);

            var aud = firstInfo.Audience.Split(',').ToList();
            if ( aud != null )
                aud.ToList().ForEach(item => entity.AddAudienceType(new MatchAudienceType(entity.Id , long.Parse(item))));

            entity.SaveImage();

            var conditionEntity = new MatchConditions(new Document(condition.File , condition.RGFile) , condition.Free ,
                condition.Payment.HasValue && !condition.Free ? condition.Payment.Value : 0 , entity.Id);

            conditionEntity.SaveRegulations();


            if ( document.Info != null )
            {
                document.Info.ToList().ForEach(doc =>
                {
                    var entityDoc = new MatchDocument(doc.Type , entity.Id , doc.EvidenceId);
                    entity.AddDocument(entityDoc);
                });
            }

            if ( award.Info != null )
            {
                award.Info.ToList().ForEach(async award =>
                {
                    var awardEntity = new MatchAward(( byte ) award.Rank , award.Prize , entity.Id);
                    entity.AddAward(awardEntity);
                });
            }

            _context.Add(entity);
            _context.Add(conditionEntity);

            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync ( Guid id )
        {
            var entity = await _context.Match.Include(u => u.Conditions).FirstOrDefaultAsync(u => u.Id == id);
            if ( entity == null )
                return;

            entity.DeleteImage();
            entity.Conditions.DeleteRegulations();
            _context.Match.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync ( MatchFirstInfoDto firstInfo , MatchSecondInfoDto secondInfo , MatchConditionDto condition , MatchDocumentDto document , MatchAwardListDto award )
        {
            var entity = await _context.Match
                            .Include(u => u.Conditions)
                            .Include(u => u.Documents)
                            .Include(u => u.Awards)
                            .Include(u => u.AudienceTypes)
                            .FirstOrDefaultAsync(u => u.Id == firstInfo.Id);

            if ( entity == null )
                return;


            var startPutOn = DateTimeConvertor.GetDateFromString(firstInfo.StartPutOn);
            var endPutOn = DateTimeConvertor.GetDateFromString(firstInfo.EndPutOn);
            var startRegister = DateTimeConvertor.GetDateFromString(firstInfo.StartRegister);
            var endRegister = DateTimeConvertor.GetDateFromString(firstInfo.EndRegister);

            entity.WithName(firstInfo.Name)
                .WithCapacity(firstInfo.Capacity)
                .WithLevel(firstInfo.Level)
                .WithTeamCount(firstInfo.TeamCount)
                .WithGender(firstInfo.Gender)
                .WithRegister(new DateTimeRange(startRegister , endRegister))
                .WithPutOn(new DateTimeRange(startPutOn , endPutOn))
                .WithFestivalId(firstInfo.FestivalId)
                .WithPlaceId(firstInfo.PlaceId)
                .WithSportId(firstInfo.SportId)
                .WithDescription(secondInfo.Description);

            if ( secondInfo.ImageFile != null )
            {
                entity.DeleteImage();
                entity.WithImage(secondInfo.ImageFile == null ? entity.Image : new Document(secondInfo.Image , secondInfo.ImageFile));
                entity.SaveImage();
            }

            entity.Conditions.WithFree(condition.Free)
                .WithPayment(condition.Payment.HasValue && !condition.Free ? condition.Payment.Value : 0);

            if ( condition.RGFile != null )
            {
                entity.Conditions.DeleteRegulations();
                entity.Conditions.WithRegulations(new Document(condition.File , condition.RGFile));
                entity.Conditions.SaveRegulations();
            }

            var aud = firstInfo.Audience.Split(',').ToList();
            entity.AudienceTypes.ToList().ForEach(item => _context.MatchAudienceType.Remove(item));
            if ( aud != null )
                aud.ToList().ForEach(item => entity.AddAudienceType(new MatchAudienceType(entity.Id , long.Parse(item))));


            entity.Documents.ToList().ForEach(doc => _context.MatchDocument.Remove(doc));
            if ( document.Info != null )
                document.Info.ToList().ForEach(doc => _context.MatchDocument.Add(new MatchDocument(doc.Type , entity.Id , doc.EvidenceId)));

            entity.Awards.ToList().ForEach(awa => _context.MatchAward.Remove(awa));
            if ( award.Info != null )
                award.Info.ToList().ForEach(awa => _context.MatchAward.Add(new MatchAward(( byte ) awa.Rank , awa.Prize , entity.Id)));

            _context.Match.Update(entity);
            _context.MatchConditions.Update(entity.Conditions);
            await _context.SaveChangesAsync();
        }
    }
}
