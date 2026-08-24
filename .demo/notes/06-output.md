An `Output` is the shape of data this trigger hands to the automation. 

For this we are passing enquiry ID, email and company name. Nothing clever, just three plain properties.

But this is the contract. Every action downstream, and every condition in the canvas, can bind to these three fields by name, for as long as this automation exists.

Get this shape right once, and everything after it in the chain just relies on it.