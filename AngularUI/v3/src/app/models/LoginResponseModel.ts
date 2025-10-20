export interface LoginResponseModel {
    isLoginSuccess: boolean;
    token: string;
    userId: number;
    fName: string;
    lName: string;
    email: string;
    mobile: string;
    fullName: string;
    issuedAt: string;
}
