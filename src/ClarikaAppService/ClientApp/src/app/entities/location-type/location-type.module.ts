import { NgModule } from "@angular/core";
import { SharedModule } from "app/shared/shared.module";
import { LocationTypeComponent } from "./list/location-type.component";
import { LocationTypeDetailComponent } from "./detail/location-type-detail.component";
import { LocationTypeUpdateComponent } from "./update/location-type-update.component";
import { LocationTypeDeleteDialogComponent } from "./delete/location-type-delete-dialog.component";
import { LocationTypeRoutingModule } from "./route/location-type-routing.module";

@NgModule({
  imports: [SharedModule, LocationTypeRoutingModule],
  declarations: [
    LocationTypeComponent,
    LocationTypeDetailComponent,
    LocationTypeUpdateComponent,
    LocationTypeDeleteDialogComponent,
  ],
  entryComponents: [LocationTypeDeleteDialogComponent],
})
export class LocationTypeModule {}
