import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { routes } from './app.routes';
import { interceptInterceptor } from './components/intercept.interceptor.spec';

import { JWT_OPTIONS, JwtHelperService } from '@auth0/angular-jwt';

 
export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withInterceptors([interceptInterceptor])),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
 
    JwtHelperService,
    { provide: JWT_OPTIONS, useValue: JWT_OPTIONS }
  ]
};
