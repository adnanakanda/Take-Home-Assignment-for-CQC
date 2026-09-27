# Defects found in the system under test

System under test: OWASP Juice Shop `bkimminich/juice-shop:v17.1.1`, `http://localhost:3000`.

## BUG-001 Registration accepts an invalid email address

- **Severity**: Severe
- **Endpoint**: `POST /api/Users`
- **Found by**: `RegistrationTests.Register_User_With_Invalid_Email`

**Given** a registration with an email address that has no `@`, for example
`invalid-email-2aafc32e811248de83ebd8b64ada8cc8`

**When** the request is sent to `POST /api/Users`

**Then** the response is `201 Created` and the account can be used to log in, instead of
`400 Bad Request`.

## BUG-002 Registration accepts a mismatched password confirmation

- **Severity**: Severe
- **Endpoint**: `POST /api/Users`
- **Found by**: `RegistrationTests.Register_User_With_Mismatched_Passwords`

**Given** a registration where `passwordRepeat` differs from `password`

**When** the request is sent to `POST /api/Users`

**Then** the response is `201 Created` and the account is created with `password`, instead of
`400 Bad Request`.
