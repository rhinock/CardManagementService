using CMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CMS
{
    public static class CardCollection
    {
        public static List<Card> Cards { get; set; } = new List<Card>();
        //{
        //    new Card()
        //    {
        //        Id = Guid.NewGuid(),
        //        Cvc = "101",
        //        Pan = "0000 0000 0000 0101",
        //        Expire = new Expire(12, 2021),
        //        Name = "test1",
        //        IsDefault = true,
        //        UserId = new Guid("4a9623ae-6676-494d-b91a-d1b9505985cc")
        //    },
        //    new Card()
        //    {
        //        Id = Guid.NewGuid(),
        //        Cvc = "102",
        //        Pan = "0000 0000 0000 0102",
        //        Expire = new Expire(12, 2022),
        //        Name = "test2",
        //        IsDefault = false,
        //        UserId = new Guid("b7dc2008-c9a4-4cfd-a589-a6dd6235fda9")
        //    },
        //    new Card()
        //    {
        //        Id = Guid.NewGuid(),
        //        Cvc = "103",
        //        Pan = "0000 0000 0000 0103",
        //        Expire = new Expire(12, 2023),
        //        Name = "test3",
        //        IsDefault = false,
        //        UserId = new Guid("b7dc2008-c9a4-4cfd-a589-a6dd6235fda9")
        //    }
        //};

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
