export interface CategoryModel {
  uniqueId?: number;
  name: string;
  description?: string;
  parentId?: number;
  createdDate?: string | null;
  lastUpdatedDate?: string | null;
  userId?: number;
  appUser?: any; // You can strongly type this if needed
  subCategories?: CategoryModel[] | null;
  allocatedAmount?: number;
  totalAllocatedAmountOfSubCategories?: number;
  remainingBalance?: number;
}

// export interface SubCategoryWrapper {
//   $id: string;
//   $values: CategoryModel[];
// }
