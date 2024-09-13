/**
 * Routes available for everyone
 * @type {string[]}
**/
export const publicRoutes = [
    "/",
    "/auth/verification",
    "/api/uploadthing"
]

/**
 * Routes used for authentication
 * @type {string[]}
**/
export const authRoutes = [
    "/auth/login",
    "/auth/register",
    "/auth/error",
    "/auth/reset",
    "/auth/resetPassword",
];

/**
 * Prefix for API authentication routes
 * @type {string}
**/
export const apiAuthPrefix = "/api/auth";

/**
 * Redirects to this address once logged in
 * @type {string}
**/
export const DEFAULT_LOGIN_REDIRECT = "/home"
