import { CommonModule } from '@angular/common';
import { Component, OnInit, OnDestroy } from '@angular/core';

import { Store } from '@ngrx/store';
import * as IncomeSourceActions from '../../state/incomeSource/income-source.actions';
import { selectAllIncomeSources, selectLoading } from '../../state/incomeSource/income-source.selectors';

import { Observable, Subscription } from 'rxjs';
import { IncomeSourceModel } from '../../models/IncomeSourceModel';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-income-page',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './income-page.html',
  styleUrl: './income-page.scss',
})


export class IncomePage implements OnInit, OnDestroy {
  readonly incomeSources$: Observable<IncomeSourceModel[]>;
  readonly loading$: Observable<boolean>;

  incomeForm: FormGroup;
  private subs: Subscription[] = [];

  // local UI state
  currentItems: IncomeSourceModel[] = [];
  deletingIds = new Set<number>();
  editingId: number | null = null;
  saving = false;
  errorMsg = '';
  successMsg = '';

  constructor(private readonly store: Store, private fb: FormBuilder) {
    this.incomeSources$ = this.store.select(selectAllIncomeSources);
    this.loading$ = this.store.select(selectLoading);

    this.incomeForm = this.fb.group({
      uniqueId: [0],
      sourceName: ['', [Validators.required]],
      incomeAmount: [0, [Validators.required, Validators.min(1)]]
    });
  }

  ngOnInit() {
    this.load();
    this.subs.push(this.incomeSources$.subscribe(list => { this.currentItems = list || []; }));
  }

  load() {
    this.store.dispatch(IncomeSourceActions.loadIncomeSources());
  }


  ngOnDestroy() {
    this.subs.forEach(s => s.unsubscribe());
  }

  // Add sample (keeps existing helper for quick testing)
  add() {
    const newSource: IncomeSourceModel = {
      uniqueId: 0,
      userId: 9,
      sourceName: 'New Company',
      incomeAmount: 55000,
      createdDate: new Date(),
      lastUpdatedDate: new Date(),
    };
    this.store.dispatch(IncomeSourceActions.addIncomeSource({ data: newSource }));
  }

  // UI triggered create/update
  onSubmit() {
    if (this.incomeForm.invalid) {
      return;
    }

    const payload: IncomeSourceModel = this.incomeForm.value as IncomeSourceModel;

    this.saving = true;
    this.errorMsg = '';
    this.successMsg = '';

    if (this.editingId && this.editingId > 0) {
      // update
      this.store.dispatch(IncomeSourceActions.updateIncomeSource({ data: payload }));
      this.successMsg = 'Update submitted';
    } else {
      // create
      this.store.dispatch(IncomeSourceActions.addIncomeSource({ data: payload }));
      this.successMsg = 'Create submitted';
    }

    // reset UI form quickly (effects should update store)
    this.saving = false;
    this.editingId = null;
    this.incomeForm.reset({ uniqueId: 0, sourceName: '', incomeAmount: 0 });
  }

  // Called from the list to start edit
  startEdit(item: IncomeSourceModel) {
    this.editingId = item.uniqueId || null;
    this.incomeForm.patchValue({ ...item });
  }

  // Delete via ngrx action (keeps a small local spinner state per id)
  delete(id: number) {
    this.deletingIds.add(id);
    this.store.dispatch(IncomeSourceActions.deleteIncomeSource({ id }));

    // remove spinner after a short time; the real removal will be handled by reducer when success arrives
    setTimeout(() => this.deletingIds.delete(id), 1200);
  }

  // small helpers used by template
  savingIncome() {
    return this.saving;
  }

  getTotalIncome() {
    return (this.currentItems || []).reduce((s, x) => s + (x.incomeAmount || 0), 0);
  }

  // trackBy used by *ngFor
  trackById(index: number, item: IncomeSourceModel) {
    return item?.uniqueId ?? index;
  }

}