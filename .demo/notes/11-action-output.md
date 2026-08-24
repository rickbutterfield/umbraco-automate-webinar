And then for an action's `Output`, it's the same idea as a trigger's output: whatever this action hands back, the next step in the chain can bind to.

Here, we just need one item back from our supplier, a booking reference — one string.

It doesn't need to be more than that. The moment it exists on this class, it's available to bind from anywhere later in the automation, including a Slack message we're about to send.