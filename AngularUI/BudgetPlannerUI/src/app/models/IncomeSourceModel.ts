export interface IncomeSourceModel {
    uniqueId: number;
    userId: number;
    sourceName: string;
    incomeAmount: number;
    createdDate: Date;
    lastUpdatedDate: Date;
}