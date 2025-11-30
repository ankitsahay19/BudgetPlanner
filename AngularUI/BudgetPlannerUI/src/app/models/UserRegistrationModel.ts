export interface UserRegistrationModel {
    firstName: string;
    lastLame: string;
    phoneNumber: string;
    email: string;
    password: string;
    confirmPassword: string;
    roles: string[];
    errorMessage?: string;
    successMessage?: string;
    notificationMessage?: string;
  }
  
  export interface RegistrationResponseModel {
    isCreated: boolean;
    uniqueId: number;
    errorMessages?: string[] | null;
    successMessages?: string[] | null;
    details?: any;
  }
  