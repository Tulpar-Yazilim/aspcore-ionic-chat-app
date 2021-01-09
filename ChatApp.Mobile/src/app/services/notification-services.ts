import { Injectable } from '@angular/core';
import { Storage } from '@ionic/storage';
import { Http } from '@angular/http';
import { BaseService } from './base-services';
import { Router } from '@angular/router';

@Injectable()
export class NotificationService extends BaseService {
        constructor(http: Http, storage: Storage, router: Router) { super(http, storage, "Notification/", router); }
} 