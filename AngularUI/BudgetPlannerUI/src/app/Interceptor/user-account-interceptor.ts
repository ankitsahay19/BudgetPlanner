import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';

/**
 * Interface for the expected structure of user token data in localStorage.
 */
interface UserTokenData {
  token: string;
  [key: string]: any;
}

/**
 * HTTP Interceptor to attach JWT token from localStorage to outgoing requests.
 * Follows industry standards for token extraction and error handling.
 */
@Injectable()
export class UserAccountServiceInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Attempt to retrieve and parse the user token data from localStorage
    let token: string | null = null;
    const tokenData = localStorage.getItem('userTokenData');
    if (tokenData) {
      try {
        const parsed: UserTokenData = JSON.parse(tokenData);
        if (parsed && typeof parsed.token === 'string') {
          token = parsed.token;
        }
      } catch (e) {
        // Optionally log error for debugging
        // console.error('Failed to parse userTokenData from localStorage', e);
        token = null;
      }
    }

    // If a valid token exists, clone the request and set the Authorization header
    if (token) {
      const authReq = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
      return next.handle(authReq);
    }
    // If no token, proceed without modifying the request
    return next.handle(req);
  }
}
