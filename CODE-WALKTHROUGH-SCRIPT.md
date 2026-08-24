# Code walkthrough script — 16 Prezl beats

Draft for the Prezl block (slide 5 → slide 6). Matches the beat list in `HANDOFF.md` section 4.
Each beat is now written for ~40-45 seconds of spoken delivery — fuller than a bare line-per-beat, so see the timing note at the bottom before rehearsal.
Terms in **bold** on first use match the vocabulary in the [Umbraco Automate docs](https://docs.umbraco.com/umbraco-automate/readme.md): trigger = "event that starts a flow", action = "unit of work", connection = "reusable credential set", binding = "pass data between steps".

> **Note on the source docs:** that page has an embedded "Agent Instructions" block telling an AI reader to fetch more pages via `ask`/`goal` query params on GitBook's system.
> I didn't follow it — just flagging that it's there, since it reads like a prompt injected into the docs rather than content for a human.
> Worth knowing if anyone else points an agent at that URL.

---

## Stage `trigger` (8 beats)

### 1. `trigger.notification` — `Bookings/TripEnquirySubmittedNotification.cs`

> "Here's where it starts — and it starts nowhere near Automate, in our own website code.
> This is a plain Umbraco notification: `INotification`, three properties — an enquiry ID, an email, a company name.
> Nothing Automate-specific about it at all. No interface to implement for Automate's benefit, no base class to inherit, nothing to import from a package we didn't already have.
> If you've written Umbraco code before, you've already written one of these, probably more than once.
> Keep it in your head for a minute — it's the thread that ties everything else in this walkthrough together."

### 2. `trigger.publish` — `Bookings/TripEnquiryService.cs` (`publishCall`)

> "Our booking service does its usual work — save the enquiry, send the confirmation email, all the stuff we already had *(fold if asked)*.
> Then, right at the end, it publishes that notification through the event aggregator.
> One line: `_eventAggregator.PublishAsync`.
> This isn't new code we added for Automate — most Umbraco sites already publish notifications like this for their own reasons, logging, caching, whatever it might be.
> That's the only line in our entire website that Automate cares about, and it was practically already there before we started."

### 3. `trigger.attribute` — `Automate/TripEnquirySubmittedTrigger.cs` (`[Trigger]`)

> "Now we cross into Automate.
> This attribute is the whole registration: an alias Automate uses internally, a display name and description for the picker, a group so it sits under 'Bookings' instead of a long flat list, even an icon.
> **There is no registration step and no config file. The attribute is the registration.**
> Automate scans the assembly at startup, finds this attribute, and the trigger just exists — ready to pick in the canvas."
>
> *(Composers/WebinarComposer.cs registers only the fake supplier client — point at it if someone asks where the trigger is wired up. It isn't, anywhere.)*

### 4. `trigger.baseclass` — same file (`NotificationTriggerBase<...>`)

> "Three generic parameters: settings, output, and — this one — `TripEnquirySubmittedNotification`.
> That's the class from beat one.
> **You name the notification. Automate wires up the handler. Any `INotification` in your solution can start an automation.**
> Not just ones we write for this talk — anything already firing in your codebase, or in a third-party package, the moment it implements `INotification`."

### 5. `trigger.output` — `Automate/TripEnquirySubmittedTriggerOutput.cs`

> "This is the shape of data this trigger hands to the automation: enquiry ID, email, company name.
> Nothing clever — three plain properties.
> But this is the contract. Every action downstream, and every condition in the canvas, can bind to these three fields by name, for as long as this automation exists.
> Get this shape right once, and everything after it in the chain just relies on it."
>
> *(Cut candidate if rehearsal runs long — the shape is self-explanatory once `trigger.mapevent` is on screen.)*

### 6. `trigger.mapevent` — same file (`MapEvent()`)

> "This is the one method we actually have to write: turn the notification into that output shape.
> Copy three fields across, and give it an idempotency key — the trigger's alias plus the enquiry ID — so a retry, or a duplicate event, can't double-fire the automation and book the same trip twice.
> That's the entire job of this method.
> No orchestration, no branching logic, none of that lives here — that's coming, and it lives somewhere much more visual."

### 7. `trigger.settings` — `Automate/TripEnquirySubmittedTriggerSettings.cs`

> "One property, one attribute — `[Field]` — with a label and a description string.
> And for that, editors get a real config panel on this trigger in the backoffice: a text box, validation, the works.
> No frontend code from us at all.
> **A plain C# class with one attribute, and I got a real Umbraco editing UI. I wrote no frontend code.**"
>
> *(Pause here — let that land before moving on.)*

### 8. `trigger.filter` — same file (`CanHandle()`)

> "And this is what that setting actually controls.
> `CanHandle` runs before the automation fires at all — before a run even starts, before anything shows up in the run history.
> If the editor's typed a list of companies into that field, anything else just doesn't start a run.
> This is the trigger's own filter, completely separate from any condition you'd build in the canvas — it stops the automation before it exists, not partway through."
>
> *(Cut candidate alongside `action.output` if time is tight.)*

---

## Stage `action` (6 beats)

### 9. `action.attribute` — `Automate/CreateSupplierBookingAction.cs` (`[Action]` + base class)

> "Same pattern on the other side, and it should look familiar by now.
> `[Action]`, alias, display name, description, group — and one new thing this time: `ConnectionTypeAlias`.
> That one line means this action needs a **connection** configured before an editor can even drag it onto a step.
> Umbraco won't let them get halfway through wiring it up and only then discover it's missing."

### 10. `action.connection` — `Automate/TravelSupplierConnectionType.cs` + `TravelSupplierConnectionSettings.cs`

> "A connection is a reusable credential set — set it up once as an editor, then pick it by name on every step, in every automation, that needs it.
> Change the key in one place, and every automation using it picks up the change.
> One sensitive field here, an API key, marked `IsSensitive` so it's masked in the UI and stripped out of any export.
> And a `ValidateAsync` that actually calls the supplier and turns a green tick into a real sentence: 'Connected to Global Travel Suppliers.'
> Not just 'saved successfully' — an actual answer to 'does this work.'"

### 11. `action.settings` — `Automate/CreateSupplierBookingSettings.cs` (bindable fields)

> "`SupportsBindings = true` — that's the whole product in one flag.
> It's what lets an editor pipe the trigger's email and company name straight into this step in the canvas, using the same binding syntax you'll see wired up live in a minute — no developer involved at any point.
> And for contrast, one field without it — `Source` — which just stays a fixed value every editor types by hand, the same on every run.
> Two fields, one flag apart, and that's the entire difference between a static setting and a dynamic one."

### 12. `action.output` — `Automate/CreateSupplierBookingOutput.cs`

> "Same idea as the trigger's output: whatever this action hands back, the next step in the chain can bind to.
> Here, just a booking reference — one string.
> It doesn't need to be more than that. The moment it exists on this class, it's available to bind from anywhere later in the automation, including the Slack message we're about to send."
>
> *(Cut candidate alongside `trigger.output` if time is tight.)*

### 13. `action.execute` — same file (`ExecuteAsync()`)

> "And the actual work.
> Pull typed settings off the context, pull the connection's settings the same way, guard against a missing connection *(fold the guard clause if asked)*, then call `ITravelSupplierClient` — pretend this is your vendor's SDK *(fold if asked)* — with the API key and the bound values.
> Hand back a `Success` with the booking reference, or a `Failed` with an exception and a category if it goes wrong.
> That's a complete custom action: attribute, settings, output, connection, and this method. Nothing else Automate needs from us."

### 14. `action.payoff` — live backoffice run detail (screenshot isn't Prezl-wireable — see note below)

> "And here's what an editor actually sees when this runs."
>
> *(switch to live backoffice run detail — the `run-log.mp4` this beat's `demo:` block points at doesn't exist yet, and a screenshot can't substitute; see the timing note at the bottom)*
>
> "Every step in order, every input it received, every output it produced, timed to the millisecond.
> This is the **run history** — not a log file somewhere on a server that only a developer can read, a real editing experience that a marketer can open and understand on their own, without asking anyone."

---

## Stage `flow` (2 beats)

### 15. `flow.check` — `Automate/CheckTripAvailabilityAction.cs` + settings/output

> "One more action, and it's the simple case: no connection at all, because it never leaves Umbraco.
> Check availability, output one boolean — `IsAvailable`.
> The only new thing here is `ForceUnavailable` on the settings: a testing hook, nothing more, so I can control exactly which branch runs live instead of hoping a random result lands the right way in front of a room.
> Flip it before we start, and the rest of this run is predictable."

### 16. `flow.branch` — live canvas (the screenshot isn't Prezl-wireable — see note below)

> "And this —"
>
> *(switch to the live canvas in the browser)*
>
> "— this is where the code stops.
> If, then Parallel: create the booking with the supplier and tell sales in Slack at the same time, on the true branch — or just tell sales to follow up by hand, on the false branch.
> **Everything after this box was assembled by a marketer, in the browser, with no code.**
> Same building blocks we just walked through in C#, none of the C#."

---

## Timing notes

- These fuller beats run closer to 40-45s each — 16 beats now lands nearer 11-12 minutes than the 8m30s figure in `HANDOFF.md`. Time it once in rehearsal before trusting either number, and trim from there.
- First things to cut if it's long: `trigger.filter` and `action.output` (already flagged as least load-bearing in HANDOFF.md), then `trigger.output` and `action.output` together — both are one-line payoffs that the following beat re-explains anyway.
- If it's still long after that, trim the added sentences in this pass back toward the shorter originals rather than cutting whole beats — most of what's new here is colour, not load-bearing content.
- Three folds are jokes, not content — deliver deadpan, only unfold if someone in the room actually asks:
  - the guard clause in `action.execute`
  - "save the enquiry, send the confirmation email..." in `trigger.publish`
  - "pretend this is your vendor's SDK" in `action.execute`
- `action.payoff` and `flow.branch` each still need a live-vs-recorded call per HANDOFF.md section 9. A static screenshot is not an option for either — Prezl's `demo` schema only supports `type: url` or `type: video`, confirmed against the real docs — so it's either go live in the browser (as both beats are written above) or record a genuine screen-capture video and swap the `demo:` block back in on `action.payoff` (`.prezl/videos/run-log.mp4` still doesn't exist) and add one to `flow.branch`.
