
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

#if DEBUG
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);
#endif

builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddComposers()
    .Build();

WebApplication app = builder.Build();


await app.BootUmbracoAsync();


app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

// Stand-in for a real "book a trip" enquiry form. Hit this in the browser during the talk to
// fire TripEnquirySubmittedNotification, which is what starts the automation.
//   /trip-enquiry?email=rick@example.com&company=Acme%20Ltd
app.MapGet("/trip-enquiry", async (
    string email,
    string company,
    Webinar.Bookings.TripEnquiryService bookings,
    CancellationToken ct) =>
{
    await bookings.SubmitEnquiryAsync(email, company, ct);
    return Results.Ok(new { submitted = true, email, company });
});

await app.RunAsync();
