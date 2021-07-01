using Microsoft.AspNetCore.Mvc;
using Moduit.Interview.Interfaces;
using Moduit.Interview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moduit.Interview.Controllers
{
    [Route("[controller]/question")]
    [ApiController]
    public class BackendController : ControllerBase
    {
        private readonly IRequester requester;

        public BackendController(IRequester requester)
        {
            this.requester = requester;
        }

        [HttpGet("one")]
        public async Task<IActionResult> Satu()
        {
            ObjectClass result;
            try
            {
                result = await requester.GetRequest<ObjectClass>(this.Request.Path.ToString().ToLower());
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return new JsonResult(result);
        }

        [HttpGet("two")]
        public async Task<IActionResult> Dua()
        {
            FilterObject result = new FilterObject
            {
                Filter = new FilterData
                {
                    Description = "Ergonomics",
                    Title = "Ergonomics",
                    Tags = new List<string>() { "Sports" }
                }
            };

            try
            {
                var openData = await requester.GetRequest<IEnumerable<ObjectClass>>(this.Request.Path.ToString().ToLower());

                result.Response = openData
                    .Where(c => (c.Description.Contains(result.Filter.Description, StringComparison.OrdinalIgnoreCase)
                        || c.Title.Contains(result.Filter.Title, StringComparison.OrdinalIgnoreCase))
                        && (c.Tags == null ? false : c.Tags.Any(t => result.Filter.Tags.Contains(t, StringComparer.OrdinalIgnoreCase)))
                    )
                    .OrderByDescending(c => c.Id)
                    .TakeLast(3)
                    .ToArray();
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return new JsonResult(result);
        }

        [HttpGet("three")]
        public async Task<IActionResult> Tiga()
        {
            IEnumerable<ObjectClass> result;
            try
            {
                var openData = await requester.GetRequest<IEnumerable<ObjectClass>>(this.Request.Path.ToString().ToLower());

                result = openData
                    .AsParallel()
                    .Where(c => (c.Items != null) && c.Items.Any())
                    .SelectMany(c => c.Items, (data, item) => new ObjectClass
                    {
                        Id = data.Id,
                        Category = data.Category,
                        Tags = data.Tags,
                        CreatedAt = data.CreatedAt,

                        Title = item?.Title,
                        Description = item?.Description,
                        Footer = item?.Description
                    })
                    .Concat(
                        openData
                            .AsParallel()
                            .Where(c => (c.Items == null) || !c.Items.Any())
                            .Select(data => new ObjectClass
                            {
                                Id = data.Id,
                                Category = data.Category,
                                Tags = data.Tags,
                                CreatedAt = data.CreatedAt
                            })
                    )
                    .ToArray();
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return new JsonResult(result.ToArray());
        }
    }
}