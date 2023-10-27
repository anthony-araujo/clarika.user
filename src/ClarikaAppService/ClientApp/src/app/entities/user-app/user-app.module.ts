import { NgModule } from "@angular/core";
import { SharedModule } from "app/shared/shared.module";
import { UserAppComponent } from "./list/user-app.component";
import { UserAppDetailComponent } from "./detail/user-app-detail.component";
import { UserAppUpdateComponent } from "./update/user-app-update.component";
import { UserAppDeleteDialogComponent } from "./delete/user-app-delete-dialog.component";
import { UserAppRoutingModule } from "./route/user-app-routing.module";

@NgModule({
  imports: [SharedModule, UserAppRoutingModule],
  declarations: [
    UserAppComponent,
    UserAppDetailComponent,
    UserAppUpdateComponent,
    UserAppDeleteDialogComponent,
  ],
  entryComponents: [UserAppDeleteDialogComponent],
})
export class UserAppModule {}
