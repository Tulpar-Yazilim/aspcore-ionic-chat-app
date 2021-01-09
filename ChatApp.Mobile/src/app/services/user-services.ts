import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { BaseService } from './base-services';
import { Storage } from '@ionic/storage';
import { Router } from '@angular/router';

@Injectable()
export class UserService extends BaseService {
        constructor(http: Http, storage: Storage, router: Router) { super(http, storage, "User/", router); }
} 