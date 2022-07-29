using System;

namespace EzCommon.Models
{
    public class EzPrincipal
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string UserName { get; private set; }
        public string Email { get; private set; }

        public EzPrincipal(Guid id, string name, string userName,string email)
        {
            Id = id;
            Name = name;
            UserName = userName;
            Email = email;
        }
    }
}
