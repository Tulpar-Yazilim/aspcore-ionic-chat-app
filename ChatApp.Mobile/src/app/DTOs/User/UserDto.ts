import { BaseAddDto, BaseCardDto, BaseUpdateDto } from "../Base/BaseCardDto";

export class UserAddDto extends BaseAddDto {
    username: string;
    name: string;
    surname: string;
    password: string;
    confirmPassword: string;
    phone: string;
    email: string;
    saysisID: string;
    rolesDto: Array<any> = new Array<any>();
    roles: Array<string> = new Array<string>();
}
export class UserCardDto extends BaseCardDto {
    username: string;
    name: string;
    surname: string;
    phone: string;
    email: string;
    saysisID: string;
    identityUserID: string;
    roles: Array<string> = new Array<string>();
}
export class UserUpdateDto extends BaseUpdateDto {
    username: string;
    name: string;
    surname: string;
    phone: string;
    email: string;
    saysisID: string;
    identityUserID: string;
    rolesDto: Array<any> = new Array<any>();
    roles: Array<string> = new Array<string>();
}

