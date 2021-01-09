import { Component, OnInit } from "@angular/core";

import { CommonServices } from "../services/common-services";

import { OneSignal } from "@ionic-native/onesignal/ngx";
import { Router } from "@angular/router";
import { AuthService } from "../services/auth-services";
import { AuthLoginDto } from "../DTOs/Auth/AuthDto";
import { APIResult } from "../DTOs/Base/APIResult";
import { BaseConfig } from "../DTOs/Base/BaseConfig";
import { Storage } from "@ionic/storage";

@Component({
  selector: "app-login",
  templateUrl: "./login.page.html",
  styleUrls: ["./login.page.scss"],
})
export class LoginPage implements OnInit {
  postData = {
    email: "tulparyazilim",
    password: "*****",
  };

  constructor(
    private commonServices: CommonServices,
    private storage: Storage,
    private oneSignal: OneSignal,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit() {}

  login() {
    this.commonServices.showLoading("Lütfen bekleyiniz...");

    let loginData: AuthLoginDto = new AuthLoginDto();
    loginData.email = this.postData.email;
    loginData.password = btoa(this.postData.password);

    this.authService.login(loginData).then((response: APIResult<any>) => {
      if (response.isSuccess) {
        response.data = this.authService.decrypt(response.data);

        this.storage
          .set("Tulpar_Chat_App_User_" + BaseConfig.BaseVersion, {
            access_token: response.data.value,
            username: response.data.username,
            email: response.data.email,
            roles: response.data.roles,
            name: response.data.name,
            surname: response.data.surname,
          })
          .then(() => {
            setTimeout(() => {
              this.commonServices.hideLoading();
            }, 250);

            this.oneSignal.sendTags({
              Username: response.data.username,
              Email: response.data.email,
            });
            this.router.navigateByUrl("chat-list");
          });
      } else {
        this.commonServices.hideLoading();
        this.commonServices.showToast(
          "Giriş bilgileriniz hatalıdır! Lütfen kontrol ediniz."
        );
      }
    });
  }
}
