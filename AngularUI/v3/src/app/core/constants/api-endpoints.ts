// src/app/core/constants/api-endpoints.ts

//export const ApiBaseUrl = 'https://localhost:7255/api'; // windows base url local
export const ApiBaseUrl = 'https://localhost:7255/api'; // mac base url local
export const ApiSwagger = 'http://mbp.bitprosofttech.com/swagger/index.html';
//export const ApiBaseUrl = 'http://mbp.bitprosofttech.com/api';
export const ApiEndpoints = {
    userAccount: {
        login: `${ApiBaseUrl}/UserAccount/Login`,
        UserRegistration: `${ApiBaseUrl}/UserAccount/UserRegistration`,
    },
    Categories: {
        getAllCategories: `${ApiBaseUrl}/Categories`,
        getCategoriesById: (id: number) => `${ApiBaseUrl}/Categories/${id}  `,
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
    },
    IncomeSource: {
        createOrEdit: `${ApiBaseUrl}/IncomeSource/CreateOrEdit`,
        getAll: `${ApiBaseUrl}/IncomeSource`,
        getById: (id: number) => `${ApiBaseUrl}/IncomeSource/${id}`,
        delete: (id: number) => `${ApiBaseUrl}/IncomeSource/${id}`
    }
};
