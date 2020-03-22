using System;
using Budget.Facets;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Web
{
    [ApiController]
    public class DAtaSourcesController
    {
        private readonly ITagsProvider _tags;

        public DAtaSourcesController(ITagsProvider tags) => _tags = tags;

        [Route("/datasources/tags")]
        public string[] Tags() => _tags.Tags();

        [Route("/datasources/dateRange")]
        public string[] DateRange() => Enum.GetNames(typeof(Dates.Range));

        [Route("/datasources/dateResolution")]
        public string[] DateResolution() => Enum.GetNames(typeof(Dates.Resolution));
    }
}
