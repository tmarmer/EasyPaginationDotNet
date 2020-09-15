# Easy Pagination
This library provides classes and methods meant to setup paginated responses quickly and easily within a .NET Core application.

## Response Codes
The following response codes are returned when using a `PaginationObjectResult`:

- `200 (OK)`: The result has returned the entire list of items. No other pages needed.
- `204 (No Content)`: The result has returned nothing. The user has provided no offset, so the lack of content may be due to a filter or data being currently unavailable. This is considered a valid response.
- `206 (Partial Content)`: The result has returned 1 page of items and more are available. Additional pages can be found in the `Link` header.
- `416 (Range Not Satisfiable)`: The result has returned nothing. The user has asked for an offset that is invalid given the items being queried, so this is considered a user error (thus, the 4xx code).

## Headers
The following headers are automatically generated when using a `PaginationObjectResult`:

- `Content-Range`: Using the format `items {start-range}-{end-range}/{totalItems}`. The range or total items are replaced with an asterisk if not available.
- `Link`: A comma separated list of links in the format `<{uri}>; rel={relationship}`, where the relationship typically `first`, `last`, `next` or `prev`.

# TODO
- Finish the readme
- Unit tests
- NuGet???