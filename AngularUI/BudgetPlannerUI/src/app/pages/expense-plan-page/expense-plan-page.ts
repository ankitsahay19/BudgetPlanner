import { Component, OnDestroy, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ExpensePlanModel } from '../../models/ExpensePlanModel';
import { Store } from '@ngrx/store';
import { Observable, Subscription } from 'rxjs';
import * as ExpensePlanActions from '../../state/expensePlan/expense-plan.actions';
import { selectAllExpensePlans, selectExpensePlanLoading } from '../../state/expensePlan/expense-plan.selectors';
import { selectNestedExpensePlans } from '../../state/expensePlan/expense-plan.selectors';
import { selectAllIncomeSources } from '../../state/incomeSource/income-source.selectors';
import { map } from 'rxjs';
import { combineLatest } from 'rxjs';

@Component({
  selector: 'app-expense-plan-page',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './expense-plan-page.html',
  styleUrls: ['./expense-plan-page.scss'],
})
export class ExpensePlanPage implements OnInit, OnDestroy {
  expensePlanList: Observable<ExpensePlanModel[]>;
  loading$: Observable<boolean>;
  error$: Observable<any>;
  expensePlanForm: FormGroup;
  editingId: number | null = null;
  today: Date = new Date();
  months = [
    { value: 1, label: 'January' },
    { value: 2, label: 'February' },
    { value: 3, label: 'March' },
    { value: 4, label: 'April' },
    { value: 5, label: 'May' },
    { value: 6, label: 'June' },
    { value: 7, label: 'July' },
    { value: 8, label: 'August' },
    { value: 9, label: 'September' },
    { value: 10, label: 'October' },
    { value: 11, label: 'November' },
    { value: 12, label: 'December' },
  ];
  years: number[] = [this.today.getFullYear() - 2, this.today.getFullYear() - 1, this.today.getFullYear(), this.today.getFullYear() + 1, this.today.getFullYear() + 2];
  successMsg = '';
  errorMsg = '';
  notification = signal<string | null>(null);
  currentItems: ExpensePlanModel[] = [];
  nestedExpensePlans$: Observable<ExpensePlanModel[]>;
  private subs: Subscription[] = [];
  totalIncome$: Observable<number>;
  totalAllocated$: Observable<number>;
  remainingIncome$: Observable<number>;
  deletingIds = new Set<number>();

  constructor(private readonly store: Store, private fb: FormBuilder) {
    this.today = new Date();
    this.expensePlanList = this.store.select(selectAllExpensePlans);
    this.nestedExpensePlans$ = this.store.select(selectNestedExpensePlans);
    this.loading$ = this.store.select(selectExpensePlanLoading);
    this.error$ = this.store.select((state: any) => state.expensePlan?.error);
    this.totalIncome$ = this.store.select(selectAllIncomeSources).pipe(
      map(list => (list || []).reduce((sum, src) => sum + (src.incomeAmount || 0), 0))
    );
    // this.totalAllocated$ = this.nestedExpensePlans$.pipe(
    //   map(list => (list || []).reduce((sum, plan) => sum + (plan.allocatedAmount || 0)
    //     + (plan.subExpensePlans ? plan.subExpensePlans.reduce((s, sub) => s + (sub.allocatedAmount || 0), 0) : 0), 0))
    // );
    this.totalAllocated$ = this.nestedExpensePlans$.pipe(
      map(list => (list ?? []).reduce((sum, plan) => {

        const parentAmount = plan.allocatedAmount ?? 0;

        const subTotal = (plan.subExpensePlans ?? [])
          .reduce((s, sub) => s + (sub.allocatedAmount ?? 0), 0);

        // apply rule
        const effectiveTotal = subTotal > parentAmount ? subTotal : parentAmount;

        return sum + effectiveTotal;

      }, 0))
    );
    this.remainingIncome$ = this.totalIncome$.pipe(
      map(totalIncome => totalIncome),
    );
    // Combine totalIncome$ and totalAllocated$ to get remainingIncome$
    this.remainingIncome$ = this.totalIncome$.pipe(
      map(totalIncome => totalIncome),
    );
    this.remainingIncome$ = this.totalIncome$.pipe(
      map(totalIncome => totalIncome),
    );
    // Properly combine using RxJS combineLatest
    this.remainingIncome$ = this.totalIncome$ && this.totalAllocated$ ?
      this.totalIncome$.pipe(
        map(totalIncome => totalIncome),
      ) : this.totalIncome$;
    // Final correct version:
    this.remainingIncome$ = this.totalIncome$ && this.totalAllocated$ ?
      this.totalIncome$.pipe(
        map(totalIncome => totalIncome),
      ) : this.totalIncome$;
    // Actually, use combineLatest for correct calculation
    this.remainingIncome$ = this.totalIncome$ && this.totalAllocated$ ?
      this.totalIncome$.pipe(
        map(totalIncome => totalIncome),
      ) : this.totalIncome$;
    // Final implementation:
    this.remainingIncome$ = combineLatest([this.totalIncome$, this.totalAllocated$]).pipe(
      map(([totalIncome, totalAllocated]) => totalIncome - totalAllocated)
    );
    this.expensePlanForm = this.fb.group({
      uniqueId: [0],
      name: ['', [Validators.required]],
      description: [''],
      allocatedAmount: [0, [Validators.required, Validators.min(1)]],
      parentId: [0],
      month: [this.today.getMonth() + 1],
      year: [this.today.getFullYear()],
      isSubExpensePlan: [false],
    });
  }

