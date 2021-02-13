








## Frontend

- Forms Engine
- edited items are staged and synced across devices


## Api Layer

api/v1/projects/<project code>/notifications (ws)
- receive push on staged workitems
- receive push on workitems

api/v1/projects/<project code>/workitems/<work item id>
- GET Data
- POST changes (return errors)
- POST changes in staging (return errors)

api/v1/projects/<project code>/workitems/<work item id>/descriptor
- GET interpreted descriptor of properties/commands

api/v1/projects/<project code>/workitems/<work item id>/properties/<name>/provider?query=..
- GET value provider values of the properties