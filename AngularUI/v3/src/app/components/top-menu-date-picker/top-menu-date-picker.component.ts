import { Component, EventEmitter, Output, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

/**
 * Top menu date picker - shows current month and year with left/right buttons
 * Standalone so it can be embedded in the top navbar.
 */
@Component({
    selector: 'app-top-menu-date-picker',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './top-menu-date-picker.component.html',
    styleUrls: ['./top-menu-date-picker.component.scss']
})
export class TopMenuDatePickerComponent {
    // signals for month and year
    private _year = signal<number>(new Date().getFullYear());
    private _month = signal<number>(new Date().getMonth()); // 0-based

    // emitters for parent components to react
    @Output() yearChange = new EventEmitter<number>();
    @Output() monthChange = new EventEmitter<number>();

    year() { return this._year(); }
    month() { return this._month(); }

    monthNames = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    monthName() { return this.monthNames[this._month()]; }

    prevYear() {
        const next = this._year() - 1;
        this._year.set(next);
        this.yearChange.emit(next);
    }
    nextYear() {
        const next = this._year() + 1;
        this._year.set(next);
        this.yearChange.emit(next);
    }

    prevMonth() {
        let m = this._month() - 1;
        let y = this._year();
        if (m < 0) { m = 11; y -= 1; this._year.set(y); this.yearChange.emit(y); }
        this._month.set(m);
        this.monthChange.emit(m);
    }

    nextMonth() {
        let m = this._month() + 1;
        let y = this._year();
        if (m > 11) { m = 0; y += 1; this._year.set(y); this.yearChange.emit(y); }
        this._month.set(m);
        this.monthChange.emit(m);
    }
}
