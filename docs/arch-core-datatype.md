## The core type: WorkItem

A WorkItem is found in a *Project* and has within this project an *Identifier*. Each WorkItem hosts a series of *Properties* which each have a *Value*. Any change to the properties of the WorkItem in addition to arbitrary comment is *logged*.

### Why no State?

A state implies having a decided concept how to handle the state. Different specialization might need different models for the states. A issue might have a simple open/closed state. But a bug can already have multiple states depending on the amount of branches of a software in which the bug occurs. Compared to the project and identifier (the only static properties of WorkItem), the identifier is not standardized enough to be statically described.

### Regards Naming

- **WorkItem**: Every tracking system tracks a registered item which needs to be worked on (a.k.a. WorkItem). This should not be mixed up with the *work* as a result of the item. Alternative terms like Issue Tracker, Bug Tracker or Ticket System already specialize the kind of item which needs to be tracked. This framework intents to allow specialization on top of it but not be rooted in one.
- **Property**: A WorkItem has multiple properties. A property is more than a *field* which is more focused on a visualization (like an entry field or an excel field). A property owns a **Value** (and is not the same as the value).
- **Log(Entry)**: The log attached to a WorkItem. Not named *Change* since there might be entries without a change on the properties (e.g. comment only). Also not named *Audit* since not all systems require an auditable infrastructure.