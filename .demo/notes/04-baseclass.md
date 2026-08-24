Three generic parameters: settings, output, and — this one — `TripEnquirySubmittedNotification`.

You name the notification. Automate wires up the handler. Any `INotification` in your solution can start an automation.

Not just ones we write for this talk — anything already firing in your codebase, or in a third-party package, the moment it implements `INotification`.