# Webinar handoff: "How to build your own triggers and actions"

**Presenter:** Rick Butterfield · **Slot:** 15 minutes · **Prepared:** 19-08-2026

Webinar framing (from the published description): _"It allows you to setup native and external triggers and actions to optimize your work processes and customer journeys directly in Umbraco."_

Audience: developers **and** technical marketing profiles.

---

## 1. The through-line

Say this sentence three times across the talk:

> **The developer ships the building blocks. The marketer assembles the automation.**

It is what makes a mixed room work. Developers get the code; marketing gets why it matters that the code exists.

## 2. The scenario

**A company submits a trip enquiry. Check supplier availability. If available, book it with the supplier and tell sales in Slack at the same time. If not, just tell sales to follow up by hand.**

| Piece | Type | Status |
|---|---|---|
| `TripEnquirySubmittedNotification` to **Trip Enquiry Submitted** trigger | custom, native | built, compiles, runs |
| **Check Trip Availability** action | custom, native | built, compiles, runs |
| **Create Supplier Booking** action | custom, external | built, compiles, runs |
| **Global Travel Suppliers** connection type | custom | built, validates green |
| **Slack Send Message** | ships in `Umbraco.Automate.Slack` | referenced |

Chosen because Forms / Commerce / Engage already ship their own triggers, so building one of those would have been dishonest. This trigger genuinely does not exist anywhere.

The If/Parallel branch after "Check Trip Availability" is assembled live in the backoffice automation canvas — Umbraco Automate ships those control-flow step types already, so nothing here writes workflow config in code. "Force unavailable" on the availability action's settings is a deliberate testing hook: flip it before the talk to control which branch runs, instead of hoping a random result lands the right way live.

## 3. Running order

10 deck slides with the Demo Time code walkthrough in the middle.

| Time | Slide | Content | Link? |
|---|---|---|---|
| 0:00 | 1 | Finished automation, screenshot | |
| 0:30 | 2 | Green run log, screenshot | |
| 1:00 | 3 | The native/external 2x2 | |
| 1:45 | 4 | 2x2 filled in with what ships | |
| 2:30 | 5 | "Check before you build" | **to `trigger.notification`** |
| 3:00 | -- | **Demo Time, 16 beats** | |
| 11:30 | 6 | OAuth comes free | |
| 12:20 | 7 | Binding filters | |
| 13:10 | 8 | Reuse Umbraco's property editors | |
| 14:00 | 9 | "And there's more" (10 bullets) | |
| 14:30 | 10 | Where to go next | |

**Only slide 5 carries a link.** Shift+Esc returns you to slide 5; the next press goes to slide 6.

**Timing note:** adding the `flow` stage (2 beats) pushed the code walkthrough block from 8 minutes to about 8m30s -- and CODE-WALKTHROUGH-SCRIPT.md's fuller beats push it closer to 11-12 minutes; re-time in rehearsal before trusting either number. Slides 6-8 above are trimmed from 60s to ~50s each to help absorb it. If it's still tight, cut `trigger.filter` and `action.output` live (see below) to claw back time.

### The 2x2 (slides 3 and 4)

|  | Trigger | Action |
|---|---|---|
| **Native** | Something happened in Umbraco | Do something in Umbraco |
| **External** | Another system calls in | Call another system |

### Slide 9, the full list

More ways to start an automation: `ScheduledTriggerBase`, `WebhookTriggerBase`, `IWebhookAuthenticator`, `IEventTrigger<T>`

Control what runs: `ITriggerDispatchAuthorizer`, `IAutomationOriginatedEventBehavior`, `IActionMiddleware`

Free polish: `DynamicOutputActionBase`, `ICmsAction`, `ISensitiveSettingsStripper`

> `IAutomationOriginatedEventBehavior` goes on the trigger **settings** class, not the trigger.

## 4. The 16 code walkthrough beats

**Stage `trigger` (8):** notification, publish, attribute, baseclass, output, mapevent, settings, filter

**Stage `action` (6):** attribute, connection, settings, output, execute, payoff

**Stage `flow` (2):** check, branch

About 32 seconds per beat on the original estimate (see the timing note in section 3 -- the fuller script runs longer). If rehearsal feels tight, cut `trigger.filter` and `action.output` -- the two least load-bearing.

Three lines are deliberate jokes, left as ordinary comments in the code. There's no tool-level fold/reveal for these now (that was a Prezl-specific mechanic, and Prezl is no longer part of this setup) -- they're just always visible, so read past them naturally and only call them out if someone in the room asks:

