import { ChangeDetectorRef, Component, OnInit } from "@angular/core";
import { Headers, Http, RequestOptions } from "@angular/http";
import { ActivatedRoute } from "@angular/router";
import { File } from "@ionic-native/file/ngx";
import { Media, MediaObject } from "@ionic-native/media/ngx";
import { OSNotification } from "@ionic-native/onesignal/ngx";
import { Events, Platform } from "@ionic/angular";
import { Storage } from "@ionic/storage";
import { BaseConfig } from "../DTOs/Base/BaseConfig";
import { CommonServices } from "../services/common-services";

@Component({
  selector: "app-chat-detail",
  templateUrl: "./chat-detail.page.html",
  styleUrls: ["./chat-detail.page.scss"],
})
export class ChatDetailPage implements OnInit {
  user: any;
  username = "";

  words = [
    "Yemek hazır!",
    "Yemeğin hazır olmasına 5 dk var.",
    "Buraya gelebilir misin?",
  ];

  conversation = [];

  recording: boolean = false;
  filePath: string;
  fileName: string;
  audio: MediaObject;
  audioList: any[] = [];

  input = "";

  constructor(
    private commonServices: CommonServices,
    private storage: Storage,
    private platform: Platform,
    private media: Media,
    private file: File,
    private http: Http,
    private activatedRoute: ActivatedRoute,
    private events: Events,
    private changeRef: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.storage
      .get(BaseConfig.BaseStorageUserName + BaseConfig.BaseVersion)
      .then((_user) => {
        this.user = _user;
      });
    this.username = this.activatedRoute.snapshot.paramMap.get("username");

    this.events.subscribe(
      "notification:received",
      (notification: OSNotification) => {
        if (
          this.user.username == notification.payload.additionalData.username
        ) {
          this.conversation.push({
            text: notification.payload.body,
            sender: 0,
            image: "assets/chat/user4.jpeg",
            delivered: true,
            sent: true,
            read: true,
            sendTime: notification.payload.additionalData.sendTime,
          });
          this.changeRef.detectChanges();
        }
      }
    );
  }

  ionViewDidEnter() {
    setTimeout(() => {
      this.scrollToBottom();
    }, 10);
  }

  send(word: string = "") {
    this.input = word;
    if (this.input != "") {
      this.commonServices.showLoading("Mesajınız gönderiliyor...");

      let notificationObj: any = {
        app_id: "ONESIGNAL_APP_ID_HERE",
        contents: { en: this.input },
        headings: { en: this.user.username + " mesaj gönderdi" },
        data: {
          username: this.username,
          senderName: this.user.username,
          sendTime:
            (new Date().getHours() < 10
              ? "0" + new Date().getHours()
              : new Date().getHours()) +
            ":" +
            (new Date().getMinutes() < 10
              ? "0" + new Date().getMinutes()
              : new Date().getMinutes()),
        },
        filters: [
          {
            field: "tag",
            key: "Username",
            relation: "=",
            value: this.username,
          },
        ],
      };

      let options = new RequestOptions();
      let headers = new Headers();
      headers.set("Authorization", "Basic ONESIGNAL_APP_KEY_HERE");
      headers.set("Content-Type", "application/json");
      options.headers = headers;

      this.http
        .post(
          "https://onesignal.com/api/v1/notifications",
          notificationObj,
          options
        )
        .subscribe(
          () => {
            this.conversation.push({
              text: this.input,
              sender: 1,
              image: "assets/chat/user4.jpeg",
              delivered: true,
              sent: true,
              read: true,
              sendTime:
                (new Date().getHours() < 10
                  ? "0" + new Date().getHours()
                  : new Date().getHours()) +
                ":" +
                (new Date().getMinutes() < 10
                  ? "0" + new Date().getMinutes()
                  : new Date().getMinutes()),
            });
            this.input = "";
            setTimeout(() => {
              this.scrollToBottom();
            }, 10);

            this.commonServices.hideLoading();
          },
          (err) => {
            console.log(err);
            this.commonServices.hideLoading();
            this.input = "";
            this.commonServices.showToast(
              "Mesajınız gönderilirken hata oluştu!"
            );
          }
        );
    }
  }

  startRecord() {
    if (this.platform.is("android")) {
      this.fileName =
        "record" +
        new Date().getDate() +
        new Date().getMonth() +
        new Date().getFullYear() +
        new Date().getHours() +
        new Date().getMinutes() +
        new Date().getSeconds() +
        ".wav";
      this.filePath =
        this.file.externalDataDirectory.replace(/file:\/\//g, "") +
        this.fileName;
      this.audio = this.media.create(this.filePath);
    }
    this.audio.startRecord();
    this.recording = true;
  }

  stopRecord() {
    this.audio.stopRecord();
    let data = { filename: this.fileName };
    this.audioList.push(data);

    this.file
      .resolveLocalFilesystemUrl(this.file.externalDataDirectory)
      .then((entry) => {
        this.file.readAsDataURL(entry.nativeURL, this.fileName).then(
          (data) => {
            console.log("========= BASE64 =======");
            console.log(data);
          },
          (err) => {
            console.log(err);
          }
        );

        this.file.readAsArrayBuffer(entry.nativeURL, this.fileName).then(
          (data) => {
            console.log("========= ARRAY BUFFER =======");
            console.log(data);
          },
          (err) => {
            console.log(err);
          }
        );

        this.file.readAsBinaryString(entry.nativeURL, this.fileName).then(
          (data) => {
            console.log("========= BINARY STRING =======");
            console.log(data);
          },
          (err) => {
            console.log(err);
          }
        );
      });

    localStorage.setItem("audiolist", JSON.stringify(this.audioList));
    this.recording = false;
    this.getAudioList();
  }

  getAudioList() {
    if (localStorage.getItem("audiolist")) {
      this.audioList = JSON.parse(localStorage.getItem("audiolist"));
      console.log(this.audioList);
    }
  }

  playAudio(file, idx) {
    if (this.platform.is("android")) {
      this.filePath =
        this.file.externalDataDirectory.replace(/file:\/\//g, "") + file;
      this.audio = this.media.create(this.filePath);
    }
    this.audio.play();
    this.audio.setVolume(0.8);
  }

  stopAudio() {
    this.audio.stop();
  }

  scrollToBottom() {
    let content = document.getElementById("chat-container");
    let parent = document.getElementById("chat-parent");
    let scrollOptions = {
      left: 0,
      top: content.offsetHeight,
    };

    parent.scrollTo(scrollOptions);
  }
}
