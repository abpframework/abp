export declare namespace Profile {
    interface State {
        profile: Response;
    }
    interface Response {
        userName: string;
        email: string;
        name: string;
        surname: string;
        phoneNumber: string;
    }
    interface ChangePasswordRequest {
        currentPassword: string;
        newPassword: string;
    }
}
