import { Http, RequestOptions, Headers } from '@angular/http';
import { Injectable } from '@angular/core';
import { map } from "rxjs/operators";
import { ActivatedRouteSnapshot, Router } from '@angular/router';
import * as crypto from 'crypto-js';
import { BaseConfig } from '../DTOs/Base/BaseConfig';
import { AuthLoginDto } from '../DTOs/Auth/AuthDto';
import { APIResult } from '../DTOs/Base/APIResult';
import { Storage } from '@ionic/storage';

@Injectable()
export class AuthService {
    private MobilYoneticiKey = crypto.enc.Utf8.parse(BaseConfig.BaseAESKey);
    private ModuleName: string = BaseConfig.WebApiUrl;
    _cookieData: any;
    constructor(
        private http: Http,
        private router: Router,
        private storage: Storage
    ) {
        this.storage.get(BaseConfig.BaseStorageUserName + BaseConfig.BaseVersion).then(user => {
            this._cookieData = user;
        });
    }

    public encrypt(data) {
        let encryptedData = crypto.AES.encrypt(
            crypto.enc.Utf8.parse(btoa(unescape(encodeURIComponent(JSON.stringify(data))))),
            this.MobilYoneticiKey,
            {
                keySize: 128 / 8,
                iv: this.MobilYoneticiKey,
                mode: crypto.mode.CBC,
                padding: crypto.pad.Pkcs7
            });
        return encryptedData;
    }

    public decrypt<T>(data) {
        let decryptedData = crypto.AES.decrypt(data, this.MobilYoneticiKey, { iv: this.MobilYoneticiKey }).toString(crypto.enc.Utf8);
        let decryptedDataParsed: T = JSON.parse(decodeURIComponent(escape(atob(decryptedData))));
        return decryptedDataParsed;
    }

    login(model: AuthLoginDto) {
        return new Promise(resolve => {
            let options = new RequestOptions();
            var data = {
                username: model.email,
                password: model.password
            };
            let headers = new Headers();
            headers.append("Content-Type", "application/json");
            options.headers = headers;

            let postData = this.encrypt(data).toString();

            return this.http.post(this.ModuleName + "Auth/Token", { model: postData }, options)
                .pipe(map(response => response.json()))
                .subscribe((response: APIResult<any>) => {
                    resolve(response);
                },
                    error => {
                        console.clear();
                        let _response: APIResult<any> = new APIResult<any>();
                        _response.message = error.json().error;
                        _response.data = error.json().error_description;
                        resolve(_response);
                    });

        });
    }

    canActivate(route: ActivatedRouteSnapshot): boolean {

        if (this._cookieData) {
            return true;
        } else {
            this.router.navigate(['/authentication/login']);
            return false;
        }
    }
}
