//export const ApiBaseUrl = 'https://localhost:7255/api';
export const ApiBaseUrl = 'https://localhost:7255/api';

export const ApiEndpoints = {
    userAccount: {
        login: `${ApiBaseUrl}/UserAccount/Login`,
        UserRegistration: `${ApiBaseUrl}/UserAccount/UserRegistration`,
    },
    IncomeSource: {
        create: `${ApiBaseUrl}/IncomeSource/Create`,
        edit: `${ApiBaseUrl}/IncomeSource/Edit`,
        getAll: `${ApiBaseUrl}/IncomeSource`,
        getById: (id: number) => `${ApiBaseUrl}/IncomeSource/${id}`,
        delete: (id: number) => `${ApiBaseUrl}/IncomeSource/${id}`
    },
    ExpensePlan: {
        create: `${ApiBaseUrl}/ExpensePlans/Create`,
        edit: `${ApiBaseUrl}/ExpensePlans/Edit`,
        getAll: `${ApiBaseUrl}/ExpensePlans`,
        getById: (id: number) => `${ApiBaseUrl}/ExpensePlans/${id}`,
        delete: (id: number) => `${ApiBaseUrl}/ExpensePlans/${id}`
    },





    Categories: {
        getAllCategories: `${ApiBaseUrl}/Categories`,
        getCategoriesById: (id: number) => `${ApiBaseUrl}/Categories/${id}`,
        SaveCategories: `${ApiBaseUrl}/Categories/CreateOrEdit`
    },
    Expenses: {
        createOrEdit: `${ApiBaseUrl}/Expenses/CreateOrEdit`,
        getAll: `${ApiBaseUrl}/Expenses`,
        getById: (id: number) => `${ApiBaseUrl}/Expenses/${id}`,
        delete: (id: number) => `${ApiBaseUrl}/Expenses/${id}`
    },
    WishLists: {
        createOrEdit: `${ApiBaseUrl}/WishLists/CreateOrEdit`,
        getAll: `${ApiBaseUrl}/WishLists`,
        getById: (id: number) => `${ApiBaseUrl}/WishLists/${id}`,
        delete: (id: number) => `${ApiBaseUrl}/WishLists/${id}`
    }
};
