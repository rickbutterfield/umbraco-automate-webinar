# Umbraco Automate: build your own triggers and actions

A working Umbraco 18 site demonstrating how to build a custom native trigger, a
custom native action, a custom external action, and a connection type for
[Umbraco Automate](https://docs.umbraco.com/umbraco-automate/readme.md) — built
for a webinar of the same name.

**The scenario:** a company submits a trip enquiry on the website. Automate checks
supplier availability. If available, it books the trip with the supplier and tells
sales in Slack at the same time. If not, it just tells sales to follow up by hand.

| Piece | Type |
|---|---|
| `TripEnquirySubmittedNotification` → **Trip Enquiry Submitted** trigger | custom, native |
| **Check Trip Availability** action | custom, native |
| **Create Supplier Booking** action | custom, external |
| **Global Travel Suppliers** connection type | custom |
| **Slack Send Message** | ships in `Umbraco.Automate.Slack` |

## Running the site

Requires the .NET 10 SDK.

```
cd src
dotnet run --launch-profile Webinar
```

Then open `https://localhost:44390/umbraco` (`admin@example.com` / see
`src/appsettings.Development.json` for the unattended-install password).

Fire the trigger either from the **Book a Trip** page the site ships with, or
directly from the browser address bar:

```
https://localhost:44390/trip-enquiry?email=you@example.com&company=Acme%20Ltd
```

### Slack

The demo automation includes two Slack Send Message steps. `src/appsettings.Local.json`
(gitignored, loaded only in `DEBUG` builds — see `src/Program.cs`) is where the real
Slack app's `ClientId`/`ClientSecret` live locally; it isn't checked in.

## Repo structure

```
.demo/                 Demo Time (VS Code extension) config for the code walkthrough:
                        1.trigger.json, 2.action.json, 3.flow.json — 16 beats total,
                        one per pausable step — plus .demo/notes/, the presenter
                        script for each beat.
src/                    The Umbraco site.
  Automate/             The custom trigger, actions, and connection type.
  Bookings/             Our own site code — the notification and the surface
                        controller behind the "Book a Trip" page.
  Supplier/, Fakes/     A stand-in for a real travel supplier SDK.
  Composers/            Registers the fake supplier client. Nothing else needs
                        registering — the trigger, action, and connection type
                        are discovered from their attributes at startup.
webinar-triggers-actions.slnx
```

## Notes

- The `Umbraco.Automate` NuGet packages are versioned in `Directory.Packages.props`.
  This repo doesn't depend on any sibling checkout of the `Umbraco.Automate` source.
- `[Field(IsSensitive = true)]` masks a connection setting in the UI and encrypts it
  at rest, but does **not** by itself strip it from an `Umbraco.Deploy.Automate` export
  — that needs `Umbraco:Deploy:Automate:Connections:IgnoreSensitive` enabled too (off
  by default). Set in `src/appsettings.json` for this project.
- The Forms/Commerce/Workflow/Engage Automate provider packages referenced in the
  walkthrough notes (`.demo/notes/01-notification.md`) are not all shipped yet —
  check current status before repeating that claim publicly.
