using CMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CMS
{
    public static class CardCollection
    {
        public static List<Card> Cards { get; set; } = new List<Card>();

        public static IEnumerable<Card> GetCardByUserId(Guid userId)
        {
            return Cards.Where(c => c.UserId == userId);
        }

        public static void AddCard(Card card)
        {
            Cards.Add(card);
        }
    }
}
