export interface ExpensePlanModel {
  uniqueId: number;
  name: string;
  description?: string;
  allocatedAmount?: number;
  parentId?: number;
  createdDate?: string | null;
  lastUpdatedDate?: string | null;
  userId?: number;
  subExpensePlans?: ExpensePlanModel[] | null;
  month?: number;
  year?: number;
}

export interface SubExpensePlanWrapper {
  $id: string;
  $values: ExpensePlanModel[];
}