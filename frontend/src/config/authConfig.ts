// Tried to connect azure with msal-browser but it didn't work
import { Configuration, RedirectRequest, LogLevel } from '@azure/msal-browser'
import { apiConfig } from './apiConfig'

/**
 * Enter here the user flows and custom policies for your B2C application
 * To learn more about user flows, visit: https://docs.microsoft.com/en-us/azure/active-directory-b2c/user-flow-overview
 * To learn more about custom policies, visit: https://docs.microsoft.com/en-us/azure/active-directory-b2c/custom-policy-overview
 */
export const policies = {
  names: {
    signUpSignIn: 'login'
  },
  authorities: {
    signUpSignIn: {
      authority: `https://login.microsoftonline.com/${process.env.NEXT_PUBLIC_AZURE_AD_TENANT_ID}`
    }
  },
  authorityDomain: 'akdeniz.onmicrosoft.com'
}

export const msalConfig: Configuration = {
  auth: {
    clientId: `${process.env.NEXT_PUBLIC_AZURE_AD_CLIENT_ID}`,
    authority: policies.authorities.signUpSignIn.authority,
    knownAuthorities: [policies.authorityDomain],
    redirectUri: '/main', // You must register this URI on Azure Portal/App Registration. Defaults to window.location.origin
    postLogoutRedirectUri: '/',
    clientCapabilities: ['CP1'], // this lets the resource owner know that this client is capable of handling claims challenge.
    navigateToLoginRequestUrl: false
  },
  cache: {
    cacheLocation: 'sessionStorage', // Configures cache location. "sessionStorage" is more secure, but "localStorage" gives you SSO between tabs.
    storeAuthStateInCookie: false // Set this to "true" if you are having issues on IE11 or Edge
  },
  system: {
    /**
     * Below you can configure MSAL.js logs. For more information, visit:
     * https://docs.microsoft.com/azure/active-directory/develop/msal-logging-js
     */
    loggerOptions: {
      loggerCallback: (level, message, containsPii) => {
        if (containsPii) {
          return
        }

        if (process.env.NODE_ENV === 'production') return

        switch (level) {
          case LogLevel.Error:
            console.error(message)
            return
          case LogLevel.Info:
            console.info(message)
            return
          case LogLevel.Verbose:
            console.debug(message)
            return
          case LogLevel.Warning:
            console.warn(message)
            return
          default:
            return
        }
      }
    }
  }
}

/**
 * Scopes you add here will be prompted for user consent during sign-in.
 * By default, MSAL.js will add OIDC scopes (openid, profile, email) to any login request.
 * For more information about OIDC scopes, visit:
 * https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-permissions-and-consent#openid-connect-scopes
 */
export const loginRequest = {
  scopes: ['User.Read']
}

/**
 * An optional silentRequest object can be used to achieve silent SSO
 * between applications by providing a "login_hint" property.
 */
// export const silentRequest = {
// 	scopes: ["openid", "profile"],
// 	loginHint: "example@domain.net"
// };

export const companiesRequest = {
  apiUser: {
    endpoint: `${apiConfig.webApi}/Company`,
    scopes: [...apiConfig.scopes]
  }
}

export const employeesRequest = {
  apiUser: {
    endpoint: `${apiConfig.webApi}/Employee`,
    scopes: [...apiConfig.scopes]
  }
}

export const userRequest = {
  apiUser: {
    endpoint: `${apiConfig.webApi}/User`,
    scopes: [...apiConfig.scopes]
  }
}

export const internshipRequest = {
  apiUser: {
    endpoint: `${apiConfig.webApi}/Internship`,
    scopes: [...apiConfig.scopes]
  }
}

export const fileRequest = {
  apiUser: {
    endpoint: `${apiConfig.webApi}/File`,
    scopes: [...apiConfig.scopes]
  }
}

export const internshipStatusRequest = {
  apiUser: {
    endpoint: `${apiConfig.webApi}/InternshipStatus`,
    scopes: [...apiConfig.scopes]
  }
}

export const externalAccountRequest = {
  apiUser: {
    endpoint: `${apiConfig.webApi}/ExternalAccountContoller`
  }
}

export const tokenRequest = {
  scopes: [...apiConfig.scopes], // e.g. ["https://fabrikamb2c.onmicrosoft.com/helloapi/demo.read"]
  forceRefresh: false // Set this to "true" to skip a cached token and go to the server to get a new token
}

// clientid
// tenantid
// scope
