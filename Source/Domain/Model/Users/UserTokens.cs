using System;
using WebStudio.Entities.Core;

namespace BigCommerceApi.Domain.Model.Users
{
    public class UserTokens : Entity
    {
        public DateTime? TokenCreated { get; set; }
        public Guid? Token {  get; set; }
        public string? TokenNumber { get; set; }
        public string? PaymentType { get; set; }
        public DateTime? CreditCardExpirationDate { get; set; }
        public string? MaskedPan {  get; set; }
        public Guid? IdUser { get; set; }
        public User? User { get; set; }
    }
}