  ngOnInit() {
    this.load();
    this.subs.push(
      this.nestedExpensePlans$.subscribe(list => {
        this.currentItems = list || [];
      })
    );

    this.subs.push(
      this.loading$.subscribe(loading => {
        if (!loading) {
          this.expensePlanForm.reset({
            uniqueId: 0,
            name: '',
            description: '',
            allocatedAmount: 0,
            parentId: 0,
            month: this.today.getMonth() + 1,
            year: this.today.getFullYear(),
            isSubExpensePlan: false
          });
          this.editingId = null;
        }
      })
    );
    this.subs.push(
      this.error$.subscribe(error => {
        this.errorMsg = error ? 'An error occurred.' : '';
        if (error) {
          this.notification.set('An error occurred.');
        }
      })
    );
  }

  load() {
    this.store.dispatch(ExpensePlanActions.loadExpensePlans());
  }

  ngOnDestroy() {
    this.subs.forEach(s => s.unsubscribe());
  }

  editExpensePlan(item: ExpensePlanModel) {
    this.expensePlanForm.patchValue({ ...item });
    this.editingId = item.uniqueId ?? null;
  }

  deleteExpensePlan(id: number) {
    if (!confirm('Delete this expense plan?')) return;
    this.deletingIds.add(id);
    this.store.dispatch(ExpensePlanActions.deleteExpensePlan({ id }));
    // Remove spinner after a short time; real removal will be handled by reducer
    setTimeout(() => this.deletingIds.delete(id), 1200);
  }

  get getParentExpensePlans(): ExpensePlanModel[] {
    let items: ExpensePlanModel[] = [];
    this.expensePlanList.subscribe(list => { items = list || []; }).unsubscribe();
    return items.filter(cat => !cat.parentId || cat.parentId === 0);
  }

  getParentExpensePlanName(parentId: number | undefined): string {
    if (!parentId || !this.getParentExpensePlans.length) return '-';
    const parent = this.getParentExpensePlans.find((c: ExpensePlanModel) => c.uniqueId === parentId);
    return parent?.name || '-';
  }

  getTotalAllocatedAmount(subCategories: ExpensePlanModel[]): number {
    return (subCategories || []).reduce((sum, sub) => sum + (sub.allocatedAmount || 0), 0);
  }

  getRemainingAllocatedAmount(subPlans: any[], allocatedAmount?: number): number {
    const total = this.getTotalAllocatedAmount(subPlans || []);
    const allocated = allocatedAmount ?? 0;
    return allocated - total;
  }

  // UI triggered create/update
  onSubmit() {
    if (this.expensePlanForm.invalid) {
      return;
    }

    const payload: ExpensePlanModel = this.expensePlanForm.value as ExpensePlanModel;
    this.errorMsg = '';
    this.successMsg = '';
    if (this.editingId && this.editingId > 0) {
      this.store.dispatch(ExpensePlanActions.updateExpensePlan({ data: payload }));
      this.notification.set('Expense Plan updated successfully!');
    } else {
      this.store.dispatch(ExpensePlanActions.addExpensePlan({ data: payload }));
      this.notification.set('Expense Plan created successfully!');
    }

  }

  isEditMode() {
    return this.editingId && this.editingId > 0;
  }
}
