import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IndexIncomeComponent } from './index-income-component';

describe('IndexIncomeComponent', () => {
  let component: IndexIncomeComponent;
  let fixture: ComponentFixture<IndexIncomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [IndexIncomeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IndexIncomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
