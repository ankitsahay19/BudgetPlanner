import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpensePlanPage } from './expense-plan-page';

describe('ExpensePlanPage', () => {
  let component: ExpensePlanPage;
  let fixture: ComponentFixture<ExpensePlanPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ExpensePlanPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ExpensePlanPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
