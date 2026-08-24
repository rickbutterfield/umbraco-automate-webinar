using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Webinar.Bookings;
using Webinar.Fakes;
using Webinar.Supplier;

namespace Webinar.Composers;

/// <summary>
/// Registers only our own plumbing. Note what is NOT here: the trigger, the action and
/// the connection type need no registration at all — Automate discovers them from their
/// attributes at startup. That is worth saying out loud during the talk.
/// </summary>
public sealed class WebinarComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.Services.AddSingleton<ITravelSupplierClient, FakeTravelSupplierClient>();
        builder.Services.AddScoped<TripEnquiryService>();
    }
}
