using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace Webinar.Bookings;

public sealed class BookATripSurfaceController : SurfaceController
{
    private readonly TripEnquiryService _bookings;

    public BookATripSurfaceController(
        IUmbracoContextAccessor umbracoContextAccessor,
        IUmbracoDatabaseFactory databaseFactory,
        ServiceContext services,
        AppCaches appCaches,
        IProfilingLogger profilingLogger,
        IPublishedUrlProvider publishedUrlProvider,
        TripEnquiryService bookings)
        : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        => _bookings = bookings;

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(string email, string company, CancellationToken ct)
    {
        await _bookings.SubmitEnquiryAsync(email, company, ct);
        TempData["Success"] = "true";
        return RedirectToCurrentUmbracoPage();
    }
}
