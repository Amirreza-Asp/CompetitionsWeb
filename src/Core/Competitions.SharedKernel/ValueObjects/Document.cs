using Ardalis.GuardClauses;
using Competitions.SharedKernel.Common;
using Competitions.SharedKernel.ValueObjects.Guards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Competitions.SharedKernel.ValueObjects
{
    public class Document : ValueObject<Document>
    {
        public Document(string name, byte[] file)
        {

            name = Guard.Against.InvalidFileName(name);
            Name = $"{Guid.NewGuid()}{Path.GetExtension(name)}";
            File = Guard.Against.NullOrEmpty(file).ToArray();
        }

        private Document() { }

        public string Name { get; }
        public byte[] File { get; }

        protected override bool EqualsCore(Document other)
        {
            return Name == other.Name && File == other.File;
        }

        public static implicit operator string(Document document) => document.Name;
    }
}
