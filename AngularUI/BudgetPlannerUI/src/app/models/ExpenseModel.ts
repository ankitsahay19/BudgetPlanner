export interface ExpenseModel {
  uniqueId?: number;
  categoryId: number;
  title: string;
  description?: string;
  amount: number;
  createdDate?: string;
  lastUpdatedDate?: string;
}
