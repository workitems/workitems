import { AuthConfig } from 'angular-oauth2-oidc';

export const authCodeFlowConfig: AuthConfig = {
    issuer: 'https://localhost:6001',
    redirectUri: window.location.origin,// + '/index.html',
    clientId: 'wvi-sample-spa',
    responseType: 'code',
    scope: 'openid profile offline_access workitems',
    showDebugInformation: true,
    timeoutFactor: 0.75
};