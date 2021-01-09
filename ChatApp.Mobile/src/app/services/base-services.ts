import { map, takeUntil } from 'rxjs/operators';
import { Storage } from '@ionic/storage';
import { Http, RequestOptions, Headers, ResponseContentType } from '@angular/http';
import * as crypto from 'crypto-js';
import { BaseConfig } from '../DTOs/Base/BaseConfig';
import { Subject } from 'rxjs';
import { Router } from '@angular/router';

export abstract class BaseService {
        public loading: boolean;
        private MobilYoneticiKey = crypto.enc.Utf8.parse(BaseConfig.BaseAESKey);
        private BASE_URL = BaseConfig.WebApiUrl;
        unsubscribe: Subject<void> = new Subject();

        constructor(
                private http: Http,
                protected storage: Storage,
                protected servisName: string,
                private router: Router
        ) {
        }

        public getDatatable(data: Array<any>, pageCount, pageCurrent: number = 0) {
                var pageLength = [];
                for (let index = 0; index < Math.ceil(data.length / pageCount); index++) {
                        pageLength.push(index);
                }
                if (Math.ceil(data.length / pageCount) <= pageCurrent) {
                        pageCurrent = pageLength[pageLength.length - 1];
                }
                if (pageCurrent < 0) {
                        pageCurrent = 0;
                }
                var pageData = data.slice((pageCurrent * pageCount), (pageCurrent * pageCount) + parseInt(pageCount));
                return {
                        page: {
                                first: pageLength[0],
                                last: pageLength[pageLength.length - 1],
                                start: (pageCurrent * pageCount),
                                end: (pageCurrent * pageCount) + parseInt(pageCount),
                                current: pageCurrent,
                                length: pageLength
                        },
                        data: pageData
                }
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

        public fetchObservable(methodName: string) {

                return new Promise(resolve => {
                        this.jwt().then(jwt => {
                                this.http.get(`${this.BASE_URL + this.servisName}${methodName}`, jwt).pipe(map(res => res.json())).subscribe((response: any) => {
                                        resolve(response);
                                }, (err) => {
                                        resolve(err);
                                });
                        });
                });
        }

        public unAuthFetchObservable(methodName: string) {
                return this.http.get(`${this.BASE_URL + this.servisName}${methodName}`, this.unAuthjwt()).pipe(map(res => res.json()));
        }

        public postObservable(methodName: string, data: any) {
                return new Promise(resolve => {
                        this.jwt().then(jwt => {
                                this.http.post(`${this.BASE_URL + this.servisName}${methodName}`, { model: data }, jwt).pipe(map(res => res.json())).pipe(takeUntil(this.unsubscribe)).subscribe((response: any) => {
                                        resolve(response);
                                }, (err) => {
                                        resolve(err);
                                });
                        });
                });
        }

        public putObservable(methodName: string, data: any) {
                return new Promise(resolve => {
                        this.jwt().then(jwt => {
                                this.http.put(`${this.BASE_URL + this.servisName}${methodName}`, { model: data }, jwt).pipe(map(res => res.json())).pipe(takeUntil(this.unsubscribe)).subscribe((response: any) => {
                                        resolve(response);
                                }, (err) => {
                                        resolve(err);
                                });
                        });
                });
        }

        public deleteObservable(methodName: string) {
                return new Promise(resolve => {
                        this.jwt().then(jwt => {
                                this.http.delete(`${this.BASE_URL + this.servisName}${methodName}`, jwt).pipe(map(res => res.json())).pipe(takeUntil(this.unsubscribe)).subscribe((response: any) => {
                                        resolve(response);
                                }, (err) => {
                                        resolve(err);
                                });
                        });
                });
        }

        public fetchDownload(methodName: string) {
                return new Promise(resolve => {
                        this.jwt().then(jwt => {
                                this.http.get(`${this.BASE_URL + this.servisName}${methodName}`, ({ ...jwt, responseType: ResponseContentType.ArrayBuffer })).pipe(map(res => res.json())).pipe(takeUntil(this.unsubscribe)).subscribe((response: any) => {
                                        resolve(response);
                                }, (err) => {
                                        resolve(err);
                                });
                        });
                });
        }

        public postDownload(methodName: string, data: any) {
                return new Promise(resolve => {
                        this.jwt().then(jwt => {
                                this.http.post(`${this.BASE_URL + this.servisName}${methodName}`, ({ ...jwt, responseType: ResponseContentType.ArrayBuffer })).pipe(map(res => res.json())).pipe(takeUntil(this.unsubscribe)).subscribe((response: any) => {
                                        resolve(response);
                                }, (err) => {
                                        resolve(err);
                                });
                        });
                });
        }

        public upload(methodName: string, data: any) {
                return new Promise(resolve => {
                        this.jwtMultiPart().then(jwt => {
                                this.http.post(`${this.BASE_URL + this.servisName}${methodName}`, data, jwt).pipe(map(res => res.json())).pipe(takeUntil(this.unsubscribe)).subscribe((response: any) => {
                                        resolve(response);
                                }, (err) => {
                                        resolve(err);
                                });
                        });
                });
        }

        private jwt(): Promise<RequestOptions> {
                return new Promise(resolve => {
                        let options = new RequestOptions();
                        let headers = new Headers();
                        this.storage.get(BaseConfig.BaseStorageUserName + BaseConfig.BaseVersion).then(user => {
                                if (user) {
                                        headers.set("Authorization", "Bearer " + user.access_token);
                                }
                                else {
                                        this.router.navigateByUrl("login");
                                }
                                headers.set("Content-Type", "application/json");
                                options.headers = headers;
                                resolve(options);
                        });

                });
        }

        private unAuthjwt() {
                let options = new RequestOptions();
                let headers = new Headers();
                headers.set("Content-Type", "application/json");
                options.headers = headers;
                return options;
        }

        private jwtMultiPart(): Promise<RequestOptions> {
                return new Promise(resolve => {
                        let options = new RequestOptions();
                        let headers = new Headers();
                        this.storage.get(BaseConfig.BaseStorageUserName + BaseConfig.BaseVersion).then(user => {
                                if (user) {
                                        headers.set("Authorization", "Bearer " + user.access_token);
                                }
                                else {
                                        this.router.navigateByUrl("login");
                                }
                                options.headers = headers;
                                resolve(options);
                        });

                });
        }

}