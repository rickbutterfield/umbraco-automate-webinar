One final thing we're adding for this Action is a `Connection` — a reusable credential set that an editor can configure once and then pick by name on every step, in every automation, that needs it. It inherits from `ConnectionTypeBase` and is decorated with a `ConnectionTypeAttribute` that registers it with Automate, just like the trigger and action attributes.

It only has one type parameter, `TravelSupplierConnectionSettings`, which is a class with one property: the API key. The property is decorated with a `[Field]` attribute, just like the trigger and action settings, and that field shows up in the backoffice for an editor to type in.

Connections can be created in the backoffice, and they can be shared across automations. Change the key in one place, and every automation using it picks up the change. We support sensitive fields in connections, so adding `IsSensitive = true` to the `[Field]` attribute masks the API key in the UI and encrypts it at rest. Getting it out of a Deploy export needs one more step, on the deploy side: `Umbraco:Deploy:Automate:Connections:IgnoreSensitive` enabled, since that's off by default.

And our `ConnectionType`s allow overriding one method - `ValidateAsync` which actually calls the supplier and turns a green tick into a real sentence: 'Connected to Global Travel Suppliers.'

Not just 'saved successfully' — an actual answer to 'does this work.'

---
- Missing since the reorder: the fact that an action with a `ConnectionTypeAlias` (shown back in beat 9) won't even appear in the step picker unless the workspace has a connection of that matching type. Confirmed against the real picker code — it's filtered out of the catalogue, not just blocked after the fact. Worth a sentence here tying back to beat 9's attribute.