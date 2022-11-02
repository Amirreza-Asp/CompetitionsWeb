﻿using Competitions.SharedKernel.ValueObjects;

namespace Competitions.Domain.Entities.Managment.Events.Festivals
{
    public class SaveFestivalImageEvent : BaseDomainEvent
    {
        public SaveFestivalImageEvent ( Document document , string path )
        {
            Document = document;
            Path = Guard.Against.NullOrEmpty(path);
        }

        public Guid Id { get; private set; } = Guid.NewGuid();
        public Document Document { get; private set; }
        public string Path { get; private set; }
    }
}