- "the boring guard clause" in `ExecuteAsync`
- "save the enquiry, send the confirmation email, all the stuff we already had"
- "ITravelSupplierClient -- pretend this is your vendor's SDK"

### Lines worth saying out loud

- At `trigger.baseclass`: "You name the notification. Automate wires up the handler. **Any `INotification` in your solution can start an automation.**"
- At `trigger.attribute`: "There is no registration step and no config file. The attribute **is** the registration." (`Composers/WebinarComposer.cs` registers only the fake supplier client. The trigger, action and connection type need nothing.)
- At `trigger.settings`: "A plain C# class with one attribute, and I got a real Umbraco editing UI. I wrote no frontend code." Then pause.
- At `flow.branch`: "Everything after this box was assembled by a marketer, in the browser, with no code."

## 5. What is where

```
D:\DXP\Automate\webinar-triggers-actions\      <- repo root: presentation config lives here
  .demo/                     Demo Time config: 1.trigger.json, 2.action.json, 3.flow.json (16 beats total)
  .demo/notes/               one markdown file per beat, the CODE-WALKTHROUGH-SCRIPT.md lines
  HANDOFF.md       this file

  src/                       the actual Umbraco site -- everything below is here now, not at repo root
    Webinar.Site.csproj        Umbraco 18 web app (net10.0)
    Program.cs                 plus GET /trip-enquiry endpoint
    appsettings.Development.json
    Properties/launchSettings.json   profile "Webinar", port 44390
    Bookings/        2 files   "Our website"
    Automate/       11 files   "Our Automate extensions"
    Supplier/        1 file    "Supplier SDK"
    Composers/, Fakes/, Views/, wwwroot/       not part of the code walkthrough
    umbraco/                  Umbraco's own runtime data, including the SQLite DB with everything built live
```

