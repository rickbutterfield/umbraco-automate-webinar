We have the exact same pattern for `Actions` on the other side, so it should look familiar by now.

Every action is a class that implements `ActionBase` which has only two parameters this time: `Settings` and `Output`. The action is decorated with an `ActionAttribute` which registers it with Automate, and we add an alias, display name, description, group and icon.

This is everything we need to register an action. No config file, no registration step, no extra code. The attribute is the registration, the same as the trigger. Automate scans the assembly at startup, finds this attribute, and the action exists — ready to pick in the canvas.

---
- The highlighted code here still shows `ConnectionTypeAlias = "travel.supplierPortal"`, but nothing in this beat explains it anymore (moved to beat 12 in the reorder, but the actual explanation didn't come with it). That line is what makes the action invisible in the step picker without a matching connection in the workspace — confirmed against the real picker code. Either mention it briefly here, or make sure beat 12 covers it.