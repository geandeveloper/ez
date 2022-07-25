using System;

namespace EzCommon.Models
{
    public class EzPrincipal
    {
        public Guid? Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }

        public EzPrincipal(Guid? id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
    }
}
