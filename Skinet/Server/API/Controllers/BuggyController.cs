﻿using Microsoft.AspNetCore.Mvc;
using Skinet.Infrastructure.Data;
using Skinet.WebAPI.Errors;

namespace Skinet.WebAPI.Controllers
{
    public class BuggyController : BaseController
    {
        public readonly StoreContext context;
        public BuggyController(StoreContext context)
        {
            this.context = context;
        }

        [HttpGet("notfound")]
        public ActionResult GetNotFound()
        {
            var thing = context.Products.Find(42);

            if (thing == null)
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));
            }

            return Ok();
        }

        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var thing = context.Products.Find(42);

            var thingToReturn = thing.ToString();

            return Ok();
        }
        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest));
        }

        [HttpGet("badrequest/{id}")]
        public ActionResult GetNotFoundRequest(int id)
        {
            return Ok();
        }

    }
}