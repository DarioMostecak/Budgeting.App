
# WebSis Budgeting Api

## Description
Our application is s a powerful budgeting application that simplifies your financial management. With just a few taps, you can effortlessly track your income and expenses, gaining valuable insights into your financial health.

# Table of contents

* [1 Introduction](#1-introduction)
  * [1.1 Authentication](#11-authentication)
* [2 Services](#2-services)
  * [2.1 Identity Service](#21-identity-service)
* [3 Api enpoints](#3-api-enpoints)
  * [3.1 Identity Service](#31-identity-service )


# 1. Introduction
## 1.1 Authentication 
Our application is secured using JWT bearer tokens. When a user logs in, they receive a token which they can use to access the protected endpoints of our application. The token includes information about the user's role, which is used to determine what endpoints they can access.


# 2. Services

## 2.1 Identity Service

#### Description
The IdentityService class provides functionality for authenticating user credentials and generating identity tokens.

#### Constructor
```cs
public IdentityService(
    IUserManagerBroker userManagerBroker,
    ILoggingBroker loggingBroker,
    IOptions<JwtSettings> jwtOptions)
```

- **userManagerBroker** : An instance of the IUserManagerBroker interface for user management operations.
- **loggingBroker** :  An instance of the ILoggingBroker interface for logging operations.
- **jwtOptionsr** :  An instance of the JwtSettings options for JWT configuration.

#### Public Methods

**AuthenticateUserAsync**
```cs
public ValueTask<IdentityResponse> AuthenticateUserAsync(IdentityRequest identityRequest)
```
Authenticates a user based on the provided identity request.

#### Parameters
- **identityRequest** : An instance of the IdentityRequest class containing the user's email and password.

#### Returns
A ValueTask containing an instance of the IdentityResponse class with the generated token upon successful authentication.

#### Exceptions
- **IdentityRequestValidationException** : Thrown if the identity request is invalid or malformed.
- **FailAuthenticationIdentityRequestException** : Thrown if an unexpected error occurs during the authentication process.
- **IdentityRequestDependencyException** : Thrown if a dependency (e.g., database) fails.
- **IdentityRequestServiceException** : Thrown if an unexpected error occurs during the authentication process.

# 3. API enpoints

## 3.1 Identity Controller

#### LogIn

```https
  POST /api/v1/identities
```
#### Authentication
Don't require authorization header with a bearer token (JWT).

#### Request body
| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `Email` | `string` | The email address of the user. |
| `Password` | `string` | The password associated with the user's account. |

#### Responses
| HTTP Status Code | Description             |
| :--------------- | :---------------------- |
| `200 OK` | The user was successfully authenticated. |
| `400 Bad Request` |  The request body is invalid or malformed. |
| `401 Unauthorized` |  The provided credentials are invalid.|
| `500 Internal Server Error` |  An internal server error occurred..|

#### Example Request
```json
{
  "Email": "john.doe@example.com",
  "Password": "password123"
}
```

#### Example Response (200 OK)
```json
{
  "Token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

#### Example Response (400 Bad Request)
```json
{
  "status": 400,
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "The request body is invalid.",
  "errors": {
    "Password": [
      "Password is required."
    ],
    "Email": [
      "Email can't be blank or null and must be in email format."
    ]
  }
}
```

#### Example Response (401 Unauthorized)
```json
{
  "status": 401,
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Invalid credentials."
}
```
#### Example Response (500 Internal Server Error))
```json
{
  "status": 500,
  "type": "https://tools.ietf.org/html/rfc7231#section-6.6.1",
  "title": "An internal server error occurred."
}
```




