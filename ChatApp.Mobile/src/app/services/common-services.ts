
import { Injectable } from '@angular/core';
import { ToastController, LoadingController } from '@ionic/angular';

@Injectable()
export class CommonServices {

        Loader: any = null;
        Toaster: any = null;

        constructor(
                private loadingCtrl: LoadingController,
                private toastCtrl: ToastController
        ) {
                this.Loader = null;
                this.Toaster = null;
        }

        async showLoading(message: string) {
                if (this.Loader == null) {
                        this.Loader = await this.loadingCtrl.create({
                                message: message
                        });
                        this.Loader.present();
                }
        }

        async hideLoading() {
                if (this.Loader != null) {
                        this.Loader.dismiss();
                        this.Loader = null;
                }

        }

        async showToast(message: string, duration: number = 3000) {
                if (this.Toaster == null) {
                        this.Toaster = await this.toastCtrl.create({
                                message: message,
                                duration: duration
                        });
                        this.Toaster.present();
                }
        }

        async hideToast() {
                if (this.Toaster != null) {
                        this.Toaster.dismiss();
                        this.Toaster = null;
                }

        }

}