14 source files, all under `src/` now. Each Demo Time beat highlights one file via `startPlaceholder`/`endPlaceholder` -- literal text already in the file, no special comment markers needed (Prezl's `@prezl` directive comments have been removed from all 14 files and from `prezl.yaml`, which no longer exists). Demo Time's `path` values carry the `src/` prefix; the `.demo/` config itself stays at repo root.

## 6. How to run it

**The site** (must be Development, or Automate throws on the connection string):

```
cd D:\DXP\Automate\webinar-triggers-actions\src
dotnet run --launch-profile Webinar
```

Then https://localhost:44390/umbraco with `admin@example.com` / `password1234`

Fire the trigger from the browser address bar:

```
https://localhost:44390/trip-enquiry?email=rick@example.com&company=Acme%20Ltd
```

**Demo Time:**

Open this folder in VS Code (Demo Time extension installed). Start from the Demo Time sidebar or the `Demo Time: Start` command, then press → (or use Next) to advance one beat at a time -- each beat is its own `demo` entry across the three `.demo/*.json` files, so Next pauses on every one. Each beat's script line shows automatically as a note (`showOnTrigger: true`).

## 7. Verified

- `dotnet build` succeeds, 0 errors, after the travel-agency rename and the new `CheckTripAvailabilityAction`.
- Umbraco 18 unattended install completes.
- All 7 Automate migrations run; 20 tables created including OpenIddict credentials.
- Automate section assigned to the Admin group.
- Backoffice returns 200.
- "Trip Enquiry Submitted", "Check Trip Availability" and "Create Supplier Booking" all appear correctly in the backoffice pickers, grouped under **Bookings**.
- The **Global Travel Suppliers** connection validates green: "Connected to Global Travel Suppliers."
- `/trip-enquiry?email=...&company=...` returns `{"submitted":true,...}` and fires the notification.
- The full automation is built and published: Trip Enquiry Submitted -> Check Trip Availability -> If -> Parallel [Create Supplier Booking, Send Slack Message] on true, Send Slack Message on false.
- Ran it live: Check Trip Availability and the If condition (`steps.checkTripAvailability.isAvailable == true`) both evaluated correctly and routed into the Parallel branch; Create Supplier Booking completed successfully inside it.
- `.prezl/images/branching-automation.png` now exists (captured from the live canvas, at device-pixel resolution), with the If's true/false branches laid out as clean, separate boxes -- no overlap.
- **Slack is now connected to a real workspace.** Both Send Slack Message steps post to `#rick-test-channel`. Run `af5c3f7c` (20-08-2026, 14:39:58) is fully green end to end: Check Trip Availability -> If -> Parallel [Create Supplier Booking, Send Slack Message] all completed. This is the run to use for the "green run log" screenshot on slide 2.

## 8. NOT verified -- do these first

1. **Does the per-beat pause actually work?** An earlier version of the Demo Time config grouped all of a stage's beats into one `demo`, which ran them all automatically with no pause between -- confirmed against Demo Time's own docs that a `demo`/`scene` is the pause unit, not the `step`/`move` inside it. It's now restructured to one beat per `demo` (16 total across the three files), but this hasn't been confirmed live in VS Code since the fix.
2. **Do the presenter notes actually show automatically?** Each beat has a `notes` block with `showOnTrigger: true`. Confirmed against the schema that the field exists and takes this shape; not yet confirmed that it actually pops up per beat during a real run, in both standard and presenter view.

## 9. Open decisions

- **`action.payoff` and `flow.branch` are currently `waitForInput` placeholders** (`.demo/2.action.json`, `.demo/3.flow.json`) telling the presenter to switch to the live backoffice/canvas in a browser. Worth reconsidering: Demo Time has a real `imagePreview` action (`path` to any image file, confirmed against its docs) that Prezl never had an equivalent for. Both screenshots already exist -- `.prezl/images/run-log.png` and `.prezl/images/branching-automation.png` -- and could be wired in directly instead of going live. Not decided, and not yet changed in the config.
- **Port 44390 is held by a stale `http.sys` reservation** (owner PID 4, answers 404 to everything). It may block the site binding on the day. 44391, 44395, 44400 and 45080 were free. Moving to 44400 was proposed, not done.

## 10. Google Slides: what is actually possible

Slides can embed **only** YouTube video, Google Drive video, and Sheets charts. No iframes, no web pages, no desktop apps. So code reaches a slide as:

- an **image** (screenshot from VS Code), or
- **text** (paste; the free Code Blocks add-on colours it; 16pt minimum).

Prezl had a registered `prezl://` protocol handler that let a slide link out and reopen the app at a specific beat. That option is gone along with Prezl, and it's not yet checked whether Demo Time (a VS Code extension, not a standalone app) has any equivalent deep link. Until that's checked, assume there is no link-out option -- image or text only.

Consider a hidden backup slide with a screenshot of `action.attribute`'s code, so a mid-talk problem in VS Code lets you talk through it from a static fallback instead of restarting.

## 11. Flags before anything goes public

- **Confirm what has shipped.** The product page names Forms, Commerce and Engage triggers as available; `docs/engineering-spec.md:2222` still lists those packages under "Phase 4: DXP Providers". Check with those teams before slide 5 promises them.
- **"Unlike other SaaS automation platforms"** (from the product page) is a competitor comparison. Needs marketing or legal sign-off before external use.
- Per org policy, this deck is external-facing and should get a human review pass before it ships.

## 12. Bugs and doc drift found along the way

- **Custom webhook triggers cannot produce typed output.** `Umbraco.Automate.Web/Api/Webhook/Controllers/WebhookEndpointController.cs:217` hardcodes `WebhookTriggerOutput`. `WebhookTriggerBase` exists but nothing maps a payload into it. Do not demo a custom webhook trigger. Worth raising as a real product gap.
- **`ActionResult.Failure` does not exist.** It is `ActionResult.Failed(exception, category)`.
- **Root `CLAUDE.md` says `[EditableModelField]`.** The real attribute is `[Field]` in `Umbraco.Automate.Core.Settings`.
- **Six NuGet security advisories** in the dependency graph (`System.Security.Cryptography.Xml` 10.0.7, `Microsoft.OpenApi` 2.0.0, `SQLitePCLRaw.lib.e_sqlite3` 2.1.11, `OpenTelemetry.Api` 1.1.0). All transitive. No versions were changed. Worth a separate look by the team.
- `NU1507`: central package management with three NuGet feeds and no source mapping. Cosmetic for a demo.

## 13. Housekeeping

This folder is **outside** the `Umbraco.Automate` git repo, so nothing here pollutes it. `src/Webinar.Site.csproj` references that repo by relative path (`..\..\Umbraco.Automate\...`, one level deeper than before now that the site lives under `src/`), so `webinar-triggers-actions/` must stay a sibling of the repo folder or the project reference breaks.
