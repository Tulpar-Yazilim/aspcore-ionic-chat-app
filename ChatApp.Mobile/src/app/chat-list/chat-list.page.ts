import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { UserService } from '../services/user-services';
import { APIResult } from '../DTOs/Base/APIResult';
import { CommonServices } from '../services/common-services';
import { Storage } from '@ionic/storage';
import { BaseConfig } from '../DTOs/Base/BaseConfig';
import { Router } from '@angular/router';
import { Events } from '@ionic/angular';
import { OSNotification } from '@ionic-native/onesignal/ngx';


@Component({
  selector: 'app-chat-list',
  templateUrl: './chat-list.page.html',
  styleUrls: ['./chat-list.page.scss'],
})
export class ChatListPage implements OnInit {

  userList: Array<any>;

  constructor(
    private commonService: CommonServices,
    private userService: UserService,
    private storage: Storage,
    private router: Router,
    private events: Events,
    private changeRef: ChangeDetectorRef
  ) { }

  async ngOnInit() {

    await this.getList();

    this.events.subscribe("notification:received", (notification: OSNotification) => {

      let userChat = this.userList.find(x => x.username == notification.payload.additionalData.senderName);

      if (userChat.badge) {
        userChat.badge += 1;
      }
      else {
        userChat.badge = 1;
      }
      userChat.lastMessage = notification.payload.body;
      userChat.sendTime = notification.payload.additionalData.sendTime;
      userChat.time = (new Date().getHours() < 10 ? "0" + new Date().getHours() : new Date().getHours()) + ":" + (new Date().getMinutes() < 10 ? "0" + new Date().getMinutes() : new Date().getMinutes());

      this.changeRef.detectChanges();

    });
  }

  async getList() {

    await this.commonService.showLoading("Kullanıcı listesi yükleniyor...");

    this.userService.fetchObservable("GetList").then(async (response: APIResult<any>) => {

      if (response.isSuccess) {
        this.userList = this.userService.decrypt(response.data);

        this.storage.get(BaseConfig.BaseStorageUserName + BaseConfig.BaseVersion).then(async user => {
          this.userList = this.userList.filter(x => x.username != user.username);
          this.userList.forEach(element => {
            element.image = "../../assets/chat/user4.jpeg";
          });
          await this.commonService.hideLoading();
        });


      }
      else {
        await this.commonService.hideLoading();
        await this.commonService.showToast("Kullanıcı listesi getirilirken bir sorun oluştu!");
      }

    }, async () => {
      await this.commonService.hideLoading();
      await this.commonService.showToast("Kullanıcı listesi getirilirken bir sorun oluştu!");
    });
  }

  goDetail(user: any) {
    this.router.navigateByUrl("chat-detail/" + user.username);
  }

}
