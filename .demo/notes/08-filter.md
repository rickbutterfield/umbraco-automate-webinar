And then the other method, `CanHandle`, is what that setting actually controls.

`CanHandle` runs before the automation fires at all. Before a run even starts, before anything shows up in the run history.
If the editor's typed a list of companies into that field, anything else just doesn't start a run.

This is the trigger's own filter, completely separate from any condition you'd build in the canvas — it stops the automation before it exists, not partway through.

Most triggers don't need this at all — the default is just "always fire." When you do override it, it runs once per automation subscribed to this trigger, each with its own settings. And it's a different concern from the authorizer on slide 9: this is your business logic, that's a security gate.