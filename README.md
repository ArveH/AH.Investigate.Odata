# Testing OData and Postgres

I want to work with case-insensitive data in Postgres, so I created a non-deterministic collation for all my strings in the database, but this causes problems when using OData and $filter. Whenever the filter translates into a LIKE statement, I get the error: "nondeterministic collations are not supported for LIKE". E.g. 

```
$filter=startsWith(Description, 'The first')
```

This is a test project to recreate this error in a smaller context, and work on a solution. The project has a DbContext and migrations to setup a database with test data.

I found two solutions:

1. Hook into OData code and create a custom FilterBinder, and handle all functions that translates into a LIKE clause
2. Hook into Npgsql.PG (the EF provider for Postgres), and override the VisitLike method in the QuerySqlGenerator

> Note: The second solution is simpler, but gives this warning: EF1001: NpgsqlQuerySqlGenerator is an internal API that supports the Entity Framework Core infrastructure and not subject to the same compatibility standards as public APIs. It may be changed or removed without notice in any release.
