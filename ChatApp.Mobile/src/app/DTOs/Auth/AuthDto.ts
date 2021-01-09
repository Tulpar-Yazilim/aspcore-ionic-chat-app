export class AuthLoginDto {
    email: string = "";
    password: string = "";
}

export class ResetPasswordDto {
    code: string;
    id: string;
    newPassword: string = "";
    confirmPassword: string = "";
}
export class ChangePassword {
    oldPassword: string;
    newPassword: string;
    confirmPassword: string;
}

export class TokenResultDto {
    access_token: string;
    username: string;
    roles: string;
    email: string;
}

