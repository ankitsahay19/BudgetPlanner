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
    template: `
  <div class="d-flex align-items-center top-date-picker">
    <div class="me-2 text-muted small">Year</div>
    <div class="d-flex align-items-center me-3">
      <button class="btn btn-sm btn-outline-secondary me-1" (click)="prevYear()" aria-label="Previous year">&lt;</button>
      <div class="px-2 fw-bold">{{ year() }}</div>
      <button class="btn btn-sm btn-outline-secondary ms-1" (click)="nextYear()" aria-label="Next year">&gt;</button>
    </div>

    <div class="me-2 text-muted small">Month</div>
    <div class="d-flex align-items-center">
      <button class="btn btn-sm btn-outline-secondary me-1" (click)="prevMonth()" aria-label="Previous month">&lt;</button>
      <div class="px-2 fw-bold">{{ monthName() }} </div>
      <button class="btn btn-sm btn-outline-secondary ms-1" (click)="nextMonth()" aria-label="Next month">&gt;</button>
    </div>
  </div>
  `
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
