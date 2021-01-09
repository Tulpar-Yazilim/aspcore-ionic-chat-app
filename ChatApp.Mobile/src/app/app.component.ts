import { Component } from '@angular/core';

import { Platform, Events } from '@ionic/angular';
import { SplashScreen } from '@ionic-native/splash-screen/ngx';
import { Router } from '@angular/router';

import { OneSignal } from '@ionic-native/onesignal/ngx';
import { Storage } from '@ionic/storage';
import { BaseConfig } from './DTOs/Base/BaseConfig';


@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {

  constructor(
    private platform: Platform,
    private splashScreen: SplashScreen,
    private oneSignal: OneSignal,
    private router: Router,
    private storage: Storage,
    private events: Events
  ) {
    this.initializeApp();
  }

  initializeApp() {

    this.platform.ready().then(() => {

      this.initOneSignal();

      this.storage.get('Tulpar_Chat_App_User_' + BaseConfig.BaseVersion).then(user => {

        if (user) {
          this.router.navigateByUrl("chat-list");
        }

        setTimeout(() => {
          this.splashScreen.hide();
        }, 1500);

      });


    });

  }

  logout() {
    this.storage.clear().then(() => {
      this.router.navigateByUrl("login");
    });
  }

  initOneSignal() {

    this.oneSignal.startInit('bae5a7c3-26be-4826-8a4d-9f50e08ff85a', '303277360782');

    this.oneSignal.inFocusDisplaying(this.oneSignal.OSInFocusDisplayOption.Notification);

    this.oneSignal.handleNotificationReceived().subscribe((notification) => {
      this.events.publish("notification:received", notification);
    });

    this.oneSignal.handleNotificationOpened().subscribe((notificationOpened) => {
      this.router.navigateByUrl("chat-detail/" + notificationOpened.notification.payload.additionalData.senderName);
    });

    this.oneSignal.endInit();

  }


}
