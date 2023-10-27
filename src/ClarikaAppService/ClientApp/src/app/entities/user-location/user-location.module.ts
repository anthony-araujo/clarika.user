import { NgModule } from "@angular/core";
import { SharedModule } from "app/shared/shared.module";
import { UserLocationComponent } from "./list/user-location.component";
import { UserLocationDetailComponent } from "./detail/user-location-detail.component";
import { UserLocationUpdateComponent } from "./update/user-location-update.component";
import { UserLocationDeleteDialogComponent } from "./delete/user-location-delete-dialog.component";
import { UserLocationRoutingModule } from "./route/user-location-routing.module";

@NgModule({
  imports: [SharedModule, UserLocationRoutingModule],
  declarations: [
    UserLocationComponent,
    UserLocationDetailComponent,
    UserLocationUpdateComponent,
    UserLocationDeleteDialogComponent,
  ],
  entryComponents: [UserLocationDeleteDialogComponent],
})
export class UserLocationModule {}
