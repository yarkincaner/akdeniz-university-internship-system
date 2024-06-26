const API_BASE_URL =
  process.env.NODE_ENV === 'development'
    ? process.env.NEXT_PUBLIC_API_BASE_URL_DEVELOPMENT
    : process.env.NEXT_PUBLIC_API_BASE_URL

// The current application coordinates were pre-registered in a B2C tenant.
export const apiConfig = {
  scopes: [
    `api://${process.env.NEXT_PUBLIC_AZURE_AD_CLIENT_SCOPE}/access_as_user`
  ],
  webApi: `${API_BASE_URL}/api/v1`
}
