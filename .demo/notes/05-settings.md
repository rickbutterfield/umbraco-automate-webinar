`Settings` are another class we need to create. Each property on this class is a field in the backoffice editor for this trigger. The property type determines the field type, and the property name is used as the alias for that field in the automation canvas.

One property, one attribute — `[Field]` — with a label and a description string.

And for that, editors get a real config panel on this trigger in the backoffice: a text box, validation, the works.
No frontend code from us at all.

**A plain C# class with one attribute, and I got a real Umbraco editing UI. I wrote no frontend code. If you've created custom `FieldType`s or `WorkflowType`s in Forms, or custom property editors in Umbraco, you know how much work that usually is. This is a huge win for us.**