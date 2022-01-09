using CMS.Entities;
using CMS.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace CMS.Controllers
{
    public class CardController : Controller
    {
        [ModelValidation]
        [HttpPost]
        public IActionResult CreateCard([FromBody] Card card)
        {
            if (!CardCollection.Cards.Any(c => c.Pan == card.Pan))
                CardCollection.Cards.Add(card);
            else
                return BadRequest();

            return Ok(card);
        }

        [HttpGet]
        public IActionResult GetCard(Guid userId)
        {
            var cards = CardCollection.Cards.Where(c => c.UserId == userId);

            if (cards == null)
                return NotFound();

            return Ok(cards);
        }
    }
}
