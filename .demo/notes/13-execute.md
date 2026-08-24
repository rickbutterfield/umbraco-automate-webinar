Finally, back to our Action, and to doing the actual work in the `ExecuteAsync` method.

Pull typed settings off the context, pull the connection's settings the same way, guard against a missing connection, then call `ITravelSupplierClient`, our vendor's SDK, with the API key and the bound values.

Once a booking is made, we hand back a `Success` with the booking reference, or a `Failed` with an exception and a category if it goes wrong.

That's a complete custom action: attribute, settings, output, connection, and this method. Nothing else Automate needs from us.

---
- "Then call `ITravelSupplierClient`, our vendor's SDK" states the fake stand-in as literally real. `Supplier/ITravelSupplierClient.cs`'s own doc comment says it "stands in for whatever supplier SDK you actually use — not the interesting part," and it's still one of `HANDOFF.md`'s three deliberate jokes ("pretend this is your vendor's SDK"). Either restore "pretend," or drop it from HANDOFF's joke list so the two agree.