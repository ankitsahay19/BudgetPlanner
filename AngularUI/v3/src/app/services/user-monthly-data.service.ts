import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { ApiEndpoints } from '../core/constants/api-endpoints';

@Injectable({ providedIn: 'root' })
export class UserMonthlyDataService {
    constructor(private http: HttpClient) { }

    /**
     * Calls the backend API to get user monthly data for the given year & month.
     * Logs the response to console for now.
     */
    getUserMonthlyData(year: number, month: number): Observable<any> {
        // Backend controller uses [HttpPost("GetUserMonthlyData")] so use POST here.
        // Swagger's "Try it out" sent a POST with query params and returned 200 —
        // calling GET from the browser resulted in 405. Use POST with the same URL.
        const url = ApiEndpoints.userAccount.getUserMonthlyData(year, month);
        // Some servers accept query params on POST; if the backend expects a JSON body instead,
        // adjust to send { year, month } and update ApiEndpoints accordingly.
        return this.http.post<any>(url, {}).pipe(
            tap(response => console.log('UserMonthlyDataService.getUserMonthlyData response:', response))
        );
    }
}
