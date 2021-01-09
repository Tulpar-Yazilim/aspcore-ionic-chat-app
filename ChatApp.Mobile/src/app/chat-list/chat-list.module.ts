import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';

import { IonicModule } from '@ionic/angular';

import { ChatListPage } from './chat-list.page';

import { UserService } from '../services/user-services';
import { CommonServices } from '../services/common-services';

const routes: Routes = [
  {
    path: '',
    component: ChatListPage
  }
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    RouterModule.forChild(routes)
  ],
  providers: [
    UserService,
    CommonServices
  ],
  declarations: [ChatListPage]
})
export class ChatListPageModule { }
