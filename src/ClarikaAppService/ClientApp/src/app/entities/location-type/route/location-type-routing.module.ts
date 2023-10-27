import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { UserRouteAccessService } from "app/core/auth/user-route-access.service";
import { LocationTypeComponent } from "../list/location-type.component";
import { LocationTypeDetailComponent } from "../detail/location-type-detail.component";
import { LocationTypeUpdateComponent } from "../update/location-type-update.component";
import { LocationTypeRoutingResolveService } from "./location-type-routing-resolve.service";

const locationTypeRoute: Routes = [
  {
    path: "",
    component: LocationTypeComponent,
    canActivate: [UserRouteAccessService],
  },
  {
    path: ":id/view",
    component: LocationTypeDetailComponent,
    resolve: {
      locationType: LocationTypeRoutingResolveService,
    },
    canActivate: [UserRouteAccessService],
  },
  {
    path: "new",
    component: LocationTypeUpdateComponent,
    resolve: {
      locationType: LocationTypeRoutingResolveService,
    },
    canActivate: [UserRouteAccessService],
  },
  {
    path: ":id/edit",
    component: LocationTypeUpdateComponent,
    resolve: {
      locationType: LocationTypeRoutingResolveService,
    },
    canActivate: [UserRouteAccessService],
  },
];

@NgModule({
  imports: [RouterModule.forChild(locationTypeRoute)],
  exports: [RouterModule],
})
export class LocationTypeRoutingModule {}
