# Testing OData and Postgres

I'm using a non-deterministic collation for all my strings in the database, but this causes problems when using OData and $filter. Whenever the filter translates into a LIKE statement, I get the error: "nondeterministic collations are not supported for LIKE". E.g. 

```
$filter=startsWith(Description, 'The first')
```

This is just a test project to recreate this error in a smaller context. The project has a DbContext and a migrations to setup a database with test data.
