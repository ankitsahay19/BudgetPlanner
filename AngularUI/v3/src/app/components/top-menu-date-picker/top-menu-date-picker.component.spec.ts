import { TestBed } from '@angular/core/testing';
import { TopMenuDatePickerComponent } from './top-menu-date-picker.component';

describe('TopMenuDatePickerComponent', () => {
    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [TopMenuDatePickerComponent]
        }).compileComponents();
    });

    it('should create', () => {
        const fixture = TestBed.createComponent(TopMenuDatePickerComponent);
        const app = fixture.componentInstance;
        expect(app).toBeTruthy();
    });
});
