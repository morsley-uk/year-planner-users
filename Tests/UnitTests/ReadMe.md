# Unit Tests

A level of the software testing process where individual units/components of a software/system are tested. The purpose is to validate that each unit of the software performs as designed.

## API


## User Repository

Our implementation of the Unit of Work & Repository patterns, are such that certain repository methods are not unit testable.

That is because the saving to the context is deferred to the Unit of Work's 'Complete' method. 

These methods include:

* Create
* Delete

Also, if updating an entity, no call to an 'Update' method is required. You simply get the entity, alter it, and then call the Unit of 
Work's 'Complete' method.

The above, therefore, must be tested during integration.