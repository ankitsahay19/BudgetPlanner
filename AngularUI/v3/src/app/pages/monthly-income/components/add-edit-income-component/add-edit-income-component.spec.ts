import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddEditIncomeComponent } from './add-edit-income-component';

describe('AddEditIncomeComponent', () => {
  let component: AddEditIncomeComponent;
  let fixture: ComponentFixture<AddEditIncomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddEditIncomeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddEditIncomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
